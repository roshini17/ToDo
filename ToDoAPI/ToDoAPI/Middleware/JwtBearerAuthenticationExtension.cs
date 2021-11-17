using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ToDoAPI.Middleware
{
    /// <summary>
    /// Defines the <see cref="JwtBearerAuthenticationExtension" />.
    /// </summary>
    public static class JwtBearerAuthenticationExtension
    {
        /// <summary>
        /// The Extension method to add jwtBearer Authentication ability.
        /// </summary>
        /// <param name="service">The service<see cref="IServiceCollection"/></param>
        /// <param name="validAudience">The validAudience<see cref="string"/></param>
        /// <param name="validIssuer">The validIssuer<see cref="string"/></param>
        /// <param name="secretKey">The secretKey<see cref="string"/></param>
        public static AuthenticationBuilder AddJwtBearerAuthentication(this IServiceCollection service,
                string validAudience, string validIssuer, string secretKey)
        {
            return service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = validAudience,
                    ValidIssuer = validIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }
    }
}
