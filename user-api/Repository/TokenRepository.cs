using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using user_api.Models;
using user_api.Repository.IRepository;

namespace user_api.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration _configuration;

        public TokenRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetToken(User user)
        {
            List<Claim> claims = new List<Claim>();
            Claim email = new Claim(ClaimTypes.Name , user.Email!);
            Claim id = new Claim(ClaimTypes.Sid, user.Id.ToString()!);
            claims.Add(email);
            claims.Add(id);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
