using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.ResponseModels;
using ToDoList.Domain.Interfaces;
using ToDoList.Domain.Models;

namespace ToDoAPI.Controllers
{
    /// <summary>
    /// Defines the <see cref="ToDoItemsController" />.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemsController : ControllerBase
    {
        /// <summary>
        /// Defines the _toDoItemProvider.
        /// </summary>
        private readonly IToDoItemProvider _toDoItemProvider;

        /// <summary>
        /// Defines the _userProvider.
        /// </summary>
        private readonly IUserProvider _userProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemsController"/> class.
        /// </summary>
        /// <param name="toDoItemProvider">The toDoItemProvider<see cref="IToDoItemProvider"/></param>
        public ToDoItemsController(IToDoItemProvider toDoItemProvider, IUserProvider userProvider)
        {
            _toDoItemProvider = toDoItemProvider ?? throw new ArgumentNullException(nameof(toDoItemProvider));
            _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
        }

        /// <summary>
        /// Get the list of ToDoItems for the logged in user.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetToDoItems()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);

                if (userId <= 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User details doesnt exists!" });
                };

                var response = await _toDoItemProvider.GetToDoItemsAsync(userId);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Error in getting the todo items for the user" });
            }
        }

        /// <summary>
        /// Get the ToDoItem detail based on the ToDoItem Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItem>> GetToDoItemModel(int id)
        {
            try
            {
                var toDoItemModel = await _toDoItemProvider.GetToDoItemsByIdAsync(id);

                if (toDoItemModel == null)
                {
                    return NotFound();
                }

                return toDoItemModel;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Error in getting the todo items for the user" });
            }
        }

        /// <summary>
        /// Update the ToDoItem.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="toDoItemModel"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoItemModel(int id, ToDoItem toDoItemModel)
        {
            try
            {
                if (id != toDoItemModel.Id)
                {
                    return BadRequest();
                }

                await _toDoItemProvider.UpdateToDoItem(toDoItemModel);
                return NoContent();
            }            
            catch (DbUpdateConcurrencyException)
            {
                if (!_toDoItemProvider.CheckItem(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Error in updating the todo items for the user" });
            }
        }

        /// <summary>
        /// Create/Add new ToDoItem.
        /// </summary>
        /// <param name="toDoItemModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ToDoItem>> PostToDoItemModel(ToDoItem toDoItemModel)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);

                if (userId <= 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User details doesnt exists!" });
                };

                if (_userProvider.CheckItem(userId))
                {
                    toDoItemModel.UserId = userId;
                    await _toDoItemProvider.AddToDoItem(toDoItemModel);

                    return CreatedAtAction("GetToDoItemModel", new { id = toDoItemModel.Id }, toDoItemModel);

                }
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User details doesnt exists!" });
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Error in creating the todo items for the user" });
            }
        }

        /// <summary>
        /// Delete a ToDoItem based on the ToDoItem Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItemModel(int id)
        {
            try
            {
                var toDoItemModel = await _toDoItemProvider.GetToDoItemsByIdAsync(id);
                if (toDoItemModel == null)
                {
                    return NotFound();
                }

                await _toDoItemProvider.RemoveToDoItem(toDoItemModel);

                return NoContent();
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Error in deleting the todo items for the user" });
            }
        }
    }
}
