using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Enums;
using EmployeeManager.Infrastructure.Data;

namespace EmployeeManager.Infrastructure.Seed
{
    public static class SeedData
    {
        public static async Task Initialize(AppDbContext context)
        {
            if (!context.Employees.Any())
            {
                // Admin
                var admin = new Employee(
                    "Admin",
                    "User",
                    "12345678901",
                    "admin@empresa.com",
                    new DateTime(1990, 1, 1),
                    BCrypt.Net.BCrypt.EnhancedHashPassword("manager123", 12),
                    new List<PhoneNumber> { new PhoneNumber("+5511999999999", true) },
                    RoleType.Admin,
                    "UserTest");

                await context.Employees.AddAsync(admin);
                await context.SaveChangesAsync();
            }
        }
    }
}
