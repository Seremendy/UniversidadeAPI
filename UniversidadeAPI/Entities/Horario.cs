﻿using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.Entities
{
    public enum DiaSemana
    {
        Segunda,
        Terca,
        Quarta,
        Quinta,
        Sexta
    }
    public class Horario
    {
        public int HorarioID { get; set; }

        [Required]
        public int DisciplinaID { get; set; }

        [Required]
        public DiaSemana DiaSemana { get; set; }

        [Required]
        public TimeOnly HoraInicio { get; set; }

        [Required]
        public TimeOnly HoraFim { get; set; }
    }
}
