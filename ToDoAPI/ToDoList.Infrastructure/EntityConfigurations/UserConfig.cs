using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoList.Infrastructure.Entities;

namespace ToDoList.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Defines the <see cref="UserConfig" />.
    /// </summary>
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        /// <summary>
        /// The Configure.
        /// </summary>
        /// <param name="builder">The builder<see cref="EntityTypeBuilder{User}"/>.</param>
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "dbo");

            builder.HasKey(s => s.Id);
        }
    }
}
