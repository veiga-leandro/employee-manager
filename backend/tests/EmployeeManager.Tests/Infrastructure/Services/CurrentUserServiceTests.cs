using EmployeeManager.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using Xunit;

namespace EmployeeManager.Test.Infrastructure.Services
{
    public class CurrentUserServiceTests
    {
        [Fact]
        public void GetCurrentUserEmail_ShouldReturnEmail_WhenEmailClaimExists()
        {
            // Arrange
            var email = "test@example.com";
            var claims = new List<Claim> { new Claim(ClaimTypes.Email, email) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext { User = claimsPrincipal };
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            var currentUserService = new CurrentUserService(httpContextAccessorMock.Object);

            // Act
            var result = currentUserService.GetCurrentUserEmail();

            // Assert
            Assert.Equal(email, result);
        }

        [Fact]
        public void GetCurrentUserEmail_ShouldThrowException_WhenEmailClaimDoesNotExist()
        {
            // Arrange
            var httpContext = new DefaultHttpContext { User = new ClaimsPrincipal() };
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            var currentUserService = new CurrentUserService(httpContextAccessorMock.Object);

            // Act & Assert
            Assert.Throws<UnauthorizedAccessException>(() => currentUserService.GetCurrentUserEmail());
        }
    }
}
