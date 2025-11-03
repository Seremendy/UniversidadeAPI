using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.DTOs
{
    public class CreateMatriculaRequestDto
    {
        [Required(ErrorMessage = "O ID do Aluno é obrigatório")]
        [Range(1, int.MaxValue)]
        public int AlunoID { get; set; }

        [Required(ErrorMessage = "O ID do Curso é obrigatório")]
        [Range(1, int.MaxValue)]
        public int CursoID { get; set; }
    }

    public class UpdateMatriculaStatusRequestDto
    {
        [Required(ErrorMessage = "O status da matrícula é obrigatório")]
        public bool MatriculaAtiva { get; set; }
    }


    public class MatriculaResponseDto
    {
        public int MatriculaID { get; set; }
        public int AlunoID { get; set; }
        public int CursoID { get; set; }
        public DateTime DataMatricula { get; set; }
        public bool MatriculaAtiva { get; set; }

    }
}