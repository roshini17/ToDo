using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.Domain.Models;

namespace ToDoList.Domain.Interfaces
{
    /// <summary>
    /// Defines the <see cref="IToDoItemProvider" />.
    /// </summary>
    public interface IToDoItemProvider
    {
        /// <summary>
        /// Get the list of ToDoItems based on the UserId.
        /// </summary>
        /// <param name="userId">The userId<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{ToDoItem}}"/>.</returns>
        Task<IEnumerable<ToDoItem>> GetToDoItemsAsync(int userId);

        /// <summary>
        /// Check whether a specific ToDoItem exists based on the item id.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool CheckItem(int id);

        /// <summary>
        /// Get the ToDoItem detail based on the item Id.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{ToDoItem}"/>.</returns>
        Task<ToDoItem> GetToDoItemsByIdAsync(int id);

        /// <summary>
        /// Update the ToDoItem
        /// </summary>
        /// <param name="itemDetail">The itemDetail<see cref="ToDoItem"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task UpdateToDoItem(ToDoItem itemDetail);

        /// <summary>
        /// Create/Add the ToDoItem
        /// </summary>
        /// <param name="itemDetail">The itemDetail<see cref="ToDoItem"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task AddToDoItem(ToDoItem itemDetail);

        /// <summary>
        /// Remove/Delete the ToDoItem
        /// </summary>
        /// <param name="itemDetail">The itemDetail<see cref="ToDoItem"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task RemoveToDoItem(ToDoItem itemDetail);
    }
}
