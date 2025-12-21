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
    public class HorarioController : ControllerBase
    {
        private readonly IHorarioRepository _horarioRepository;
        private readonly IMapper _mapper; 

        public HorarioController(IHorarioRepository horarioRepository, IMapper mapper)
        {
            _horarioRepository = horarioRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<HorarioResponseDto>> CreateHorario([FromBody] CreateHorarioRequestDto horarioDto)
        {
            if (!Enum.TryParse<DiaSemana>(horarioDto.DiaSemana, true, out _))
            {
                return BadRequest(new
                {
                    Message = "Dia da semana inválido. Valores aceitos: Segunda, Terca, Quarta, Quinta, Sexta."
                });
            }

           
            var horarioEntidade = _mapper.Map<Horarios>(horarioDto);

            var novoId = await _horarioRepository.AddAsync(horarioEntidade);
            horarioEntidade.HorarioID = novoId;

            var horarioResponse = _mapper.Map<HorarioResponseDto>(horarioEntidade);

            return CreatedAtAction(nameof(GetHorarioById), new { id = horarioResponse.HorarioID }, horarioResponse);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HorarioResponseDto>>> GetAllHorarios()
        {
            var horariosEntidades = await _horarioRepository.GetAllAsync();
            var horariosResponse = _mapper.Map<IEnumerable<HorarioResponseDto>>(horariosEntidades);
            return Ok(horariosResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HorarioResponseDto>> GetHorarioById(int id)
        {
            var horarioEntidade = await _horarioRepository.GetByIdAsync(id);
            if (horarioEntidade == null)
            {
                return NotFound(new { Message = "Horário não encontrado." });
            }

            var response = _mapper.Map<HorarioResponseDto>(horarioEntidade);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteHorario(int id)
        {
            var entidadeExistente = await _horarioRepository.GetByIdAsync(id);
            if (entidadeExistente == null)
            {
                return NotFound(new { Message = "Horário não encontrado." });
            }

            await _horarioRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}