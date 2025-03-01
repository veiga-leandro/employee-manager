using EmployeeManager.Domain.Enums;

namespace EmployeeManager.Domain.Entities
{
    public class Employee : AuditableEntity
    {
        public Employee() { }
        public Employee(string firstName, string lastName, string documentNumber, string email, DateTime birthDate, string passwordHash, ICollection<PhoneNumber> phoneNumbers, RoleType role, string createdBy)
        {
            FirstName = firstName;
            LastName = lastName;
            DocumentNumber = documentNumber;
            Email = email;
            BirthDate = birthDate;
            PasswordHash = passwordHash;
            Role = role;
            PhoneNumbers = phoneNumbers;
            CreatedDate = DateTime.Now;
            CreatedBy = createdBy;
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DocumentNumber { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string PasswordHash { get; set; }
        public Guid? ManagerId { get; set; }
        public virtual Employee Manager { get; set; }
        public RoleType Role { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }
    }
}