using user_api.Models;

namespace user_api.Repository.IRepository
{
    public interface IUserRepository
    {
        public Task CreateAsync(User user);
        public Task UpdateAsync(User user);
        public Task<User> GetAsync (Guid id);
        public Task<User> VerifyUserAsync(string email);
    }
}
