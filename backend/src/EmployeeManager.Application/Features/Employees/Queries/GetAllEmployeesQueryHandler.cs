using AutoMapper;
using EmployeeManager.Application.Models;
using EmployeeManager.Domain.Interfaces;
using EmployeeManager.Domain.Shared;
using MediatR;

namespace EmployeeManager.Application.Features.Employees.Queries
{
    public sealed class GetAllEmployeesQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper) : IRequestHandler<GetAllEmployeesQuery, PaginatedResponse<EmployeeResult>>
    {
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<PaginatedResponse<EmployeeResult>> Handle(
            GetAllEmployeesQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _employeeRepository.GetAllAsync(request.Page, request.PageSize);
            var response = new PaginatedResponse<EmployeeResult>
            {
                Items = _mapper.Map<List<EmployeeResult>>(result.Items), // 🎯 AutoMapper aqui!
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalItems = result.TotalItems
            };
            return response;
        }
    }
}
