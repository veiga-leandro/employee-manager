using EmployeeManager.Domain.Interfaces;
using EmployeeManager.Infrastructure.Data;
using EmployeeManager.Infrastructure.Repositories;
using EmployeeManager.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EmployeeManager.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHttpContextAccessor(); // Habilita acesso ao HttpContext

            // Banco de Dados (EF Core)
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Repositórios
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            // Serviços de Infraestrutura
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            // Autenticação JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(configuration["JwtSettings:SecretKey"]!)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };


                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = async context =>
                        {
                            context.HandleResponse(); // Evita a resposta padrão (WWW-Authenticate header)

                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsJsonAsync(new
                            {
                                Status = 401,
                                Title = "Token Inválido",
                                Message = "Token ausente, expirado ou incorreto."
                            });
                        }
                    };
                });

            return services;
        }
    }
}
