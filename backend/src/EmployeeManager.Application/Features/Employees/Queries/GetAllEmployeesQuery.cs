using EmployeeManager.Application.Models;
using EmployeeManager.Domain.Shared;
using MediatR;

namespace EmployeeManager.Application.Features.Employees.Queries
{
    public record GetAllEmployeesQuery(int Page = 1, int PageSize = 5) : IRequest<PaginatedResponse<EmployeeResult>>;
}
