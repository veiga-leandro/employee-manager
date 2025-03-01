using EmployeeManager.Application.Models;
using EmployeeManager.Domain.Enums;
using MediatR;

namespace EmployeeManager.Application.Features.Employees.Commands.ReplaceEmployee
{
    public record ReplaceEmployeeCommand : IRequest
    {
        public ReplaceEmployeeCommand() { }

        public ReplaceEmployeeCommand(string firstName, string lastName, string email, string password, DateTime birthDate, RoleType role, bool active, ICollection<PhoneNumberResult> phoneNumbers)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            BirthDate = birthDate;
            Role = role;
            Active = active;
            PhoneNumbers = phoneNumbers;
        }

        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
        public required DateTime BirthDate { get; init; }
        public required RoleType Role { get; init; }
        public required bool Active { get; init; }
        public required ICollection<PhoneNumberResult> PhoneNumbers { get; init; }
    }
}
