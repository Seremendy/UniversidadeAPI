using System.ComponentModel.DataAnnotations;
using UniversidadeAPI.Entities;

namespace UniversidadeAPI.DTOs
{

    public class CreateHorarioRequestDto
    {

        [Required(ErrorMessage = "O Dia da Semana é obrigatório")]
        public string DiaSemana { get; set; } = string.Empty;

        [Required(ErrorMessage = "A hora de início é obrigatória")]
        public TimeOnly HoraInicio { get; set; }

        [Required(ErrorMessage = "A hora de fim é obrigatória")]
        public TimeOnly HoraFim { get; set; }
    }

    public class UpdateHorarioRequestDto
    {

        [Required(ErrorMessage = "O Dia da Semana é obrigatório")]
        public string DiaSemana { get; set; } = string.Empty;

        [Required(ErrorMessage = "A hora de início é obrigatória")]
        public TimeOnly HoraInicio { get; set; }

        [Required(ErrorMessage = "A hora de fim é obrigatória")]
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