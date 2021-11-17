using System;
using System.Threading.Tasks;
using ToDoList.Infrastructure.Interfaces;

namespace ToDoList.Infrastructure.UOW
{
    /// <summary>
    /// Defines the <see cref="IUnitOfWork" />.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the ToDoItems.
        /// </summary>
        IToDoItemRepository ToDoItems { get; }

        /// <summary>
        /// Gets the Users.
        /// </summary>
        IUserRepository Users { get; }

        /// <summary>
        /// The SaveAsync.
        /// </summary>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        Task<bool> SaveAsync();

        /// <summary>
        /// The Save.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        bool Save();
    }
}
