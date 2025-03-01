using EmployeeManager.Application.Models;
using EmployeeManager.Domain.Enums;
using MediatR;

namespace EmployeeManager.Application.Features.Employees.Commands.UpdateEmployee
{
    public record UpdateEmployeeCommand : IRequest<Unit>
    {
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? Email { get; init; }
        public string? Password { get; init; }
        public DateTime? BirthDate { get; init; }
        public RoleType? Role { get; init; }
        public bool? Active { get; init; }
        public ICollection<PhoneNumberResult>? PhoneNumbers { get; init; }
    }
}
