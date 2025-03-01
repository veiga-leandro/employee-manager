using EmployeeManager.Application.DTOs.Employee;

namespace EmployeeManager.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<Guid> CreateEmployeeAsync(CreateEmployeeDto dto);
        Task<EmployeeResponseDto> GetByIdAsync(Guid id);
        Task DeleteEmployeeAsync(Guid id);
    }
}
