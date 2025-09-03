# Transitive Closure Table (.NET 8 + EF Core)

This project implements the **Transitive Closure Table pattern** to efficiently manage and query hierarchical data using **.NET 8** and **Entity Framework Core**.  
It demonstrates **Domain-Driven Design (DDD)**, robust error handling, cross-database support, and modern ASP.NET Core architecture practices.

---

## 🚀 Features

- **Transitive Closure Table Pattern** – optimized handling of tree and graph relationships.
- **Entity Framework Core 8** – persistence with migrations and LINQ queries.
- **Cross-Database Support** – implemented via **Abstract Factory Pattern** (`PostgresAppFactory`, `SqlServerAppFactory`).
- **Global Error Handling Middleware** – centralized JSON-based error responses.
- **Secure Exception Handling** – separates user-friendly (`SecureException`) and internal errors.
- **Exception Journaling** – with correlation IDs via `IExceptionJournalService`.
- **Unit Tests** – for domain entities, validators, repositories, and services.
- **Indexes** – database indexes created via EF Core for query performance.
- **Clean Architecture** – layered structure with Domain, Application, Infrastructure, WebApi, and Tests projects.

---

## 🛠️ Patterns & Techniques Implemented

- **Domain-Driven Design (DDD)** – Domain entities, value objects, validators, exceptions.
- **Repository Pattern** – abstraction over data access (`NodeRepository`, `TreeRepository`, etc.).
- **Unit of Work Pattern** – `UnitOfWork` ensures **atomic operations** and transaction safety.
- **Abstract Factory Pattern** – cross-database setup (Postgres, SQL Server).
- **Factory Pattern** – service and unit of work factories for object creation.
- **Dependency Injection (DI)** – built-in .NET Core DI container for services and repositories.
- **Middleware Pattern** – `ErrorHandlingMiddleware` for consistent error handling pipeline.
- **Validation Pattern** – domain validators (`NodeValidator`, `TreeValidator`, `TransitiveClosureValidator`).
- **Exception Handling Strategy** – structured JSON error responses with unique error IDs.
- **CQRS-style layering** – separation of commands/queries into Application vs. Infrastructure.
- **Unit Tests** – in `Tests/DomainTests` and `Tests/InfrastructureTests` projects.

---

## 📂 Project Structure

- **Domain/** – Core entities, exceptions, and validators.
- **Application/** – Service layer with contracts and implementations.
- **Infrastructure/** – EF Core context, repositories, factories, and Unit of Work.
- **Middleware/** – Custom ASP.NET Core middleware for global exception handling.
- **WebApi/** – Controllers and API entry point.
- **Tests/** – Unit tests for domain and infrastructure logic.

---
