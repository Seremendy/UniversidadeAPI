using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.DTOs
{
    // DTO para CRIAR uma nova turma (POST)
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

    // DTO para ATUALIZAR uma turma (PUT)
    // (Idêntico ao de Criar, mas é boa prática mantê-los separados)
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

    // DTO de RESPOSTA (GET)
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