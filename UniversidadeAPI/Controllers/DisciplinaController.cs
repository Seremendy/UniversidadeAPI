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
    public class DisciplinaController : ControllerBase 
    {
        private readonly IDisciplinaRepository _disciplinaRepository;

        public DisciplinaController(IDisciplinaRepository disciplinaRepository)
        {
            _disciplinaRepository = disciplinaRepository;
        }

        // --- GET Todos ---
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisciplinaResponseDto>>> GetDisciplinas()
        {
            var disciplinasEntidades = await _disciplinaRepository.GetAllAsync();

            // Mapear para DTO
            var disciplinasResponse = disciplinasEntidades.Select(disciplina => new DisciplinaResponseDto
            {
                DisciplinaID = disciplina.DisciplinaID,
                NomeDisciplina = disciplina.NomeDisciplina
            });

            return Ok(disciplinasResponse); 
        }

        // --- POST (Criar) ---
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DisciplinaResponseDto>> CreateDisciplina([FromBody] CreateDisciplinaRequestDto disciplinaDto)
        {
            var disciplinaEntidade = new Disciplina
            {
                NomeDisciplina = disciplinaDto.NomeDisciplina
            };

            var novoId = await _disciplinaRepository.AddAsync(disciplinaEntidade);
            disciplinaEntidade.DisciplinaID = novoId;

            var disciplinaResponse = new DisciplinaResponseDto
            {
                DisciplinaID = disciplinaEntidade.DisciplinaID,
                NomeDisciplina = disciplinaEntidade.NomeDisciplina
            };

            return CreatedAtAction(nameof(GetDisciplinaById), new { id = disciplinaResponse.DisciplinaID }, disciplinaResponse);
        }

        // --- GET por ID ---
        [HttpGet("{id}")]
        public async Task<ActionResult<DisciplinaResponseDto>> GetDisciplinaById(int id)
        {
            var disciplina = await _disciplinaRepository.GetByIdAsync(id);
            if (disciplina == null)
            {
                return NotFound(new { Message = $"Disciplina com ID {id} não encontrada." });
            }

            var disciplinaResponse = new DisciplinaResponseDto
            {
                DisciplinaID = disciplina.DisciplinaID,
                NomeDisciplina = disciplina.NomeDisciplina
            };

            return Ok(disciplinaResponse); 
        }

        // --- PUT (Atualizar) ---
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateDisciplina(int id, [FromBody] UpdateDisciplinaRequestDto disciplinaDto)
        {
            var entidadeExistente = await _disciplinaRepository.GetByIdAsync(id);
            if (entidadeExistente == null)
            {
                return NotFound(new { Message = $"Disciplina com ID {id} não encontrada." });
            }

            entidadeExistente.NomeDisciplina = disciplinaDto.NomeDisciplina;

            await _disciplinaRepository.UpdateAsync(entidadeExistente);

            return NoContent();
        }

        // --- DELETE (Apagar) ---
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteDisciplina(int id)
        {
            var entidadeExistente = await _disciplinaRepository.GetByIdAsync(id);
            if (entidadeExistente == null)
            {
                return NotFound(new { Message = $"Disciplina com ID {id} não encontrada." });
            }

            await _disciplinaRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}