using EmployeeManager.Application.Features.Employees.Commands.CreateEmployee;
using EmployeeManager.Application.Models;
using EmployeeManager.Domain.Enums;
using FluentValidation.TestHelper;
using Xunit;

namespace EmployeeManager.Tests.Application.Features.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommandValidatorTests
    {
        private readonly CreateEmployeeCommandValidator _validator;

        public CreateEmployeeCommandValidatorTests()
        {
            _validator = new CreateEmployeeCommandValidator();
        }

        [Fact]
        public void ShouldHaveErrorWhenFirstNameIsEmpty()
        {
            var model = new CreateEmployeeCommand
            (
                string.Empty,
                "Doe",
                "12345678901",
                "john.doe@example.com",
                DateTime.Now,
                null,
                new List<PhoneNumberResult>(),
                RoleType.Employee,
                "password",
                RoleType.Employee
            );

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void ShouldHaveErrorWhenEmailIsInvalid()
        {
            var model = new CreateEmployeeCommand
            (
                "John",
                "Doe",
                "12345678901",
                "invalid-email",
                DateTime.Now,
                null,
                new List<PhoneNumberResult>(),
                RoleType.Employee,
                "password",
                RoleType.Employee
            );

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void ShouldHaveErrorWhenCpfIsInvalid()
        {
            var model = new CreateEmployeeCommand
            (
                "John",
                "Doe",
                "invalid-cpf",
                "john.doe@example.com",
                DateTime.Now,
                null,
                new List<PhoneNumberResult>(),
                RoleType.Employee,
                "password",
                RoleType.Employee
            );

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Cpf);
        }

        [Fact]
        public void ShouldHaveErrorWhenPasswordIsEmpty()
        {
            var model = new CreateEmployeeCommand
            (
                "John",
                "Doe",
                "12345678901",
                "john.doe@example.com",
                DateTime.Now,
                null,
                new List<PhoneNumberResult>(),
                RoleType.Employee,
                string.Empty,
                RoleType.Employee
            );

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void ShouldHaveErrorWhenPasswordIsLessThan8Characters()
        {
            var model = new CreateEmployeeCommand
            (
                "John",
                "Doe",
                "12345678901",
                "john.doe@example.com",
                DateTime.Now,
                null,
                new List<PhoneNumberResult>(),
                RoleType.Employee,
                "short",
                RoleType.Employee
            );

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void ShouldHaveErrorWhenRoleIsLessThanUserRole()
        {
            var model = new CreateEmployeeCommand
            (
                "John",
                "Doe",
                "12345678901",
                "john.doe@example.com",
                DateTime.Now,
                null,
                new List<PhoneNumberResult>(),
                RoleType.Manager,
                "password",
                RoleType.Employee
            );

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Role);
        }

        [Fact]
        public void ShouldHaveErrorWhenBirthDateIndicatesAgeLessThan18()
        {
            var model = new CreateEmployeeCommand
            (
                "John",
                "Doe",
                "12345678901",
                "john.doe@example.com",
                DateTime.Now.AddYears(-17), 
                null,
                new List<PhoneNumberResult>(),
                RoleType.Employee,
                "password",
                RoleType.Employee
            );

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.BirthDate);
        }

        [Fact]
        public void ShouldHaveErrorWhenPhoneNumbersAreInvalid()
        {
            var model = new CreateEmployeeCommand
            (
                "John",
                "Doe",
                "12345678901",
                "john.doe@example.com",
                DateTime.Now,
                null,
                new List<PhoneNumberResult> { new PhoneNumberResult("invalid-phone", PhoneNumberType.Mobile, false) },
                RoleType.Employee,
                "password",
                RoleType.Employee
            );

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor("PhoneNumbers[0].Number");
        }
    }
}
