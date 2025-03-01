using EmployeeManager.Domain.Entities;
using EmployeeManager.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<PhoneNumber> PhoneNumbers => Set<PhoneNumber>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}