using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.Entities
{
    public class GradeCurriculares
    {
        public int GradeCurricularID { get; set; }

        [Required]
        public int DisciplinaID { get; set; }

        [Required]
        public int CursoID { get; set; }
        
    }
}
