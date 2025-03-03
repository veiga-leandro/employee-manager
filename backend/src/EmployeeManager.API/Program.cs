using EmployeeManager.API.Filters;
using EmployeeManager.Application.Behavior;
using EmployeeManager.Application.Features.Employees.Commands.CreateEmployee;
using EmployeeManager.Application.Features.Employees.Commands.DeleteEmployee;
using EmployeeManager.Application.Features.Employees.Commands.ReplaceEmployee;
using EmployeeManager.Application.Features.Employees.Commands.UpdateEmployee;
using EmployeeManager.Application.Features.Login.Command;
using EmployeeManager.Application.Mappings;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Infrastructure.Data;
using EmployeeManager.Infrastructure.Extensions;
using EmployeeManager.Infrastructure.Seed;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configuração de Serviços
builder.Services.AddCors(options =>
{
    options.AddPolicy("Development", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers().AddNewtonsoftJson();

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration).Destructure.ByTransforming<Employee>(emp => new { emp.FirstName, emp.PasswordHash })
    .CreateLogger();

builder.Host.UseSerilog();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EmployeeManager API",
        Version = "v1",
        Description = "API for employee management"
    });

    // Configure Swagger to use the XML comments file
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    // Configuração de Segurança JWT no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT (ex: Bearer {token})",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Exception Filter
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>();

// Infraestrutura (DbContext, Repositories, etc.)
builder.Services.AddInfrastructure(builder.Configuration);

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(EmployeeManager.Application.AssemblyReference).Assembly));

builder.Services.AddValidatorsFromAssemblyContaining<CreateEmployeeCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateEmployeeCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ReplaceEmployeeCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DeleteEmployeeCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginCommandValidator>();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

// Build
var app = builder.Build();

app.UseCors("Development");

// Aplica as migrações
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate(); // Aplica as migrações pendentes

        // Executa o seed
        await SeedData.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro ao aplicar migrações ou popular dados iniciais.");
    }
}

app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagCtx, httpContext) =>
    {
        diagCtx.Set("ClientIP", httpContext.Connection.RemoteIpAddress);
    };
});

app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Iniciando request para {Path}", context.Request.Path);
    await next();
    logger.LogInformation("Finalizando request. Status: {StatusCode}", context.Response.StatusCode);
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
