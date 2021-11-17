using System;
using System.Threading.Tasks;
using ToDoList.Infrastructure.Context;
using ToDoList.Infrastructure.Interfaces;
using ToDoList.Infrastructure.Repositories;

namespace ToDoList.Infrastructure.UOW
{
    /// <summary>
    /// Defines the <see cref="UnitOfWork" />.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Defines the _context.
        /// </summary>
        private readonly ToDoDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The context<see cref="ToDoDbContext"/>.</param>
        public UnitOfWork(ToDoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            ToDoItems = new ToDoItemRepository(_context);
            Users = new UserRepository(_context);
        }

        /// <summary>
        /// Gets the ToDoItems.
        /// </summary>
        public IToDoItemRepository ToDoItems { get; private set; }

        /// <summary>
        /// Gets the Users.
        /// </summary>
        public IUserRepository Users { get; private set; }


        /// <summary>
        /// The Save.
        /// </summary>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// The Complete.
        /// </summary>
        /// <returns>The <see cref="int"/>.</returns>
        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        /// <summary>
        /// Defines the disposed.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// The Dispose.
        /// </summary>
        /// <param name="disposing">The disposing<see cref="bool"/>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                _context.Dispose();
            }
            this.disposed = true;
        }

        /// <summary>
        /// The Dispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
