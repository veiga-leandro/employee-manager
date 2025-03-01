using EmployeeManager.Application.Features.Employees.Commands.DeleteEmployee;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using EmployeeManager.Domain.Interfaces;
using Moq;
using Xunit;

namespace EmployeeManager.Tests.Application.Features.Employees.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommandHandlerTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly DeleteEmployeeCommandHandler _handler;

        public DeleteEmployeeCommandHandlerTests()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _handler = new DeleteEmployeeCommandHandler(_employeeRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenEmployeeNotFound()
        {
            // Arrange
            var command = new DeleteEmployeeCommand(Guid.NewGuid());
            _employeeRepositoryMock.Setup(x => x.GetByIdAsync(command.EmployeeId))
                .ReturnsAsync((Employee)null);

            // Act & Assert
            await Assert.ThrowsAsync<DomainValidationException>(() => _handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_ShouldDeactivateEmployee_WhenEmployeeExists()
        {
            // Arrange
            var employee = new Employee();
            var command = new DeleteEmployeeCommand(employee.Id);
            _employeeRepositoryMock.Setup(x => x.GetByIdAsync(command.EmployeeId))
                .ReturnsAsync(employee);

            // Act
            await _handler.Handle(command, default);

            // Assert
            Assert.False(employee.Active);
            _employeeRepositoryMock.Verify(x => x.Update(employee), Times.Once);
            _employeeRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
