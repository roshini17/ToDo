using ToDoList.Infrastructure.Entities;
using ToDoList.Infrastructure.UOW;

namespace ToDoList.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines the <see cref="IUserRepository" />.
    /// </summary>
    public interface IUserRepository : IGenericRepository<User>
    {
        /// <summary>
        /// Get the user details based on the name
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="User"/>.</returns>
        User GetUserDetailsByName(string name);

        /// <summary>
        /// Check whether the user details exists based on the id
        /// </summary>
        /// <param name="userId">The name<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool CheckUserExists(int userId);
    }
}
