using AutoMapper;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.DTOs;
using P7CreateRestApi.Repositories.Interfaces;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class RatingController : ControllerBase
    {
        private readonly IGenericRepository<Rating> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<RatingController> _logger;

        public RatingController(IGenericRepository<Rating> repository, IMapper mapper, ILogger<RatingController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RatingDTO>>> GetAll()
        {
            _logger.LogInformation("Récupération de toutes les Rating d'enchères ...");
            var ratings = await _repository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<RatingDTO>>(ratings));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RatingDTO>> GetById(int id)
        {
            _logger.LogInformation($"Récupération de la liste des Rating avec l'ID : {id}");
            var rating = await _repository.GetByIdAsync(id);
            if (rating == null)
            {
                _logger.LogWarning($"Rating avec ID {id} non trouvé.");
                return NotFound();
            }

            return Ok(_mapper.Map<RatingDTO>(rating));
        }

        [HttpPost]
        public async Task<ActionResult<RatingDTO>> Create(RatingDTO ratingDto)
        {
            _logger.LogInformation("Création d'un nouveau Rating...");
            var rating = _mapper.Map<Rating>(ratingDto);
            var newRating = await _repository.AddAsync(rating);
            _logger.LogInformation($"Rating créé avec succès avec ID: {newRating.Id}");

            return CreatedAtAction(nameof(GetById), new { id = newRating.Id }, _mapper.Map<RatingDTO>(newRating));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RatingDTO ratingDto)
        {
            if (id != ratingDto.Id)
            {
                _logger.LogWarning($"Échec de la mise à jour : Mismatch d'ID (Route: {id}, DTO: {ratingDto.Id}).");
                return BadRequest();
            }

            var rating = await _repository.GetByIdAsync(id);
            if (rating == null)
            {
                _logger.LogWarning($"Échec de la mise à jour : Rating ID {id} non trouvé.");
                return NotFound();
            }

            _mapper.Map(ratingDto, rating);
            await _repository.UpdateAsync(rating);
            _logger.LogInformation($"Rating avec ID {id} Mise à jour réussie.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Tentative de suppression Rating avec ID: {id}");
            var success = await _repository.DeleteAsync(id);
            if (!success)
            {
                _logger.LogWarning($"Suppression échouée: Rating avec ID {id} non trouvé.");
                return NotFound();
            }

            _logger.LogInformation($"Rating avec ID {id} supression réussie.");
            return NoContent();
        }
    }
}