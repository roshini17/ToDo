using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using ToDoList.Domain.Interfaces;
using ToDoList.Domain.Providers;

namespace ToDoList.Domain
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
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient<IUserProvider, UserProvider>();
            services.AddTransient<IToDoItemProvider, ToDoItemProvider>();
            return services;
        }
    }
}
