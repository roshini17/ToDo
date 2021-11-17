using System.Collections.Generic;

namespace ToDoList.Infrastructure.Entities
{
    /// <summary>
    /// User Entity.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Username.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user email Id.
        /// </summary>
        public string EmailId { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the List of ToDoItems mapped to a user.
        /// </summary>
        public virtual ICollection<ToDoItem> ToDoItems { get; set; }
    }
}
