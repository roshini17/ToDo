using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ToDoAPI.Middleware;
using ToDoList.Domain;
using ToDoList.Infrastructure;
using ToDoList.Infrastructure.Context;

namespace ToDoAPI
{
    /// <summary>
    /// Defines the <see cref="Startup" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/>.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the Configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// The ConfigureServices.
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/>.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure();

            services.AddDbContext<ToDoDbContext>(options => options.UseInMemoryDatabase("ToDoDB"));

            services.AddJwtBearerAuthentication(Configuration["JWT:ValidAudience"], Configuration["JWT:ValidIssuer"],
                Configuration["JWT:Secret"]);

            services.AddDomain();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed(x => true)
                        .AllowCredentials()
                        .WithExposedHeaders();
                });
            });

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ToDoAPI", Version = "v1" });
            });

            
        }

        /// <summary>
        /// The Configure.
        /// </summary>
        /// <param name="app">The app<see cref="IApplicationBuilder"/>.</param>
        /// <param name="env">The env<see cref="IWebHostEnvironment"/>.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDoAPI v1"));
            }

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                var myDbContext = services.GetService<ToDoDbContext>();
                AddTestData(myDbContext);
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Temporary Data for the inmemory database
        /// </summary>
        /// <param name="context">.</param>
        private static void AddTestData(ToDoDbContext context)
        {
            var firstTestUser = new ToDoList.Infrastructure.Entities.User
            {
                Id = 1,
                UserName = "Sample",
                Password = "Test@123"
            };

            var secondTestUser = new ToDoList.Infrastructure.Entities.User
            {
                Id = 2,
                UserName = "Sampletest",
                Password = "Test@123"
            };

            context.Users.Add(firstTestUser);
            context.Users.Add(secondTestUser);

            var firstTestToDoItemOne = new ToDoList.Infrastructure.Entities.ToDoItem
            {
                Id = 1,
                UserId = firstTestUser.Id,
                ItemDescription = "Design system",
                IsCompleted = false,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            var firstTestToDoItemTwo = new ToDoList.Infrastructure.Entities.ToDoItem
            {
                Id = 2,
                UserId = firstTestUser.Id,
                ItemDescription = "Implement system",
                IsCompleted = false,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };      

            var secondTestToDoItemOne = new ToDoList.Infrastructure.Entities.ToDoItem
            {
                Id = 3,
                UserId = secondTestUser.Id,
                ItemDescription = "purchase",
                IsCompleted = false,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            var secondTestToDoItemTwo = new ToDoList.Infrastructure.Entities.ToDoItem
            {
                Id = 4,
                UserId = secondTestUser.Id,
                ItemDescription = "Design system",
                IsCompleted = false,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            context.ToDoItems.Add(firstTestToDoItemOne);
            context.ToDoItems.Add(firstTestToDoItemTwo);
            context.ToDoItems.Add(secondTestToDoItemOne);
            context.ToDoItems.Add(secondTestToDoItemTwo);

            context.SaveChanges();
        }
    }
}
