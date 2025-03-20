using AutoMapper;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Areas.Identity.Data;
using P7CreateRestApi.DTOs;
using P7CreateRestApi.Repositories.Interfaces;
using System.Security.Cryptography;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class BidListController : ControllerBase
    {
        private readonly IGenericRepository<BidList> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<BidListController> _logger;

        public BidListController(IGenericRepository<BidList> repository, IMapper mapper, ILogger<BidListController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BidListDTO>>> GetAll()
        {
            _logger.LogInformation("Récupération de toutes les BidList d'enchères ...");
            var bidLists = await _repository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<BidListDTO>>(bidLists));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BidListDTO>> GetById(int id)
        {
            _logger.LogInformation($"Récupération de la liste des BidList avec l'ID : {id}");
            var bidList = await _repository.GetByIdAsync(id);
            if (bidList == null)
            {
                _logger.LogWarning($"BidList avec ID {id} non trouvé.");
                return NotFound();
            }

            return Ok(_mapper.Map<BidListDTO>(bidList));
        }

        [HttpPost]
        public async Task<ActionResult<BidListDTO>> Create(BidListDTO bidListDto)
        {
            _logger.LogInformation("Création d'un nouveau BidList...");
            var bidList = _mapper.Map<BidList>(bidListDto);
            var newBidList = await _repository.AddAsync(bidList);
            _logger.LogInformation($"BidList créé avec succès avec ID: {newBidList.BidListId}");

            return CreatedAtAction(nameof(GetById), new { id = newBidList.BidListId }, _mapper.Map<BidListDTO>(newBidList));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BidListDTO bidListDto)
        {
            if (id != bidListDto.BidListId)
            {
                _logger.LogWarning($"Échec de la mise à jour : Mismatch d'ID (Route: {id}, DTO: {bidListDto.BidListId}).");
                return BadRequest();
            }

            var bidList = await _repository.GetByIdAsync(id);
            if (bidList == null)
            {
                _logger.LogWarning($"Échec de la mise à jour : BidList ID {id} non trouvé.");
                return NotFound();
            }

            _mapper.Map(bidListDto, bidList);
            await _repository.UpdateAsync(bidList);
            _logger.LogInformation($"BidList avec ID {id} Mise à jour réussie.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Tentative de suppression BidList avec ID: {id}");
            var success = await _repository.DeleteAsync(id);
            if (!success)
            {
                _logger.LogWarning($"Suppression échouée: BidList avec ID {id} non trouvé.");
                return NotFound();
            }

            _logger.LogInformation($"BidList avec ID {id} supression réussie.");
            return NoContent();
        }
    }
}