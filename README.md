# Kick ’n Catch — Architecture Overview

## Overview

**Kick ’n Catch** is a physics-driven 2D casual arcade game focused on timing, positioning, and precision. The player kicks a ball onto a curved ramp and then try to catch it. While the gameplay loop is intentionally simple, the project is built to demonstrate **clean, scalable Unity architecture** suitable for production environments.

This repository emphasizes **engineering structure over content volume**, making it easy for reviewers to understand system responsibilities, data flow, and extensibility.

---

## Features (Architectural)

* **Physics-Centric Gameplay**: Deterministic 2D physics for ball motion and interaction.
* **State-Driven Systems**: Explicit state machines for both Player and Ball behavior.
* **Event-Driven Communication**: Loose coupling via a centralized event bus.
* **Service-Oriented Design**: Core systems exposed as explicit scene-level services.
* **Mobile-First Focus**: Optimized for Android performance and UI-based input.

---

## Implementation Details

### Design Patterns

#### Singleton Pattern (Controlled)

Used for global services that must exist exactly once:

* `GameService` — Orchestrates round flow and reset logic
* `EventService` — Central event bus
* `InputService` — Centralized input state
* `ScoreService` — Score tracking and persistence
* `UIService` — UI flow orchestration
* `SoundService` — Audio playback

**Important:**

* Singletons are **explicitly placed in the scene**
* No auto-instantiation or hidden creation is used
* Initialization order is controlled via Script Execution Order

This avoids implicit dependencies and makes startup behavior deterministic.

---

#### Observer Pattern (Event-Driven Architecture)

The project uses an event-based communication model via `EventService`.

Examples:

* Ball system raises *Caught* and *Missed* events
* UI system listens for score, game over, and reset events
* Sound system reacts to gameplay events

Systems never reference each other directly, ensuring **low coupling and high maintainability**.

---

#### State Machine Pattern

State machines manage behavior complexity without large conditional logic.

**Player States**:

* Idle
* Move
* Aim
* Kick
* TurnAndCatch

**Ball States**:

* Waiting
* Rolling
* Airborne
* Caught
* Missed

Each state has clearly defined **enter**, **update**, and **exit** responsibilities, enabling predictable transitions and easy extension.

---

#### MVC Pattern (UI)

The UI system follows a Controller–View separation:

* **Views**: Handle visuals and Unity components only
* **Controllers**: Handle UI logic and event responses
* **UIService**: Coordinates which UI is active

Gameplay logic never resides in UI classes.

---

## Scriptable Objects

Scriptable Objects are used strictly for **data configuration**, not behavior.

Used for:

* Player parameters (speed, bounds, prefabs)
* Ball parameters (spawn position, limits)
* Ramp configuration
* Power bar tuning values

Not used for:

* Gameplay logic
* State transitions
* Runtime decision-making

This enforces a clear separation between **data** and **logic**.

---

## Input Architecture

* Custom `InputService` manages all input state
* Mobile input is driven via UI buttons
* Gameplay systems consume abstracted input values

### Not Used

* Unity New Input System
* `PlayerInput` or `InputAction` assets

The legacy Input Manager is intentionally used for **simplicity and mobile performance**.

---

## What Is Intentionally Not Used

To keep the project focused and maintainable, the following are deliberately excluded:

* ❌ Scene-reload–based game flow
* ❌ God objects or monolithic managers
* ❌ Static utility classes controlling logic
* ❌ Gameplay logic inside MonoBehaviours
* ❌ Auto-created singletons
* ❌ Multiplayer or networking systems
* ❌ Over-engineered input frameworks

---

## System Communication Flow

* Player and Ball systems do **not** communicate with UI directly
* Gameplay systems raise events
* UI and Audio systems react to events
* `GameService` controls round lifecycle

This enforces a **one-directional dependency flow**.

---

## Current State (Present)

* Fully playable single-player game
* Deterministic initialization and reset flow
* Clean separation of concerns
* Stable, mobile-friendly performance

The project is considered **feature-complete at an architectural level**.

---

## Future Scope

The current architecture supports future expansion without refactoring core systems:

* Tutorial and onboarding systems
* Power-ups and modifiers
* Multiple ramp types
* Difficulty scaling
* Leaderboards and progression

---

## Purpose of This Repository

This project exists to demonstrate:

* Professional Unity architecture
* Practical use of design patterns
* Clean system boundaries
* Production-minded engineering decisions

It is designed as a **long-term foundation**, not a disposable prototype.
