namespace EmployeeManager.Application.DTOs.Employee
{
    public record CreateEmployeeDto(
        string FirstName,
        string LastName,
        string DocumentNumber,
        string Email,
        DateTime BirthDate,
        List<string> PhoneNumbers,
        Guid? ManagerId,
        string Password
    );
}
