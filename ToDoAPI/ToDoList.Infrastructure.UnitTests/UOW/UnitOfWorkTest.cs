using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using ToDoList.Infrastructure.Context;
using ToDoList.Infrastructure.UOW;
using Xunit;

namespace ToDoList.Infrastructure.UnitTests.UOW
{
    public class UnitOfWorkTest
    {
        /// <summary>
        /// Defines the dbContextOptions.
        /// </summary>
        private readonly DbContextOptions<ToDoDbContext> dbContextOptions =
                           new DbContextOptionsBuilder<ToDoDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;

        /// <summary>
        /// The ContextIsNull_ThrowsArgumentNullException.
        /// </summary>
        [Fact]
        public void ContextIsNull_ThrowsArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                new UnitOfWork(null));
        }

        /// <summary>
        /// SaveChanges.
        /// </summary>
        [Fact]
        public void SaveChanges_Success_Response()
        {
            //Arrange
            var uow = new UnitOfWork(SetInMemoryDataUOW(true));

            //Act
            var result = uow.Save();

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// SaveChangesAsync.
        /// </summary>
        [Fact]
        public async System.Threading.Tasks.Task SaveChangesAsync_Success_Response()
        {
            //Arrange
            var uow = new UnitOfWork(SetInMemoryDataUOW(true));

            //Act
            var result = await uow.SaveAsync();

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// SaveChangesAsync.
        /// </summary>
        [Fact]
        public async System.Threading.Tasks.Task SaveChangesAsync_Fail_Response()
        {
            //Arrange
            var uow = new UnitOfWork(SetInMemoryDataUOW(false));

            //Act
            var result = await uow.SaveAsync();

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// SaveChangesAsync.
        /// </summary>
        [Fact]
        public void SaveChanges_Fail_Response()
        {
            //Arrange
            var uow = new UnitOfWork(SetInMemoryDataUOW(false));

            //Act
            var result = uow.Save();

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        [Fact]
        public async System.Threading.Tasks.Task Dispose_Success_Response()
        {
            //Arrange
            var uow = new UnitOfWork(SetInMemoryDataUOW(false));

            //Act
            var result = await uow.SaveAsync();
            try
            {

                uow.Dispose();
                Assert.False(false);
            }
            catch
            {
                Assert.True(false);
            }

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// The SetInMemoryData.
        /// </summary>
        /// <param name="isExisting">The isExisting<see cref="bool"/>.</param>
        /// <returns>The <see cref="ToDoDbContext"/>.</returns>
        private ToDoDbContext SetInMemoryDataUOW(bool isExisting)
        {
            //DB Models
            var context = new ToDoDbContext(dbContextOptions);

            var firstTestUser = new Entities.User
            {
                Id = 1,
                UserName = "Sample",
                Password = "Test@123"
            };

            var secondTestUser = new Entities.User
            {
                Id = 2,
                UserName = "Sampletest",
                Password = "Test@123"
            };

            context.Users.Add(firstTestUser);
            context.Users.Add(secondTestUser);

            var firstTestToDoItemOne = new Entities.ToDoItem
            {
                Id = 1,
                UserId = firstTestUser.Id,
                ItemDescription = "Design system",
                IsCompleted = false,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            var firstTestToDoItemTwo = new Entities.ToDoItem
            {
                Id = 2,
                UserId = firstTestUser.Id,
                ItemDescription = "Implement system",
                IsCompleted = false,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            var secondTestToDoItemOne = new Entities.ToDoItem
            {
                Id = 3,
                UserId = secondTestUser.Id,
                ItemDescription = "purchase",
                IsCompleted = false,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            var secondTestToDoItemTwo = new Entities.ToDoItem
            {
                Id = 4,
                UserId = secondTestUser.Id,
                ItemDescription = "Design system",
                IsCompleted = false,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            context.ToDoItems.Add(firstTestToDoItemOne);
            context.ToDoItems.Add(firstTestToDoItemTwo);
            context.ToDoItems.Add(secondTestToDoItemOne);
            context.ToDoItems.Add(secondTestToDoItemTwo);

            if (!isExisting)
            {
                context.SaveChanges();
            }
            return context;
        }
    }
}
