using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.DTOs
{
    public class CreatePrerequisitoRequestDto
    {
        [Required(ErrorMessage = "O ID da Disciplina (principal) é obrigatório")]
        [Range(1, int.MaxValue)]
        public int DisciplinaID { get; set; }

        [Required(ErrorMessage = "O ID da Disciplina (pré-requisito) é obrigatório")]
        [Range(1, int.MaxValue)]
        public int PreRequisitoID { get; set; }
    }

    public class PrerequisitoResponseDto
    {
        public int DisciplinaID { get; set; }
        public int PreRequisitoID { get; set; }

    }
}