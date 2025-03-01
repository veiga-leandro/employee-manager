using AutoMapper;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using EmployeeManager.Domain.Interfaces;
using MediatR;

namespace EmployeeManager.Application.Features.Employees.Commands.ReplaceEmployee
{
    public class ReplaceEmployeeCommandHandler(IEmployeeRepository repository, 
        IPasswordService passwordService,
        IMapper mapper,
        ICurrentUserService currentUserService) : IRequestHandler<ReplaceEmployeeCommandWrapper>
    {
        private readonly IEmployeeRepository _repository = repository;
        private readonly IPasswordService _passwordService = passwordService;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(ReplaceEmployeeCommandWrapper wrapper, CancellationToken cancellationToken)
        {
            var employee = await _repository.GetByIdAsync(wrapper.Id);
            if (employee == null)
                throw new DomainValidationException($"Funcionario com Id {wrapper.Id} não encontrado");

            var request = wrapper.Command;

            // Atualiza todos os campos, mesmo que sejam null
            employee.FirstName = request.FirstName;
            employee.LastName = request.LastName;
            employee.Email = request.Email; 
            employee.PasswordHash = _passwordService.Hash(request.Password);
            employee.Role = request.Role;
            employee.BirthDate = request.BirthDate;
            employee.Active = request.Active;
            employee.PhoneNumbers = _mapper.Map<ICollection<PhoneNumber>>(request.PhoneNumbers);

            employee.LastModifiedDate = DateTime.UtcNow;
            employee.LastModifiedBy = _currentUserService.GetCurrentUserEmail();

            _repository.Update(employee);
            await _repository.SaveChangesAsync();
        }
    }
}
