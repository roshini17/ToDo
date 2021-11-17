using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.Infrastructure.Context;
using ToDoList.Infrastructure.Entities;
using ToDoList.Infrastructure.Repositories;
using Xunit;

namespace ToDoList.Infrastructure.UnitTests.Repositories
{
    /// <summary>
    /// Defines the <see cref="ToDoItemRepositoryTests" />.
    /// </summary>
    public class ToDoItemRepositoryTests
    {

        /// <summary>
        /// Defines the _dbContextOptions.
        /// </summary>
        private readonly DbContextOptions<ToDoDbContext> _dbContextOptions =
                        new DbContextOptionsBuilder<ToDoDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        private readonly ToDoItem toDoItemDetails = new ToDoItem()
        {
            UserId = 1,
            ItemDescription = "Test Item",
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        /// <summary>
        /// The Constructor_ContextIsNull_ThrowsArgumentNullException.
        /// </summary>
        [Fact]
        public void Constructor_ContextIsNull_ThrowsArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                new ToDoItemRepository(null));
        }

        /// <summary>
        /// The Add_New_ToDoItem_Success.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Add_New_ToDoItem_Success()
        {
            // Arrange
            var _toDoDbContext = new ToDoDbContext(_dbContextOptions);
            _toDoDbContext.Database.EnsureCreated();
            var repo = new ToDoItemRepository(_toDoDbContext);

            // Act
            await repo.AddAsync(toDoItemDetails);
            await _toDoDbContext.SaveChangesAsync();

            // Assert
            Assert.Contains(toDoItemDetails, _toDoDbContext.ToDoItems);
        }

        /// <summary>
        /// The Update_Existing_ToDoItem_Success.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Update_Existing_ToDoItem_Success()
        {
            // Arrange
            var _toDoDbContext = new ToDoDbContext(_dbContextOptions);
            _toDoDbContext.Database.EnsureCreated();
            var repo = new ToDoItemRepository(_toDoDbContext);

            _toDoDbContext.ToDoItems.Add(toDoItemDetails);

            await _toDoDbContext.SaveChangesAsync();

            ToDoItem updateDetails = await _toDoDbContext.ToDoItems.LastOrDefaultAsync();

            updateDetails.ItemDescription = "Changed text";

            // Act
            repo.Update(updateDetails);
            await _toDoDbContext.SaveChangesAsync();

            // Assert
            Assert.Contains(updateDetails, _toDoDbContext.ToDoItems);
        }


        /// <summary>
        /// The Remove_Existing_ToDoItem_Success.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Remove_Existing_ToDoItem_Success()
        {
            // Arrange
            var _toDoDbContext = new ToDoDbContext(_dbContextOptions);
            _toDoDbContext.Database.EnsureCreated();
            var repo = new ToDoItemRepository(_toDoDbContext);

            _toDoDbContext.ToDoItems.Add(toDoItemDetails);

            await _toDoDbContext.SaveChangesAsync();

            ToDoItem removeDetails = await _toDoDbContext.ToDoItems.LastOrDefaultAsync();

            // Act
            repo.Remove(removeDetails);
            await _toDoDbContext.SaveChangesAsync();

            // Assert
            Assert.DoesNotContain(removeDetails, _toDoDbContext.ToDoItems);
        }

        /// <summary>
        /// GetToDoItemsAsync_Success.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task GetToDoItemsAsync_Success()
        {
            // Arrange
            var _toDoDbContext = new ToDoDbContext(_dbContextOptions);
            _toDoDbContext.Database.EnsureCreated();
            var repo = new ToDoItemRepository(_toDoDbContext);

            int testUserId = 1;
            
            _toDoDbContext.ToDoItems.Add(toDoItemDetails);
            await _toDoDbContext.SaveChangesAsync();

            // Act
            var actualResponse = await repo.GetToDoItemsForUserAsync(testUserId);

            // Assert
            Assert.NotNull(actualResponse);
            Assert.IsAssignableFrom<IEnumerable<ToDoItem>>(actualResponse);
        }

        /// <summary>
        /// GetToDoItemAsync_Null_Response.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task GetToDoItemAsync_Null_Response()
        {
            // Arrange
            var _toDoDbContext = new ToDoDbContext(_dbContextOptions);
            _toDoDbContext.Database.EnsureCreated();
            var repo = new ToDoItemRepository(_toDoDbContext);

            int testItemId = 1;

            // Act
            var actualResponse = await repo.GetByIdAsyncChange(testItemId);

            // Assert
            Assert.Null(actualResponse);
        }

        /// <summary>
        /// IsItemExist_Success.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task IsItemExist_Success()
        {
            // Arrange
            var _toDoDbContext = new ToDoDbContext(_dbContextOptions);
            _toDoDbContext.Database.EnsureCreated();
            var repo = new ToDoItemRepository(_toDoDbContext);

            int testItemId = 1;

            _toDoDbContext.ToDoItems.Add(toDoItemDetails);

            await _toDoDbContext.SaveChangesAsync();

            // Act
            var actualResponse = repo.CheckItemExsists(testItemId);

            // Assert
            Assert.IsType<bool>(actualResponse);
            Assert.True(actualResponse);
        }

        /// <summary>
        /// IsItemExists_Failure.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public void IsItemExists_Failure()
        {
            // Arrange
            var _toDoDbContext = new ToDoDbContext(_dbContextOptions);
            _toDoDbContext.Database.EnsureCreated();
            var repo = new ToDoItemRepository(_toDoDbContext);

            int testItemId = 5;

            // Act
            var responseForEmptyItem = repo.CheckItemExsists(testItemId);

            // Assert
            Assert.IsType<bool>(responseForEmptyItem);
            Assert.False(responseForEmptyItem);
        }

        /// <summary>
        /// IsItemExists_Exception_Failure.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public void IsItemExists_Exception_Failure()
        {
            // Arrange
            var _toDoDbContext = new ToDoDbContext(_dbContextOptions);
            _toDoDbContext.Database.EnsureCreated();
            var repo = new ToDoItemRepository(_toDoDbContext);

            _toDoDbContext.ToDoItems = null;
            int testItemId = 5;

            // Assert
            Assert.Throws<ArgumentNullException>(() => repo.CheckItemExsists(testItemId));
        }
    }
}
