namespace user_api.Models
{
    public class EncodedPassword
    {
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
    }
}
