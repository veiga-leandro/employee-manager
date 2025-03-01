using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Shared;

namespace EmployeeManager.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        Task AddAsync(Employee employee);
        void Update(Employee employee);
        Task<Employee?> GetByIdAsync(Guid id);
        Task<Employee?> GetByDocumentAsync(string document);
        Task<Employee?> GetByEmailAsync(string email);
        Task<PaginatedResponse<Employee>> GetAllAsync(int page, int pageSize);
        Task SaveChangesAsync();
    }
}
