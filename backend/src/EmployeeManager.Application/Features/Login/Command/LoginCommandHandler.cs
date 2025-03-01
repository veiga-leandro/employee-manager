using EmployeeManager.Domain.Exceptions;
using EmployeeManager.Domain.Interfaces;
using MediatR;

namespace EmployeeManager.Application.Features.Login.Command
{
    public class LoginCommandHandler(
        IEmployeeRepository employeeRepository,
        IPasswordService passwordService,
        ITokenService tokenService)
        : IRequestHandler<LoginCommand, string>
    {
        public async Task<string> Handle(
            LoginCommand request, CancellationToken cancellationToken)
        {
            var employee = await employeeRepository.GetByEmailAsync(request.Email);
            if (employee is null || !passwordService.Verify(request.Password, employee.PasswordHash))
                throw new DomainValidationException("Credenciais inválidas.");

            if (!employee.Active)
                throw new DomainValidationException("Usuário desativado.");

            return tokenService.GenerateToken(employee);
        }
    }
}
