//using AutoMapper;
//using Dot.Net.WebApi.Domain;
//using Dot.Net.WebApi.Repositories;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using P7CreateRestApi.DTOs;
//using P7CreateRestApi.Repositories.Interfaces;

//namespace Dot.Net.WebApi.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class UserController : ControllerBase
//    {
//        private readonly IGenericRepository<User> _repository;
//        private readonly IMapper _mapper;
//        private readonly ILogger<UserController> _logger;
//        private readonly IPasswordHasher<User> _passwordHasher;

//        public UserController(IGenericRepository<User> repository, IMapper mapper, ILogger<UserController> logger, IPasswordHasher<User> passwordHasher)
//        {
//            _repository = repository;
//            _mapper = mapper;
//            _logger = logger;
//            _passwordHasher = passwordHasher;
//        }

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
//        {
//            _logger.LogInformation("R�cup�ration de toutes les Users ...");
//            var users = await _repository.GetAllAsync();
//            return Ok(_mapper.Map<IEnumerable<UserDTO>>(users));
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<UserDTO>> GetById(int id)
//        {
//            _logger.LogInformation($"R�cup�ration de la liste des Users avec l'ID : {id}");
//            var user = await _repository.GetByIdAsync(id);
//            if (user == null)
//            {
//                _logger.LogWarning($"User avec ID {id} non trouv�.");
//                return NotFound();
//            }

//            return Ok(_mapper.Map<UserDTO>(user));
//        }

//        [HttpPost]
//        public async Task<ActionResult<UserDTO>> Create(UserDTO userDto)
//        {
//            _logger.LogInformation("Cr�ation d'un nouveau User...");
//            var user = _mapper.Map<User>(userDto);

//            // Hachage du mot de passe avant l'enregistrement
//            user.Password = _passwordHasher.HashPassword(user, userDto.Password);

//            var newUser = await _repository.AddAsync(user);
//            _logger.LogInformation($"User cr�� avec succ�s avec ID: {newUser.Id}");

//            return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, _mapper.Map<UserDTO>(newUser));
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(int id, UserDTO userDto)
//        {
//            if (id != userDto.Id)
//            {
//                _logger.LogWarning($"�chec de la mise � jour : Mismatch d'ID (Route: {id}, DTO: {userDto.Id}).");
//                return BadRequest();
//            }

//            var user = await _repository.GetByIdAsync(id);
//            if (user == null)
//            {
//                _logger.LogWarning($"�chec de la mise � jour : User ID {id} non trouv�.");
//                return NotFound();
//            }

//            _mapper.Map(userDto, user);
//            await _repository.UpdateAsync(user);
//            _logger.LogInformation($"User avec ID {id} Mise � jour r�ussie.");

//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            _logger.LogInformation($"Tentative de suppression User avec ID: {id}");
//            var success = await _repository.DeleteAsync(id);
//            if (!success)
//            {
//                _logger.LogWarning($"Suppression �chou�e: User avec ID {id} non trouv�.");
//                return NotFound();
//            }

//            _logger.LogInformation($"User avec ID {id} supression r�ussie.");
//            return NoContent();
//        }
//    }
//}