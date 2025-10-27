using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.Models
{
    public class RegisterRequestModel
    {
        [Required(ErrorMessage = "O Login é obrigatório.")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        public string SenhaHash { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Role é obrigatório.")]
        public string Role { get; set; } = string.Empty;
    }
}