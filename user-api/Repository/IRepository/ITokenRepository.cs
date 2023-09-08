using user_api.Models;

namespace user_api.Repository.IRepository
{
    public interface ITokenRepository
    {
        public string GetToken(User user);
    }
}
