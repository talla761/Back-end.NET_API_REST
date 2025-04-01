using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using P7CreateRestApi.Models;
using P7CreateRestApi.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dot.Net.WebApi.Controllers
{
    public class LoginController : ControllerBase
    {
        private readonly IJwtAuthenticationRepository _jwtAuthenticationRepository;
        private readonly IConfiguration _config;

        public LoginController(IJwtAuthenticationRepository JwtAuthenticationRepository, IConfiguration config)
        {
            _jwtAuthenticationRepository = JwtAuthenticationRepository;
            _config = config;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = _jwtAuthenticationRepository.Authenticate(model.Email , model.Password);
            if (user == null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, model.Email),
                };
                var token = _jwtAuthenticationRepository.GenerateToken(_config["Jwt:Key"], claims);
                return Ok(token);
            }
            return Unauthorized();
        }
    }
}