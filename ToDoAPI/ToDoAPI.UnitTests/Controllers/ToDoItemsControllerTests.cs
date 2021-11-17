using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoAPI.Controllers;
using ToDoAPI.ResponseModels;
using ToDoList.Domain.Interfaces;
using ToDoList.Domain.Models;
using Xunit;

namespace ToDoAPI.UnitTests.Controllers
{
    public class ToDoItemsControllerTests
    {
        /// <summary>
        /// Defines the mockToDoItemProvider.
        /// </summary>
        private readonly Mock<IToDoItemProvider> mockToDoItemProvider = new Mock<IToDoItemProvider>();

        /// <summary>
        /// Defines the mockUserProvider.
        /// </summary>
        private readonly Mock<IUserProvider> mockUserProvider = new Mock<IUserProvider>();

        private static readonly ToDoItem toDoItemDetail = new ToDoItem()
        {
            Id = 1,
            UserId = 1,
            ItemDescription = "Test Item",
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        /// <summary>
        /// The Constructor_ToDoItemProviderIsNull_ThrowsArgumentNullException.
        /// </summary>
        [Fact]
        public void Constructor_ToDoItemProviderIsNull_ThrowsArgumentNullException()
        {
            // assert
            Assert.Throws<ArgumentNullException>(() => new ToDoItemsController(null, mockUserProvider.Object));
        }

        /// <summary>
        /// The Constructor_UserProviderIsNull_ThrowsArgumentNullException.
        /// </summary>
        [Fact]
        public void Constructor_UserProviderIsNull_ThrowsArgumentNullException()
        {
            // assert
            Assert.Throws<ArgumentNullException>(() => new ToDoItemsController(mockToDoItemProvider.Object, null));
        }

        /// <summary>
        /// The GetToDoItems_WhenUserIsNull_Failure.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task GetToDoItems_WhenUserIsNull_Failure()
        {
            // Arrange
            var toDoItemsController = new ToDoItemsController(mockToDoItemProvider.Object, mockUserProvider.Object);

            // Act 
            var response = await toDoItemsController.GetToDoItems();

            // Assert
            Assert.NotNull(response);
            Assert.Equal(500, ((ObjectResult)response.Result).StatusCode);
        }

        /// <summary>
        /// The GetToDoItems_Success.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task GetToDoItems_Success()
        {
            // Arrange
            var toDoItemsController = new ToDoItemsController(mockToDoItemProvider.Object, mockUserProvider.Object);
            int testUserId = 1;

            var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                                new Claim(ClaimTypes.Name, testUserId.ToString()),
                        }, "TestAuthentication"));

            toDoItemsController.ControllerContext.HttpContext = new DefaultHttpContext();
            toDoItemsController.ControllerContext.HttpContext.User = principal;

            mockToDoItemProvider.Setup(result => result.GetToDoItemsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<IEnumerable<ToDoItem>>(
                    new List<ToDoItem> {
                        toDoItemDetail
                    }));

            // Act 
            var response = await toDoItemsController.GetToDoItems();
            var responseResult = (ObjectResult)response.Result;

