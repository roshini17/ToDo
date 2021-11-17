using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Threading.Tasks;
using ToDoAPI.Controllers;
using ToDoAPI.ResponseModels;
using ToDoList.Domain.Interfaces;
using Xunit;

namespace ToDoAPI.UnitTests.Controllers
{
    /// <summary>
    /// Defines the <see cref="AuthenticationControllerTest" />.
    /// </summary>
    public class AuthenticationControllerTest
    {
        /// <summary>
        /// Defines the mockUserProvider.
        /// </summary>
        private readonly Mock<IUserProvider> mockUserProvider = new Mock<IUserProvider>();

        /// <summary>
        /// Defines the mockIConfiguration.
        /// </summary>
        private readonly Mock<IConfiguration> mockIConfiguration = new Mock<IConfiguration>();

        /// <summary>
        /// The Constructor_UserProviderIsNull_ThrowsArgumentNullException.
        /// </summary>
        [Fact]
        public void Constructor_UserProviderIsNull_ThrowsArgumentNullException()
        {
            // assert
            Assert.Throws<ArgumentNullException>(() => new AuthenticationController(null, mockIConfiguration.Object));
        }

        /// <summary>
        /// The Constructor_ConfigurationIsNull_ThrowsArgumentNullException.
        /// </summary>
        [Fact]
        public void Constructor_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            // assert
            Assert.Throws<ArgumentNullException>(() => new AuthenticationController(mockUserProvider.Object, null));
        }

        /// <summary>
        /// The Constructor_IsValid.
        /// </summary>
        [Fact]
        public void Constructor_IsValid()
        {
            //act
            var authenticationController = new AuthenticationController(mockUserProvider.Object, mockIConfiguration.Object);

            //assert
            Assert.NotNull(authenticationController);
            Assert.IsType<AuthenticationController>(authenticationController);
        }

        /// <summary>
        /// The Get_Login_Authentication_Failure.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public void Get_Login_Authentication_Failure()
        {
            // Arrange
            var authenticationController = new AuthenticationController(mockUserProvider.Object, mockIConfiguration.Object);

            // Act 
            var response = authenticationController.Login(new ResponseModels.LoginModel
            {
                Username = "test",
                Password = "****"
            });

            // Assert
            Assert.NotNull(response);
            Assert.Equal(401, ((UnauthorizedResult)response).StatusCode);
        }

        /// <summary>
        /// The Get_Login_Authentication_Exception_Failure.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public void Get_Login_Authentication_Exception_Failure()
        {
            // Arrange
            var authenticationController = new AuthenticationController(mockUserProvider.Object, mockIConfiguration.Object);

            mockUserProvider.Setup(result => result.GetUserDetails(It.IsAny<string>()))
                .Throws(new ArgumentNullException());

            string expectedResult = "User authentication failed! Please check user details and try again.";

            // Act 
            var response = authenticationController.Login(new ResponseModels.LoginModel
            {
                Username = "test",
                Password = "****"
            });
            var resultResponse = (ObjectResult)response;

            // Assert
            Assert.NotNull(response);
            Assert.Equal(500, resultResponse.StatusCode);
            Assert.Equal(expectedResult, ((Response)resultResponse.Value).Message);
        }

        /// <summary>
        /// The Get_Login_Authentication_Success.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public void Get_Login_Authentication_Success()
        {
            // Arrange
            var authenticationController = new AuthenticationController(mockUserProvider.Object, mockIConfiguration.Object);

            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "JWT:Secret")]).Returns("secretkeyyyyyyyyyy");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "JWT:ValidIssuer")]).Returns("validUser url");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "JWT:ValidAudience")]).Returns("validAudience url");

            
            mockUserProvider.Setup(result => result.GetUserDetails(It.IsAny<string>()))
                .Returns(new ToDoList.Domain.Models.User
                {
                    UserName = "test",
                    Password = "****",
                    Id = 1,
                    EmailId = "test@test.com"
                });

            // Act 
            var response = authenticationController.Login(new ResponseModels.LoginModel
            {
                Username = "test",
                Password = "****"
            });

            // Assert
            Assert.NotNull(response);
            Assert.Equal(200, ((OkObjectResult)response).StatusCode);
        }

        /// <summary>
        /// The RegisterUser_Success.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task RegisterUser_Success()
        {
            // Arrange
            var authenticationController = new AuthenticationController(mockUserProvider.Object, mockIConfiguration.Object);

            // Act 
            var response = await authenticationController.Register(new ResponseModels.RegisterModel
            {
                UserName = "test",
                Password = "****",
                EmailId = "test@test.com"
            });

            // Assert
            Assert.NotNull(response);
            Assert.Equal(200, ((OkObjectResult)response).StatusCode);
        }

        /// <summary>
        /// The RegisterUser_Failure.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task RegisterUser_Failure()
        {
            // Arrange
            var authenticationController = new AuthenticationController(mockUserProvider.Object, mockIConfiguration.Object);

            mockUserProvider.Setup(result => result.GetUserDetails(It.IsAny<string>()))
                .Returns(new ToDoList.Domain.Models.User
                {
                    UserName = "test",
                    Password = "****",
                    Id = 1,
                    EmailId = "test@test.com"
                });

            // Act 
            var response = await authenticationController.Register(new ResponseModels.RegisterModel
            {
                UserName = "test",
                Password = "****",
                EmailId = "test@test.com"
            });

            // Assert
            Assert.NotNull(response);
            Assert.Equal(500, ((ObjectResult)response).StatusCode);
        }

        /// <summary>
        /// The RegisterUser_Exception_Failure.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task RegisterUser_Exception_Failure()
        {
            // Arrange
            var authenticationController = new AuthenticationController(mockUserProvider.Object, mockIConfiguration.Object);

            mockUserProvider.Setup(result => result.GetUserDetails(It.IsAny<string>()))
                .Throws(new ArgumentNullException());

            string expectedResult = "User creation failed! Please check user details and try again.";


            // Act 
            var response = await authenticationController.Register(new RegisterModel
            {
                UserName = "test",
                Password = "****",
                EmailId = "test@test.com"
            });

            var resultResponse = (ObjectResult)response;

            // Assert
            Assert.NotNull(response);
            Assert.Equal(500, resultResponse.StatusCode);
            Assert.Equal(expectedResult, ((Response)resultResponse.Value).Message);
        }
    }
}
