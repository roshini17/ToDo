using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Infrastructure.Entities
{

    /// <summary>
    /// ToDoItem Entity
    /// </summary>
    public class ToDoItem
    {
        /// <summary>
        /// Gets or sets the toDoItem id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user id to be mapped to a toDoItem.
        /// </summary>
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the ToDoItem Description.
        /// </summary>
        public string ItemDescription { get; set; }

        /// <summary>
        /// Gets or sets the status of the ToDoItem.
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Gets or sets the created date of the ToDoItem.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the updated date of the ToDoItem.
        /// </summary>
        public DateTime ModifiedDate { get; set; }
    }
}
