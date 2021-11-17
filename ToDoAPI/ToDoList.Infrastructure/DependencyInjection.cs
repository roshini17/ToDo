using Microsoft.Extensions.DependencyInjection;
using ToDoList.Infrastructure.UOW;

namespace ToDoList.Infrastructure
{
    /// <summary>
    /// Defines the <see cref="DependencyInjection" />.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// The AddInfrastructure.
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
