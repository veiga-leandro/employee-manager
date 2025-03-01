using EmployeeManager.Domain.Exceptions;
using EmployeeManager.Domain.Interfaces;
using MediatR;

namespace EmployeeManager.Application.Features.Employees.Commands.DeleteEmployee
{
    public sealed class DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository) : IRequestHandler<DeleteEmployeeCommand>
    {
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;

        public async Task Handle(
            DeleteEmployeeCommand request,
            CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
            if (employee == null)
                throw new DomainValidationException("Funcíonário não encontrado");

            employee.Active = false;

            // 4. Salva no banco
            _employeeRepository.Update(employee);
            await _employeeRepository.SaveChangesAsync();
        }
    }
}
