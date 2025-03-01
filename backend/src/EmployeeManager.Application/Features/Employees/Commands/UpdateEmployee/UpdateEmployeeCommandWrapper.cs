using MediatR;

namespace EmployeeManager.Application.Features.Employees.Commands.UpdateEmployee
{
    public record UpdateEmployeeCommandWrapper : IRequest<Unit>
    {
        public Guid Id { get; init; }
        public UpdateEmployeeCommand Command { get; init; }
    }
}
