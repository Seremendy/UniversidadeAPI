using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversidadeAPI.DTOs;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces; 
namespace UniversidadeAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SalasDeAulaController : ControllerBase
    {
        private readonly ISalaDeAulaRepository _salaDeAulaRepository;
        private readonly ITurmaRepository _turmaRepository;

        public SalasDeAulaController(ISalaDeAulaRepository salaDeAulaRepository, ITurmaRepository turmaRepository)
        {
            _salaDeAulaRepository = salaDeAulaRepository;
            _turmaRepository = turmaRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] 
        public async Task<ActionResult<SalaDeAulaResponseDto>> CreateSalaDeAula([FromBody] CreateSalaDeAulaRequestDto salaDto)
        {
            var salaEntidade = new SalasDeAula
            {
                Capacidade = salaDto.Capacidade,
                NumeroSala = salaDto.NumeroSala,
                PredioNome = salaDto.PredioNome
            };

            var novoId = await _salaDeAulaRepository.AddAsync(salaEntidade);
            salaEntidade.SalaDeAulaID = novoId;

            var salaResponse = MapToResponseDto(salaEntidade);

            return CreatedAtAction(nameof(GetSalaDeAulaById), new { id = salaResponse.SalaDeAulaID }, salaResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalaDeAulaResponseDto>> GetSalaDeAulaById(int id)
        {
            var salaEntidade = await _salaDeAulaRepository.GetByIdAsync(id);

            if (salaEntidade == null)
            {
                return NotFound(new { Message = "Sala de aula não encontrada." });
            }

            var salaResponse = MapToResponseDto(salaEntidade);
            return Ok(salaResponse);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalaDeAulaResponseDto>>> GetAllSalasDeAula()
        {
            var salasEntidades = await _salaDeAulaRepository.GetAllAsync();

            var salasResponse = salasEntidades.Select(MapToResponseDto);

            return Ok(salasResponse);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateSalaDeAula(int id, [FromBody] UpdateSalaDeAulaRequestDto salaDto)
        {
            var entidadeExistente = await _salaDeAulaRepository.GetByIdAsync(id);
            if (entidadeExistente == null)
            {
                return NotFound(new { Message = "Sala de aula não encontrada." });
            }

            entidadeExistente.Capacidade = salaDto.Capacidade;
            entidadeExistente.NumeroSala = salaDto.NumeroSala;
            entidadeExistente.PredioNome = salaDto.PredioNome;

            await _salaDeAulaRepository.UpdateAsync(entidadeExistente);

            return NoContent(); 
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteSalaDeAula(int id)
        {
            var entidadeExistente = await _salaDeAulaRepository.GetByIdAsync(id);
            if (entidadeExistente == null)
            {
                return NotFound(new { Message = "Sala de aula não encontrada." });
            }

            var turmasNestaSala = await _turmaRepository.GetTurmasBySalaIdAsync(id);

            if (turmasNestaSala.Any())
            {
                
                return Conflict(new { Message = "Esta sala de aula não pode ser apagada pois está a ser utilizada por uma ou mais turmas." });
            }

            
            await _salaDeAulaRepository.DeleteAsync(id);

            return NoContent();
        }

        private SalaDeAulaResponseDto MapToResponseDto(SalasDeAula sala)
        {
            return new SalaDeAulaResponseDto
            {
                SalaDeAulaID = sala.SalaDeAulaID,
                Capacidade = sala.Capacidade,
                NumeroSala = sala.NumeroSala,
                PredioNome = sala.PredioNome
            };
        }
    }
}