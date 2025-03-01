using EmployeeManager.Application.Features.Login.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [AllowAnonymous]
    public class AuthenticationController(ILogger<AuthenticationController> logger, IMediator mediator) : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger = logger;
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Handles user login requests.
        /// </summary>
        /// <param name="request">The login request containing email and password.</param>
        /// <returns>Returns an Ok result with a token if login is successful, otherwise logs an error.</returns>
        /// <response code="200">Returns the token if login is successful.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                _logger.LogInformation("Realizando login...");

                var command = new LoginCommand(request.Email, request.Password);
                var token = await _mediator.Send(command);

                return Ok(new TokenResponse(token));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao realizar login");
                throw;
            }
        }
    }

    record TokenResponse(string Token);
}
