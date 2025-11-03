using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversidadeAPI.DTOs;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces; // O seu 'using' está correto

namespace UniversidadeAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotasController : ControllerBase
    {
        private readonly INotaRepository _notaRepository;
        private readonly IAlunoRepository _alunoRepository;
        private readonly IDisciplinaRepository _disciplinaRepository;

        public NotasController(
            INotaRepository notaRepository,
            IAlunoRepository alunoRepository,
            IDisciplinaRepository disciplinaRepository)
        {
            _notaRepository = notaRepository;
            _alunoRepository = alunoRepository;
            _disciplinaRepository = disciplinaRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Professor")]
        public async Task<ActionResult<NotaResponseDto>> CreateNota([FromBody] CreateNotaRequestDto notaDto)
        {
            if (await _alunoRepository.GetByIdAsync(notaDto.AlunoID) == null)
            {
                return NotFound(new { Message = $"Aluno com ID {notaDto.AlunoID} não encontrado." });
            }

            if (await _disciplinaRepository.GetByIdAsync(notaDto.DisciplinaID) == null)
            {
                return NotFound(new { Message = $"Disciplina com ID {notaDto.DisciplinaID} não encontrada." });
            }

            var notaEntidade = new Nota
            {
                NotaValor = notaDto.NotaValor,
                AlunoID = notaDto.AlunoID,
                DisciplinaID = notaDto.DisciplinaID
            };

            var novoId = await _notaRepository.AddAsync(notaEntidade);
            notaEntidade.NotaID = novoId;

            var notaResponse = MapToResponseDto(notaEntidade);

            return CreatedAtAction(nameof(GetNotaById), new { id = notaResponse.NotaID }, notaResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NotaResponseDto>> GetNotaById(int id)
        {
            var notaEntidade = await _notaRepository.GetByIdAsync(id);

            if (notaEntidade == null)
            {
                return NotFound(new { Message = "Nota não encontrada." });
            }

            var notaResponse = MapToResponseDto(notaEntidade);
            return Ok(notaResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Professor")]
        public async Task<ActionResult<IEnumerable<NotaResponseDto>>> GetAllNotas()
        {
            var notasEntidades = await _notaRepository.GetAllAsync();
            var notasResponse = notasEntidades.Select(MapToResponseDto);
            return Ok(notasResponse);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Professor")]
        public async Task<ActionResult> UpdateNota(int id, [FromBody] UpdateNotaRequestDto notaDto)
        {
            var entidadeExistente = await _notaRepository.GetByIdAsync(id);
            if (entidadeExistente == null)
            {
                return NotFound(new { Message = "Nota não encontrada." });
            }

            entidadeExistente.NotaValor = notaDto.NotaValor;

            await _notaRepository.UpdateAsync(entidadeExistente);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteNota(int id)
        {
            var entidadeExistente = await _notaRepository.GetByIdAsync(id);
            if (entidadeExistente == null)
            {
                return NotFound(new { Message = "Nota não encontrada." });
            }

            await _notaRepository.DeleteAsync(id);

            return NoContent();
        }

        private NotaResponseDto MapToResponseDto(Nota nota)
        {
            return new NotaResponseDto
            {
                NotaID = nota.NotaID,
                NotaValor = nota.NotaValor,
                AlunoID = nota.AlunoID,
                DisciplinaID = nota.DisciplinaID
            };
        }
    }
}