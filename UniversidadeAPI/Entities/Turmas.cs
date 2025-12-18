using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.Entities
{
    public class Turmas
    {
        public int TurmaID { get; set; }

        [Required]
        [StringLength(10)]
        public string Semestre { get; set; } = string.Empty;

        [Required]
        public int DisciplinaID { get; set; }

        [Required]
        public int SalaDeAulaID { get; set; }

        [Required]
        public int ProfessorID { get; set; }

        [Required]
        public int HorarioID { get; set; }
    }
}
