using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.Entities
{
    public class Departamentos
    {
        public int DepartamentoID { get; set; }

        [Required]
        [StringLength(100)]
        public string DepartamentoNome { get; set; } = string.Empty;
    }
}
