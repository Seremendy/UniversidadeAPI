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
    public class MatriculasController : ControllerBase
    {
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly IAlunoRepository _alunoRepository;
        private readonly ICursoRepository _cursoRepository;
        private readonly IMapper _mapper; 

        public MatriculasController(
            IMatriculaRepository matriculaRepository,
            IAlunoRepository alunoRepository,
            ICursoRepository cursoRepository,
            IMapper mapper)
        {
            _matriculaRepository = matriculaRepository;
            _alunoRepository = alunoRepository;
            _cursoRepository = cursoRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MatriculaResponseDto>> CreateMatricula([FromBody] CreateMatriculaRequestDto matriculaDto)
        {
            if (await _alunoRepository.GetByIdAsync(matriculaDto.AlunoID) == null)
            {
                return NotFound(new { Message = $"Aluno com ID {matriculaDto.AlunoID} não encontrado." });
            }

            if (await _cursoRepository.GetByIdAsync(matriculaDto.CursoID) == null)
            {
                return NotFound(new { Message = $"Curso com ID {matriculaDto.CursoID} não encontrado." });
            }

            var matriculaEntidade = _mapper.Map<Matriculas>(matriculaDto);

            matriculaEntidade.DataMatricula = DateTime.UtcNow;
            matriculaEntidade.MatriculaAtiva = true;

            var novoId = await _matriculaRepository.AddAsync(matriculaEntidade);
            matriculaEntidade.MatriculaID = novoId;

            var matriculaResponse = _mapper.Map<MatriculaResponseDto>(matriculaEntidade);

            return CreatedAtAction(nameof(GetMatriculaById), new { id = matriculaResponse.MatriculaID }, matriculaResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MatriculaResponseDto>> GetMatriculaById(int id)
        {
            var matriculaEntidade = await _matriculaRepository.GetByIdAsync(id);

            if (matriculaEntidade == null)
            {
                return NotFound(new { Message = "Matrícula não encontrada." });
            }

            var matriculaResponse = _mapper.Map<MatriculaResponseDto>(matriculaEntidade);
            return Ok(matriculaResponse);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatriculaResponseDto>>> GetAllMatriculas()
        {
            var matriculasEntidades = await _matriculaRepository.GetAllAsync();
            var matriculasResponse = _mapper.Map<IEnumerable<MatriculaResponseDto>>(matriculasEntidades);
            return Ok(matriculasResponse);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateMatriculaStatus(int id, [FromBody] UpdateMatriculaStatusRequestDto matriculaDto)
        {
            var entidadeExistente = await _matriculaRepository.GetByIdAsync(id);
            if (entidadeExistente == null)
            {
                return NotFound(new { Message = "Matrícula não encontrada." });
            }

            entidadeExistente.MatriculaAtiva = matriculaDto.MatriculaAtiva;

            await _matriculaRepository.UpdateAsync(entidadeExistente);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteMatricula(int id)
        {
            var entidadeExistente = await _matriculaRepository.GetByIdAsync(id);
            if (entidadeExistente == null)
            {
                return NotFound(new { Message = "Matrícula não encontrada." });
            }

            await _matriculaRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}