using Microsoft.EntityFrameworkCore;
using user_api.Data;
using user_api.Models;
using user_api.Repository.IRepository;

namespace user_api.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(User user)
        {
            try
            {
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();   
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<User> GetAsync(Guid id)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(x=>x.Id == id);
            return user!;
        }

        public async Task UpdateAsync(User user)
        {
            try
            {
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<User> VerifyUserAsync(string email)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email!.ToLower() == email.ToLower());
            return user!;
        }
    }
}
