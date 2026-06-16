# Skills Map — JD Requirement → Code Location

Maps every backend engineering skill required by the target role to its implementation location in the codebase.

| Skill | Where It's Demonstrated | Files / Namespaces |
|-------|------------------------|--------------------|
| **C# / .NET 8** | Full solution with clean architecture across 4 projects | `MobiGate.sln` |
| **Domain-Driven Design** | Bounded contexts, aggregates, value objects, domain events | `src/MobiGate.Domain/Entities/` |
| **REST API Design** | Resourceful controllers, versioned endpoints, RFC 7807 errors | `src/MobiGate.Api/Controllers/` |
| **Entity Framework Core** | Code-first migrations, Fluent API config, seed data, Npgsql | `src/MobiGate.Infrastructure/Data/` |
| **PostgreSQL** | Write model with EF Core Npgsql provider, CQRS read model | `docker-compose.yml`, `appsettings.json` |
| **JWT Auth** | Bearer token issuance (login) and validation middleware | `src/MobiGate.Infrastructure/Auth/JwtService.cs`, `src/MobiGate.Api/Program.cs` |
| **API Key Auth** | Custom authentication scheme for provider-to-platform calls | `src/MobiGate.Api/Middleware/ApiKeyAuthMiddleware.cs` |
| **Password Hashing** | BCrypt for secure password storage | `src/MobiGate.Application/Auth/Register/RegisterHandler.cs` |
| **CQRS** | Command dispatch via MediatR (Phase 1b); separated read/write models (Phase 2) | `src/MobiGate.Application/Auth/` (commands), `src/MobiGate.Infrastructure/Data/` (read model, future) |
| **Result Pattern** | Consistent error handling without exceptions for business logic via `Result<T>` + MediatR pipeline | `src/MobiGate.Domain/Common/Result.cs`, `src/MobiGate.Application/Common/ResultBehavior.cs` |
| **Event-Driven Architecture (Planned)** | Transactional outbox → RabbitMQ → inbox pattern (Phase 4) | `src/MobiGate.Infrastructure/` (future) |
| **Distributed Locking (Planned)** | Redis-based locks for booking concurrency (Phase 2) | `docker-compose.yml` (Redis) |
| **Containerization** | Docker Compose with PostgreSQL, Redis, RabbitMQ, healthchecks | `docker-compose.yml` |
| **Testing** | xUnit unit tests, Testcontainers integration tests, k6 load tests | `tests/` |
| **Logging** | Serilog structured logging with request logging middleware | `src/MobiGate.Api/Program.cs`, `appsettings.json` |
| **Observability (Planned)** | OpenTelemetry traces + metrics (Phase 5) | `src/MobiGate.Api/` (future) |
| **CI/CD (Planned)** | GitHub Actions build → test → dockerize → deploy (Phase 6) | `.github/workflows/` (future) |
| **Cloud (Planned)** | Bicep templates for Azure Container Apps (Phase 6) | `infra/` (future) |

---

## Architecture Decision Records

| # | Title | File |
|---|-------|------|
| ADR-001 | Modular Monolith over Microservices | `docs/adrs/ADR-001-modular-monolith.md` |

---

## Cross-Reference: Roadmap Phase → Code

| Phase | Key Deliverables | Primary Files |
|-------|-----------------|---------------|
| 1a | Solution scaffold, Docker Compose, health endpoint, seed data | `Program.cs`, `docker-compose.yml`, `SeedData.cs`, `HealthController.cs` |
| 1b | JWT auth, register/login, API key auth | `AuthController.cs`, `JwtService.cs`, `UserRepository.cs`, `ApiKeyAuthMiddleware.cs` |
| 2 | Vehicle catalog, booking engine, CQRS | (future) |
| 3 | Provider integration, Polly retry, circuit breaker | (future) |
| 4 | Outbox, RabbitMQ, inbox pattern | (future) |
| 5 | OTel, rate limiting, integration tests | (future) |
| 6 | CI/CD, Azure deploy | (future) |
