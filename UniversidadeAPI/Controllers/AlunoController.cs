using Microsoft.AspNetCore.Mvc;
using UniversidadeAPI.DTOs;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;

namespace UniversidadeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlunoController : ControllerBase
    {
        private readonly IAlunoRepository _repository;

        public AlunoController(IAlunoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlunoResponseDto>>> GetAll()
        {
            var alunos = await _repository.GetAllAsync();

            var response = alunos.Select(a => new AlunoResponseDto
            {
                AlunoID = a.AlunoID,
                Nome = a.AlunoNome,
                CPF = a.CPF,
                DataNascimento = a.DataNascimento
            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AlunoResponseDto>> GetById(int id)
        {
            var aluno = await _repository.GetByIdAsync(id);

            if (aluno == null) return NotFound("Aluno não encontrado.");

            var response = new AlunoResponseDto
            {
                AlunoID = aluno.AlunoID,
                Nome = aluno.AlunoNome,
                CPF = aluno.CPF,
                DataNascimento = aluno.DataNascimento
            };

            return Ok(response);
        }

        // GET: api/Alunos/cpf/123.456.789-00
        [HttpGet("cpf/{cpf}")]
        public async Task<ActionResult<AlunoResponseDto>> GetByCpf(string cpf)
        {
            var aluno = await _repository.GetByCPFAsync(cpf);

            if (aluno == null) return NotFound("CPF não encontrado.");

            var response = new AlunoResponseDto
            {
                AlunoID = aluno.AlunoID,
                Nome = aluno.AlunoNome, 
                CPF = aluno.CPF,
                DataNascimento = aluno.DataNascimento
            };

            return Ok(response);
        }

   
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] AlunoRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

        
            var aluno = new Aluno
            {
                AlunoNome = request.NomeAluno, 
                CPF = request.CPF,
                DataNascimento = request.DataNascimento 
            };

            var novoId = await _repository.AddAsync(aluno);

            return CreatedAtAction(nameof(GetById), new { id = novoId }, request);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] AlunoRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existente = await _repository.GetByIdAsync(id);
            if (existente == null) return NotFound("Aluno não existe.");

            
            existente.AlunoNome = request.NomeAluno; 
            existente.CPF = request.CPF;
            existente.DataNascimento = request.DataNascimento; 
            
            var sucesso = await _repository.UpdateAsync(existente);

            if (!sucesso) return BadRequest("Erro ao atualizar aluno.");

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existente = await _repository.GetByIdAsync(id);
            if (existente == null) return NotFound();

            var sucesso = await _repository.DeleteAsync(id);

            if (!sucesso) return BadRequest("Erro ao deletar.");

            return NoContent();
        }
    }
}