using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.Entities
{
    public class Matricula
    {
        public int MatriculaID { get; set; }

        [Required]
        public int AlunoID { get; set; }

        [Required]
        public int CursoID { get; set; }

        [Required]
        public DateTime DataMatricula { get; set; }

        [Required]
        public bool MatriculaAtiva { get; set; }
    }
}
