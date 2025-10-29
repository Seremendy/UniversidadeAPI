using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.Models
{
    public class RegisterRequestModel
    {
        [Required(ErrorMessage = "O Login é obrigatório.")]
        [MinLength(8, ErrorMessage = "A Login deve ter no mínimo 8 caracteres.")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Senha é obrigatória.")]
        [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Role é obrigatório.")]
        public string Role { get; set; } = string.Empty;
    }
}