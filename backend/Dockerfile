# backend/Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/EmployeeManager.API/EmployeeManager.API.csproj"
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=build /app .
ENTRYPOINT ["dotnet", "EmployeeManager.API.dll"]
