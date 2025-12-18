using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.Entities
{
    public class SalasDeAula
    {
        public int SalaDeAulaID { get; set; }

        [Required]
        public int Capacidade { get; set; }

        [Required]
        public int NumeroSala { get; set; }

        [Required]
        [StringLength(100)]
        public string PredioNome { get; set; } = string.Empty;
    }
}
