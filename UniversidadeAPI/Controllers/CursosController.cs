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
    public class CursosController : ControllerBase
    {
        private readonly ICursoRepository _cursoRepository;
        private readonly IDepartamentoRepository _departamentoRepository;

        public CursosController(ICursoRepository cursoRepository, IDepartamentoRepository departamentoRepository)
        {
            _cursoRepository = cursoRepository;
            _departamentoRepository = departamentoRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] 
        public async Task<ActionResult<CursoResponseDto>> CreateCurso([FromBody] CreateCursoRequestDto cursoDto)
        {
            if (await _departamentoRepository.GetByIdAsync(cursoDto.DepartamentoID) == null)
            {
                return NotFound(new { Message = $"Departamento com ID {cursoDto.DepartamentoID} não encontrado." });
            }

            var cursoEntidade = new Curso
            {
                NomeCurso = cursoDto.NomeCurso,
                DepartamentoID = cursoDto.DepartamentoID
            };

            var novoId = await _cursoRepository.AddAsync(cursoEntidade);
            cursoEntidade.CursoID = novoId;

            var cursoResponse = new CursoResponseDto
            {
                CursoID = cursoEntidade.CursoID,
                NomeCurso = cursoEntidade.NomeCurso,
                DepartamentoID = cursoEntidade.DepartamentoID
            };

            return CreatedAtAction(nameof(GetCursoById), new { id = cursoResponse.CursoID }, cursoResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CursoResponseDto>> GetCursoById(int id)
        {
            var cursoEntidade = await _cursoRepository.GetByIdAsync(id);

            if (cursoEntidade == null)
            {
                return NotFound(new { Message = "Curso não encontrado." });
            }

            var cursoResponse = new CursoResponseDto
            {
                CursoID = cursoEntidade.CursoID,
                NomeCurso = cursoEntidade.NomeCurso,
                DepartamentoID = cursoEntidade.DepartamentoID
            };

            return Ok(cursoResponse);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CursoResponseDto>>> GetAllCursos()
        {
            var cursosEntidades = await _cursoRepository.GetAllAsync();

            var cursosResponse = cursosEntidades.Select(curso => new CursoResponseDto
            {
                CursoID = curso.CursoID,
                NomeCurso = curso.NomeCurso,
                DepartamentoID = curso.DepartamentoID
            });

            return Ok(cursosResponse);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateCurso(int id, [FromBody] UpdateCursoRequestDto cursoDto)
        {
            var entidadeExistente = await _cursoRepository.GetByIdAsync(id);
            if (entidadeExistente == null)
            {
                return NotFound(new { Message = "Curso não encontrado." });
            }

            if (await _departamentoRepository.GetByIdAsync(cursoDto.DepartamentoID) == null)
            {
                return NotFound(new { Message = $"Departamento com ID {cursoDto.DepartamentoID} não encontrado." });
            }

            entidadeExistente.NomeCurso = cursoDto.NomeCurso;
            entidadeExistente.DepartamentoID = cursoDto.DepartamentoID;

            await _cursoRepository.UpdateAsync(entidadeExistente);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteCurso(int id)
        {
            var entidadeExistente = await _cursoRepository.GetByIdAsync(id);
            if (entidadeExistente == null)
            {
                return NotFound(new { Message = "Curso não encontrado." });
            }

            await _cursoRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}