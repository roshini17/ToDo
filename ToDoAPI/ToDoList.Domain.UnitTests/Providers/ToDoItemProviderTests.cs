using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ToDoList.Domain.Models;
using ToDoList.Domain.Providers;
using ToDoList.Infrastructure.UOW;
using Xunit;

namespace ToDoList.Domain.UnitTests.Providers
{
    /// <summary>
    /// Defines the <see cref="ToDoItemProviderTests" />.
    /// </summary>
    public class ToDoItemProviderTests
    {
        /// <summary>
        /// Defines the _mockIMapper.
        /// </summary>
        private readonly IMapper _mockIMapper;

        /// <summary>
        /// Defines the _mockUOW.
        /// </summary>
        private readonly Mock<IUnitOfWork> _mockUOW = new Mock<IUnitOfWork>();

        private static readonly ToDoItem toDoItemDetail = new ToDoItem()
        {
            UserId = 1,
            ItemDescription = "Test Item",
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemProviderTests"/> class.
        /// </summary>
        public ToDoItemProviderTests()
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
                new ToDoItemProvider(
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
               new ToDoItemProvider(_mockUOW.Object, null));
        }

        /// <summary>
        /// The GetToDoItem_Success.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task GetToDoItem_Success()
        {
            //Arrange
            var toDoItemProvider = new ToDoItemProvider(_mockUOW.Object, _mockIMapper);
        
            _mockUOW.Setup(result => result.ToDoItems.AddAsync(It.IsAny<Infrastructure.Entities.ToDoItem>()))
               .Returns(Task.FromResult(true));

            _mockUOW.Setup(result => result.ToDoItems.GetToDoItemsForUserAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<IEnumerable<Infrastructure.Entities.ToDoItem>>(
                    new List<Infrastructure.Entities.ToDoItem> {
                        new Infrastructure.Entities.ToDoItem
                        {
                            UserId = toDoItemDetail.UserId,
                            ItemDescription = toDoItemDetail.ItemDescription,
                            CreatedDate = toDoItemDetail.CreatedDate,
                            ModifiedDate = toDoItemDetail.ModifiedDate
                        }
                    }));

            //Act
            await toDoItemProvider.AddToDoItem(toDoItemDetail);
            var response = await toDoItemProvider.GetToDoItemsAsync(1);

            //Assert
            Assert.Equal(toDoItemDetail.ItemDescription, response.ElementAt(0).ItemDescription);
        }

        /// <summary>
        /// The GetToDoItem_Failure.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        //[Fact]
        //public async Task GetToDoItem_Failure()
        //{
        //    //Arrange
        //    var toDoItemProvider = new ToDoItemProvider(_mockUOW.Object, _mockIMapper);

        //    _mockUOW.Setup(result => result.ToDoItems.AddAsync(It.IsAny<Infrastructure.Entities.ToDoItem>()))
        //       .Returns(Task.FromResult(false));

        //    _mockUOW.Setup(result => result.ToDoItems.GetToDoItemsForUserAsync(It.IsAny<int>()))
        //        .Returns(Task.FromResult<IEnumerable<Infrastructure.Entities.ToDoItem>>(
        //            new List<Infrastructure.Entities.ToDoItem> {
        //                new Infrastructure.Entities.ToDoItem
        //                {
        //                    UserId = toDoItemDetail.UserId,
        //                    ItemDescription = toDoItemDetail.ItemDescription,
        //                    CreatedDate = toDoItemDetail.CreatedDate,
        //                    ModifiedDate = toDoItemDetail.ModifiedDate
        //                }
        //            }));

        //    //Act
        //    await toDoItemProvider.AddToDoItem(null);
        //    var response = await toDoItemProvider.GetToDoItemsAsync(1);

        //    //Assert
        //    Assert.DoesNotContain(new ToDoItem(), response);
        //}

        /// <summary>
        /// The IsToDoItemExists_Success.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task IsToDoItemExists_Success()
        {
            //Arrange
            var toDoItemProvider = new ToDoItemProvider(_mockUOW.Object, _mockIMapper);
            _mockUOW.Setup(result => result.ToDoItems.AddAsync(It.IsAny<Infrastructure.Entities.ToDoItem>()))
               .Returns(Task.FromResult(true));

            _mockUOW.Setup(result => result.ToDoItems.CheckItemExsists(It.IsAny<int>()))
                .Returns(true);

            await toDoItemProvider.AddToDoItem(toDoItemDetail);
            int testItemId = 1;

            //Act
            var isExists = toDoItemProvider.CheckItem(testItemId);

            //Assert
            Assert.IsType<bool>(isExists);
            Assert.True(isExists);
        }

