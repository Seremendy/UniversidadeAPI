using AutoMapper;
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
        private readonly IMapper _mapper;

        public PrerequisitoController(
            IPrerequisitoRepository prerequisitoRepository,
            IDisciplinaRepository disciplinaRepository,
            IMapper mapper)
        {
            _prerequisitoRepository = prerequisitoRepository;
            _disciplinaRepository = disciplinaRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PrerequisitoResponseDto>> CreatePrerequisito([FromBody] CreatePrerequisitoRequestDto prerequisitoDto)
        {
            // Validação de Negócio: Dependência Circular Simples
            if (prerequisitoDto.DisciplinaID == prerequisitoDto.PreRequisitoID)
            {
                return BadRequest(new { Message = "Uma disciplina não pode ser pré-requisito de si mesma." });
            }

            // Validação de Integridade
            if (await _disciplinaRepository.GetByIdAsync(prerequisitoDto.DisciplinaID) == null)
            {
                return NotFound(new { Message = $"Disciplina principal com ID {prerequisitoDto.DisciplinaID} não encontrada." });
            }

            if (await _disciplinaRepository.GetByIdAsync(prerequisitoDto.PreRequisitoID) == null)
            {
                return NotFound(new { Message = $"Disciplina pré-requisito com ID {prerequisitoDto.PreRequisitoID} não encontrada." });
            }

            // Conversão DTO -> Entidade
            var prerequisitoEntidade = _mapper.Map<Prerequisito>(prerequisitoDto);

            await _prerequisitoRepository.AddAsync(prerequisitoEntidade);

            // Retorno
            var response = _mapper.Map<PrerequisitoResponseDto>(prerequisitoEntidade);

            return StatusCode(201, response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrerequisitoResponseDto>>> GetAllPrerequisitos()
        {
            var prerequisitosEntidades = await _prerequisitoRepository.GetAllAsync();
            var response = _mapper.Map<IEnumerable<PrerequisitoResponseDto>>(prerequisitosEntidades);
            return Ok(response);
        }

        [HttpGet("ParaDisciplina/{disciplinaId}")]
        public async Task<ActionResult<IEnumerable<PrerequisitoResponseDto>>> GetPrerequisitosParaDisciplina(int disciplinaId)
        {
            if (await _disciplinaRepository.GetByIdAsync(disciplinaId) == null)
            {
                return NotFound(new { Message = $"Disciplina com ID {disciplinaId} não encontrada." });
            }

            var prerequisitosEntidades = await _prerequisitoRepository.GetPrerequisitosParaDisciplinaAsync(disciplinaId);
            var response = _mapper.Map<IEnumerable<PrerequisitoResponseDto>>(prerequisitosEntidades);

            return Ok(response);
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
    }
}