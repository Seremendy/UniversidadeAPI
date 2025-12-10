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
    public class PrerequisitoController : ControllerBase
    {
        private readonly IPrerequisitoRepository _prerequisitoRepository;
        private readonly IDisciplinaRepository _disciplinaRepository;

        public PrerequisitoController(
            IPrerequisitoRepository prerequisitoRepository,
            IDisciplinaRepository disciplinaRepository)
        {
            _prerequisitoRepository = prerequisitoRepository;
            _disciplinaRepository = disciplinaRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PrerequisitoResponseDto>> CreatePrerequisito([FromBody] CreatePrerequisitoRequestDto prerequisitoDto)
        {
            if (prerequisitoDto.DisciplinaID == prerequisitoDto.PreRequisitoID)
            {
                return BadRequest(new { Message = "Uma disciplina não pode ser pré-requisito de si mesma." });
            }

            if (await _disciplinaRepository.GetByIdAsync(prerequisitoDto.DisciplinaID) == null)
            {
                return NotFound(new { Message = $"Disciplina principal com ID {prerequisitoDto.DisciplinaID} não encontrada." });
            }

            if (await _disciplinaRepository.GetByIdAsync(prerequisitoDto.PreRequisitoID) == null)
            {
                return NotFound(new { Message = $"Disciplina pré-requisito com ID {prerequisitoDto.PreRequisitoID} não encontrada." });
            }

            var prerequisitoEntidade = new Prerequisito
            {
                DisciplinaID = prerequisitoDto.DisciplinaID,
                PreRequisitoID = prerequisitoDto.PreRequisitoID
            };

            await _prerequisitoRepository.AddAsync(prerequisitoEntidade);

            var prerequisitoResponse = MapToResponseDto(prerequisitoEntidade);

            return StatusCode(201, prerequisitoResponse);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrerequisitoResponseDto>>> GetAllPrerequisitos()
        {
            var prerequisitosEntidades = await _prerequisitoRepository.GetAllAsync();
            var prerequisitosResponse = prerequisitosEntidades.Select(MapToResponseDto);
            return Ok(prerequisitosResponse);
        }

        [HttpGet("ParaDisciplina/{disciplinaId}")]
        public async Task<ActionResult<IEnumerable<PrerequisitoResponseDto>>> GetPrerequisitosParaDisciplina(int disciplinaId)
        {
            
            if (await _disciplinaRepository.GetByIdAsync(disciplinaId) == null)
            {
                return NotFound(new { Message = $"Disciplina com ID {disciplinaId} não encontrada." });
            }

            var prerequisitosEntidades = await _prerequisitoRepository.GetPrerequisitosParaDisciplinaAsync(disciplinaId);
            var prerequisitosResponse = prerequisitosEntidades.Select(MapToResponseDto);

            return Ok(prerequisitosResponse);
        }

        
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletePrerequisito([FromQuery] int disciplinaId, [FromQuery] int preRequisitoId)
        {
            var success = await _prerequisitoRepository.DeleteAsync(disciplinaId, preRequisitoId);

            if (!success)
            {
                return NotFound(new { Message = "Relação de pré-requisito não encontrada." });
            }

            return NoContent();
        }
        private PrerequisitoResponseDto MapToResponseDto(Prerequisito prerequisito)
        {
            return new PrerequisitoResponseDto
            {
                DisciplinaID = prerequisito.DisciplinaID,
                PreRequisitoID = prerequisito.PreRequisitoID
            };
        }
    }
}