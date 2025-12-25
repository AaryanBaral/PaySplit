# PaySplit

PaySplit is a .NET 8 Web API for managing tenants and merchants with status
lifecycles (active, inactive, suspended). It follows a layered architecture
to keep domain rules isolated from application workflows and infrastructure
details.

## Architecture (Layered)
- API: HTTP controllers, DTOs, DI composition root, Swagger.
- Application: use cases (commands/queries), handlers, interfaces, filters.
- Domain: entities and business rules (Tenant, TenantUser, Merchant).
- Infrastructure: EF Core persistence, repositories, DbContext.

Dependency direction:
- API -> Application, Infrastructure
- Application -> Domain
- Infrastructure -> Application, Domain
- Domain -> (no dependencies)

## Running Locally (Step by Step)
Prerequisites:
- .NET SDK 8
- SQL Server (LocalDB by default) or a SQL Server instance

1) Restore dependencies:
```bash
dotnet restore
```

2) Configure the database connection string:
- Update `src/API/appsettings.Development.json` or `src/API/appsettings.json`
- Default uses LocalDB and database name `PlaySplit`

3) Create the database schema (no migrations are checked in yet):
```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate -p src/Infrastructure/Infrastructure.csproj -s src/API/API.csproj
dotnet ef database update -p src/Infrastructure/Infrastructure.csproj -s src/API/API.csproj
```

4) Run the API:
```bash
dotnet run --project src/API/API.csproj
```

5) Open Swagger:
- `https://localhost:7210/swagger`
- `http://localhost:5115/swagger`

(Ports are from `src/API/Properties/launchSettings.json`.)

## Project Structure
```
src/API
src/Application
src/Domain
src/Infrastructure
```

## Typical Change Flow (Layered)
1) Domain: add or update entity behavior and invariants.
2) Application: add command/query + handler and update interfaces.
3) Infrastructure: implement repository/persistence changes.
4) API: expose endpoint and DTOs.

## Notes
- Connection string is configured in `src/API/appsettings.json`.
- EF Core uses SQL Server provider in `src/Infrastructure`.
