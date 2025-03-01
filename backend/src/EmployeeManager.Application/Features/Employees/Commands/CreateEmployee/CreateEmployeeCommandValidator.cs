using EmployeeManager.Domain.Shared.Utils;
using FluentValidation;

namespace EmployeeManager.Application.Features.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MaximumLength(100).WithMessage("Máximo de 100 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório")
                .EmailAddress().WithMessage("E-mail inválido");

            RuleFor(x => x.Cpf)
                .Must(DocumentValidator.IsCpf)
                .WithMessage("CPF inválido");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres.");

            RuleFor(x => x.Role)
                .Must((model, role) => (int)role >= (int)model.UserRole)
                .WithMessage("A Role deve ser maior ou igual que a sua role.");

            RuleFor(x => x.BirthDate)
                .Must((model, birthDate) => birthDate.AddYears(18).Date <= DateTime.UtcNow.Date)
                .WithMessage("A idade mínima é 18 anos.");

            RuleForEach(x => x.PhoneNumbers)
                .SetValidator(new PhoneNumberValidator());
        }
    }
}
