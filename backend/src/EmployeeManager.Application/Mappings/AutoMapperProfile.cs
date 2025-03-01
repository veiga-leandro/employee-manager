using AutoMapper;
using EmployeeManager.Application.Features.Employees.Commands.CreateEmployee;
using EmployeeManager.Application.Models;
using EmployeeManager.Domain.Entities;

namespace EmployeeManager.Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PhoneNumberResult, PhoneNumber>();

            CreateMap<CreateEmployeeCommand, Employee>()
            .ForMember(dest => dest.LastModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.LastModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.DocumentNumber, opt => opt.MapFrom(src => src.Cpf));

            CreateMap<Employee, EmployeeResult>().ConvertUsing(employee => EmployeeResult.FromEmployee(employee));
        }
    }
}
