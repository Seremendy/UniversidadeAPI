using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories;


namespace UniversidadeAPI.Controllers
{
    [Authorize] // Protege toda a classe
    [ApiController]
    [Route("api/[controller]")]
    public class DepartamentosController : ControllerBase
    {
        private readonly IDepartamentoRepository _repository;

        public DepartamentosController(IDepartamentoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Departamento>>> GetAll()
        {
            var departamentos = await _repository.GetAllAsync();
            return Ok(departamentos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Departamento>> GetById(int id)
        {
            var departamento = await _repository.GetByIdAsync(id);

            if (departamento == null)
            {
                return NotFound(); // 404
            }
            return Ok(departamento); // 200
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([FromBody] Departamento departamento)
        {
            var novoId = await _repository.AddAsync(departamento);
            departamento.DepartamentoID = novoId;

            // Retorna 201 Created (Com a URL do novo recurso)
            return CreatedAtAction(
                nameof(GetById),
                new { id = departamento.DepartamentoID },
                departamento
            );
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(int id, [FromBody] Departamento departamento)
        {
            departamento.DepartamentoID = id;

            var success = await _repository.UpdateAsync(departamento);

            if (!success)
            {
                return NotFound(new { Message = "Departamento não encontrado." });
            }
            // Retorna 204 No Content
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _repository.DeleteAsync(id);

            if (!success)
            {
                return NotFound(new { Message = "Departamento não encontrado." });
            }
            return NoContent(); // 204 No Content
        }
    }
}