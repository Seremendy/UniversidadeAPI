namespace UniversidadeAPI.DTOs.Auth
{
    public class UsuarioResponseDto
    {
        public int UsuarioID { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
