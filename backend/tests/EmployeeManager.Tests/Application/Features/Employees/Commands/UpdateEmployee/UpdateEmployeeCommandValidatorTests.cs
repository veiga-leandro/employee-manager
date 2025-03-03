using EmployeeManager.Application.Features.Employees.Commands.UpdateEmployee;
using EmployeeManager.Application.Models;
using FluentValidation.TestHelper;
using Xunit;

namespace EmployeeManager.Tests.Application.Features.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandValidatorTests
    {
        private readonly UpdateEmployeeCommandValidator _validator;

        public UpdateEmployeeCommandValidatorTests()
        {
            _validator = new UpdateEmployeeCommandValidator();
        }

        [Fact]
        public void ShouldHaveErrorWhenFirstNameIsEmpty()
        {
            var command = new UpdateEmployeeCommand
            {
                FirstName = string.Empty,
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "password",
                BirthDate = DateTime.Now.AddYears(-30),
                PhoneNumbers = new List<PhoneNumberResult> { new PhoneNumberResult("123456789", Domain.Enums.PhoneNumberType.Mobile, true) }
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void ShouldHaveErrorWhenEmailIsInvalid()
        {
            var command = new UpdateEmployeeCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "invalid-email",
                Password = "password",
                BirthDate = DateTime.Now.AddYears(-30),
                PhoneNumbers = new List<PhoneNumberResult> { new PhoneNumberResult("123456789", Domain.Enums.PhoneNumberType.Mobile, true) }
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void ShouldHaveErrorWhenPasswordIsEmpty()
        {
            var command = new UpdateEmployeeCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = string.Empty,
                BirthDate = DateTime.Now.AddYears(-30),
                PhoneNumbers = new List<PhoneNumberResult> { new PhoneNumberResult("123456789", Domain.Enums.PhoneNumberType.Mobile, true) }
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void ShouldHaveErrorWhenPasswordIsLessThan8Characters()
        {
            var command = new UpdateEmployeeCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "short",
                BirthDate = DateTime.Now.AddYears(-30),
                PhoneNumbers = new List<PhoneNumberResult> { new PhoneNumberResult("123456789", Domain.Enums.PhoneNumberType.Mobile, true) }
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void ShouldHaveErrorWhenBirthDateIndicatesAgeLessThan18()
        {
            var command = new UpdateEmployeeCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "password",
                BirthDate = DateTime.Now.AddYears(-17),
                PhoneNumbers = new List<PhoneNumberResult> { new PhoneNumberResult("123456789", Domain.Enums.PhoneNumberType.Mobile, true) }
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.BirthDate);
        }

        [Fact]
        public void ShouldHaveErrorWhenPhoneNumbersAreInvalid()
        {
            var command = new UpdateEmployeeCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "password",
                BirthDate = DateTime.Now.AddYears(-30),
                PhoneNumbers = new List<PhoneNumberResult> { new PhoneNumberResult("invalid-phone", Domain.Enums.PhoneNumberType.Mobile, false) }
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor("PhoneNumbers[0].Number");
        }
    }
}
