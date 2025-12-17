using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.DTOs
{
    public class AlunoRequestDto
    {
        [Required(ErrorMessage = "O Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string NomeAluno { get; set; } = string.Empty;

        public string RG { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório")]
        [StringLength(14, ErrorMessage = "O CPF deve ter no máximo 14 caracteres")]
        public string CPF { get; set; } = string.Empty;

        public DateTime DataNascimento { get; set; }
    }

    public class AlunoResponseDto
    {
        public int AlunoID { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string RG { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;

        public DateTime DataNascimento { get; set; }
    }

    public class CreateAlunoRequestDto
    {
        [Required(ErrorMessage = "O Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string NomeAluno { get; set; } = string.Empty;
        public string RG { get; set; } = string.Empty;
        [Required(ErrorMessage = "O CPF é obrigatório")]
        [StringLength(14, ErrorMessage = "O CPF deve ter no máximo 14 caracteres")]
        public string CPF { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
    }
}