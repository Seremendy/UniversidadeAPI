using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.Entities
{
    public class Prerequisitos
    {
        public int DisciplinaID { get; set; }

        [Required]
        public int PreRequisitoID { get; set; }
    }
}
