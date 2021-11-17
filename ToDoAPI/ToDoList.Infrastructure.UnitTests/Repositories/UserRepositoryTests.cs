using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ToDoList.Infrastructure.Context;
using ToDoList.Infrastructure.Entities;
using ToDoList.Infrastructure.Repositories;
using Xunit;

namespace ToDoList.Infrastructure.UnitTests.Repositories
{
    /// <summary>
    /// Defines the <see cref="UserRepositoryTests" />.
    /// </summary>
    public class UserRepositoryTests
    {

        /// <summary>
        /// Defines the _dbContextOptions.
        /// </summary>
        private readonly DbContextOptions<ToDoDbContext> _dbContextOptions =
                        new DbContextOptionsBuilder<ToDoDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        private readonly User userDetails = new User()
        {
            UserName = "Test User",
            EmailId = "test@test.com",
            Password = "************"
        };

        /// <summary>
        /// The Constructor_ContextIsNull_ThrowsArgumentNullException.
        /// </summary>
        [Fact]
        public void Constructor_ContextIsNull_ThrowsArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                new UserRepository(null));
        }

        /// <summary>
        /// The Add_New_User_Success.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Add_New_User_Success()
        {
            // Arrange
            var _toDoDbContext = new ToDoDbContext(_dbContextOptions);
            _toDoDbContext.Database.EnsureCreated();
            var repo = new UserRepository(_toDoDbContext);

            // Act
            await repo.AddAsync(userDetails);
            await _toDoDbContext.SaveChangesAsync();

            // Assert
            Assert.Contains(userDetails, _toDoDbContext.Users);
        }

        /// <summary>
        /// The Update_Existing_User_Success.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Update_Existing_User_Success()
        {
            // Arrange
            var _toDoDbContext = new ToDoDbContext(_dbContextOptions);
            _toDoDbContext.Database.EnsureCreated();
            var repo = new UserRepository(_toDoDbContext);

            _toDoDbContext.Users.Add(userDetails);

            await _toDoDbContext.SaveChangesAsync();

            User updateDetails = await _toDoDbContext.Users.LastOrDefaultAsync();

            updateDetails.UserName = "Changed user text";

            // Act
            repo.Update(updateDetails);
            await _toDoDbContext.SaveChangesAsync();

            // Assert
            Assert.Contains(updateDetails, _toDoDbContext.Users);
        }


        /// <summary>
        /// The Remove_Existing_User_Success.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Remove_Existing_User_Success()
        {
            // Arrange
            var _toDoDbContext = new ToDoDbContext(_dbContextOptions);
            _toDoDbContext.Database.EnsureCreated();
            var repo = new UserRepository(_toDoDbContext);

            _toDoDbContext.Users.Add(userDetails);

            await _toDoDbContext.SaveChangesAsync();

            User removeDetails = await _toDoDbContext.Users.LastOrDefaultAsync();

            // Act
            repo.Remove(removeDetails);
            await _toDoDbContext.SaveChangesAsync();

            // Assert
            Assert.DoesNotContain(removeDetails, _toDoDbContext.Users);
        }

        /// <summary>
        /// GetUserDetailsByName_Success.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task GetUserDetailsByName_Success()
        {
            // Arrange
            var _toDoDbContext = new ToDoDbContext(_dbContextOptions);
            _toDoDbContext.Database.EnsureCreated();
            var repo = new UserRepository(_toDoDbContext);

            string testUserName = "Test User";

            _toDoDbContext.Users.Add(userDetails);

            await _toDoDbContext.SaveChangesAsync();

            // Act
            var actualResponse = repo.GetUserDetailsByName(testUserName);

            // Assert
            Assert.IsType<User>(actualResponse);
            Assert.Equal(userDetails.EmailId, actualResponse.EmailId);
            Assert.Equal(userDetails.UserName, actualResponse.UserName);
            Assert.Equal(userDetails.Password, actualResponse.Password);
        }

        /// <summary>
        /// GetUserDetailsByName_Failure.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task GetUserDetailsByName_Failure()
        {
            // Arrange
            var _toDoDbContext = new ToDoDbContext(_dbContextOptions);
            _toDoDbContext.Database.EnsureCreated();
            var repo = new UserRepository(_toDoDbContext);

            string testUserName = "Test Items";

            _toDoDbContext.Users.Add(userDetails);

            await _toDoDbContext.SaveChangesAsync();

            // Act
            var actualResponse = repo.GetUserDetailsByName(testUserName);

            // Assert
            Assert.Null(actualResponse);
        }

        /// <summary>
        /// CheckUserExists_Success.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task CheckUserExists_Success()
        {
            // Arrange
            var _toDoDbContext = new ToDoDbContext(_dbContextOptions);
            _toDoDbContext.Database.EnsureCreated();
            var repo = new UserRepository(_toDoDbContext);

            int testUserId = 1;

            _toDoDbContext.Users.Add(userDetails);

            await _toDoDbContext.SaveChangesAsync();

            // Act
            bool isUserExists = repo.CheckUserExists(testUserId);

            // Assert
            Assert.True(isUserExists);
        }

        /// <summary>
        /// CheckUserExists_Failure.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task CheckUserExists_Failure()
        {
            // Arrange
            var _toDoDbContext = new ToDoDbContext(_dbContextOptions);
            _toDoDbContext.Database.EnsureCreated();
            var repo = new UserRepository(_toDoDbContext);

            int testUserId = 5;

            _toDoDbContext.Users.Add(userDetails);

            await _toDoDbContext.SaveChangesAsync();

            // Act
            bool isUserExists = repo.CheckUserExists(testUserId);

            // Assert
            Assert.False(isUserExists);
        }
    }
}
