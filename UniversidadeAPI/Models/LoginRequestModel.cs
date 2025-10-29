using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.Models
{
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "O Login � obrigat�rio.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "A Senha � obrigat�ria.")]
        public string Senha { get; set; }
    }
}