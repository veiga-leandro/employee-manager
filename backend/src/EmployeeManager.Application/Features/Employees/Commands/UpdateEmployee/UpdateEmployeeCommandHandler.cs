using AutoMapper;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using EmployeeManager.Domain.Interfaces;
using MediatR;

namespace EmployeeManager.Application.Features.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandHandler(IEmployeeRepository repository, 
        IPasswordService passwordService,
        IMapper mapper,
        ICurrentUserService currentUserService) : IRequestHandler<UpdateEmployeeCommandWrapper, Unit>
    {
        private readonly IEmployeeRepository _repository = repository;
        private readonly IPasswordService _passwordService = passwordService;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(UpdateEmployeeCommandWrapper wrapper, CancellationToken cancellationToken)
        {
            var employee = await _repository.GetByIdAsync(wrapper.Id);
            if (employee == null)
                throw new DomainValidationException($"Funcionario com Id {wrapper.Id} não encontrado");

            var request = wrapper.Command;

            // Atualiza apenas os campos preenchidos
            if (request.FirstName != null)
                employee.FirstName = request.FirstName;

            if (request.LastName != null)
                employee.LastName = request.LastName;

            if (request.Email != null)
                employee.Email = request.Email;

            if (request.Password != null)
                employee.PasswordHash = _passwordService.Hash(request.Password);

            if (request.Role != null)
                employee.Role = request.Role.Value;

            if (request.Active != null)
                employee.Active = request.Active.Value;

            if (request.BirthDate != null)
                employee.BirthDate = request.BirthDate.Value;

            if (request.PhoneNumbers != null)
            {
                var phoneNumbers = _mapper.Map<ICollection<PhoneNumber>>(request.PhoneNumbers);
                employee.PhoneNumbers = phoneNumbers;
            }

            employee.LastModifiedDate = DateTime.UtcNow;
            employee.LastModifiedBy = _currentUserService.GetCurrentUserEmail();

            _repository.Update(employee);
            await _repository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
