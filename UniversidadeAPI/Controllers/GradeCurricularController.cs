using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversidadeAPI.DTOs;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories;
using UniversidadeAPI.Repositories.Interfaces;

namespace UniversidadeAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class GradeCurricularController : ControllerBase
    {
        private readonly IGradeCurricularRepository _gradeRepository;
        private readonly IDisciplinaRepository _disciplinaRepository;
        private readonly ICursoRepository _cursoRepository;
        private readonly IMapper _mapper;

        public GradeCurricularController(
            IGradeCurricularRepository gradeRepository,
            IDisciplinaRepository disciplinaRepository,
            ICursoRepository cursoRepository,
            IMapper mapper)
        {
            _gradeRepository = gradeRepository;
            _disciplinaRepository = disciplinaRepository;
            _cursoRepository = cursoRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GradeCurricularResponseDto>> CreateGrade([FromBody] CreateGradeRequestDto gradeDto)
        {
            if (await _disciplinaRepository.GetByIdAsync(gradeDto.DisciplinaID) == null)
            {
                return NotFound(new { Message = $"Disciplina com ID {gradeDto.DisciplinaID} não encontrada." });
            }

            if (await _cursoRepository.GetByIdAsync(gradeDto.CursoID) == null)
            {
                return NotFound(new { Message = $"Curso com ID {gradeDto.CursoID} não encontrado." });
            }

            var gradeEntidade = _mapper.Map<GradeCurriculares>(gradeDto);

            var novoId = await _gradeRepository.AddAsync(gradeEntidade);
            gradeEntidade.GradeCurricularID = novoId;

            var gradeResponse = _mapper.Map<GradeCurricularResponseDto>(gradeEntidade);

            return CreatedAtAction(nameof(GetGradeById), new { id = gradeResponse.GradeCurricularID }, gradeResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GradeCurricularResponseDto>> GetGradeById(int id)
        {
            var gradeEntidade = await _gradeRepository.GetByIdAsync(id);

            if (gradeEntidade == null)
            {
                return NotFound(new { Message = "Relação de grade não encontrada." });
            }

            var gradeResponse = _mapper.Map<GradeCurricularResponseDto>(gradeEntidade);
            return Ok(gradeResponse);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GradeCurricularResponseDto>>> GetAllGrades()
        {
            var gradesEntidades = await _gradeRepository.GetAllAsync();

            var gradesResponse = _mapper.Map<IEnumerable<GradeCurricularResponseDto>>(gradesEntidades);

            return Ok(gradesResponse);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteGrade(int id)
        {
            var entidadeExistente = await _gradeRepository.GetByIdAsync(id);
            if (entidadeExistente == null)
            {
                return NotFound(new { Message = "Relação de grade não encontrada." });
            }

            await _gradeRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}