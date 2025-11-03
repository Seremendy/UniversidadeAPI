using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.DTOs
{
    // DTO usado para CRIAR um novo curso
    // (O que o cliente envia num POST)
    public class CreateCursoRequestDto
    {
        [Required(ErrorMessage = "O nome do curso é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres")]
        public string NomeCurso { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ID do Departamento é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "ID do Departamento inválido")]
        public int DepartamentoID { get; set; }
    }

    // DTO usado para ATUALIZAR um curso existente
    // (O que o cliente envia num PUT)
    public class UpdateCursoRequestDto
    {
        [Required(ErrorMessage = "O nome do curso é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres")]
        public string NomeCurso { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ID do Departamento é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "ID do Departamento inválido")]
        public int DepartamentoID { get; set; }
    }

    // DTO usado para ENVIAR dados de um curso ao cliente
    // (O que o servidor responde num GET)
    public class CursoResponseDto
    {
        public int CursoID { get; set; }
        public string NomeCurso { get; set; } = string.Empty;
        public int DepartamentoID { get; set; }

    }
}