using AutoMapper;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.DTOs;
using P7CreateRestApi.Repositories.Interfaces;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TradeController : ControllerBase
    {
        private readonly IGenericRepository<Trade> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TradeController> _logger;

        public TradeController(IGenericRepository<Trade> repository, IMapper mapper, ILogger<TradeController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TradeDTO>>> GetAll()
        {
            _logger.LogInformation("Récupération de toutes les Trade d'enchères ...");
            var trades = await _repository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<TradeDTO>>(trades));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TradeDTO>> GetById(int id)
        {
            _logger.LogInformation($"Récupération de la liste des Trade avec l'ID : {id}");
            var trade = await _repository.GetByIdAsync(id);
            if (trade == null)
            {
                _logger.LogWarning($"Trade avec ID {id} non trouvé.");
                return NotFound();
            }

            return Ok(_mapper.Map<TradeDTO>(trade));
        }

        [HttpPost]
        public async Task<ActionResult<TradeDTO>> Create(TradeDTO tradeDto)
        {
            _logger.LogInformation("Création d'un nouveau Trade...");
            var trade = _mapper.Map<Trade>(tradeDto);
            var newTrade = await _repository.AddAsync(trade);
            _logger.LogInformation($"Trade créé avec succès avec ID: {newTrade.TradeId}");

            return CreatedAtAction(nameof(GetById), new { id = newTrade.TradeId }, _mapper.Map<TradeDTO>(newTrade));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TradeDTO tradeDto)
        {
            if (id != tradeDto.TradeId)
            {
                _logger.LogWarning($"Échec de la mise à jour : Mismatch d'ID (Route: {id}, DTO: {tradeDto.TradeId}).");
                return BadRequest();
            }

            var trade = await _repository.GetByIdAsync(id);
            if (trade == null)
            {
                _logger.LogWarning($"Échec de la mise à jour : Trade ID {id} non trouvé.");
                return NotFound();
            }

            _mapper.Map(tradeDto, trade);
            await _repository.UpdateAsync(trade);
            _logger.LogInformation($"Trade avec ID {id} Mise à jour réussie.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Tentative de suppression Trade avec ID: {id}");
            var success = await _repository.DeleteAsync(id);
            if (!success)
            {
                _logger.LogWarning($"Suppression échouée: Trade avec ID {id} non trouvé.");
                return NotFound();
            }

            _logger.LogInformation($"Trade avec ID {id} supression réussie.");
            return NoContent();
        }
    }
}