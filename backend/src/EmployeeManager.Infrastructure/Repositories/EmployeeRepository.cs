using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Interfaces;
using EmployeeManager.Domain.Shared;
using EmployeeManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
        }

        public void Update(Employee employee)
        {
            _context.Employees.Update(employee);
        }

        public async Task<Employee?> GetByIdAsync(Guid id)
            => await _context.Employees
                .Include(e => e.PhoneNumbers)
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<Employee?> GetByDocumentAsync(string document)
            => await _context.Employees
                .FirstOrDefaultAsync(e => e.DocumentNumber == document);

        public async Task<Employee?> GetByEmailAsync(string email)
            => await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == email);

        public async Task<PaginatedResponse<Employee>> GetAllAsync(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var totalItems = await _context.Employees.Where(e => e.Active).CountAsync();

            var items = await _context.Employees.Include(e => e.PhoneNumbers).Where(e => e.Active)
                .OrderBy(p => p.FirstName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResponse<Employee>
            {
                Items = items,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
