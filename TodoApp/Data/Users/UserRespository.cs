using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TodoApp.API.Entities.Commons.Operations;
using TodoApp.API.Entities.Commons.Repositories;
using TodoApp.API.Entities.Enums;
using TodoApp.API.Entities.Users;
using TodoApp.API.Entities.Users.DTOs;

namespace TodoApp.API.Data.Users
{
    public class UserRespository : GenericRepository<User>, IUserRespository
    {
        private readonly DataContext _dbContext;
        public readonly ILogger<UserRespository> _logger;

        public UserRespository(DataContext dbContext, ILogger<UserRespository> logger) : base(dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<PagedList<User>> ListAsync(ListUserRequest request)
        {
            var result = _dbContext.Users.AsQueryable();

            if (!string.IsNullOrEmpty(request.Name))
                result = result.Where(x => x.FirstName.Contains(request.Name) || x.LastName.Contains(request.Name));

            Expression<Func<User, object>> orderByExpression = request.OrderByName switch
            {
                "Name" => x => x.FirstName + " " + x.LastName,
                _ => x => x.CreatedDate
            };

            result = request.OrderBy == BaseListOrderBy.Descending
                ? result.OrderByDescending(orderByExpression)
                : result.OrderBy(orderByExpression);

            return await PagedList<User>.ListAsync(result,
                            request.PageNumber, request.PageSize);
        }

        public async Task<User> GetAsync(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(y => y.Id == id);
        }

        public async Task<bool> UpdateAsync(User user)
        {
            try
            {
                var existingUser = await _dbContext.Users.SingleAsync(t => t.Id == user.Id);

                // Update User properties
                _dbContext.Entry(existingUser).CurrentValues.SetValues(user);

                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
