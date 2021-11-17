using AutoMapper;
using Moq;
using System;
using System.Reflection;
using System.Threading.Tasks;
using ToDoList.Domain.Models;
using ToDoList.Domain.Providers;
using ToDoList.Infrastructure.UOW;
using Xunit;

namespace ToDoList.Domain.UnitTests.Providers
{
    public class UserProviderTests
    {
        // <summary>
        /// Defines the _mockIMapper.
        /// </summary>
        private readonly IMapper _mockIMapper;

        /// <summary>
        /// Defines the _mockUOW.
        /// </summary>
        private readonly Mock<IUnitOfWork> _mockUOW = new Mock<IUnitOfWork>();

        private static readonly User userDetail = new User
        {
            UserName = "Test User",
            EmailId = "Test@test.com",
            Password = "**********"
        };


        /// <summary>
        /// Initializes a new instance of the <see cref="UserProviderTests"/> class.
        /// </summary>
        public UserProviderTests()
        {
            MapperConfiguration configuration = new MapperConfiguration(cfg =>
                cfg.AddMaps(Assembly.Load("ToDoList.Domain")));
            _mockIMapper = new Mapper(configuration);
        }

        /// <summary>
        /// The Constructor_UOW_IsNull_ThrowsArgumentNullException.
        /// </summary>
        [Fact]
        public void Constructor_UOW_IsNull_ThrowsArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                new UserProvider(
                    null,
                   _mockIMapper
                    ));
        }

        /// <summary>
        /// The Constructor_IMapperIsNull_ThrowsArgumentNullException.
        /// </summary>
        [Fact]
        public void Constructor_IMapperIsNull_ThrowsArgumentNullException()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
               new UserProvider(_mockUOW.Object, null));
        }

        /// <summary>
        /// The GetUser_Success.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task GetUser_Success()
        {
            //Arrange
            var userProvider = new UserProvider(_mockUOW.Object, _mockIMapper);

            _mockUOW.Setup(result => result.Users.AddAsync(It.IsAny<Infrastructure.Entities.User>()))
               .Returns(Task.FromResult(true));

            _mockUOW.Setup(result => result.Users.GetUserDetailsByName(It.IsAny<string>()))
                .Returns(
                    new Infrastructure.Entities.User
                    {
                        UserName = userDetail.UserName,
                        Password = userDetail.Password,
                        EmailId = userDetail.EmailId,
                    });

            //Act
            await userProvider.AddUser(userDetail);
            var response = userProvider.GetUserDetails(userDetail.UserName);

            //Assert
            Assert.Equal(userDetail.UserName, response.UserName);
            Assert.Equal(userDetail.Password, response.Password);
            Assert.Equal(userDetail.EmailId, response.EmailId);
        }

        /// <summary>
        /// The CheckItem_Success.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void CheckItem_Success()
        {
            //Arrange
            var userProvider = new UserProvider(_mockUOW.Object, _mockIMapper);

            _mockUOW.Setup(result => result.Users.CheckUserExists(It.IsAny<int>()))
                .Returns(true);

            int testUserId = 1;
            //Act
            bool isUserExists = userProvider.CheckItem(testUserId);

            //Assert
            Assert.True(isUserExists);
        }

        /// <summary>
        /// The CheckItem_Failure.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void CheckItem_Failure()
        {
            //Arrange
            var userProvider = new UserProvider(_mockUOW.Object, _mockIMapper);

            _mockUOW.Setup(result => result.Users.CheckUserExists(It.IsAny<int>()))
                .Returns(false);

            int testUserId = 5;
            //Act
            bool isUserExists = userProvider.CheckItem(testUserId);

            //Assert
            Assert.False(isUserExists);
        }
    }
}
