using System;
using System.Threading.Tasks;
using ToDoList.Infrastructure.Context;

namespace ToDoList.Infrastructure.UOW
{
    /// <summary>
    /// Defines the <see cref="GenericRepository{T}" />.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Defines the _context.
        /// </summary>
        protected readonly ToDoDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository{T}"/> class.
        /// </summary>
        /// <param name="context">The context<see cref="ToDoDbContext"/>.</param>
        protected GenericRepository(ToDoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// The AddAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="T"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        /// <summary>
        /// The Update.
        /// </summary>
        /// <param name="entity">The entity<see cref="T"/>.</param>
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        /// <summary>
        /// The Remove.
        /// </summary>
        /// <param name="entity">The entity<see cref="T"/>.</param>
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}
