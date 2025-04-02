using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using P7CreateRestApi.Areas.Identity.Data;
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
        private readonly UserManager<IdentityUser> _userManager;

        public LoginController(IJwtAuthenticationRepository JwtAuthenticationRepository, IConfiguration config, UserManager<IdentityUser> userManager)
        {
            _jwtAuthenticationRepository = JwtAuthenticationRepository;
            _config = config;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest("Email et mot de passe sont requis.");
            }

            // Récupérer l'utilisateur depuis AspNetUsers
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized("Utilisateur non trouvé.");
            }

            // Vérifier le mot de passe avec Identity
            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordValid)
            {
                return Unauthorized("Mot de passe incorrect.");
            }

            // Si l'utilisateur est authentifié, générer un JWT
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName), // Par exemple, ajouter un nom d'utilisateur comme revendication
                new Claim(ClaimTypes.Email, user.Email),   // Ajouter l'email comme revendication
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Ajouter un identifiant unique (JTI)
            };

            // Générer le token JWT avec la clé secrète
            var token = _jwtAuthenticationRepository.GenerateToken(_config["Jwt:Key"], claims);

            // Retourner le token JWT à l'utilisateur
            return Ok(new { Token = token });
        }
    }
}