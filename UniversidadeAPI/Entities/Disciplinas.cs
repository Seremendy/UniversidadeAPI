using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.Entities
{
    public class Disciplinas
    {
        public int DisciplinaID { get; set; }

        [Required]
        [StringLength(100)]
        public string NomeDisciplina { get; set; } = string.Empty;
    }
}
