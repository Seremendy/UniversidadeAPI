﻿using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.Entities
{
    public class Curso
    {

        public int CursosID { get; set; }

        [Required]
        [StringLength(100)]
        public string NomeCurso { get; set; } = string.Empty;

        [Required]
        public int DepartamentoID { get; set; }
    }
}