        /// <summary>
        /// The IsToDoItemExists_Failure.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task IsToDoItemExists_Failure()
        {
            //Arrange
            var toDoItemProvider = new ToDoItemProvider(_mockUOW.Object, _mockIMapper);
            _mockUOW.Setup(result => result.ToDoItems.AddAsync(It.IsAny<Infrastructure.Entities.ToDoItem>()))
               .Returns(Task.FromResult(true));

            _mockUOW.Setup(result => result.ToDoItems.CheckItemExsists(It.IsAny<int>()))
                .Returns(false);

            await toDoItemProvider.AddToDoItem(toDoItemDetail);
            int testItemId = 5;

            //Act
            var isExists = toDoItemProvider.CheckItem(testItemId);

            //Assert
            Assert.IsType<bool>(isExists);
            Assert.False(isExists);
        }

        /// <summary>
        /// The IsToDoItemExists_Exception_Failure.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public void IsToDoItemExists_Exception_Failure()
        {
            //Arrange
            var toDoItemProvider = new ToDoItemProvider(_mockUOW.Object, _mockIMapper);
            _mockUOW.Setup(result => result.ToDoItems.AddAsync(It.IsAny<Infrastructure.Entities.ToDoItem>()))
               .Returns(Task.FromResult(true));

            _mockUOW.Setup(result => result.ToDoItems.CheckItemExsists(It.IsAny<int>()))
                .Throws(new ArgumentNullException());

            int testItemId = 5;

            //Assert
            Assert.Throws<ArgumentNullException>(() => toDoItemProvider.CheckItem(testItemId));
        }

        /// <summary>
        /// The GetToDoItemsById_Success.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task GetToDoItemsById_Success()
        {
            //Arrange
            var toDoItemProvider = new ToDoItemProvider(_mockUOW.Object, _mockIMapper);
            _mockUOW.Setup(result => result.ToDoItems.AddAsync(It.IsAny<Infrastructure.Entities.ToDoItem>()))
               .Returns(Task.FromResult(true));

            _mockUOW.Setup(result => result.ToDoItems.GetByIdAsyncChange(It.IsAny<int>()))
                .Returns(Task.FromResult(new Infrastructure.Entities.ToDoItem
                {
                    UserId = toDoItemDetail.UserId,
                    ItemDescription = toDoItemDetail.ItemDescription,
                    CreatedDate = toDoItemDetail.CreatedDate,
                    ModifiedDate = toDoItemDetail.ModifiedDate,
                    IsCompleted = toDoItemDetail.IsCompleted
                }));

            await toDoItemProvider.AddToDoItem(toDoItemDetail);
            int testItemId = 1;

            //Act
            var response = await toDoItemProvider.GetToDoItemsByIdAsync(testItemId);

            //Assert
            Assert.IsType<ToDoItem>(response);
            Assert.Equal(toDoItemDetail.ItemDescription, response.ItemDescription);
        }


        /// <summary>
        /// The UpdateToDoItem_Success.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task UpdateToDoItem_Success()
        {
            //Arrange
            var toDoItemProvider = new ToDoItemProvider(_mockUOW.Object, _mockIMapper);

            _mockUOW.Setup(result => result.ToDoItems.AddAsync(It.IsAny<Infrastructure.Entities.ToDoItem>()))
               .Returns(Task.FromResult(true));

            await toDoItemProvider.AddToDoItem(toDoItemDetail);

            toDoItemDetail.ItemDescription = "Test Item Change";

            _mockUOW.Setup(result => result.ToDoItems.GetToDoItemsForUserAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<IEnumerable<Infrastructure.Entities.ToDoItem>>(
                    new List<Infrastructure.Entities.ToDoItem> {
                        new Infrastructure.Entities.ToDoItem
                        {
                            UserId = toDoItemDetail.UserId,
                            ItemDescription = toDoItemDetail.ItemDescription,
                            CreatedDate = toDoItemDetail.CreatedDate,
                            ModifiedDate = toDoItemDetail.ModifiedDate
                        }
                    }));

            int testItemId = 1;
            
            //Act
            await toDoItemProvider.UpdateToDoItem(toDoItemDetail);
            var response = await toDoItemProvider.GetToDoItemsAsync(testItemId);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(toDoItemDetail.ItemDescription, response.ElementAt(0).ItemDescription);
        }

        /// <summary>
        /// The RemoveToDoItem_Success.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task RemoveToDoItem_Success()
        {
            //Arrange
            var toDoItemProvider = new ToDoItemProvider(_mockUOW.Object, _mockIMapper);

            _mockUOW.Setup(result => result.ToDoItems.AddAsync(It.IsAny<Infrastructure.Entities.ToDoItem>()))
               .Returns(Task.FromResult(true));

            await toDoItemProvider.AddToDoItem(toDoItemDetail);

            _mockUOW.Setup(result => result.ToDoItems.GetToDoItemsForUserAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<IEnumerable<Infrastructure.Entities.ToDoItem>>(
                    new List<Infrastructure.Entities.ToDoItem>()));

            int testItemId = 1;

            //Act
            await toDoItemProvider.RemoveToDoItem(toDoItemDetail);
            var response = await toDoItemProvider.GetToDoItemsAsync(testItemId);

            //Assert
            Assert.NotNull(response);
            Assert.Empty(response);
        }
    }
}
