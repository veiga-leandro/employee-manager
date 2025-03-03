using EmployeeManager.Application.Models;
using EmployeeManager.Domain.Entities;

namespace EmployeeManager.Application.Extensions
{
    public static class EmployeeExtensions
    {
        public static EmployeeResult ToEmployeeResult(this Employee employee)
        {
            return new EmployeeResult(
                employee.Id,
                employee.FirstName,
                employee.LastName,
                $"{employee.FirstName} {employee.LastName}",
                employee.DocumentNumber, 
                employee.Email, 
                employee.ManagerId,
                employee.BirthDate,
                employee.CreatedDate,
                employee.LastModifiedDate,
                employee.PhoneNumbers.Select(p => new PhoneNumberResult(p.Number, p.Type, p.IsActive)).ToList(),
                employee.Role);
        }
    }
}
