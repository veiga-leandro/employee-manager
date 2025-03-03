using AutoMapper;
using EmployeeManager.Application.Models;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using EmployeeManager.Domain.Interfaces;
using MediatR;

namespace EmployeeManager.Application.Features.Employees.Commands.CreateEmployee
{
    public sealed class CreateEmployeeCommandHandler(
        IEmployeeRepository employeeRepository,
        IPasswordService passwordService,
        IMapper mapper,
        ICurrentUserService currentUserService) : IRequestHandler<CreateEmployeeCommand, EmployeeResult>
    {
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        private readonly IPasswordService _passwordService = passwordService;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IMapper _mapper = mapper;

        public async Task<EmployeeResult> Handle(
            CreateEmployeeCommand request,
            CancellationToken cancellationToken)
        {
            // Converter PhoneNumberRequest para PhoneNumber (Value Object)
            var phoneNumbers = request.PhoneNumbers
                .Select(p => new PhoneNumber(p.Number, p.Type, p.IsActive))
                .ToList();

            // 1. Valida se o CPF já existe
            var existingEmployee = await _employeeRepository.GetByDocumentAsync(request.Cpf);
            if (existingEmployee != null)
                throw new DomainValidationException("CPF já cadastrado");

            // 2. Mapeia o Command para a Entidade
            var employee = _mapper.Map<Employee>(request);

            // 3. Hash da senha
            employee.PasswordHash = _passwordService.Hash(request.Password);

            employee.Active = true;
            employee.CreatedDate = DateTime.UtcNow;
            employee.CreatedBy = _currentUserService.GetCurrentUserEmail();

            // 4. Salva no banco
            await _employeeRepository.AddAsync(employee);
            await _employeeRepository.SaveChangesAsync();

            return EmployeeResult.FromEmployee(employee);
        }
    }
}
