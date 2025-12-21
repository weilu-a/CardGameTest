# Card Battle System (Slay the Spire-like)

A turn-based card battle system inspired by Slay the Spire,
focused on action sequencing and buff resolution.

## Features
- Turn-based card-driven combat
- Action queue / timeline-based resolution system
- Buff & debuff calculation before and after card execution
- Multiple card effects (single target, AoE, buffs)
- Character & monster generation

## Core Design
This project focuses on separating:
- card intent
- action execution
- effect resolution

to ensure extensibility and predictable combat order.
