using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.DTOs
{
    
    public class CreateTurmaRequestDto
    {
        public string Semestre { get; set; } = string.Empty;
        public int DisciplinaID { get; set; }
        public int SalaDeAulaID { get; set; }
        public int ProfessorID { get; set; }
        public int HorarioID { get; set; }
    }

    
    public class UpdateTurmaRequestDto
    {
        public string Semestre { get; set; } = string.Empty;
        public int DisciplinaID { get; set; }
        public int SalaDeAulaID { get; set; }
        public int ProfessorID { get; set; }
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