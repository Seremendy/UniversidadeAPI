using AutoMapper; // <--- Não esqueça deste using
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
    public class AlunosController : ControllerBase
    {
        private readonly IAlunoRepository _repository;
        private readonly IMapper _mapper; // <--- 1. Injeção do Mapper

        public AlunosController(IAlunoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alunos>>> GetAll()
        {
            return Ok(await _repository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Alunos>> GetById(int id)
        {
            var aluno = await _repository.GetByIdAsync(id);
            if (aluno == null) return NotFound();
            return Ok(aluno);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([FromBody] CreateAlunoRequestDto dto)
        {
            
            var aluno = _mapper.Map<Alunos>(dto);

            var id = await _repository.AddAsync(aluno);

            // Retorna o objeto criado. Se criar um AlunoResponseDto, use _mapper.Map<AlunoResponseDto>(aluno) aqui.
            return CreatedAtAction(nameof(GetById), new { id = id }, aluno);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}