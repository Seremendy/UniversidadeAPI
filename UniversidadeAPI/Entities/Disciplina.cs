﻿using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.Entities
{
    public class Disciplina
    {
        public int DisciplinaID { get; set; }

        [Required]
        [StringLength(100)]
        public string NomeDisciplina { get; set; } = string.Empty;
    }
}
