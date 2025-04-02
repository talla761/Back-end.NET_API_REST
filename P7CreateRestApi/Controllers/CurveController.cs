using AutoMapper;
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
    public class CurveController : ControllerBase
    {
        private readonly IGenericRepository<CurvePoint> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CurveController> _logger;

        public CurveController(IGenericRepository<CurvePoint> repository, IMapper mapper, ILogger<CurveController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurvePointDTO>>> GetAll()
        {
            _logger.LogInformation("Récupération de toutes les CurvePoint d'enchères ...");
            var curvePoints = await _repository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<CurvePointDTO>>(curvePoints));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CurvePointDTO>> GetById(int id)
        {
            _logger.LogInformation($"Récupération de la liste des CurvePoint avec l'ID : {id}");
            var curvePoint = await _repository.GetByIdAsync(id);
            if (curvePoint == null)
            {
                _logger.LogWarning($"CurvePoint avec ID {id} non trouvé.");
                return NotFound();
            }

            return Ok(_mapper.Map<CurvePointDTO>(curvePoint));
        }

        [HttpPost]
        public async Task<ActionResult<CurvePointDTO>> Create(CurvePointDTO curvePointDto)
        {
            _logger.LogInformation("Création d'un nouveau CurvePoint...");
            var curvePoint = _mapper.Map<CurvePoint>(curvePointDto);
            var newCurvePoint = await _repository.AddAsync(curvePoint);
            _logger.LogInformation($"CurvePoint créé avec succès avec ID: {newCurvePoint.Id}");

            return CreatedAtAction(nameof(GetById), new { id = newCurvePoint.Id }, _mapper.Map<CurvePointDTO>(newCurvePoint));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CurvePointDTO curvePointDto)
        {
            if (id != curvePointDto.Id)
            {
                _logger.LogWarning($"Échec de la mise à jour : Mismatch d'ID (Route: {id}, DTO: {curvePointDto.Id}).");
                return BadRequest();
            }

            var curvePoint = await _repository.GetByIdAsync(id);
            if (curvePoint == null)
            {
                _logger.LogWarning($"Échec de la mise à jour : CurvePoint ID {id} non trouvé.");
                return NotFound();
            }

            _mapper.Map(curvePointDto, curvePoint);
            await _repository.UpdateAsync(curvePoint);
            _logger.LogInformation($"CurvePoint avec ID {id} Mise à jour réussie.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Tentative de suppression CurvePoint avec ID: {id}");
            var success = await _repository.DeleteAsync(id);
            if (!success)
            {
                _logger.LogWarning($"Suppression échouée: CurvePoint avec ID {id} non trouvé.");
                return NotFound();
            }

            _logger.LogInformation($"CurvePoint avec ID {id} supression réussie.");
            return NoContent();
        }
    }
}