using MediatR;

namespace EmployeeManager.Application.Features.Employees.Commands.ReplaceEmployee
{
    public record ReplaceEmployeeCommandWrapper : IRequest
    {
        public Guid Id { get; init; }
        public ReplaceEmployeeCommand Command { get; init; }
    }
}
