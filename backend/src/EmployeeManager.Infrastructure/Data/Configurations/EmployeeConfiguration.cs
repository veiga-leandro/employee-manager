using EmployeeManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManager.Infrastructure.Data.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(e => e.LastName).HasMaxLength(100).IsRequired();
            builder.Property(e => e.DocumentNumber).HasMaxLength(11).IsRequired();
            builder.Property(e => e.Email).HasMaxLength(200).IsRequired();
            builder.Property(e => e.Active).IsRequired().HasDefaultValue(true);

            builder.HasIndex(c => c.DocumentNumber).IsUnique();

            // Relacionamento com Manager (auto-relacionamento)
            builder.HasOne(e => e.Manager)
                .WithMany()
                .HasForeignKey("ManagerId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // OwnsMany para PhoneNumbers (objeto de valor)
            builder.OwnsMany(e => e.PhoneNumbers, p =>
            {
                p.ToTable("PhoneNumbers");
                p.WithOwner().HasForeignKey("EmployeeId");
                p.Property<int>("Id").ValueGeneratedOnAdd();
                p.HasKey("Id");
                p.Property(pn => pn.Number).HasMaxLength(20).IsRequired();
            });
        }
    }
}
