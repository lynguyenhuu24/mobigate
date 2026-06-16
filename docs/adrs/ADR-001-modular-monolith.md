# ADR-001: Modular Monolith over Microservices

**Status:** Accepted  
**Date:** 2026-06-16  
**Phase:** 1b — Identity & Access

---

## Context

MobiGate must aggregate multiple mobility providers (scooters, bikes, cars) into a single platform. The system has clear bounded contexts (Identity, Vehicle Catalog, Booking Engine, Provider Integration, Payment, Trip Tracking), which could each be deployed as independent microservices.

We had two architectural options:

| Option | Description |
|--------|-------------|
| **Microservices** | Each bounded context as a separate service with its own DB, communicating via HTTP/event bus |
| **Modular Monolith** | Single deployment unit with module boundaries enforced at the code/project level, sharing a single database schema |

---

## Decision

We will build a **modular monolith** with:

- A single `.NET 8` solution containing 4 projects representing clean architecture layers: `Api`, `Application`, `Domain`, `Infrastructure`
- Bounded contexts remain as **logical groupings** within each layer (e.g., `Application/Auth/`, `Application/Bookings/`)
- **One shared database** (PostgreSQL) with schema-level separation per context
- Event-driven communication between contexts via **transactional outbox → RabbitMQ → inbox** (introduced in Phase 4)

---

## Rationale

1. **Simpler deployment** — One Docker image, one process. CI/CD pipeline is trivial compared to orchestrating N services.

2. **Lower operational overhead** — No service discovery, API gateway routing, or distributed tracing setup required during development.

3. **Faster development velocity** — Single solution build, one-step debugging, no cross-service integration overhead. Critical for a solo developer/portfolio project.

4. **Enables microservices later** — Module boundaries are real. Each bounded context lives in its own namespace/folder. Extracting to a separate service later requires only:
   - Copying code to a new project
   - Exposing its API surface
   - Routing traffic

5. **Transactional consistency** — Booking flows (reserve → confirm → charge) are simpler without distributed transactions across services.

---

## Consequences

### Positive

- Fast build times and simple local setup
- Single-step debugging across all contexts
- Refactoring cross-context flows is trivial
- Easy to test end-to-end

### Negative

- **Cannot scale contexts independently** — If booking is hot and catalog is cold, both share the same resources
- **Single database contention** — All contexts share the same PostgreSQL instance
- **Codebase grows large** — Discipline required to prevent bounded context boundaries from blurring
- **No independent deploy per context** — A change to vehicle catalog means redeploying the entire API

### Mitigations

- Schema-level separation (`mobigate.identity`, `mobigate.bookings`, etc.) in PostgreSQL keeps DB concerns organized
- Event-driven outbox pattern (Phase 4) creates async boundaries between contexts even within the same process
- Strict code review enforces bounded context encapsulation

---

## Related

- [SKILLS_MAP.md](../../SKILLS_MAP.md) — Maps JD requirements to code locations
- Compares favorably to microservice alternative: more pragmatic for a 1-person project targeting a job application
