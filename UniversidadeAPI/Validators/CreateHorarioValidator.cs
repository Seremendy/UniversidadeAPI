using FluentValidation;
using UniversidadeAPI.DTOs;
using UniversidadeAPI.Entities;

namespace UniversidadeAPI.Validators
{
    public class CreateHorarioValidator : AbstractValidator<CreateHorarioRequestDto>
    {
        public CreateHorarioValidator()
        {
            RuleFor(x => x.DiaSemana)
                .NotEmpty().WithMessage("O Dia da Semana é obrigatório.")
                .IsEnumName(typeof(DiaSemana), caseSensitive: false)
                .WithMessage("Dia da semana inválido. Valores permitidos: Segunda, Terca, Quarta, Quinta, Sexta.");

            RuleFor(x => x.HoraInicio)
                .NotEmpty().WithMessage("A hora de início é obrigatória.");

            RuleFor(x => x.HoraFim)
                .NotEmpty().WithMessage("A hora de fim é obrigatória.")
                .GreaterThan(x => x.HoraInicio).WithMessage("A hora de fim deve ser posterior à hora de início.");
        }
    }
}