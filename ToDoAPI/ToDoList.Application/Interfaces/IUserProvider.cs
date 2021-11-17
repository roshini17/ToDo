using System.Threading.Tasks;
using ToDoList.Domain.Models;

namespace ToDoList.Domain.Interfaces
{
    /// <summary>
    /// Defines the <see cref="IUserProvider" />.
    /// </summary>
    public interface IUserProvider
    {
        /// <summary>
        /// Get the user details based on the name
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="User"/>.</returns>
        User GetUserDetails(string name);

        /// <summary>
        /// Create/Add user
        /// </summary>
        /// <param name="userDetail">The userDetail<see cref="User"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task AddUser(User userDetail);

        /// <summary>
        /// Check whether a User exists based on its id
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool CheckItem(int id);
    }
}
