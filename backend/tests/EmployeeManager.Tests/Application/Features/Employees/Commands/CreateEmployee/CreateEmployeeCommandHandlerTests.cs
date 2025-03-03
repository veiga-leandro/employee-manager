using AutoMapper;
using EmployeeManager.Application.Features.Employees.Commands.CreateEmployee;
using EmployeeManager.Application.Mappings;
using EmployeeManager.Application.Models;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Enums;
using EmployeeManager.Domain.Exceptions;
using EmployeeManager.Domain.Interfaces;
using Moq;
using Xunit;

namespace EmployeeManager.Tests.Application.Features.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommandHandlerTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly IMapper _mapper;
        private readonly CreateEmployeeCommandHandler _handler;

        public CreateEmployeeCommandHandlerTests()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            _mapper = configuration.CreateMapper();

            _handler = new CreateEmployeeCommandHandler(
                _employeeRepositoryMock.Object,
                _passwordServiceMock.Object,
                _mapper,
                _currentUserServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenCpfAlreadyExists()
        {
            // Arrange
            var command = new CreateEmployeeCommand
            (
                "John",
                "Doe",
                "12345678900",
                "john.doe@example.com",
                DateTime.Now,
                null,
                new List<PhoneNumberResult>(),
                RoleType.Employee,
                "password",
                RoleType.Employee
            );
            _employeeRepositoryMock.Setup(x => x.GetByDocumentAsync(command.Cpf))
                .ReturnsAsync(new Employee());

            // Act & Assert
            await Assert.ThrowsAsync<DomainValidationException>(() => _handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_ShouldCreateEmployee_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateEmployeeCommand
            (
                "John",
                "Doe",
                "12345678900",
                "john.doe@example.com",
                DateTime.Now,
                null,
                new List<PhoneNumberResult> { new PhoneNumberResult("123456789", PhoneNumberType.Mobile, true) },
                RoleType.Employee,
                "password",
                RoleType.Employee
            );

            _employeeRepositoryMock.Setup(x => x.GetByDocumentAsync(command.Cpf))
                .ReturnsAsync((Employee)null);
            _passwordServiceMock.Setup(x => x.Hash(command.Password))
                .Returns("hashedpassword");
            _currentUserServiceMock.Setup(x => x.GetCurrentUserEmail())
                .Returns("test@example.com");

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            _employeeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Employee>()), Times.Once);
            _employeeRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            Assert.NotNull(result);
        }
    }
}