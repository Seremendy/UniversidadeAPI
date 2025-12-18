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
    public class ProfessoresController : ControllerBase
    {
        private readonly IProfessorRepository _professorRepository;
        private readonly IMapper _mapper; // Injeção do Mapper

        public ProfessoresController(IProfessorRepository professorRepository, IMapper mapper)
        {
            _professorRepository = professorRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProfessorResponseDto>> CreateProfessor([FromBody] CreateProfessorRequestDto professorDto)
        {
            // Validação automática do DTO já ocorreu (ModelState)

            // Conversão DTO -> Entidade
            var professorEntidade = _mapper.Map<Professores>(professorDto);

            var novoId = await _professorRepository.AddAsync(professorEntidade);
            professorEntidade.ProfessorID = novoId;

            // Retorno (Entidade -> DTO)
            var professorResponse = _mapper.Map<ProfessorResponseDto>(professorEntidade);

            return CreatedAtAction(nameof(GetProfessorById), new { id = professorResponse.ProfessorID }, professorResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProfessorResponseDto>> GetProfessorById(int id)
        {
            var professorEntidade = await _professorRepository.GetByIdAsync(id);

            if (professorEntidade == null)
            {
                return NotFound(new { Message = "Professor não encontrado." });
            }

            var professorResponse = _mapper.Map<ProfessorResponseDto>(professorEntidade);
            return Ok(professorResponse);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfessorResponseDto>>> GetAllProfessores()
        {
            var professoresEntidades = await _professorRepository.GetAllAsync();
            var professoresResponse = _mapper.Map<IEnumerable<ProfessorResponseDto>>(professoresEntidades);

            return Ok(professoresResponse);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateProfessor(int id, [FromBody] UpdateProfessorRequestDto professorDto)
        {
            var entidadeExistente = await _professorRepository.GetByIdAsync(id);
            if (entidadeExistente == null)
            {
                return NotFound(new { Message = "Professor não encontrado." });
            }

            // Atualiza os dados da entidade existente com o que veio do DTO
            _mapper.Map(professorDto, entidadeExistente);

            // Garante que o ID não se perdeu
            entidadeExistente.ProfessorID = id;

            await _professorRepository.UpdateAsync(entidadeExistente);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteProfessor(int id)
        {
            var entidadeExistente = await _professorRepository.GetByIdAsync(id);
            if (entidadeExistente == null)
            {
                return NotFound(new { Message = "Professor não encontrado." });
            }

            await _professorRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}