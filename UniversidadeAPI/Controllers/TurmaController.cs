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
    public class TurmasController : ControllerBase
    {
        private readonly ITurmaRepository _turmaRepository;
        private readonly IDisciplinaRepository _disciplinaRepository;
        private readonly ISalaDeAulaRepository _salaDeAulaRepository;
        private readonly IProfessorRepository _professorRepository;
        private readonly IHorarioRepository _horarioRepository;

        public TurmasController(
            ITurmaRepository turmaRepository,
            IDisciplinaRepository disciplinaRepository,
            ISalaDeAulaRepository salaDeAulaRepository,
            IProfessorRepository professorRepository,
            IHorarioRepository horarioRepository)
        {
            _turmaRepository = turmaRepository;
            _disciplinaRepository = disciplinaRepository;
            _salaDeAulaRepository = salaDeAulaRepository;
            _professorRepository = professorRepository;
            _horarioRepository = horarioRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TurmaResponseDto>> CreateTurma([FromBody] CreateTurmaRequestDto turmaDto)
        {
            var validacao = await ValidateForeignKeyIds(turmaDto.DisciplinaID, turmaDto.SalaDeAulaID, turmaDto.ProfessorID, turmaDto.HorarioID);
            if (validacao != null) 
            {
                return validacao;
            }

            var turmaEntidade = new Turma
            {
                Semestre = turmaDto.Semestre,
                DisciplinaID = turmaDto.DisciplinaID,
                SalaDeAulaID = turmaDto.SalaDeAulaID,
                ProfessorID = turmaDto.ProfessorID,
                HorarioID = turmaDto.HorarioID
            };

            var novoId = await _turmaRepository.AddAsync(turmaEntidade);
            turmaEntidade.TurmaID = novoId;

            var turmaResponse = MapToResponseDto(turmaEntidade);

            return CreatedAtAction(nameof(GetTurmaById), new { id = turmaResponse.TurmaID }, turmaResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TurmaResponseDto>> GetTurmaById(int id)
        {
            var turmaEntidade = await _turmaRepository.GetByIdAsync(id);
            if (turmaEntidade == null)
            {
                return NotFound(new { Message = "Turma não encontrada." });
            }
            return Ok(MapToResponseDto(turmaEntidade));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TurmaResponseDto>>> GetAllTurmas()
        {
            var turmasEntidades = await _turmaRepository.GetAllAsync();
            var turmasResponse = turmasEntidades.Select(MapToResponseDto);
            return Ok(turmasResponse);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateTurma(int id, [FromBody] UpdateTurmaRequestDto turmaDto)
        {
            var entidadeExistente = await _turmaRepository.GetByIdAsync(id);
            if (entidadeExistente == null)
            {
                return NotFound(new { Message = "Turma não encontrada." });
            }

            var validacao = await ValidateForeignKeyIds(turmaDto.DisciplinaID, turmaDto.SalaDeAulaID, turmaDto.ProfessorID, turmaDto.HorarioID);
            if (validacao != null)
            {
                return validacao;
            }

            entidadeExistente.Semestre = turmaDto.Semestre;
            entidadeExistente.DisciplinaID = turmaDto.DisciplinaID;
            entidadeExistente.SalaDeAulaID = turmaDto.SalaDeAulaID;
            entidadeExistente.ProfessorID = turmaDto.ProfessorID;
            entidadeExistente.HorarioID = turmaDto.HorarioID;

            await _turmaRepository.UpdateAsync(entidadeExistente);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteTurma(int id)
        {
            var entidadeExistente = await _turmaRepository.GetByIdAsync(id);
            if (entidadeExistente == null)
            {
                return NotFound(new { Message = "Turma não encontrada." });
            }

            await _turmaRepository.DeleteAsync(id);

            return NoContent();
        }

        private TurmaResponseDto MapToResponseDto(Turma turma)
        {
            return new TurmaResponseDto
            {
                TurmaID = turma.TurmaID,
                Semestre = turma.Semestre,
                DisciplinaID = turma.DisciplinaID,
                SalaDeAulaID = turma.SalaDeAulaID,
                ProfessorID = turma.ProfessorID,
                HorarioID = turma.HorarioID
            };
        }
        private async Task<ActionResult?> ValidateForeignKeyIds(int disciplinaId, int salaId, int professorId, int horarioId)
        {
            if (await _disciplinaRepository.GetByIdAsync(disciplinaId) == null)
            {
                return NotFound(new { Message = $"Disciplina com ID {disciplinaId} não encontrada." });
            }
            if (await _salaDeAulaRepository.GetByIdAsync(salaId) == null)
            {
                return NotFound(new { Message = $"Sala de Aula com ID {salaId} não encontrada." });
            }
            if (await _professorRepository.GetByIdAsync(professorId) == null)
            {
                return NotFound(new { Message = $"Professor com ID {professorId} não encontrado." });
            }
            if (await _horarioRepository.GetByIdAsync(horarioId) == null)
            {
                return NotFound(new { Message = $"Horário com ID {horarioId} não encontrado." });
            }

            return null;
        }
    }
}