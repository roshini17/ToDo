using System.Linq;
using ToDoList.Infrastructure.Context;
using ToDoList.Infrastructure.Entities;
using ToDoList.Infrastructure.Interfaces;
using ToDoList.Infrastructure.UOW;

namespace ToDoList.Infrastructure.Repositories
{
    /// <summary>
    /// Defines the <see cref="UserRepository" />.
    /// </summary>
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The context<see cref="ToDoDbContext"/>.</param>
        public UserRepository(ToDoDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Get the user details based on the name
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="User"/>.</returns>
        public User GetUserDetailsByName(string name)
        {
            return _context.Users.FirstOrDefault(eachItem => eachItem.UserName == name);
        }

        /// <summary>
        /// Check whether the user details exists based on the id
        /// </summary>
        /// <param name="userId">The name<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool CheckUserExists(int userId)
        {
            return _context.Users.Any(eachItem => eachItem.Id == userId);
        }
    }
}
