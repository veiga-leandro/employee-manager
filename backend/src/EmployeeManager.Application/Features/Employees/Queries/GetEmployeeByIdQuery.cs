using EmployeeManager.Application.Models;
using MediatR;

namespace EmployeeManager.Application.Features.Employees.Queries
{
    public record GetEmployeeByIdQuery(Guid EmployeeId) : IRequest<EmployeeResult>;
}