            // Assert
            Assert.NotNull(response);
            Assert.Equal(200, responseResult.StatusCode);
            Assert.Contains(toDoItemDetail, (IEnumerable<ToDoItem>)responseResult.Value);

        }

        /// <summary>
        /// The GetToDoItems_WhenUserIdInvalid_Failure.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task GetToDoItems_WhenUserIdInvalid_Failure()
        {
            // Arrange
            var toDoItemsController = new ToDoItemsController(mockToDoItemProvider.Object, mockUserProvider.Object);

            int testUserId = 0;

            var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                                new Claim(ClaimTypes.Name, testUserId.ToString()),
                        }, "TestAuthentication"));

            toDoItemsController.ControllerContext.HttpContext = new DefaultHttpContext();
            toDoItemsController.ControllerContext.HttpContext.User = principal;

            // Act 
            var response = await toDoItemsController.GetToDoItems();

            // Assert
            Assert.NotNull(response);
            Assert.Equal(500, ((ObjectResult)response.Result).StatusCode);

        }

        /// <summary>
        /// The PostToDoItems_WhenUserIsNull_Failure.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task PostToDoItems_WhenUserIsNull_Failure()
        {
            // Arrange
            var toDoItemsController = new ToDoItemsController(mockToDoItemProvider.Object, mockUserProvider.Object);

            // Act 
            var response = await toDoItemsController.PostToDoItemModel(toDoItemDetail);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(500, ((ObjectResult)response.Result).StatusCode);
        }

        /// <summary>
        /// The PostToDoItems_WhenUserDetailsNotExists_Failure.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task PostToDoItems_WhenUserDetailsNotExists_Failure()
        {
            // Arrange
            var toDoItemsController = new ToDoItemsController(mockToDoItemProvider.Object, mockUserProvider.Object);
            int testUserId = 1;

            var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                                new Claim(ClaimTypes.Name, testUserId.ToString()),
                        }, "TestAuthentication"));

            toDoItemsController.ControllerContext.HttpContext = new DefaultHttpContext();
            toDoItemsController.ControllerContext.HttpContext.User = principal;

            mockToDoItemProvider.Setup(result => result.GetToDoItemsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<IEnumerable<ToDoItem>>(
                    new List<ToDoItem> {
                        toDoItemDetail
                    }));

            // Act 
            var response = await toDoItemsController.PostToDoItemModel(toDoItemDetail);
            var responseResult = (ObjectResult)response.Result;

            // Assert
            Assert.NotNull(response);
            Assert.Equal(500, responseResult.StatusCode);
        }

        /// <summary>
        /// The PostToDoItems_Success.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task PostToDoItems_Success()
        {
            // Arrange
            var toDoItemsController = new ToDoItemsController(mockToDoItemProvider.Object, mockUserProvider.Object);
            int testUserId = 1;

            var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                                new Claim(ClaimTypes.Name, testUserId.ToString()),
                        }, "TestAuthentication"));

            toDoItemsController.ControllerContext.HttpContext = new DefaultHttpContext();
            toDoItemsController.ControllerContext.HttpContext.User = principal;

            mockUserProvider.Setup(result => result.CheckItem(It.IsAny<int>()))
                .Returns(true);

            mockToDoItemProvider.Setup(result => result.GetToDoItemsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<IEnumerable<ToDoItem>>(
                    new List<ToDoItem> {
                        toDoItemDetail
                    }));

            // Act 
            var response = await toDoItemsController.PostToDoItemModel(toDoItemDetail);
            var responseResult = (ObjectResult)response.Result;

            // Assert
            Assert.NotNull(response);
            Assert.Equal(201, responseResult.StatusCode);
            Assert.Equal(toDoItemDetail, responseResult.Value);

        }

        /// <summary>
        /// The PostToDoItems_WhenUserIdInvalid_Failure.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task PostToDoItems_WhenUserIdInvalid_Failure()
        {
            // Arrange
            var toDoItemsController = new ToDoItemsController(mockToDoItemProvider.Object, mockUserProvider.Object);

            int testUserId = 0;

            var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                                new Claim(ClaimTypes.Name, testUserId.ToString()),
                        }, "TestAuthentication"));

            toDoItemsController.ControllerContext.HttpContext = new DefaultHttpContext();
            toDoItemsController.ControllerContext.HttpContext.User = principal;

            // Act 
            var response = await toDoItemsController.PostToDoItemModel(toDoItemDetail);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(500, ((ObjectResult)response.Result).StatusCode);

        }

        /// <summary>
        /// The DeleteToDoItems_WhenToDoItemNotFound_Failure.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task DeleteToDoItems_WhenToDoItemNotFound_Failure()
        {
            // Arrange
            var toDoItemsController = new ToDoItemsController(mockToDoItemProvider.Object, mockUserProvider.Object);
            int testToDoItemId = 1;
            mockToDoItemProvider.Setup(result => result.GetToDoItemsByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<ToDoItem>(null));

            // Act 
            var response = await toDoItemsController.DeleteToDoItemModel(testToDoItemId);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(404, ((NotFoundResult)response).StatusCode);
        }

        /// <summary>
        /// The DeleteToDoItems_Success.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task DeleteToDoItems_Success()
        {
            // Arrange
            var toDoItemsController = new ToDoItemsController(mockToDoItemProvider.Object, mockUserProvider.Object);
            int testUserId = 1;

            var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                                new Claim(ClaimTypes.Name, testUserId.ToString()),
                        }, "TestAuthentication"));

            toDoItemsController.ControllerContext.HttpContext = new DefaultHttpContext();
            toDoItemsController.ControllerContext.HttpContext.User = principal;

            mockToDoItemProvider.Setup(result => result.GetToDoItemsByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(toDoItemDetail));

            // Act 
            var response = await toDoItemsController.DeleteToDoItemModel(testUserId);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(204, ((NoContentResult)response).StatusCode);
        }

        /// <summary>
        /// The DeleteToDoItems_Failure.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task DeleteToDoItems_Failure()
        {
            // Arrange
            var toDoItemsController = new ToDoItemsController(mockToDoItemProvider.Object, mockUserProvider.Object);
            int testUserId = 1;
            string expectedResult = "Error in deleting the todo items for the user";

            var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                                new Claim(ClaimTypes.Name, testUserId.ToString()),
                        }, "TestAuthentication"));

            toDoItemsController.ControllerContext.HttpContext = new DefaultHttpContext();
            toDoItemsController.ControllerContext.HttpContext.User = principal;

            mockToDoItemProvider.Setup(result => result.GetToDoItemsByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromException<ToDoItem>(new ArgumentNullException()));

            // Act 
            var response = await toDoItemsController.DeleteToDoItemModel(testUserId);
            var resultResponse = ((ObjectResult)response);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(500, resultResponse.StatusCode);
            Assert.Equal(expectedResult, ((Response)resultResponse.Value).Message);
        }

        /// <summary>
        /// The PutToDoItemModel_WhenToDoItemNotFound_Failure.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task PutToDoItemModel_WhenToDoItemIdNotMatch_Failure()
        {
            // Arrange
            var toDoItemsController = new ToDoItemsController(mockToDoItemProvider.Object, mockUserProvider.Object);
            int testToDoItemId = 3;
            mockToDoItemProvider.Setup(result => result.GetToDoItemsByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<ToDoItem>(null));

            // Act 
            var response = await toDoItemsController.PutToDoItemModel(testToDoItemId, toDoItemDetail);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(400, ((BadRequestResult)response).StatusCode);
        }

        /// <summary>
        /// The PutToDoItemModel_Success.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task PutToDoItemModel_Success()
        {
            // Arrange
            var toDoItemsController = new ToDoItemsController(mockToDoItemProvider.Object, mockUserProvider.Object);
            int testUserId = 1;

            // Act 
            var response = await toDoItemsController.PutToDoItemModel(testUserId, toDoItemDetail);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(204, ((NoContentResult)response).StatusCode);
        }

        /// <summary>
        /// The PutToDoItems_Failure.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task PutToDoItems_Failure()
        {
            // Arrange
            var toDoItemsController = new ToDoItemsController(mockToDoItemProvider.Object, mockUserProvider.Object);
            int testUserId = 1;
            string expectedResult = "Error in updating the todo items for the user";

            
            mockToDoItemProvider.Setup(result => result.UpdateToDoItem(It.IsAny<ToDoItem>()))
                .Returns(Task.FromException<ToDoItem>(new ArgumentNullException()));

            // Act 
            var response = await toDoItemsController.PutToDoItemModel(testUserId, toDoItemDetail);
            var resultResponse = ((ObjectResult)response);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(500, resultResponse.StatusCode);
            Assert.Equal(expectedResult, ((Response)resultResponse.Value).Message);
        }

        /// <summary>
        /// The PutToDoItems_Concurrency_itemExists_Failure.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task PutToDoItems_Concurrency_itemExists_Failure()
        {
            // Arrange
            var toDoItemsController = new ToDoItemsController(mockToDoItemProvider.Object, mockUserProvider.Object);
            int testUserId = 1;

            mockToDoItemProvider.Setup(result => result.CheckItem(It.IsAny<int>()))
                .Returns(true);

            mockToDoItemProvider.Setup(result => result.UpdateToDoItem(It.IsAny<ToDoItem>()))
                .Returns(Task.FromException<ToDoItem>(new DbUpdateConcurrencyException()));


            // assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => toDoItemsController.PutToDoItemModel(testUserId, toDoItemDetail));
        }

        /// <summary>
        /// The PutToDoItems_Concurrency_itemNotExists_Failure.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task PutToDoItems_Concurrency_itemNotExists_Failure()
        {
            // Arrange
            var toDoItemsController = new ToDoItemsController(mockToDoItemProvider.Object, mockUserProvider.Object);
            int testUserId = 1;


            mockToDoItemProvider.Setup(result => result.UpdateToDoItem(It.IsAny<ToDoItem>()))
                .Returns(Task.FromException<ToDoItem>(new DbUpdateConcurrencyException()));

            mockToDoItemProvider.Setup(result => result.CheckItem(It.IsAny<int>()))
                .Returns(false);

            // Act 
            var response = await toDoItemsController.PutToDoItemModel(testUserId, toDoItemDetail);
            var resultResponse = ((NotFoundResult)response);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(404, resultResponse.StatusCode);
        }

            
        /// <summary>
        /// The GetToDoItems_WhenToDoItemNotExists_Failure.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task GetToDoItems_WhenToDoItemNotExists_Failure()
        {
            // Arrange
            var toDoItemsController = new ToDoItemsController(mockToDoItemProvider.Object, mockUserProvider.Object);
            int testToDoItemId = 3;
            mockToDoItemProvider.Setup(result => result.GetToDoItemsByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<ToDoItem>(null));

            // Act 
            var response = await toDoItemsController.GetToDoItemModel(testToDoItemId);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(404, ((NotFoundResult)response.Result).StatusCode);
        }

        /// <summary>
        /// The GetToDoItemOnId_Success.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task GetToDoItemOnId_Success()
        {
            // Arrange
            var toDoItemsController = new ToDoItemsController(mockToDoItemProvider.Object, mockUserProvider.Object);
            int testUserId = 1;
            mockToDoItemProvider.Setup(result => result.GetToDoItemsByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(toDoItemDetail));

            // Act 
            var response = await toDoItemsController.GetToDoItemModel(testUserId);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(toDoItemDetail, response.Value);
        }

        /// <summary>
        /// The GetToDoItems_Exception_Failure.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task GetToDoItems_Exception_Failure()
        {
            // Arrange
            var toDoItemsController = new ToDoItemsController(mockToDoItemProvider.Object, mockUserProvider.Object);
            int testUserId = 1;
            string expectedResult = "Error in getting the todo items for the user";


            mockToDoItemProvider.Setup(result => result.GetToDoItemsByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromException<ToDoItem>(new ArgumentNullException()));

            // Act 
            var response = await toDoItemsController.GetToDoItemModel(testUserId);
            var resultResponse = (ObjectResult)response.Result;

            // Assert
            Assert.NotNull(response);
            Assert.Equal(500, resultResponse.StatusCode);
            Assert.Equal(expectedResult, ((Response)resultResponse.Value).Message);
        }
    }
}
