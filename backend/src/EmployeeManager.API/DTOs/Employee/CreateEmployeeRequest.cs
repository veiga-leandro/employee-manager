using EmployeeManager.Application.Models;
using EmployeeManager.Domain.Enums;

namespace EmployeeManager.API.DTOs.Employee
{
    public sealed record CreateEmployeeRequest(
        string FirstName,
        string LastName,
        string CPF,
        string Email,
        DateTime BirthDate,
        Guid ManagerId,
        List<PhoneNumberResult> PhoneNumbers,
        RoleType Role,
        string Password
    );
}
