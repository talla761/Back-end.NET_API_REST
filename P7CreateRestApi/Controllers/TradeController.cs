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
            _logger.LogInformation("R�cup�ration de toutes les Trade d'ench�res ...");
            var trades = await _repository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<TradeDTO>>(trades));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TradeDTO>> GetById(int id)
        {
            _logger.LogInformation($"R�cup�ration de la liste des Trade avec l'ID : {id}");
            var trade = await _repository.GetByIdAsync(id);
            if (trade == null)
            {
                _logger.LogWarning($"Trade avec ID {id} non trouv�.");
                return NotFound();
            }

            return Ok(_mapper.Map<TradeDTO>(trade));
        }

        [HttpPost]
        public async Task<ActionResult<TradeDTO>> Create(TradeDTO tradeDto)
        {
            _logger.LogInformation("Cr�ation d'un nouveau Trade...");
            var trade = _mapper.Map<Trade>(tradeDto);
            var newTrade = await _repository.AddAsync(trade);
            _logger.LogInformation($"Trade cr�� avec succ�s avec ID: {newTrade.TradeId}");

            return CreatedAtAction(nameof(GetById), new { id = newTrade.TradeId }, _mapper.Map<TradeDTO>(newTrade));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TradeDTO tradeDto)
        {
            if (id != tradeDto.TradeId)
            {
                _logger.LogWarning($"�chec de la mise � jour : Mismatch d'ID (Route: {id}, DTO: {tradeDto.TradeId}).");
                return BadRequest();
            }

            var trade = await _repository.GetByIdAsync(id);
            if (trade == null)
            {
                _logger.LogWarning($"�chec de la mise � jour : Trade ID {id} non trouv�.");
                return NotFound();
            }

            _mapper.Map(tradeDto, trade);
            await _repository.UpdateAsync(trade);
            _logger.LogInformation($"Trade avec ID {id} Mise � jour r�ussie.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Tentative de suppression Trade avec ID: {id}");
            var success = await _repository.DeleteAsync(id);
            if (!success)
            {
                _logger.LogWarning($"Suppression �chou�e: Trade avec ID {id} non trouv�.");
                return NotFound();
            }

            _logger.LogInformation($"Trade avec ID {id} supression r�ussie.");
            return NoContent();
        }
    }
}