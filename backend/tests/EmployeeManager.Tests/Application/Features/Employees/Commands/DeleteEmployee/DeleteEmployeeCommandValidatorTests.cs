using EmployeeManager.Application.Features.Employees.Commands.DeleteEmployee;
using FluentValidation.TestHelper;
using Xunit;

namespace EmployeeManager.Tests.Application.Features.Employees.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommandValidatorTests
    {
        private readonly DeleteEmployeeCommandValidator _validator;

        public DeleteEmployeeCommandValidatorTests()
        {
            _validator = new DeleteEmployeeCommandValidator();
        }

        [Fact]
        public void ShouldNotHaveAnyValidationErrors()
        {
            var command = new DeleteEmployeeCommand(Guid.NewGuid());

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
