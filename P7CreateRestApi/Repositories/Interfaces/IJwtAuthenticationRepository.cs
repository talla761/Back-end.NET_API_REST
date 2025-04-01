using Dot.Net.WebApi.Domain;
using System.Security.Claims;

namespace P7CreateRestApi.Repositories.Interfaces
{
    public interface IJwtAuthenticationRepository
    {

        User Authenticate(string email, string password);

        string GenerateToken(string secret, List<Claim> claims);
    }
}
