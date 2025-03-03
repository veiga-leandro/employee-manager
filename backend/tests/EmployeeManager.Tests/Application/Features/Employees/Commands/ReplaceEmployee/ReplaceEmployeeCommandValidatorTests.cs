using EmployeeManager.Application.Features.Employees.Commands.ReplaceEmployee;
using EmployeeManager.Application.Models;
using FluentValidation.TestHelper;
using Xunit;

namespace EmployeeManager.Tests.Application.Features.Employees.Commands.ReplaceEmployee
{
    public class ReplaceEmployeeCommandValidatorTests
    {
        private readonly ReplaceEmployeeCommandValidator _validator;

        public ReplaceEmployeeCommandValidatorTests()
        {
            _validator = new ReplaceEmployeeCommandValidator();
        }

        [Fact]
        public void ShouldHaveErrorWhenFirstNameIsEmpty()
        {
            var command = new ReplaceEmployeeCommand
            {
                FirstName = string.Empty,
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "password",
                BirthDate = DateTime.Now.AddYears(-30),
                Role = Domain.Enums.RoleType.Admin,
                Active = true,
                PhoneNumbers = new List<PhoneNumberResult> { new PhoneNumberResult("123456789", Domain.Enums.PhoneNumberType.Mobile, true) }
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void ShouldHaveErrorWhenEmailIsInvalid()
        {
            var command = new ReplaceEmployeeCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "invalid-email",
                Password = "password",
                BirthDate = DateTime.Now.AddYears(-30),
                Role = Domain.Enums.RoleType.Admin,
                Active = true,
                PhoneNumbers = new List<PhoneNumberResult> { new PhoneNumberResult("123456789", Domain.Enums.PhoneNumberType.Mobile, true) }
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void ShouldHaveErrorWhenPasswordIsEmpty()
        {
            var command = new ReplaceEmployeeCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = string.Empty,
                BirthDate = DateTime.Now.AddYears(-30),
                Role = Domain.Enums.RoleType.Admin,
                Active = true,
                PhoneNumbers = new List<PhoneNumberResult> { new PhoneNumberResult("123456789", Domain.Enums.PhoneNumberType.Mobile, true) }
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void ShouldHaveErrorWhenPasswordIsLessThan8Characters()
        {
            var command = new ReplaceEmployeeCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "short",
                BirthDate = DateTime.Now.AddYears(-30),
                Role = Domain.Enums.RoleType.Admin,
                Active = true,
                PhoneNumbers = new List<PhoneNumberResult> { new PhoneNumberResult("123456789", Domain.Enums.PhoneNumberType.Mobile, true) }
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void ShouldHaveErrorWhenBirthDateIndicatesAgeLessThan18()
        {
            var command = new ReplaceEmployeeCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "password",
                BirthDate = DateTime.Now.AddYears(-17),
                Role = Domain.Enums.RoleType.Admin,
                Active = true,
                PhoneNumbers = new List<PhoneNumberResult> { new PhoneNumberResult("123456789", Domain.Enums.PhoneNumberType.Mobile, true) }
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.BirthDate);
        }

    }
}
