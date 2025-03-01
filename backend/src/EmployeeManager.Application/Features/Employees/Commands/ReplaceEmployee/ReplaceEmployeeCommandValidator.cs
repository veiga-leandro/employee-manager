using FluentValidation;

namespace EmployeeManager.Application.Features.Employees.Commands.ReplaceEmployee
{
    public class ReplaceEmployeeCommandValidator : AbstractValidator<ReplaceEmployeeCommand>
    {
        public ReplaceEmployeeCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MaximumLength(100).WithMessage("Máximo de 100 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório")
                .EmailAddress().WithMessage("E-mail inválido");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres.");

            RuleFor(x => x.BirthDate)
                .Must((model, birthDate) => birthDate.AddYears(18).Date <= DateTime.UtcNow.Date)
                .WithMessage("A idade mínima é 18 anos.");

            RuleForEach(x => x.PhoneNumbers)
                .SetValidator(new PhoneNumberValidator());
        }
    }
}
