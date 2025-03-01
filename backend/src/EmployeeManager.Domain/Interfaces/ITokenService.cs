using EmployeeManager.Domain.Entities;

namespace EmployeeManager.Domain.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Employee employee);
        Guid? ReadUserIdFromToken(string token);
    }
}
