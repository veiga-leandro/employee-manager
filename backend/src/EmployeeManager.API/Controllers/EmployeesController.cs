using EmployeeManager.API.DTOs.Employee;
using EmployeeManager.Application.Features.Employees.Commands.CreateEmployee;
using EmployeeManager.Application.Features.Employees.Commands.DeleteEmployee;
using EmployeeManager.Application.Features.Employees.Commands.ReplaceEmployee;
using EmployeeManager.Application.Features.Employees.Commands.UpdateEmployee;
using EmployeeManager.Application.Features.Employees.Queries;
using EmployeeManager.Application.Models;
using EmployeeManager.Domain.Enums;
using EmployeeManager.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace EmployeeManager.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController(ILogger<EmployeesController> logger, IMediator mediator) : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger = logger;
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Get an employee by ID.
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Employee details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmployeeResult), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                _logger.LogInformation("Consultando lista de funcionários...");

                var query = new GetEmployeeByIdQuery(id);
                var employee = await _mediator.Send(query);

                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar funcionários");
                throw;
            }
        }

        /// <summary>
        /// Get all employees with pagination.
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List of employees</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResponse<EmployeeResult>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAll([FromQuery, Range(1, int.MaxValue)] int page = 1, [FromQuery, Range(1, 100)] int pageSize = 10)
        {
            try
            {
                _logger.LogInformation("Consultando todos os funcionários...");
                if (page < 1 || pageSize < 1)
                    return BadRequest("Página e tamanho da página devem ser maiores que zero.");

                var query = new GetAllEmployeesQuery(page, pageSize);
                var employees = await _mediator.Send(query);

                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todos os funcionários");
                throw;
            }
        }

        /// <summary>
        /// Replace an employee.
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <param name="command">Replace employee command</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Replace(Guid id, ReplaceEmployeeCommand command)
        {
            try
            {
                _logger.LogInformation("Substituindo funcionário...");

                var wrapper = new ReplaceEmployeeCommandWrapper
                {
                    Id = id,
                    Command = command
                };
                await _mediator.Send(wrapper);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao substituir funcionário");
                throw;
            }
        }

        /// <summary>
        /// Update an employee.
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <param name="command">Update employee command</param>
        /// <returns>No content</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(Guid id, UpdateEmployeeCommand command)
        {
            try
            {
                _logger.LogInformation("Atualizando funcionário...");

                var wrapper = new UpdateEmployeeCommandWrapper
                {
                    Id = id,
                    Command = command
                };
                await _mediator.Send(wrapper);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar funcionário");
                throw;
            }
        }

        /// <summary>
        /// Create a new employee.
        /// </summary>
        /// <param name="request">Create employee request</param>
        /// <returns>Created employee ID</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Create(CreateEmployeeRequest request)
        {
            try
            {
                _logger.LogInformation("Adicionando novo funcionário...");

                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                if (userRole == null)
                    return base.Unauthorized();

                var userRoleEnum = Enum.Parse<RoleType>(userRole);

                var command = new CreateEmployeeCommand(
                    request.FirstName,
                    request.LastName,
                    request.CPF,
                    request.Email,
                    request.BirthDate,
                    request.PhoneNumbers,
                    request.Role,
                    request.Password,
                    userRoleEnum
                );

                var employeeId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = employeeId }, employeeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar funcionário");
                throw;
            }
        }

        /// <summary>
        /// Delete an employee.
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                _logger.LogInformation("Deletando funcionário...");

                var command = new DeleteEmployeeCommand(id);
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar funcionário");
                throw;
            }
        }
    }
}