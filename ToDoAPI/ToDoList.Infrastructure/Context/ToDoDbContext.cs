using Microsoft.EntityFrameworkCore;
using ToDoList.Infrastructure.Entities;
using ToDoList.Infrastructure.EntityConfigurations;

namespace ToDoList.Infrastructure.Context
{
    /// <summary>
    /// Defines the <see cref="ToDoDbContext" />.
    /// </summary>
    public class ToDoDbContext : DbContext
    {
        // <summary>
        /// Initializes a new instance of the <see cref="ToDoDbContext"/> class.
        /// </summary>
        /// <param name="options">The options<see cref="DbContextOptions{ToDoDbContext}"/>.</param>
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// Gets or sets the Users.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the ToDoItems.
        /// </summary>
        public DbSet<ToDoItem> ToDoItems { get; set; }

        /// <summary>
        /// The OnModelCreating.
        /// </summary>
        /// <param name="modelBuilder">The modelBuilder<see cref="ModelBuilder"/>.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new ToDoListConfig());
        }

    }
}
