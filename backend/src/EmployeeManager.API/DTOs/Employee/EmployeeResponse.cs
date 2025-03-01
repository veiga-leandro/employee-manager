using EmployeeManager.Domain.Enums;

namespace EmployeeManager.Application.DTOs.Employee
{
    public sealed record EmployeeResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string CPF,
        string Email,
        DateTime BirthDate,
        List<string> PhoneNumbers,
        RoleType Role
    );
}
