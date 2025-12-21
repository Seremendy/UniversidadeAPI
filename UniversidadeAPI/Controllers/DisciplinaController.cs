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
    public class DisciplinasController : ControllerBase
    {
        private readonly IDisciplinaRepository _repository;
        private readonly IMapper _mapper;

        public DisciplinasController(IDisciplinaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisciplinaResponseDto>>> GetAll()
        {
            var disciplinas = await _repository.GetAllAsync();
            var response = _mapper.Map<IEnumerable<DisciplinaResponseDto>>(disciplinas);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DisciplinaResponseDto>> GetById(int id)
        {
            var disciplina = await _repository.GetByIdAsync(id);

            if (disciplina == null)
            {
                return NotFound(new { Message = "Disciplina não encontrada." });
            }

            var response = _mapper.Map<DisciplinaResponseDto>(disciplina);

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DisciplinaResponseDto>> Create([FromBody] CreateDisciplinaRequestDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var disciplina = _mapper.Map<Disciplinas>(dto);

            var novoId = await _repository.AddAsync(disciplina);
            disciplina.DisciplinaID = novoId;

            var response = _mapper.Map<DisciplinaResponseDto>(disciplina);

            return CreatedAtAction(nameof(GetById), new { id = response.DisciplinaID }, response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateDisciplinaRequestDto dto)
        {
            var disciplinaExistente = await _repository.GetByIdAsync(id);
            if (disciplinaExistente == null)
            {
                return NotFound(new { Message = "Disciplina não encontrada." });
            }

            _mapper.Map(dto, disciplinaExistente);

            disciplinaExistente.DisciplinaID = id;

            await _repository.UpdateAsync(disciplinaExistente);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var disciplina = await _repository.GetByIdAsync(id);
            if (disciplina == null)
            {
                return NotFound(new { Message = "Disciplina não encontrada." });
            }

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}