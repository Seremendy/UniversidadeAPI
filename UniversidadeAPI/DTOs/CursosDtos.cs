using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.DTOs
{
    
    public class CreateCursoRequestDto
    {
        [Required(ErrorMessage = "O nome do curso é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres")]
        public string NomeCurso { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ID do Departamento é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "ID do Departamento inválido")]
        public int DepartamentoID { get; set; }
    }

   
    public class UpdateCursoRequestDto
    {
        [Required(ErrorMessage = "O nome do curso é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres")]
        public string NomeCurso { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ID do Departamento é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "ID do Departamento inválido")]
        public int DepartamentoID { get; set; }
    }

    
    public class CursoResponseDto
    {
        public int CursoID { get; set; }
        public string NomeCurso { get; set; } = string.Empty;
        public int DepartamentoID { get; set; }

    }
}