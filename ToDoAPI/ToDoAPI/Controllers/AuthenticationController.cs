using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using ToDoAPI.ResponseModels;
using ToDoList.Domain.Interfaces;
using ToDoList.Domain.Models;

namespace ToDoAPI.Controllers
{
    /// <summary>
    /// Defines the <see cref="AuthenticationController" />.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        /// <summary>
        /// Defines the _userProvider.
        /// </summary>
        private readonly IUserProvider _userProvider;

        /// <summary>
        /// Defines the _configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
        /// </summary>
        /// <param name="userProvider">The userProvider<see cref="IUserProvider"/></param>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/></param>
        public AuthenticationController(IUserProvider userProvider, IConfiguration configuration)
        {
            _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Authenticate User.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            try
            {
                var user = _userProvider.GetUserDetails(model.Username);

                if (user != null && user.Password == model.Password)
                {
                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Id.ToString()),
                    };

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
                return Unauthorized();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User authentication failed! Please check user details and try again." });
            }
        }

        /// <summary>
        /// Register new user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                var userExists = _userProvider.GetUserDetails(model.UserName);

                if (userExists != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
                };

                User user = new User
                {
                    UserName = model.UserName,
                    EmailId = model.EmailId,
                    Password = model.Password
                };

                await _userProvider.AddUser(user);

                return Ok(new Response { Status = "Success", Message = "User created successfully" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            }
        }
    }
}
