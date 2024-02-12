using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        public UserService(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<User> GetUser()
        {
            return await _dbContext
                .Users
                .AsNoTracking()
                .Include(x => x.Orders)
                .OrderByDescending(x => x.Orders.Count)
                .Select(x=> new User()
                {
                    Id = x.Id,
                    Email  = x.Email,
                    Status = x.Status,
                })
                .FirstAsync();
        }

        public async Task<List<User>> GetUsers()
        {
            return await _dbContext
                .Users
                .AsNoTracking()
                .Where(u=>u.Status==Enums.UserStatus.Inactive)
                .ToListAsync();
        }
    }
}
