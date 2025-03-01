namespace EmployeeManager.Application.DTOs.Employee
{
    public record EmployeeResponseDto(
        Guid Id,
        string FirstName,
        string LastName,
        string DocumentNumber,
        string Email,
        string Role,
        List<string> PhoneNumbers
    );
}
