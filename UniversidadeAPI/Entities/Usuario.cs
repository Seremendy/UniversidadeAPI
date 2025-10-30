using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UniversidadeAPI.Entities
{
    public enum Role
    { 
        Admin,
        Professor,
        Aluno
    }

    public class Usuario
    {
        public int UsuarioID { get; set; }

        [Required]
        [StringLength(100)]
        public string Login { get; set; } = string.Empty;

        [Required]
        public Role Role { get; set; } 


        [JsonIgnore] 
        public string SenhaHash { get; set; } = string.Empty;
    }
}
