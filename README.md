# MobiGate — Mobility Aggregation API

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE)

**MobiGate** is a modular monolith backend API that aggregates mobility providers (scooters, bikes, cars) into a single unified platform. Built with Domain-Driven Design, CQRS, and event-driven patterns using .NET 8.

> 🎯 Portfolio project demonstrating production-grade backend engineering skills — from clean architecture and event-driven systems to CI/CD and cloud deployment.

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| **Runtime** | .NET 8 (C# 12) |
| **ORM** | Entity Framework Core 8 + Npgsql |
| **Database** | PostgreSQL 16 (write model + CQRS read model) |
| **Cache & Locks** | Redis 7 (distributed locking, rate limiting) |
| **Message Broker** | RabbitMQ 3 (event-driven outbox/inbox) |
| **API** | RESTful with RFC 7807 Problem Details |
| **Auth** | JWT + API Key authentication |
| **Logging** | Serilog (structured logging) |
| **Telemetry** | OpenTelemetry (traces, metrics, logs) |
| **Testing** | xUnit + FluentAssertions + Testcontainers |
| **Containerization** | Docker Compose (local dev) |
| **CI/CD** | GitHub Actions → Azure (optional) |

---

## Architecture

```
┌──────────────────────────────────────────────────┐
│                   API Gateway                     │
│  (JWT auth, rate limiting, versioning, logging)   │
├──────────────────────────────────────────────────┤
│                                                    │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐         │
│  │ Identity │  │ Vehicle  │  │ Booking  │         │
│  │ & Access │  │ Catalog  │  │ Engine   │         │
│  │   (BC)   │  │   (BC)   │  │   (BC)   │         │
│  └──────────┘  └──────────┘  └──────────┘         │
│                                                    │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐         │
│  │ Provider │  │ Payment  │  │ Trip     │         │
│  │ Mgmt     │  │ & Billing│  │ Tracking │         │
│  │   (BC)   │  │   (BC)   │  │   (BC)   │         │
│  └──────────┘  └──────────┘  └──────────┘         │
│                                                    │
│  ┌──────────────────────────────────────────┐      │
│  │   Outbox → RabbitMQ → Inbox (event bus)  │      │
│  └──────────────────────────────────────────┘      │
│                                                    │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐         │
│  │PostgreSQL│  │   Read   │  │  Redis   │         │
│  │ (write)  │  │  Model   │  │(cache/lk)│         │
│  └──────────┘  └──────────┘  └──────────┘         │
└──────────────────────────────────────────────────┘
```

### Clean Architecture (4-layer)

```
src/
├── MobiGate.Api/             # Controllers, middleware, DI composition
├── MobiGate.Application/     # Use cases, commands/queries, DTOs
├── MobiGate.Domain/          # Aggregates, value objects, domain events
└── MobiGate.Infrastructure/  # EF Core, repos, message bus, provider gateways
```

### Key Design Decisions

| Decision | Choice | Rationale |
|----------|--------|-----------|
| Architecture | Modular Monolith | Simpler deployment than microservices, preserves bounded contexts |
| Event Delivery | Transactional Outbox | Guarantees at-least-once delivery without dual-write problems |
| Read Model | CQRS on same DB | Avoids polyglot complexity while keeping read/write separation |
| Provider Integration | In-process simulation | Enables realistic end-to-end testing without external dependencies |

---

## Quick Start

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (with Compose V2)

### Run

```bash
# Start infrastructure (PostgreSQL, Redis, RabbitMQ)
docker compose up -d

# Run the API
dotnet run --project src/MobiGate.Api
```

The API will be available at `http://localhost:5000` with Swagger UI at `http://localhost:5000/swagger`.

### Verify

```bash
curl http://localhost:5000/api/health
# → { "status": "healthy", "timestamp": "...", "version": "1.0.0" }
```

---

## API Endpoints

### Current

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/health` | Health check |

### Planned (Phased Delivery)

| Phase | Method | Endpoint | Description |
|-------|--------|----------|-------------|
| 1b | `POST` | `/api/v1/auth/register` | User registration |
| 1b | `POST` | `/api/v1/auth/login` | JWT login |
| 2 | `GET` | `/api/v1/vehicles` | Search vehicles (lat/lng/radius/type) |
| 2 | `POST` | `/api/v1/bookings` | Create booking |
| 2 | `POST` | `/api/v1/bookings/{id}/cancel` | Cancel booking |
| 2 | `GET` | `/api/v1/bookings/{id}` | Get booking details |
| 2 | `GET` | `/api/v1/bookings/history` | Booking history |

---

## Project Status

**Current Phase: 1b — Identity & Access** (in progress)

| Phase | Description | Status |
|-------|-------------|--------|
| 0 | Preflight (env setup, GitHub repo) | ✅ Done |
| 1a | Solution scaffold + Docker Compose + health endpoint | ✅ Done |
| 1b | JWT auth + API key auth | 🔄 In progress |
| 2 | Core Domain (Catalog + Booking) | ⬜ Planned |
| 3 | Provider Integration (3 simulated providers) | ⬜ Planned |
| 4 | Event-Driven Infrastructure (outbox, RabbitMQ) | ⬜ Planned |
| 5 | Production Hardening (OTel, rate limiting, tests) | ⬜ Planned |
| 6 | CI/CD + Cloud Deploy (optional) | ⬜ Planned |

---

## Skill Coverage

MobiGate demonstrates the following backend engineering skills relevant to the **Senior/Mid-level .NET Backend** role:

| Skill | Where It's Demonstrated |
|-------|------------------------|
| **C# / .NET** | Full solution in .NET 8 with clean architecture |
| **Domain-Driven Design** | Bounded contexts, aggregates, domain events, value objects |
| **Event-Driven Systems** | Transactional outbox, message broker, inbox pattern |
| **CQRS** | Separated read/write models on PostgreSQL |
| **Entity Framework Core** | Migrations, seed data, relational mappings |
| **Authentication & Authorization** | JWT bearer tokens, API key middleware |
| **REST API Design** | Resourceful endpoints, RFC 7807 errors, versioning |
| **Containerization** | Docker Compose with PostgreSQL, Redis, RabbitMQ |
| **Testing** | xUnit unit tests, Testcontainers integration tests, k6 load tests |
| **CI/CD** | GitHub Actions (build → test → dockerize → deploy) |
| **Cloud** | Bicep templates for Azure deployment |
| **Observability** | OpenTelemetry tracing, Serilog structured logging |

---

## License

MIT
