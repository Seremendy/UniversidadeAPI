using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.Entities
{
    public class Professores
    {
        public int ProfessorID { get; set; }

        [Required]
        [StringLength(150)]
        public string ProfessorNome { get; set; } = string.Empty;

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        [StringLength(9)]
        public string RG { get; set; }

        [Required]
        [StringLength(11)]
        public string CPF { get; set; }

        [Required]
        [StringLength(100)]
        public string Formacao { get; set; }
    }
}
