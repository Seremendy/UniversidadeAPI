using System.Text.Json.Serialization;

namespace UniversidadeAPI.Models
{
    public enum Role
    { 
        Admin,
        Professor,
        Aluno
    }

    public class Usuarios
    {
        public int UsuarioID { get; set; }

        public string Login { get; set; } = string.Empty;

        [JsonIgnore]
        public string SenhaHash { get; set; } = string.Empty;

        public Role Role { get; set; }
    }
}
