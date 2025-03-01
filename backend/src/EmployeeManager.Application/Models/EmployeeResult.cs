using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Enums;

namespace EmployeeManager.Application.Models
{
    public record EmployeeResult(
        Guid Id,
        string FirstName,
        string LastName,
        string FullName,
        string Cpf,
        string Email,
        DateTime BirthDate,
        DateTime CreatedAt,
        DateTime? UpdatedAt,
        List<PhoneNumberResult> PhoneNumbers,
        RoleType Role)
    {
        public static EmployeeResult FromEmployee(Employee employee)
       => new EmployeeResult(
                employee.Id,
                employee.FirstName,
                employee.LastName,
                $"{employee.FirstName} {employee.LastName}",
                employee.DocumentNumber,
                employee.Email,
                employee.BirthDate,
                employee.CreatedDate,
                employee.LastModifiedDate,
                PhoneNumbers: employee.PhoneNumbers.Select(p => new PhoneNumberResult(p.Number, p.IsActive)).ToList(),
                employee.Role);
    }

    public record PhoneNumberResult(
        string Number,
        bool IsActive);
}
