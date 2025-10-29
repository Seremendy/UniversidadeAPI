using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.Models
{
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "O Login é obrigatório.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "A Senha é obrigatória.")]
        public string Senha { get; set; }
    }
}