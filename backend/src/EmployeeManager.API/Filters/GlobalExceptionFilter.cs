using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmployeeManager.API.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException validationException)
            {
                var errors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).Distinct().ToArray()
                );

                context.Result = new ObjectResult(new
                {
                    Title = "Erros de Validação",
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Erros de Validação",
                    Errors = errors
                })
                { StatusCode = StatusCodes.Status400BadRequest };
            }
            else if (context.Exception is DomainValidationException domainValidationException)
            {
                context.Result = new ObjectResult(new
                {
                    Message = domainValidationException.Message,
                    Status = StatusCodes.Status400BadRequest
                })
                { StatusCode = StatusCodes.Status400BadRequest };
            }
            else
            {
                context.Result = new ObjectResult(new
                {
                    Message = "Erro interno no servidor",
                    Status = StatusCodes.Status500InternalServerError
                })
                { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
    }
}
