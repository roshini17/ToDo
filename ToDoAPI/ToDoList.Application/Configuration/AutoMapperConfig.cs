using AutoMapper;
using ToDoList.Domain.Models;

namespace ToDoList.Domain.Configuration
{
    /// <summary>
    /// Defines the <see cref="AutoMapperConfig" />.
    /// </summary>
    public class AutoMapperConfig : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMapperConfig"/> class.
        /// </summary>
        public AutoMapperConfig()
        {
            CreateMap<ToDoItem, Infrastructure.Entities.ToDoItem>().ReverseMap();
            CreateMap<User, Infrastructure.Entities.User>().ReverseMap();
        }
    }
}
