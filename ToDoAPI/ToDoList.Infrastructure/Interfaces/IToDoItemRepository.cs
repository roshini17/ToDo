using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.Infrastructure.Entities;
using ToDoList.Infrastructure.UOW;

namespace ToDoList.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines the <see cref="IToDoItemRepository" />.
    /// </summary>
    public interface IToDoItemRepository : IGenericRepository<ToDoItem>
    {
        /// <summary>
        /// Get the list of ToDoItems based on the UserId.
        /// </summary>
        /// <param name="userId">The userId<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{ToDoItem}}"/>.</returns>
        Task<IEnumerable<ToDoItem>> GetToDoItemsForUserAsync(int userId);

        /// <summary>
        /// Check whether a specific ToDoItem exists based on the item id.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool CheckItemExsists(int id);

        /// <summary>
        /// Get the ToDoItem detail based on the item Id.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{ToDoItem}"/>.</returns>
        Task<ToDoItem> GetByIdAsyncChange(int id);
    }
}
