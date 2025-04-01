using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using P7CreateRestApi.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace P7CreateRestApi.Repositories
{
    public class JwtAuthenticationRepository : IJwtAuthenticationRepository
    {
        private readonly List<User> Users = new List<User>()
        {
            new User
            {
                Id = "1",
                UserName  = "Test",
                Email = "yvan@gmail.com",
                Password = "123"
            }
        };
        public User Authenticate(string email, string password)
        {
            return Users.Where(u => u.Email.ToUpper().Equals(email.ToUpper()) 
                && u.Password.Equals(password)).FirstOrDefault();
        }

        public string GenerateToken(string secret, List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
         }
    }
}
