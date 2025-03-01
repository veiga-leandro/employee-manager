using EmployeeManager.Application.Models;
using EmployeeManager.Domain.Enums;
using MediatR;

namespace EmployeeManager.Application.Features.Employees.Commands.CreateEmployee
{
    public record CreateEmployeeCommand(
        string FirstName,
        string LastName,
        string Cpf,
        string Email,
        DateTime BirthDate,
        List<PhoneNumberResult> PhoneNumbers,
        RoleType Role,
        string Password,
        RoleType UserRole) : IRequest<EmployeeResult>;
}
