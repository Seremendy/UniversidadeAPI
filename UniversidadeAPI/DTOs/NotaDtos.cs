using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.DTOs
{
    public class CreateNotaRequestDto
    {
        [Required(ErrorMessage = "O valor da nota é obrigatório")]
        [Range(0.0, 10.0, ErrorMessage = "A nota deve estar entre 0.0 e 10.0")]
        public decimal NotaValor { get; set; }

        [Required(ErrorMessage = "O ID do Aluno é obrigatório")]
        [Range(1, int.MaxValue)]
        public int AlunoID { get; set; }

        [Required(ErrorMessage = "O ID da Disciplina é obrigatório")]
        [Range(1, int.MaxValue)]
        public int DisciplinaID { get; set; }
    }

    public class UpdateNotaRequestDto
    {
        [Required(ErrorMessage = "O valor da nota é obrigatório")]
        [Range(0.0, 10.0, ErrorMessage = "A nota deve estar entre 0.0 e 10.0")]
        public decimal NotaValor { get; set; }
    }

    public class NotaResponseDto
    {
        public int NotaID { get; set; }
        public decimal NotaValor { get; set; }
        public int AlunoID { get; set; }
        public int DisciplinaID { get; set; }

    }
}