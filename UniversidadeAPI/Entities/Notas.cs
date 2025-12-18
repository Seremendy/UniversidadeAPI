using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.Entities
{
    public class Notas
    {
        public int NotaID { get; set; }

        [Required]
        public decimal NotaValor { get; set; }

        [Required]
        public int AlunoID { get; set; }

        [Required]
        public int DisciplinaID { get; set; }
    }
}
