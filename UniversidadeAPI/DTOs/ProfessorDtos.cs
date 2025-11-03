using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.DTOs
{
    // DTO para CRIAR um novo professor (POST)
    public class CreateProfessorRequestDto
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100)]
        public string ProfessorNome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        public DateOnly DataNascimento { get; set; }

        [Required(ErrorMessage = "O RG é obrigatório")]
        [StringLength(20)]
        public string RG { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter 11 caracteres")]
        public string CPF { get; set; } = string.Empty;

        [Required(ErrorMessage = "A formação é obrigatória")]
        [StringLength(100)]
        public string Formacao { get; set; } = string.Empty;
    }

    // DTO para ATUALIZAR um professor (PUT)
    public class UpdateProfessorRequestDto
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100)]
        public string ProfessorNome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        public DateOnly DataNascimento { get; set; }

        [Required(ErrorMessage = "O RG é obrigatório")]
        [StringLength(20)]
        public string RG { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter 11 caracteres")]
        public string CPF { get; set; } = string.Empty;

        [Required(ErrorMessage = "A formação é obrigatória")]
        [StringLength(100)]
        public string Formacao { get; set; } = string.Empty;
    }

    // DTO de RESPOSTA (GET)
    public class ProfessorResponseDto
    {
        public int ProfessorID { get; set; }
        public string ProfessorNome { get; set; } = string.Empty;
        public string Formacao { get; set; } = string.Empty;

        // NOTA DE SEGURANÇA:
        // Não incluímos DataNascimento, RG ou CPF na resposta padrão
        // para proteger os dados pessoais do professor.
    }
}