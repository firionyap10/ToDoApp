using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TodoApp.API.Entities.Commons.Operations;
using TodoApp.API.Entities.Commons.Repositories;
using TodoApp.API.Entities.Enums;
using TodoApp.API.Entities.Todos;
using TodoApp.API.Entities.ToDos;
using TodoApp.API.Entities.ToDos.DTOs;

namespace TodoApp.API.Data.ToDos
{
    public class ToDoRespository : GenericRepository<ToDo>, IToDoRespository
    {
        private readonly DataContext _dbContext;
        public readonly ILogger<ToDoRespository> _logger;

        public ToDoRespository(DataContext dbContext, ILogger<ToDoRespository> logger) : base(dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<PagedList<ToDo>> ListAsync(ListToDoRequest request)
        {
            var result = _dbContext.ToDos.Include(x => x.Tags).AsQueryable();

            if (!string.IsNullOrEmpty(request.Name))
                result = result.Where(x => x.Name.Contains(request.Name));

            if (request.Status != null)
                result = result.Where(x => request.Status.Contains(x.Status));

            if (request.DueDateStart.HasValue)
                result = result.Where(x => x.DueDate >= request.DueDateStart);

            if (request.DueDateEnd.HasValue)
                result = result.Where(x => x.DueDate <= request.DueDateEnd);

            Expression<Func<ToDo, object>> orderByExpression = request.OrderByName switch
            {
                "DueDate" => x => x.DueDate,
                "Status" => x => x.Status,
                "Name" => x => x.Name,
                _ => x => x.CreatedDate
            };

            result = request.OrderBy == BaseListOrderBy.Descending
                ? result.OrderByDescending(orderByExpression)
                : result.OrderBy(orderByExpression);

            return await PagedList<ToDo>.ListAsync(result,
                            request.PageNumber, request.PageSize);
        }

        public async Task<ToDo> GetAsync(int id)
        {
            return await _dbContext.ToDos.Include(x => x.Tags).FirstOrDefaultAsync(y => y.Id == id);
        }

        public async Task<bool> UpdateAsync(ToDo toDo)
        {
            try
            {
                toDo.UpdatedDate = DateTime.UtcNow;

                // Ensure the toDo.Tags is not null
                if (toDo.Tags == null)
                {
                    toDo.Tags = new List<Tag>();
                }

                // Load the existing ToDo from the database
                var existingToDo = await _dbContext.ToDos.Include(t => t.Tags).SingleAsync(t => t.Id == toDo.Id);

                // Update ToDo properties
                _dbContext.Entry(existingToDo).CurrentValues.SetValues(toDo);

                foreach (var updatedTag in toDo.Tags)
                {
                    // Ensure you're only matching against valid Ids to avoid the "Sequence contains more than one matching element" error
                    var existingTag = updatedTag.Id != 0
                        ? existingToDo.Tags.SingleOrDefault(t => t.Id == updatedTag.Id)
                        : null;

                    if (existingTag != null)
                    {
                        _dbContext.Entry(existingTag).CurrentValues.SetValues(updatedTag);
                    }
                    else
                    {
                        updatedTag.ToDoId = toDo.Id;
                        existingToDo.Tags.Add(updatedTag);
                    }
                }

                foreach (var existingTag in existingToDo.Tags.ToList())
                {
                    if (!toDo.Tags.Any(t => t.Id == existingTag.Id))
                    {
                        existingToDo.Tags.Remove(existingTag);
                    }
                }

                var result = await _dbContext.SaveChangesAsync() > 0;

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
