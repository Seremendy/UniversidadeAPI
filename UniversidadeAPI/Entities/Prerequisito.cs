using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.Entities
{
    public class Prerequisito
    {
        public int DisciplinaID { get; set; }

        [Required]
        public int PreRequisitoID { get; set; }
    }
}
