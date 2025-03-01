using AutoMapper;
using EmployeeManager.Application.Features.Employees.Commands.UpdateEmployee;
using EmployeeManager.Application.Mappings;
using EmployeeManager.Application.Models;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Enums;
using EmployeeManager.Domain.Exceptions;
using EmployeeManager.Domain.Interfaces;
using Moq;
using Xunit;

namespace EmployeeManager.Tests.Application.Features.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandHandlerTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly IMapper _mapper;
        private readonly UpdateEmployeeCommandHandler _handler;

        public UpdateEmployeeCommandHandlerTests()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            _mapper = configuration.CreateMapper();

            _handler = new UpdateEmployeeCommandHandler(
                _employeeRepositoryMock.Object,
                _passwordServiceMock.Object,
                _mapper,
                _currentUserServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenEmployeeNotFound()
        {
            // Arrange
            var command = new UpdateEmployeeCommandWrapper
            {
                Id = Guid.NewGuid(),
                Command = new UpdateEmployeeCommand()
            };
            _employeeRepositoryMock.Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync((Employee)null);

            // Act & Assert
            await Assert.ThrowsAsync<DomainValidationException>(() => _handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_ShouldUpdateEmployee_WhenEmployeeExists()
        {
            // Arrange
            var employee = new Employee();
            var birthDate = DateTime.Now.AddYears(-30);

            var command = new UpdateEmployeeCommandWrapper
            {
                Id = employee.Id,
                Command = new UpdateEmployeeCommand
                {
                    FirstName = "UpdatedFirstName",
                    LastName = "UpdatedLastName",
                    Email = "updated.email@example.com",
                    Password = "newpassword",
                    Role = RoleType.Manager,
                    Active = true,
                    BirthDate = birthDate,
                    PhoneNumbers = new List<PhoneNumberResult> { new PhoneNumberResult("123456789", true) }
                }
            };
            _employeeRepositoryMock.Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync(employee);
            _passwordServiceMock.Setup(x => x.Hash(command.Command.Password))
                .Returns("hashedpassword");
            _currentUserServiceMock.Setup(x => x.GetCurrentUserEmail())
                .Returns("test@example.com");

            // Act
            await _handler.Handle(command, default);

            // Assert
            Assert.Equal("UpdatedFirstName", employee.FirstName);
            Assert.Equal("UpdatedLastName", employee.LastName);
            Assert.Equal("updated.email@example.com", employee.Email);
            Assert.Equal("hashedpassword", employee.PasswordHash);
            Assert.Equal(RoleType.Manager, employee.Role);
            Assert.True(employee.Active);
            Assert.Equal(birthDate, employee.BirthDate);
            Assert.Single(employee.PhoneNumbers);
            Assert.Equal("123456789", employee.PhoneNumbers.First().Number);
            _employeeRepositoryMock.Verify(x => x.Update(employee), Times.Once);
            _employeeRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
