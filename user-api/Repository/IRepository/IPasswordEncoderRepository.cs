using user_api.Models;

namespace user_api.Repository.IRepository
{
    public interface IPasswordEncoderRepository
    {
        public EncodedPassword GetPassword(string password);
        public bool VerfiyPassword(string password, byte[] PasswordHash, byte[] PasswordSalt);

    }
}
