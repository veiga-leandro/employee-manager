using MediatR;

namespace EmployeeManager.Application.Features.Employees.Commands.DeleteEmployee
{
    public record DeleteEmployeeCommand(
        Guid EmployeeId) : IRequest;
}
