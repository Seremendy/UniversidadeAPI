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

        public HorarioController(IHorarioRepository horarioRepository)
        {
            _horarioRepository = horarioRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<HorarioResponseDto>> CreateHorario([FromBody] CreateHorarioRequestDto horarioDto)
        {
            if (!Enum.TryParse(horarioDto.DiaSemana, true, out DiaSemana diaSemanaEnum))
            {
                return BadRequest(new { Message = "Dia da semana inválido. Use: Segunda, Terca, Quarta, Quinta, Sexta." });
            }

            if (horarioDto.HoraInicio >= horarioDto.HoraFim)
            {
                return BadRequest(new { Message = "A hora de início deve ser anterior à hora de fim." });
            }

            var horarioEntidade = new Horario
            {
                DiaSemana = diaSemanaEnum, 
                HoraInicio = horarioDto.HoraInicio,
                HoraFim = horarioDto.HoraFim
            };

            var novoId = await _horarioRepository.AddAsync(horarioEntidade);
            horarioEntidade.HorarioID = novoId;

            var horarioResponse = MapToResponseDto(horarioEntidade);
            return CreatedAtAction(nameof(GetHorarioById), new { id = horarioResponse.HorarioID }, horarioResponse);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HorarioResponseDto>>> GetAllHorarios()
        {
            var horariosEntidades = await _horarioRepository.GetAllAsync();
            var horariosResponse = horariosEntidades.Select(MapToResponseDto);
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
            return Ok(MapToResponseDto(horarioEntidade));
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

        private HorarioResponseDto MapToResponseDto(Horario horario)
        {
            return new HorarioResponseDto
            {
                HorarioID = horario.HorarioID,
                DiaSemana = horario.DiaSemana.ToString(),
                HoraInicio = horario.HoraInicio,
                HoraFim = horario.HoraFim
            };
        }
    }
}