using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.DTOs
{
    public class DepartamentoDtos
    {
        public int DepartamentoID { get; set; }

        [Required]
        [StringLength(100)]
        public string DepartamentoNome { get; set; } = string.Empty;
    }

    public class CreateDepartamentoRequestDto
    {
        [Required(ErrorMessage = "O nome do departamento é obrigatório.")]
        public string DepartamentoNome { get; set; } = string.Empty;

    }

}
