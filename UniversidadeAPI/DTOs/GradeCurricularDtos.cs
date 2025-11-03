using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.DTOs
{
    // DTO usado para CRIAR uma nova relação
    // (Ex: "Adicionar a disciplina 'Cálculo I' ao curso 'Engenharia'")
    public class CreateGradeRequestDto
    {
        [Required(ErrorMessage = "O ID da Disciplina é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "ID da Disciplina inválido")]
        public int DisciplinaID { get; set; }

        [Required(ErrorMessage = "O ID do Curso é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "ID do Curso inválido")]
        public int CursoID { get; set; }
    }

    public class GradeCurricularResponseDto
    {
        public int GradeCurricularID { get; set; }
        public int DisciplinaID { get; set; }
        public int CursoID { get; set; }


    }
}