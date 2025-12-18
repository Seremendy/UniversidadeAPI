using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversidadeAPI.DTOs;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;

// 1. Correção de Namespace (Essencial para o ASP.NET encontrar o controller)
namespace UniversidadeAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DisciplinasController : ControllerBase
    {
        private readonly IDisciplinaRepository _repository;
        private readonly IMapper _mapper; // Injeção do AutoMapper

        public DisciplinasController(IDisciplinaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Disciplinas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisciplinaResponseDto>>> GetAll()
        {
            var disciplinas = await _repository.GetAllAsync();

            // Converte Lista de Entidades -> Lista de DTOs
            var response = _mapper.Map<IEnumerable<DisciplinaResponseDto>>(disciplinas);

            return Ok(response);
        }

        // GET: api/Disciplinas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DisciplinaResponseDto>> GetById(int id)
        {
            var disciplina = await _repository.GetByIdAsync(id);

            if (disciplina == null)
            {
                return NotFound(new { Message = "Disciplina não encontrada." });
            }

            // Converte Entidade -> DTO
            var response = _mapper.Map<DisciplinaResponseDto>(disciplina);

            return Ok(response);
        }

        // POST: api/Disciplinas
        [HttpPost]
        [Authorize(Roles = "Admin")] // 2. Segurança: Só Admin pode criar
        public async Task<ActionResult<DisciplinaResponseDto>> Create([FromBody] CreateDisciplinaRequestDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Converte DTO -> Entidade
            var disciplina = _mapper.Map<Disciplinas>(dto);

            var novoId = await _repository.AddAsync(disciplina);
            disciplina.DisciplinaID = novoId;

            // Retorna o objeto criado formatado como DTO
            var response = _mapper.Map<DisciplinaResponseDto>(disciplina);

            return CreatedAtAction(nameof(GetById), new { id = response.DisciplinaID }, response);
        }

        // PUT: api/Disciplinas/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // 2. Segurança: Só Admin pode editar
        public async Task<ActionResult> Update(int id, [FromBody] UpdateDisciplinaRequestDto dto)
        {
            var disciplinaExistente = await _repository.GetByIdAsync(id);
            if (disciplinaExistente == null)
            {
                return NotFound(new { Message = "Disciplina não encontrada." });
            }

            // Atualiza a entidade com os dados do DTO
            _mapper.Map(dto, disciplinaExistente);

            // Garante que o ID não foi alterado acidentalmente
            disciplinaExistente.DisciplinaID = id;

            await _repository.UpdateAsync(disciplinaExistente);

            return NoContent();
        }

        // DELETE: api/Disciplinas/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // 2. Segurança: Só Admin pode deletar
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