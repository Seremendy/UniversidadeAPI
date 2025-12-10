using System.ComponentModel.DataAnnotations;

namespace UniversidadeAPI.DTOs
{
    
    public class CreateSalaDeAulaRequestDto
    {
        [Required(ErrorMessage = "A capacidade é obrigatória")]
        [Range(1, 500, ErrorMessage = "A capacidade deve ser entre 1 e 500")]
        public int Capacidade { get; set; }

        [Required(ErrorMessage = "O número da sala é obrigatório")]
        [Range(1, 9999, ErrorMessage = "Número de sala inválido")]
        public int NumeroSala { get; set; }

        [Required(ErrorMessage = "O nome do prédio é obrigatório")]
        [StringLength(50, ErrorMessage = "O nome do prédio não pode ter mais de 50 caracteres")]
        public string PredioNome { get; set; } = string.Empty;
    }

    
    public class UpdateSalaDeAulaRequestDto
    {
        [Required(ErrorMessage = "A capacidade é obrigatória")]
        [Range(1, 500, ErrorMessage = "A capacidade deve ser entre 1 e 500")]
        public int Capacidade { get; set; }

        [Required(ErrorMessage = "O número da sala é obrigatório")]
        [Range(1, 9999, ErrorMessage = "Número de sala inválido")]
        public int NumeroSala { get; set; }

        [Required(ErrorMessage = "O nome do prédio é obrigatório")]
        [StringLength(50, ErrorMessage = "O nome do prédio não pode ter mais de 50 caracteres")]
        public string PredioNome { get; set; } = string.Empty;
    }

    
    public class SalaDeAulaResponseDto
    {
        public int SalaDeAulaID { get; set; }
        public int Capacidade { get; set; }
        public int NumeroSala { get; set; }
        public string PredioNome { get; set; } = string.Empty;
    }
}