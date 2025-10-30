using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.DTOs.Auth
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "O Login � obrigat�rio.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "A Senha � obrigat�ria.")]
        public string Senha { get; set; }
    }
}