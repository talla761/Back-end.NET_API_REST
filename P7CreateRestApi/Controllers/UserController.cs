using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using P7CreateRestApi.DTOs;
using P7CreateRestApi.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<IdentityUser> userManager, IMapper mapper, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<IdentityUser>> GetAll()
        {
            _logger.LogInformation("R�cup�ration de tous les utilisateurs...");
            var users = _userManager.Users;
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IdentityUser>> GetById(string id)
        {
            _logger.LogInformation($"R�cup�ration de l'utilisateur avec l'ID : {id}");
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning($"Utilisateur avec ID {id} non trouv�.");
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<IdentityUser>> Create(UserDTO userDto)
        {
            _logger.LogInformation("Cr�ation d'un nouvel utilisateur...");
            var user = new IdentityUser { UserName = userDto.Email, Email = userDto.Email };
            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
            {
                _logger.LogWarning("�chec de la cr�ation de l'utilisateur.");
                return BadRequest(result.Errors);
            }

            _logger.LogInformation($"Utilisateur cr�� avec succ�s avec ID: {user.Id}");
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UserDTO userDto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning($"�chec de la mise � jour : Utilisateur ID {id} non trouv�.");
                return NotFound();
            }

            user.Email = userDto.Email;
            user.UserName = userDto.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogWarning("�chec de la mise � jour de l'utilisateur.");
                return BadRequest(result.Errors);
            }

            _logger.LogInformation($"Utilisateur avec ID {id} mise � jour r�ussie.");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation($"Tentative de suppression de l'utilisateur avec ID: {id}");
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning($"Suppression �chou�e: Utilisateur avec ID {id} non trouv�.");
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogWarning("�chec de la suppression de l'utilisateur.");
                return BadRequest(result.Errors);
            }

            _logger.LogInformation($"Utilisateur avec ID {id} supprim� avec succ�s.");
            return NoContent();
        }
    }
}
