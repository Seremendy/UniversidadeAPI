using FluentValidation;
using UniversidadeAPI.DTOs;

namespace UniversidadeAPI.Validators
{
    public class CreateTurmaValidator : AbstractValidator<CreateTurmaRequestDto>
    {
        public CreateTurmaValidator()
        {
            RuleFor(x => x.Semestre)
                .NotEmpty().WithMessage("O semestre é obrigatório.")
                .MaximumLength(10).WithMessage("O semestre não pode ter mais de 10 caracteres.")
                .Matches(@"^\d{4}\.\d$").WithMessage("O formato do semestre deve ser 'AAAA.S' (ex: 2025.2).");

            RuleFor(x => x.DisciplinaID).GreaterThan(0).WithMessage("ID da Disciplina inválido.");
            RuleFor(x => x.SalaDeAulaID).GreaterThan(0).WithMessage("ID da Sala inválido.");
            RuleFor(x => x.ProfessorID).GreaterThan(0).WithMessage("ID do Professor inválido.");
            RuleFor(x => x.HorarioID).GreaterThan(0).WithMessage("ID do Horário inválido.");
        }
    }
}