# PROJECT KNOWLEDGE BASE

**Generated:** 2026-06-15
**Branch:** (no git)

## OVERVIEW

Empty / newly initialized repository. No code, config, packages, or git history. Only OpenCode session state exists. This is a blank canvas — any project type, language, or framework can be started here.

## STRUCTURE

```
umob/
├── AGENTS.md            # This file
├── roadmap.md           # Project implementation roadmap
├── .omo/                # OpenCode session state (auto-managed)
    └── run-continuation/
```

## WHERE TO LOOK

| Task | Location | Notes |
|------|----------|-------|
| Session history | `.omo/` | Auto-managed by OpenCode |
| Roadmap | `roadmap.md` | Project plan, progress tracking |
| Source code | (none yet) | Define when project starts |
| Config | (none yet) | Define when project starts |

## STATE

This repository is **empty / newly initialized**. No toolchain, language, framework, or build system is present. Nothing has been scaffolded.

## CONVENTIONS

- **Keep roadmap.md up to date**: After completing any task, update `roadmap.md` — mark checkboxes `[x]`, add notes, or adjust plan as needed. The roadmap is the single source of truth for project progress.
- **Use controllers, not minimal APIs**: All API endpoints go in Controller classes under `MobiGate.Api/Controllers/`. No `MapGet`/`MapPost` inline handlers.
- **Use primary constructors**: All classes use primary constructor syntax (`class Foo(IBar bar)`) where dependency injection is needed.

## ANTI-PATTERNS

- Do NOT assume any toolchain or runtime is present
- Do NOT rely on existing patterns — there are none
- Do NOT skip `git init` before any commits

## COMMANDS

```bash
git init                     # initialize git (required first step)
npm init / cargo init / etc  # whichever package manager the project uses
git commit --allow-empty -m "initial commit"
```

## NOTES

- The only structure is `.omo/` (OpenCode session state)
- **Always reply with an emoji** — every response must begin with a relevant emoji
- When links are involved, append the "🔗" emoji at the end
- Before implementing: clarify language, framework, toolchain, and project type with the user first
