using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.DTOs
{
    
    public class CreateTurmaRequestDto
    {
        [Required(ErrorMessage = "O semestre é obrigatório (ex: 2025.2)")]
        [StringLength(10)]
        public string Semestre { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ID da Disciplina é obrigatório")]
        [Range(1, int.MaxValue)]
        public int DisciplinaID { get; set; }

        [Required(ErrorMessage = "O ID da Sala de Aula é obrigatório")]
        [Range(1, int.MaxValue)]
        public int SalaDeAulaID { get; set; }

        [Required(ErrorMessage = "O ID do Professor é obrigatório")]
        [Range(1, int.MaxValue)]
        public int ProfessorID { get; set; }

        [Required(ErrorMessage = "O ID do Horário é obrigatório")]
        [Range(1, int.MaxValue)]
        public int HorarioID { get; set; }
    }

    
    public class UpdateTurmaRequestDto
    {
        [Required(ErrorMessage = "O semestre é obrigatório (ex: 2025.2)")]
        [StringLength(10)]
        public string Semestre { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ID da Disciplina é obrigatório")]
        [Range(1, int.MaxValue)]
        public int DisciplinaID { get; set; }

        [Required(ErrorMessage = "O ID da Sala de Aula é obrigatório")]
        [Range(1, int.MaxValue)]
        public int SalaDeAulaID { get; set; }

        [Required(ErrorMessage = "O ID do Professor é obrigatório")]
        [Range(1, int.MaxValue)]
        public int ProfessorID { get; set; }

        [Required(ErrorMessage = "O ID do Horário é obrigatório")]
        [Range(1, int.MaxValue)]
        public int HorarioID { get; set; }
    }

    
    public class TurmaResponseDto
    {
        public int TurmaID { get; set; }
        public string Semestre { get; set; } = string.Empty;
        public int DisciplinaID { get; set; }
        public int SalaDeAulaID { get; set; }
        public int ProfessorID { get; set; }
        public int HorarioID { get; set; }

    }
}