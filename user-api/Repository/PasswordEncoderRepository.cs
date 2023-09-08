using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using user_api.Models;
using user_api.Repository.IRepository;

namespace user_api.Repository
{
    public class PasswordEncoderRepository : IPasswordEncoderRepository
    {
        public EncodedPassword GetPassword(string password)
        {
            EncodedPassword encodedPassword = new EncodedPassword();
            using (var hmac = new HMACSHA512())
            {
                encodedPassword.PasswordSalt = hmac.Key;
                encodedPassword.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            return encodedPassword;
        }

        public bool VerfiyPassword(string password , byte[] PasswordHash , byte[] PasswordSalt)
        {
            using (var hmac = new HMACSHA512(PasswordSalt))
            {
                byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(PasswordHash);
            }
        }
    }
}
