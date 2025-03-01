using FluentValidation;

namespace EmployeeManager.Application.Features.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidator()
        {
            RuleFor(x => x)
                .Must(HasAtLeastOneField)
                .WithMessage("Pelo menos um campo deve ser informado para atualização.");

            // Regras individuais (aplicadas apenas se o campo for preenchido)
            When(x => x.FirstName != null, () =>
            {
                RuleFor(x => x.FirstName)
                    .NotEmpty().WithMessage("FirstName não pode ser vazio.")
                    .MaximumLength(100).WithMessage("Máximo de 100 caracteres");
            });

            When(x => x.LastName != null, () =>
            {
                RuleFor(x => x.FirstName)
                    .NotEmpty().WithMessage("LastName não pode ser vazio.")
                    .MaximumLength(100).WithMessage("Máximo de 100 caracteres");
            });

            When(x => x.Email != null, () =>
            {
                RuleFor(x => x.Email)
                    .EmailAddress().WithMessage("E-mail inválido.");
            });

            When(x => x.Role != null, () =>
            {
                RuleFor(x => x.Role)
                    .IsInEnum().WithMessage("Role inválida.");
            });

            When(x => x.Password != null, () =>
            {
                RuleFor(x => x.Password)
                    .MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres.");
            });

            When(x => x.BirthDate != null, () =>
            {
                RuleFor(x => x.BirthDate)
                    .Must((model, birthDate) => birthDate.Value.AddYears(18).Date <= DateTime.UtcNow.Date)
                    .WithMessage("A idade mínima é 18 anos.");
            });

            When(x => x.PhoneNumbers != null, () =>
            {
                RuleFor(x => x.PhoneNumbers)
                    .ForEach(p => p.SetValidator(new PhoneNumberValidator()));
            });
        }

        private bool HasAtLeastOneField(UpdateEmployeeCommand command)
        {
            return command.FirstName != null ||
                   command.LastName != null ||
                   command.Email != null ||
                   command.Password != null ||
                   command.BirthDate != null ||
                   command.Role != null ||
                   command.Active != null ||
                   command.PhoneNumbers != null;
        }
    }
}
