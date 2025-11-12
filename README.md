# Valetax Test Assignment

ASP.NET Core 8 REST API application for managing independent hierarchical tree structures with comprehensive exception logging and journal management.

## Technology Stack

- **ASP.NET Core 8** 
- **Entity Framework Core**
- **PostgreSQL** 
- **Npgsql**
- **Docker**

### Run with Docker compose
- git clone
- cd ValetaxTest
- docker-compose up -d --build
- dotnet ef database update --context ApplicationDbContext
- dotnet run --project ValetaxTest.Web

API will be available at http://localhost:5055

Swagger UI: http://localhost:5055/swagger/index.html

### Architecture
- **Clean Architecture**
- **Repository Pattern**
- **Dependency Injection**
- **Middleware Pattern**
