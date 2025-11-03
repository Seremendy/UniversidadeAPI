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
    public class ProfessoresController : ControllerBase
    {
        private readonly IProfessorRepository _professorRepository;

        public ProfessoresController(IProfessorRepository professorRepository)
        {
            _professorRepository = professorRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] 
        public async Task<ActionResult<ProfessorResponseDto>> CreateProfessor([FromBody] CreateProfessorRequestDto professorDto)
        {
            var professorEntidade = new Professor
            {
                ProfessorNome = professorDto.ProfessorNome,
                DataNascimento = professorDto.DataNascimento,
                RG = professorDto.RG,
                CPF = professorDto.CPF,
                Formacao = professorDto.Formacao
            };

            var novoId = await _professorRepository.AddAsync(professorEntidade);
            professorEntidade.ProfessorID = novoId;

            var professorResponse = MapToResponseDto(professorEntidade);

            return CreatedAtAction(nameof(GetProfessorById), new { id = professorResponse.ProfessorID }, professorResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProfessorResponseDto>> GetProfessorById(int id)
        {
            var professorEntidade = await _professorRepository.GetByIdAsync(id);

            if (professorEntidade == null)
            {
                return NotFound(new { Message = "Professor não encontrado." });
            }

            var professorResponse = MapToResponseDto(professorEntidade);
            return Ok(professorResponse);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfessorResponseDto>>> GetAllProfessores()
        {
            var professoresEntidades = await _professorRepository.GetAllAsync();

            var professoresResponse = professoresEntidades.Select(MapToResponseDto);

            return Ok(professoresResponse);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateProfessor(int id, [FromBody] UpdateProfessorRequestDto professorDto)
        {
            var entidadeExistente = await _professorRepository.GetByIdAsync(id);
            if (entidadeExistente == null)
            {
                return NotFound(new { Message = "Professor não encontrado." });
            }

            entidadeExistente.ProfessorNome = professorDto.ProfessorNome;
            entidadeExistente.DataNascimento = professorDto.DataNascimento;
            entidadeExistente.RG = professorDto.RG;
            entidadeExistente.CPF = professorDto.CPF;
            entidadeExistente.Formacao = professorDto.Formacao;

            await _professorRepository.UpdateAsync(entidadeExistente);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteProfessor(int id)
        {
            var entidadeExistente = await _professorRepository.GetByIdAsync(id);
            if (entidadeExistente == null)
            {
                return NotFound(new { Message = "Professor não encontrado." });
            }

            await _professorRepository.DeleteAsync(id);

            return NoContent();
        }

        private ProfessorResponseDto MapToResponseDto(Professor professor)
        {
            return new ProfessorResponseDto
            {
                ProfessorID = professor.ProfessorID,
                ProfessorNome = professor.ProfessorNome,
                Formacao = professor.Formacao
            };
        }
    }
}