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
    public class RuleNameController : ControllerBase
    {
        private readonly IGenericRepository<RuleName> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<RuleNameController> _logger;

        public RuleNameController(IGenericRepository<RuleName> repository, IMapper mapper, ILogger<RuleNameController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RuleNameDTO>>> GetAll()
        {
            _logger.LogInformation("R�cup�ration de toutes les RuleName d'ench�res ...");
            var ruleNames = await _repository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<RuleNameDTO>>(ruleNames));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RuleNameDTO>> GetById(int id)
        {
            _logger.LogInformation($"R�cup�ration de la liste des RuleName avec l'ID : {id}");
            var ruleName = await _repository.GetByIdAsync(id);
            if (ruleName == null)
            {
                _logger.LogWarning($"RuleName avec ID {id} non trouv�.");
                return NotFound();
            }

            return Ok(_mapper.Map<RuleNameDTO>(ruleName));
        }

        [HttpPost]
        public async Task<ActionResult<RuleNameDTO>> Create(RuleNameDTO ruleNameDto)
        {
            _logger.LogInformation("Cr�ation d'un nouveau RuleName...");
            var ruleName = _mapper.Map<RuleName>(ruleNameDto);
            var newRuleName = await _repository.AddAsync(ruleName);
            _logger.LogInformation($"RuleName cr�� avec succ�s avec ID: {newRuleName.Id}");

            return CreatedAtAction(nameof(GetById), new { id = newRuleName.Id }, _mapper.Map<RuleNameDTO>(newRuleName));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RuleNameDTO ruleNameDto)
        {
            if (id != ruleNameDto.Id)
            {
                _logger.LogWarning($"�chec de la mise � jour : Mismatch d'ID (Route: {id}, DTO: {ruleNameDto.Id}).");
                return BadRequest();
            }

            var ruleName = await _repository.GetByIdAsync(id);
            if (ruleName == null)
            {
                _logger.LogWarning($"�chec de la mise � jour : RuleName ID {id} non trouv�.");
                return NotFound();
            }

            _mapper.Map(ruleNameDto, ruleName);
            await _repository.UpdateAsync(ruleName);
            _logger.LogInformation($"RuleName avec ID {id} Mise � jour r�ussie.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Tentative de suppression RuleName avec ID: {id}");
            var success = await _repository.DeleteAsync(id);
            if (!success)
            {
                _logger.LogWarning($"Suppression �chou�e: RuleName avec ID {id} non trouv�.");
                return NotFound();
            }

            _logger.LogInformation($"RuleName avec ID {id} supression r�ussie.");
            return NoContent();
        }
    }
}