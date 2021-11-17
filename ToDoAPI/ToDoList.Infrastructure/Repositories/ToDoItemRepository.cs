using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoList.Infrastructure.Context;
using ToDoList.Infrastructure.Entities;
using ToDoList.Infrastructure.Interfaces;
using ToDoList.Infrastructure.UOW;

namespace ToDoList.Infrastructure.Repositories
{
    /// <summary>
    /// Defines the <see cref="ToDoItemRepository" />.
    /// </summary>
    public class ToDoItemRepository : GenericRepository<ToDoItem>, IToDoItemRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The context<see cref="ToDoDbContext"/>.</param>
        public ToDoItemRepository(ToDoDbContext context)
           : base(context)
        {

        }

        /// <summary>
        /// Get the list of ToDoItems based on the UserId.
        /// </summary>
        /// <param name="userId">The userId<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{ToDoItem}}"/>.</returns>
        public async Task<IEnumerable<ToDoItem>> GetToDoItemsForUserAsync(int userId)
        {
            return await _context.ToDoItems.AsNoTracking().Where(x => x.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// Get the ToDoItem detail based on the item Id.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{ToDoItem}"/>.</returns>
        public async Task<ToDoItem> GetByIdAsyncChange(int itemId)
        {
            return await _context.ToDoItems.AsNoTracking().Where(x => x.Id == itemId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Check whether a specific ToDoItem exists based on the item id.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool CheckItemExsists(int id)
        {
            return _context.ToDoItems.Any(item => item.Id == id);
        }
    }
}
