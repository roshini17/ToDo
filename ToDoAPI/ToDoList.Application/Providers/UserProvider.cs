using AutoMapper;
using System;
using System.Threading.Tasks;
using ToDoList.Domain.Interfaces;
using ToDoList.Domain.Models;
using ToDoList.Infrastructure.UOW;

namespace ToDoList.Domain.Providers
{
    /// <summary>
    /// Defines the <see cref="IUserProvider" />.
    /// </summary>
    public class UserProvider : IUserProvider
    {
        /// <summary>
        /// Defines the _unitOfWork.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Defines the _mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unitOfWork<see cref="IUnitOfWork"/>.</param>
        /// <param name="mapper">The mapper<see cref="IMapper"/>.</param>
        public UserProvider(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Create/Add user
        /// </summary>
        /// <param name="userDetail">The userDetail<see cref="User"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task AddUser(User userDetail)
        {
            Infrastructure.Entities.User user = new Infrastructure.Entities.User()
            {
                EmailId = userDetail.EmailId,
                UserName = userDetail.UserName,
                Password = userDetail.Password
            };

            await _unitOfWork.Users.AddAsync(user);

            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Get the user details based on the name
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="User"/>.</returns>
        public User GetUserDetails(string name)
        {
            var userDetails = _unitOfWork.Users.GetUserDetailsByName(name);
            User userValue = _mapper.Map<User>(userDetails);
            return userValue;
        }

        /// <summary>
        /// Check whether a User exists based on its id
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool CheckItem(int id)
        {
            return _unitOfWork.Users.CheckUserExists(id);
        }
    }
}
