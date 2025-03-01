using EmployeeManager.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace EmployeeManager.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserEmail()
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HTTP Context não disponível.");

            // Obtém o e-mail do claim (assumindo que está no token JWT)
            var email = httpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value; // ClaimTypes.Email ou seu tipo personalizado

            return email
                ?? throw new UnauthorizedAccessException("E-mail não encontrado no token.");
        }
    }
}
