using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace P7CreateRestApi.Repositories.Interfaces
{
    public interface IJwtAuthenticationRepository
    {

        Task<IdentityUser> Authenticate(string email, string password);

        string GenerateToken(string secret, List<Claim> claims);
    }
}
