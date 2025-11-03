using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.DTOs
{
    public class CreateDisciplinaRequestDto
    {
        [Required(ErrorMessage = "Nome da Disciplina é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres")]
        public string NomeDisciplina { get; set; } = string.Empty;

    }

    public class UpdateDisciplinaRequestDto
    {
        [Required(ErrorMessage = "Nome da Disciplina é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres")]
        public string NomeDisciplina { get; set; } = string.Empty;

    }

    public class DisciplinaResponseDto
    {
        public int DisciplinaID { get; set; }
        public string NomeDisciplina { get; set; } = string.Empty;

    }
}