# MobiGate — Mobility Aggregation API (.NET 8)

**Target**: Backend Software Engineer (.NET) @ [umob](https://www.linkedin.com/jobs/view/4412788423)
**Architecture**: Modular monolith with DDD, event-driven via outbox pattern
**Local dev**: `docker compose up`

---

## Overview

```
Phase 0 — Preflight ........................................................ [optional]
Phase 1 — Foundation + Identity ........................................... [~2 sessions]
  ├── 1a: Solution scaffold + Docker Compose + health endpoint
  └── 1b: JWT auth + API key auth + ADR #1
  ▶ Gate: register → login → call /api/v1/vehicles with JWT

Phase 2 — Core Domain (Catalog + Booking) ................................ [~3 sessions]
  ├── Vehicle aggregate + catalog search
  ├── Booking aggregate + state machine
  ├── CQRS read model (PostgreSQL, same DB)
  ├── Idempotency keys + Redis distributed locking
  └── k6 gate test: 50 concurrent bookings → exactly 1 succeeds
  ▶ Gate: search → book → complete trip end-to-end

Phase 3 — Provider Integration ........................................... [~2 sessions]
  ├── IProviderGateway interface
  ├── 3 simulated providers (in-process HTTP handlers)
  ├── Polly retry + circuit breaker
  └── Health-based provider exclusion
  ▶ Gate: 3 providers in search → booking routes to correct provider

Phase 4 — Event-Driven Infrastructure .................................... [~2 sessions]
  ├── RabbitMQ + transactional outbox pattern
  ├── Background outbox publisher
  ├── Idempotent consumers (inbox pattern)
  ├── Dead-letter queue
  └── Versioned events with upcaster
  ▶ Gate: kill RabbitMQ mid-booking → restart → events drain correctly

Phase 5 — Production Hardening ........................................... [~2 sessions]
  ├── OpenTelemetry (traces + metrics + logs)
  ├── Rate limiting per API key
  ├── API versioning
  ├── Unit tests (domain logic)
  ├── Integration tests (Testcontainers: real PG + Redis + RabbitMQ)
  └── k6 load test: 100 concurrent users, p95 < 500ms
  ▶ Gate: dotnet test → k6 green → README with architecture diagram

Phase 6 — CI/CD + Cloud Deploy ........................................... [~1 session]
  ├── GitHub Actions CI (build → test → dockerize)
  ├── GitHub Actions CD (push to registry)
  ├── Bicep templates for Azure (free tier where possible)
  └── Production URL responding
  ▶ Gate: git push → CI green → curl https://<deploy>/api/v1/health = 200
  │
  └── ⚠ Phase 6 is optional. Stop at Phase 5 for a fully demonstrable
      project that proves every JD skill without cloud costs.
```

---

## JD Skill Coverage

All skills proven by **Phase 5**:

| JD Requirement | Where it's proven |
|---|---|
| C# .NET | Full solution in .NET 8 |
| Cloud computing | Bicep templates + containerized (Phase 6 proves deploy) |
| Databases | PostgreSQL + read model (CQRS) |
| CI/CD | GitHub Actions (Phase 6) |
| Domain-Driven Design | Bounded contexts, aggregates, domain events |
| Event-driven systems | Outbox pattern, message broker, idempotency |
| Modular monolith tradeoffs | ADR-001 explains the decision |
| Scaling systems | CQRS, Redis locks, async processing, rate limiting |

---

## Solution Structure

```
MobiGate/
├── src/
│   ├── MobiGate.Api/               # Controllers, middleware, DI composition
│   ├── MobiGate.Domain/            # Aggregates, value objects, domain events
│   ├── MobiGate.Application/       # Use cases, commands/queries, DTOs
│   └── MobiGate.Infrastructure/     # EF Core, repos, message bus, provider gateways
├── tests/
│   ├── MobiGate.UnitTests/         # Domain logic (fast)
│   └── MobiGate.IntegrationTests/   # Testcontainers (real infra)
├── docker-compose.yml              # PostgreSQL + Redis + RabbitMQ
├── SKILLS_MAP.md                   # JD requirement → code location
├── docs/
│   └── adrs/                        # Architecture Decision Records
└── README.md                       # Setup instructions + arch diagram
```

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

---

## ADRs to Write

| # | Title | Phase |
|---|---|---|
| ADR-001 | Modular Monolith over Microservices | Phase 1 |
| ADR-002 | In-process Provider Simulation | Phase 3 |
| ADR-003 | Outbox Pattern over Dual-Write or CDC | Phase 4 |
| ADR-004 | Event Schema Evolution Strategy | Phase 4 |
| ADR-005 | Why PostgreSQL Read Model over MongoDB | Phase 2 |

---

## Testing Strategy

| Layer | Tool | What | Speed |
|---|---|---|---|
| Unit | xUnit + FluentAssertions | Aggregate state machines | ms |
| Unit | xUnit + FluentAssertions | Value object invariants | ms |
| Integration | Testcontainers | Booking flow (real PG+Redis+RabbitMQ) | s |
| Integration | Testcontainers | Outbox → broker → inbox cycle | s |
| Load | k6 | 100 concurrent bookings | min |
| Security | Manual | JWT expiry, rate limit bypass | min |

---

## Phase Details

### Phase 0 — Preflight (optional, ~15 min)

Verify your environment:

- [x] .NET 8 SDK installed → `8.0.127`
- [x] Docker + Compose installed → `29.5.3` + `5.1.4`
- [x] Git installed → `2.54.0`
- [x] GitHub repo created → https://github.com/lynguyenhuu24/mobigate

---

### Phase 1 — Foundation + Identity (~2 sessions)

#### 1a — Solution scaffold + Docker Compose + health endpoint

- [x] Create solution: `dotnet new sln -n MobiGate`
- [x] Create projects: Domain, Application, Infrastructure, Api
- [x] Add NuGet packages: EF Core (Npgsql), Serilog, Swashbuckle
- [x] Write `docker-compose.yml` (PostgreSQL 16, Redis 7, RabbitMQ 3)
- [x] `Program.cs`: health endpoint at `GET /api/health`
- [x] `Program.cs`: Serilog, Problem Details middleware, Swagger
- [x] Seed data migration: 3 providers, 15 vehicles
- [x] Verify: `docker compose up` → `curl localhost:5000/api/health` = 200 (tested via Swagger UI) 🔗

#### 1b — Identity & Access

- [ ] `User` aggregate (Email, PasswordHash, Roles)
- [ ] `POST /api/v1/auth/register`
- [ ] `POST /api/v1/auth/login` → returns JWT
- [ ] API Key auth middleware (for simulated provider-to-platform calls)
- [ ] Write ADR-001: "Modular Monolith over Microservices"
- [ ] Write `SKILLS_MAP.md` (JD → code location mapping)

**▶ Gate**: Register user → login → call `/api/v1/vehicles` with JWT → 200

---

### Phase 2 — Core Domain (Catalog + Booking) (~3 sessions)

- [ ] `Vehicle` aggregate — states: Available, Reserved, InMaintenance
- [ ] VehicleRepository (EF Core, PostgreSQL)
- [ ] CQRS read model: denormalized VehicleView in separate PG schema
- [ ] `GET /api/v1/vehicles?lat=&lng=&radius=&type=` (reads from read model)
- [ ] `Booking` aggregate — states: Requested → Confirmed → Active → Completed → Cancelled
- [ ] Domain events: BookingRequested, BookingConfirmed, BookingCancelled
- [ ] BookingRepository (EF Core, PostgreSQL)
- [ ] Idempotency-Key header handling on `POST /api/v1/bookings`
- [ ] Redis distributed lock on VehicleId during booking creation
- [ ] `POST /api/v1/bookings`
- [ ] `POST /api/v1/bookings/{id}/cancel`
- [ ] `GET /api/v1/bookings/{id}`
- [ ] `GET /api/v1/bookings/history`

**▶ Gate**: Search → book → cancel/complete → verify state transitions
**▶ Gate**: k6 50 concurrent booking requests → exactly 1 success, 49 × 409

---

### Phase 3 — Provider Integration (~2 sessions)

- [ ] `IProviderGateway` interface:
  - `Task<AvailabilityResponse> CheckAvailabilityAsync(...)`
  - `Task<ReserveResponse> ReserveAsync(...)`
  - `Task<ConfirmResponse> ConfirmAsync(...)`
  - `Task CancelAsync(...)`
- [ ] Provider #1 — ScooterCo (in-process HTTP handler, REST)
- [ ] Provider #2 — BikeNow (in-process HTTP handler, REST)
- [ ] Provider #3 — TaxiGo (in-process HTTP handler, REST)
- [ ] Polly retry policy (3 retries with exponential backoff)
- [ ] Polly circuit breaker (5 failures → open 30s)
- [ ] Provider health check endpoint + background health monitor
- [ ] Unhealthy providers excluded from search results
- [ ] Strategy pattern: booking routes to correct provider gateway
- [ ] Write ADR-002: "In-process Provider Simulation"
- [ ] k6: verify circuit breaker opens when a provider times out

**▶ Gate**: All 3 providers visible in search → booking each type succeeds

---

### Phase 4 — Event-Driven Infrastructure (~2 sessions)

- [ ] RabbitMQ setup (already in docker-compose.yml)
- [ ] MassTransit or raw RabbitMQ.Client publisher configuration
- [ ] OutboxMessage table (EF Core migration)
- [ ] Domain event → OutboxMessage in same DbTransaction as aggregate
- [ ] Background service: poll OutboxMessage → publish to RabbitMQ
  - Delete or mark as Processed after successful publish
- [ ] Dead-letter exchange for failed messages (3 retry cap)
- [ ] Inbox pattern: consumer-side IdempotentMessageHandler
  - InboxMessage table tracks processed message IDs
- [ ] Versioned events: BookingConfirmedV1 → BookingConfirmedV2 upcaster
- [ ] Write ADR-003: "Outbox Pattern over Dual-Write or CDC"
- [ ] Write ADR-004: "Event Schema Evolution Strategy"

**▶ Gate**: Kill RabbitMQ → make booking → restart RabbitMQ → events drain → Booking confirmed exactly once, no duplicates

---

### Phase 5 — Production Hardening (~2 sessions)

- [ ] OpenTelemetry: traces + metrics + logs (export to console/Jaeger)
- [ ] Rate limiting middleware (per API key: 100 req/min)
- [ ] API versioning (routes under `/api/v1/`, `/api/v2/` if needed)
- [ ] xUnit + FluentAssertions unit tests:
  - Vehicle state machine transitions
  - Booking aggregate invariants
  - Value object validation
- [ ] Integration tests with Testcontainers:
  - PostgreSQL + Redis + RabbitMQ in containers
  - Full booking flow test
  - Outbox → broker → inbox cycle test
- [ ] k6 load test script (100 concurrent users, 3s think time)
- [ ] README.md with:
  - Architecture diagram (Mermaid)
  - Quick start (docker compose up)
  - Tech stack summary
  - JD skill coverage table

**▶ Gate**: `dotnet test` (all green)
**▶ Gate**: `k6 run` — p95 < 500ms, zero errors
**▶ Gate**: Fresh clone → `docker compose up` → API works in 2 minutes

---

### Phase 6 — CI/CD + Cloud Deploy (~1 session, OPTIONAL)

- [ ] GitHub Actions CI:
  - Trigger: push to any branch, PR to main
  - Steps: `dotnet build` → `dotnet test` → `docker build`
- [ ] GitHub Actions CD:
  - Trigger: push to main
  - Steps: build → docker push → deploy
- [ ] Bicep templates:
  - Azure Container App (free tier: 180 vCPU-seconds/day)
  - Azure PostgreSQL Flexible Server (Burstable B1ms ~$7/mo)
  - Azure Cache for Redis (Basic C0 — free)
  - Azure Service Bus (Basic tier ~$0.05/mo)
- [ ] Verification:
  - `git push` → CI green check
  - `curl https://mobigate.<region>.azurecontainer.io/api/v1/health` = 200

**▶ Gate**: `git push` → CI green → deploy succeeds → public URL returns 200

> **⚠ Cost warning**: Azure free tier covers Container Apps (first 180 vCPU-seconds/day) and Basic Redis. PostgreSQL Burstable B1ms costs ~$7/month. Service Bus Basic costs ~$0.05/month. Skip this phase if you don't want cloud costs; **Phase 5 proves all JD skills locally**.
