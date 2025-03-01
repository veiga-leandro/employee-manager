using AutoMapper;
using EmployeeManager.Application.Features.Employees.Commands.ReplaceEmployee;
using EmployeeManager.Application.Models;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Enums;
using EmployeeManager.Domain.Exceptions;
using EmployeeManager.Domain.Interfaces;
using Moq;
using Xunit;

namespace EmployeeManager.Tests.Application.Features.Employees.Commands.ReplaceEmployee
{
    public class ReplaceEmployeeCommandHandlerTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ReplaceEmployeeCommandHandler _handler;

        public ReplaceEmployeeCommandHandlerTests()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _mapperMock = new Mock<IMapper>();

            var passwordServiceMock = new Mock<IPasswordService>();
            var currentUserServiceMock = new Mock<ICurrentUserService>();

            _handler = new ReplaceEmployeeCommandHandler(
                _employeeRepositoryMock.Object,
                passwordServiceMock.Object,
                _mapperMock.Object,
                currentUserServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenEmployeeNotFound()
        {
            // Arrange
            var command = new ReplaceEmployeeCommandWrapper
            {
                Id = Guid.NewGuid(),
                Command = new ReplaceEmployeeCommand
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    Password = "password123",
                    BirthDate = new DateTime(1990, 1, 1),
                    Role = RoleType.Employee,
                    Active = true,
                    PhoneNumbers = new List<PhoneNumberResult>()
                }
            };
            _employeeRepositoryMock.Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync((Employee)null);

            // Act & Assert
            await Assert.ThrowsAsync<DomainValidationException>(() => _handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_ShouldReplaceEmployee_WhenEmployeeExists()
        {
            // Arrange
            var employee = new Employee();
            var command = new ReplaceEmployeeCommandWrapper
            {
                Id = employee.Id,
                Command = new ReplaceEmployeeCommand
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    Password = "password123",
                    BirthDate = new DateTime(1990, 1, 1),
                    Role = RoleType.Employee,
                    Active = true,
                    PhoneNumbers = new List<PhoneNumberResult>()
                }
            };
            _employeeRepositoryMock.Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync(employee);
            _mapperMock.Setup(x => x.Map(command.Command, employee));

            // Act
            await _handler.Handle(command, default);

            // Assert
            _employeeRepositoryMock.Verify(x => x.Update(employee), Times.Once);
            _employeeRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
