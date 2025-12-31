# ADR 0001: Adapter Plugin Architecture

## Context
We need to support multiple device transports (serial, wifi, bluetooth) and industrial PLC protocols.
We want to add new devices without rewriting core logic.

## Decision
Use an **Adapter** interface (`IDeviceAdapter`) that creates a `IDeviceSession`.
Core system depends only on abstractions, not concrete protocols.

## Consequences
- New transports are added as new projects.
- Core stays stable and testable.
- Sessions implement reconnect/backoff strategies per adapter.
