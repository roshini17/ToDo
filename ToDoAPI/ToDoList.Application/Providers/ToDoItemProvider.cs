using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ToDoList.Domain.Interfaces;
using ToDoList.Domain.Models;
using ToDoList.Infrastructure.UOW;

namespace ToDoList.Domain.Providers
{
    /// <summary>
    /// Class for toDoItemProvider.
    /// </summary>
    public class ToDoItemProvider : IToDoItemProvider
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
        /// Initializes a new instance of the <see cref="ToDoItemProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unitOfWork<see cref="IUnitOfWork"/>.</param>
        /// <param name="mapper">The mapper<see cref="IMapper"/>.</param>
        public ToDoItemProvider(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Create/Add ToDoItem
        /// </summary>
        /// <param name="itemDetail">The itemDetail<see cref="ToDoItem"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task AddToDoItem(ToDoItem itemDetail)
        {
            itemDetail.CreatedDate = itemDetail.ModifiedDate = DateTime.Now;

            Infrastructure.Entities.ToDoItem toDoItems = MapToEntity(itemDetail);
            await _unitOfWork.ToDoItems.AddAsync(toDoItems);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Check whether a ToDoItem exists based on its id
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool CheckItem(int id)
        {
            return _unitOfWork.ToDoItems.CheckItemExsists(id);
        }

        /// <summary>
        /// Get the ToDoItem detail based on the item Id.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{ToDoItem}"/>.</returns>
        public async Task<ToDoItem> GetToDoItemsByIdAsync(int id)
        {
            var response = await _unitOfWork.ToDoItems.GetByIdAsyncChange(id);
            return _mapper.Map<ToDoItem>(response);
        }

        /// <summary>
        /// Get the list of ToDoItems based on the UserId.
        /// </summary>
        /// <param name="userId">The userId<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{ToDoItem}}"/>.</returns>
        public async Task<IEnumerable<ToDoItem>> GetToDoItemsAsync(int userId)
        {
            var response = await _unitOfWork.ToDoItems.GetToDoItemsForUserAsync(userId);
            return _mapper.Map<IEnumerable<ToDoItem>>(response);
        }

        /// <summary>
        /// Update the ToDoItem
        /// </summary>
        /// <param name="itemDetail">The itemDetail<see cref="ToDoItem"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task UpdateToDoItem(ToDoItem itemDetail)
        {
            itemDetail.ModifiedDate = DateTime.Now;
            Infrastructure.Entities.ToDoItem toDoItems = MapToEntity(itemDetail);
            _unitOfWork.ToDoItems.Update(toDoItems);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Remove/Delete the ToDoItem
        /// </summary>
        /// <param name="itemDetail">The itemDetail<see cref="ToDoItem"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task RemoveToDoItem(ToDoItem itemDetail)
        {
            Infrastructure.Entities.ToDoItem toDoItems = MapToEntity(itemDetail);

            _unitOfWork.ToDoItems.Remove(toDoItems);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Map the model to entities.
        /// </summary>
        /// <param name="itemDetail">The itemDetail<see cref="ToDoItem"/>.</param>
        /// <returns>The <see cref="Infrastructure.Entities.ToDoItem"/>.</returns>
        private static Infrastructure.Entities.ToDoItem MapToEntity(ToDoItem itemDetail)
        {
            return new Infrastructure.Entities.ToDoItem
            {
                Id = itemDetail.Id,
                ItemDescription = itemDetail.ItemDescription,
                IsCompleted = itemDetail.IsCompleted,
                CreatedDate = itemDetail.CreatedDate,
                ModifiedDate = itemDetail.ModifiedDate,
                UserId = itemDetail.UserId
            };
        }
    }
}
