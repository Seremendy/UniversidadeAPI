using System.ComponentModel.DataAnnotations;
using UniversidadeAPI.Entities;

namespace UniversidadeAPI.DTOs
{

    public class CreateHorarioRequestDto
    {
        [Required(ErrorMessage = "O dia da semana é obrigatório.")]
        public string DiaSemana { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hora de início é obrigatória.")]
        public TimeOnly HoraInicio { get; set; }

        [Required(ErrorMessage = "Hora de fim é obrigatória.")]
        public TimeOnly HoraFim { get; set; }
    }

    public class UpdateHorarioRequestDto
    {
        public string DiaSemana { get; set; } = string.Empty;
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFim { get; set; }
    }

    public class HorarioResponseDto
    {
        public int HorarioID { get; set; }
        public string DiaSemana { get; set; } = string.Empty;
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFim { get; set; }
    }
}