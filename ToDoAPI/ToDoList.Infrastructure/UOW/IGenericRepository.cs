using System.Threading.Tasks;

namespace ToDoList.Infrastructure.UOW
{
    /// <summary>
    /// Defines the <see cref="IGenericRepository{T}" />.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// The AddAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="T"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task AddAsync(T entity);

        /// <summary>
        /// The Update.
        /// </summary>
        /// <param name="entity">The entity<see cref="T"/>.</param>
        void Update(T entity);

        /// <summary>
        /// The Remove.
        /// </summary>
        /// <param name="entity">The entity<see cref="T"/>.</param>
        void Remove(T entity);
    }
}
