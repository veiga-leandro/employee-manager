# EmployeeManager - Backend

## Overview

EmployeeManager is a .NET 8-based application designed to manage employee data. It includes features for creating, updating, deleting, and retrieving employee information. The solution is structured into multiple projects, each responsible for different aspects of the application.

## Projects

### EmployeeManager.API
This project contains the API controllers and configurations for the EmployeeManager application.

### EmployeeManager.Application
This project contains the application logic, including commands, handlers, validators, and DTOs.

### EmployeeManager.Domain
This project contains the domain entities, interfaces, and shared utilities.

### EmployeeManager.Infrastructure
This project contains the infrastructure services, data access configurations, and repository implementations.

## Key Features

- **Employee Management:** Create, update, delete, and retrieve employee information.
- **Authentication:** JWT-based authentication.
- **Validation:** FluentValidation for request validation.
- **Logging:** Serilog for logging.
- **Database:** Entity Framework Core for data access.
- **API Documentation:** Swagger/OpenAPI for interactive documentation.

## Architecture

The application follows Clean Architecture principles and CQRS:

- **Domain:** Contains business entities and domain rules.
- **Application:** Implements use cases using the CQRS pattern with MediatR.
- **Infrastructure:** Provides concrete implementations for persistence and external services.
- **API:** Exposes RESTful endpoints and manages HTTP communication.

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (local or containerized)
- Docker (optional for containerization)
- Visual Studio 2022, VS Code, or another compatible editor

### Setup

Clone the repository:

```sh
git clone https://github.com/veiga-leandro/employee-manager.git
```

Navigate to the solution directory:

```sh
cd backend
```

Update the connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=db;Database=employeedb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Apply database migrations:

```sh
dotnet ef database update --project src/EmployeeManager.Infrastructure --startup-project src/EmployeeManager.API
```

Run the application:

```sh
dotnet run --project src/EmployeeManager.API
```

## Building and Running with Docker

Build the Docker image:

```sh
docker build -t employee-manager -f Dockerfile .
```

Run the Docker container:

```sh
docker run -p 8080:80 -e "ASPNETCORE_ENVIRONMENT=Development" employee-manager
```

For running with a containerized database, use docker-compose:

```sh
docker-compose up
```

## Running Unit Tests

Navigate to the tests project directory:

```sh
cd backend/tests/EmployeeManager.Tests
```

Run the tests:

```sh
dotnet test
```

## API Endpoints

### Employees

- `GET /api/employees` - Retrieve all employees.
- `GET /api/employees/{id}` - Retrieve an employee by ID.
- `POST /api/employees` - Create a new employee.
- `PUT /api/employees/{id}` - Update an existing employee.
- `DELETE /api/employees/{id}` - Delete an employee.

### Authentication

- `POST /api/auth/login` - Authenticate a user and retrieve a JWT token.

## API Documentation (Swagger)

The application uses Swagger/OpenAPI for interactive API documentation. The documentation is available at:

```
https://[your-base-url]/swagger
```

### Swagger Configuration

Swagger is configured in the project to automatically document the API endpoints. The configuration is in the `Program.cs` file:

```csharp
// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
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
    
    // Configuration for JWT in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
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
```

### Enabling XML Comments

To include your XML comments in the Swagger documentation, ensure that XML file generation is enabled in the `.csproj` file:

```xml
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
  <NoWarn>$(NoWarn);1591</NoWarn>
</PropertyGroup>
```

## Configuration

### Serilog

Logging is configured using Serilog. The configuration is located in `appsettings.json`:

```json
"Serilog": {
  "MinimumLevel": {
    "Default": "Information",
    "Override": {
      "Microsoft": "Warning",
      "System": "Warning"
    }
  },
  "WriteTo": [
    {
      "Name": "Console"
    },
    {
      "Name": "File",
      "Args": {
        "path": "logs/log-.txt",
        "rollingInterval": "Day"
      }
    }
  ]
}
```

### JWT Authentication

JWT authentication is configured in `ServiceCollectionExtensions.cs`.

### Error Handling

The API implements global error handling using custom middleware, which captures exceptions and returns appropriate HTTP responses.

## Troubleshooting

- **Database connection errors:** Check that your connection string is correct and SQL Server is running.
- **Migration errors:** Run `dotnet ef migrations add Initial --project src/EmployeeManager.Infrastructure` to create new migrations.
- **Authentication errors:** Verify that JWT tokens are configured correctly.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

## License

This project is licensed under the MIT License.

