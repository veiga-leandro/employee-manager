# Employee Manager System

## 📋 Overview

Employee Manager is a full-stack web application for employee management, featuring a .NET 8 backend and a React/TypeScript frontend. The system enables users to create, edit, view, and remove employees, with authentication, role management, and organizational hierarchy control.

## 🏛️ Project Structure

The project is divided into two main parts:

```
employee-manager/
├── backend/        # .NET 8 API with Clean Architecture and CQRS
│   ├── src/
│   │   ├── EmployeeManager.API            # API controllers and configurations
│   │   ├── EmployeeManager.Application    # Application logic (commands, handlers)
│   │   ├── EmployeeManager.Domain         # Domain entities and business rules
│   │   └── EmployeeManager.Infrastructure # Infrastructure implementations
│   └── tests/    # Unit and integration tests
│
├── frontend/       # React application with TypeScript
│   ├── public/    # Static files
│   ├── src/
│   │   ├── api/          # Axios services and configuration
│   │   ├── components/   # Reusable components
│   │   ├── contexts/     # React contexts (authentication, etc.)
│   │   ├── models/       # Interfaces and types
│   │   └── pages/        # Application pages
│   └── ...
│
├── docker-compose.yml  # Docker setup for the entire environment
├── README.md           # This file
└── ...
```

## 🚀 Technologies Used

### Backend:
- .NET 8
- Entity Framework Core
- MediatR (CQRS)
- FluentValidation
- JWT Authentication
- SQL Server
- Serilog

### Frontend:
- React 18
- TypeScript
- Material UI
- Formik & Yup
- Axios
- React Router

## 🐳 Running with Docker

The project is fully containerized for easy execution across different environments.

### Prerequisites
- Docker
- Docker Compose

### Running the full environment

```bash
# Clone the repository
git clone https://github.com/veiga-leandro/employee-manager.git
cd employee-manager

# Start the containers (database, backend, and frontend)
docker-compose up -d
```

**Services will be available at:**
- **Frontend**: [http://localhost:3000](http://localhost:3000)
- **Backend API**: [http://localhost:8080](http://localhost:8080)
- **Swagger UI**: [http://localhost:8080/swagger](http://localhost:8080/swagger)
- **SQL Server**: `localhost:1433`

### Development Mode

For hot-reload development:

```bash
# Start only the database and backend
docker-compose up db app -d

# Start the frontend in development mode
docker-compose up frontend-dev
```

## 🛠️ Manual Installation

If you prefer running without Docker, check the specific READMEs:
- [Backend README](backend/README.md)
- [Frontend README](frontend/README.md)

## 📱 Key Features

- 🔐 **Authentication**: JWT-based login and session control
- 👥 **Employee Management**: Create, update, delete, and view employees
- 🌳 **Hierarchy**: Organizational structure visualization and management
- 🔑 **Role-Based Access Control**: Admin, HR, Manager, Employee roles
- 📝 **Validated Forms**: Real-time input validation

## 🔄 Architecture

- **Backend**: Clean Architecture with CQRS for clear separation of concerns
- **Frontend**: Component-based architecture with contextual state management
- **Communication**: RESTful API with JWT authentication
- **Persistence**: SQL Server managed via Entity Framework Core

## 📊 API Endpoints

### Employees
- `GET /api/employees` - Retrieve all employees
- `GET /api/employees/{id}` - Retrieve employee by ID
- `POST /api/employees` - Create a new employee
- `PUT /api/employees/{id}` - Update an existing employee
- `DELETE /api/employees/{id}` - Remove an employee

### Authentication
- `POST /api/auth/login` - Authenticate a user and return a JWT token

For detailed API documentation, visit `/swagger`.

## 🧪 Testing

Unit tests are included for the backend:

```bash
# Run backend tests
cd backend
dotnet test
```

## 📜 License

This project is licensed under the MIT License.

## 👥 Contribution

Contributions are welcome! Please open an issue or pull request.

## 📞 Contact

- **Developer**: Leandro Veiga
- **LinkedIn**: [Leandro's Profile](https://www.linkedin.com/in/leandro-camargo-da-veiga/)
- **GitHub**: veiga-leandro

Developed with ❤️