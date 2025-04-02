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
            _logger.LogInformation("R�cup�ration de toutes les CurvePoint d'ench�res ...");
            var curvePoints = await _repository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<CurvePointDTO>>(curvePoints));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CurvePointDTO>> GetById(int id)
        {
            _logger.LogInformation($"R�cup�ration de la liste des CurvePoint avec l'ID : {id}");
            var curvePoint = await _repository.GetByIdAsync(id);
            if (curvePoint == null)
            {
                _logger.LogWarning($"CurvePoint avec ID {id} non trouv�.");
                return NotFound();
            }

            return Ok(_mapper.Map<CurvePointDTO>(curvePoint));
        }

        [HttpPost]
        public async Task<ActionResult<CurvePointDTO>> Create(CurvePointDTO curvePointDto)
        {
            _logger.LogInformation("Cr�ation d'un nouveau CurvePoint...");
            var curvePoint = _mapper.Map<CurvePoint>(curvePointDto);
            var newCurvePoint = await _repository.AddAsync(curvePoint);
            _logger.LogInformation($"CurvePoint cr�� avec succ�s avec ID: {newCurvePoint.Id}");

            return CreatedAtAction(nameof(GetById), new { id = newCurvePoint.Id }, _mapper.Map<CurvePointDTO>(newCurvePoint));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CurvePointDTO curvePointDto)
        {
            if (id != curvePointDto.Id)
            {
                _logger.LogWarning($"�chec de la mise � jour : Mismatch d'ID (Route: {id}, DTO: {curvePointDto.Id}).");
                return BadRequest();
            }

            var curvePoint = await _repository.GetByIdAsync(id);
            if (curvePoint == null)
            {
                _logger.LogWarning($"�chec de la mise � jour : CurvePoint ID {id} non trouv�.");
                return NotFound();
            }

            _mapper.Map(curvePointDto, curvePoint);
            await _repository.UpdateAsync(curvePoint);
            _logger.LogInformation($"CurvePoint avec ID {id} Mise � jour r�ussie.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Tentative de suppression CurvePoint avec ID: {id}");
            var success = await _repository.DeleteAsync(id);
            if (!success)
            {
                _logger.LogWarning($"Suppression �chou�e: CurvePoint avec ID {id} non trouv�.");
                return NotFound();
            }

            _logger.LogInformation($"CurvePoint avec ID {id} supression r�ussie.");
            return NoContent();
        }
    }
}