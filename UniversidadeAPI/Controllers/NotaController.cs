using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // Necessário para ler o Token
using UniversidadeAPI.DTOs;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;

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
        private readonly IMapper _mapper;

        public NotasController(
            INotaRepository notaRepository,
            IAlunoRepository alunoRepository,
            IDisciplinaRepository disciplinaRepository,
            IMapper mapper)
        {
            _notaRepository = notaRepository;
            _alunoRepository = alunoRepository;
            _disciplinaRepository = disciplinaRepository;
            _mapper = mapper;
        }

        [HttpGet("PorAluno/{alunoId}")]
        [Authorize(Roles = "Admin, Professor, Aluno")]
        public async Task<ActionResult<IEnumerable<NotaResponseDto>>> GetNotasPorAluno(int alunoId)
        {
            
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            var userIdClaim = User.FindFirst("id")?.Value;

            // Se for Aluno, ele SÓ pode ver as próprias notas
            if (role == "Aluno" && userIdClaim != null)
            {
                if (int.Parse(userIdClaim) != alunoId)
                {
                    
                    return Forbid();
                }
            }

            var notasEntidades = await _notaRepository.GetNotasPorAlunoAsync(alunoId);

            if (notasEntidades == null || !notasEntidades.Any())
            {
                return Ok(new List<NotaResponseDto>());
            }

            var notasResponse = _mapper.Map<IEnumerable<NotaResponseDto>>(notasEntidades);

            return Ok(notasResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NotaResponseDto>> GetNotaById(int id)
        {
            var notaEntidade = await _notaRepository.GetByIdAsync(id);

            if (notaEntidade == null)
            {
                return NotFound(new { Message = "Nota não encontrada." });
            }

            var response = _mapper.Map<NotaResponseDto>(notaEntidade);
            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Professor")] // Alunos não podem ver TODAS as notas
        public async Task<ActionResult<IEnumerable<NotaResponseDto>>> GetAllNotas()
        {
            var notasEntidades = await _notaRepository.GetAllAsync();
            var response = _mapper.Map<IEnumerable<NotaResponseDto>>(notasEntidades);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Professor")]
        public async Task<ActionResult<NotaResponseDto>> CreateNota([FromBody] CreateNotaRequestDto notaDto)
        {
            // 1. Validação de Existência (Já existia)
            if (await _alunoRepository.GetByIdAsync(notaDto.AlunoID) == null)
            {
                return NotFound(new { Message = $"Aluno com ID {notaDto.AlunoID} não encontrado." });
            }

            if (await _disciplinaRepository.GetByIdAsync(notaDto.DisciplinaID) == null)
            {
                return NotFound(new { Message = $"Disciplina com ID {notaDto.DisciplinaID} não encontrada." });
            }

            var notasDoAluno = await _notaRepository.GetNotasPorAlunoAsync(notaDto.AlunoID);

            // Verificamos se alguma delas é da mesma disciplina que estamos tentando salvar
            if (notasDoAluno.Any(n => n.DisciplinaID == notaDto.DisciplinaID))
            {
                return BadRequest(new { Message = "Este aluno já possui uma nota lançada para esta disciplina. Use a edição para alterar a nota." });
            }

            var notaEntidade = _mapper.Map<Notas>(notaDto);

            var novoId = await _notaRepository.AddAsync(notaEntidade);
            notaEntidade.NotaID = novoId;

            var response = _mapper.Map<NotaResponseDto>(notaEntidade);

            return CreatedAtAction(nameof(GetNotaById), new { id = response.NotaID }, response);
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

            // Atualiza apenas o valor da nota (AlunoID e DisciplinaID não mudam na edição)
            _mapper.Map(notaDto, entidadeExistente);

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
    }
}