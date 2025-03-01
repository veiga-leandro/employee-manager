using MediatR;

namespace EmployeeManager.Application.Features.Login.Command
{
    public record LoginCommand(string Email, string Password) : IRequest<string>;
}
