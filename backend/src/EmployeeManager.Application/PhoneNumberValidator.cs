using EmployeeManager.Application.Models;
using FluentValidation;

namespace EmployeeManager.Application
{
    public class PhoneNumberValidator : AbstractValidator<PhoneNumberResult>
    {
        public PhoneNumberValidator()
        {
            RuleFor(x => x.Number)
                .NotEmpty()
                .Matches(@"^\+?[0-9\s]{8,15}$").WithMessage("Número inválido");
        }
    }
}
