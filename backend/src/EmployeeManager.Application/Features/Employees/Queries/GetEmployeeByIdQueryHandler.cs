using EmployeeManager.Application.Extensions;
using EmployeeManager.Application.Models;
using EmployeeManager.Domain.Interfaces;
using MediatR;

namespace EmployeeManager.Application.Features.Employees.Queries
{
    public sealed class GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository) : IRequestHandler<GetEmployeeByIdQuery, EmployeeResult?>
    {
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;

        public async Task<EmployeeResult?> Handle(
            GetEmployeeByIdQuery request,
            CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
            return employee?.ToEmployeeResult();
        }
    }
}
