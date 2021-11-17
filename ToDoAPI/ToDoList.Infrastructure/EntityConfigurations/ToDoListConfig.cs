using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoList.Infrastructure.Entities;

namespace ToDoList.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Defines the <see cref="ToDoListConfig" />.
    /// </summary>
    public class ToDoListConfig : IEntityTypeConfiguration<ToDoItem>
    {
        /// <summary>
        /// The Configure.
        /// </summary>
        /// <param name="builder">The builder<see cref="EntityTypeBuilder{ToDoItems}"/>.</param>
        public void Configure(EntityTypeBuilder<ToDoItem> builder)
        {
            builder.ToTable("ToDoItems", "dbo");

            builder.HasKey(s => s.Id);

            // builder.HasOne(s => s.User).WithMany(s => s.ToDoItems).HasForeignKey(x => x.UserId);
        }
    }
}
