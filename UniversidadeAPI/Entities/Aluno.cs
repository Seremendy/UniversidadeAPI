using System.ComponentModel.DataAnnotations; 

namespace UniversidadeAPI.Entities
{
    public class Aluno
    {
        public int AlunoID { get; set; }

        [Required(ErrorMessage = "O nome do aluno é obrigatório")]
        [StringLength(150, ErrorMessage = "O nome não pode ter mais de 150 caracteres")]
        public string AlunoNome { get; set; } = string.Empty;

        public DateOnly DataNascimento { get; set; }

        [Required(ErrorMessage = "O RG é obrigatório")]
        [StringLength(9)]
        public string RG { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter 11 caracteres")]
        public string CPF { get; set; } = string.Empty;
    }
}