using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversidadeAPI.DTOs;
using UniversidadeAPI.Services.Interfaces;

namespace UniversidadeAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TurmasController : ControllerBase
    {
        private readonly ITurmaService _turmaService;

        public TurmasController(ITurmaService turmaService)
        {
            _turmaService = turmaService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TurmaResponseDto>> CreateTurma([FromBody] CreateTurmaRequestDto turmaDto)
        {
            try
            {
                var turmaResponse = await _turmaService.CreateTurmaAsync(turmaDto);
                return CreatedAtAction(nameof(GetTurmaById), new { id = turmaResponse.TurmaID }, turmaResponse);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TurmaResponseDto>> GetTurmaById(int id)
        {
            var turma = await _turmaService.GetTurmaByIdAsync(id);
            if (turma == null) return NotFound(new { Message = "Turma não encontrada." });
            return Ok(turma);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TurmaResponseDto>>> GetAllTurmas()
        {
            var turmas = await _turmaService.GetAllTurmasAsync();
            return Ok(turmas);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateTurma(int id, [FromBody] UpdateTurmaRequestDto turmaDto)
        {
            try
            {
                var sucesso = await _turmaService.UpdateTurmaAsync(id, turmaDto);
                if (!sucesso) return NotFound(new { Message = "Turma não encontrada." });
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteTurma(int id)
        {
            var sucesso = await _turmaService.DeleteTurmaAsync(id);
            if (!sucesso) return NotFound(new { Message = "Turma não encontrada." });
            return NoContent();
        }
    }
}