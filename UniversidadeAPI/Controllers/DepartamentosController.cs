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
    public class DepartamentosController : ControllerBase
    {
        private readonly IDepartamentoRepository _repository;
        private readonly IMapper _mapper;

        public DepartamentosController(IDepartamentoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartamentoDtos>>> GetAll()
        {
            var departamentos = await _repository.GetAllAsync();

            var departamentosDto = _mapper.Map<IEnumerable<DepartamentoDtos>>(departamentos);

            return Ok(departamentosDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartamentoDtos>> GetById(int id)
        {
            var departamento = await _repository.GetByIdAsync(id);

            if (departamento == null)
            {
                return NotFound(new { Message = "Departamento não encontrado." });
            }

            var departamentoDto = _mapper.Map<DepartamentoDtos>(departamento);

            return Ok(departamentoDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([FromBody] CreateDepartamentoRequestDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var departamento = _mapper.Map<Departamentos>(dto);

            var novoId = await _repository.AddAsync(departamento);
            departamento.DepartamentoID = novoId;

            var responseDto = _mapper.Map<DepartamentoDtos>(departamento);

            return CreatedAtAction(
                nameof(GetById),
                new { id = departamento.DepartamentoID },
                responseDto
            );
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(int id, [FromBody] CreateDepartamentoRequestDto dto)
        {
            var departamentoExistente = await _repository.GetByIdAsync(id);

            if (departamentoExistente == null)
            {
                return NotFound(new { Message = "Departamento não encontrado." });
            }

            _mapper.Map(dto, departamentoExistente);

            departamentoExistente.DepartamentoID = id;

            await _repository.UpdateAsync(departamentoExistente);

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
            return NoContent();
        }
    }
}