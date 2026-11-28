# After Life (Unity project folder "After LIfe")

2D puzzle-platformer, made for a client. Unity 6000.6.4f1, URP 2D, Input System 1.20, Cinemachine 3.x (install via Package Manager).
Deadline 2026-12-11; target finish about 2026-12-07.

## How we work (rules)

- Delivery: write NO game code or assets into this project. Give the code in chat, one stage at a time, with the target path (`Assets/Scripts/<Folder>/<File>.cs`). The user reviews it and pastes it into Unity.
- Only this CLAUDE.md and graphify output are written here by Claude.
- Stop after each stage: list the exact Editor setup (objects, layers, assets to assign) and wait for confirmation. Fix compile errors the user pastes back before moving on.
- Token economy: short replies, no repeated context, no unrequested prose. Read the graphify graph before reading source files.
- Half-vibe-coded: the user reads and owns the code. Keep it clear.
- Git: never add a Co-Authored-By trailer or any Claude/AI attribution to commits or PRs. The repo credits the user only.
- Teaching pace: split every stage into small steps (1.1, 1.2, ...). Each step adds one concept (about 80 lines of code at most), ends with a test the user can see in Play mode, and a commit. Explain briefly what the new code does and why.
- Simplicity first: one-week scope. If something is risky or slow, propose a simpler alternative. No over-engineering, no third-party packages, free or built-in assets only.

## Code style (AAA, not junior)

- C#, one class per file, under `Assets/Scripts/{Player,Dimensions,Interactables,Enemies,Data,Systems,UI}`.
- Namespace: AfterLife.<Folder> (for example AfterLife.Player). Allman braces on every method.
- XML doc comments (`/// <summary>`) on every class, method and public member.
- `[SerializeField] private` fields, no public fields; `[Header]`/`[Tooltip]` on inspector fields.
- No magic numbers (serialized fields or constants). Clear naming, small methods, early returns.
- Decoupled: ScriptableObject event channels or C# events, no hard references between switches and doors.
- Stable unique IDs on every door, switch, key and orb (needed for save/load).
- Unity 6 APIs: `Rigidbody2D.linearVelocity`, Cinemachine 3 (`CinemachineCamera`, `CinemachineConfiner2D`, namespace `Unity.Cinemachine`).

## Game design (agreed)

- Theme: a lost flame spirit (small floating flame with a little face) that died for an unknown reason, trapped between Light (living world) and Shadow (spirit realm), looking for the way out. Each place holds a trace of its old life and the clues say someone was waiting. The cause of death stays unknown.
- Endings (two): all orbs (fragments of its life) collected, it comes back to life; otherwise it crosses to the other side.
- Setting: modern day. Light is an ordinary town, city or village; Shadow is the corrupted, deformed copy of the same places (inspired by the film "Coma": torn, tilted, floating blocks held by twisted roots). Each Shadow place mirrors a Light place.
- Genre: puzzle-platformer. No combat for now; a simple attack only if time remains.
- Opening: 15-20 s cutscene (fade plus text lines, no Timeline), then player control.
- Worlds: two tilemap groups, `LightWorld` and `ShadowWorld` (each Ground, Hazards, Decoration), same coordinates in one scene. Switching toggles renderers and colliders only; player and camera do not move. `DimensionObject` marks objects that exist in one world only. Touching a solid or hazard of the other world on switch deals damage.
- Camera: side-view follow, smooth damping, confiner.
- Palette (dark in both worlds, defined in one place): Light warm dusk, sky `#241624` to `#7A4640`, ground `#8A5A48`, edge `#F2C9A0`, hazard `#FF7A5C`, gold `#F5D76E` accent. Shadow night, sky `#100D22` to `#2C2350`, ground `#4E477F`, edge `#9C96C9`, hazard `#D94FA8`, cyan `#7FD1D9` accent. Player white core with mint glow `#B8F0DC`.
- Art direction: moody pixel art, overlapping parallax layers for depth, light fog, URP Bloom and 2D Lights. Main menu: layered dusk town with the Shadow copy hanging in the sky, spirit on a rooftop at the right, title and buttons on the left over dark soft fog (own art, inspired by the Dead Cells menu layout).
- Collectibles: keys (required for progress) and spirit orbs (optional, some exist in only one world). HUD counter, plus vague clue lines unlocked at orb thresholds, stored in `LevelData`. Orb IDs and unlocked clues are saved. No orb is ever required to finish a level, but they decide the ending. Level 1 has two orbs, one per world. Tutorial hints change at checkpoints (`CheckpointHint`).
- Controls: A/D move, Space jump, Shift run, Q switch dimension, E interact, Esc pause. Coyote time and jump buffer.
- Features required: New Input System, Tilemaps, Cinemachine, ScriptableObjects (LevelData, EnemyData, GameSettings), event system, JSON save/load (Continue and New Game), UI (main menu, pause, HUD) and AudioManager.
- Placeholders: coloured squares and named art slots to swap later.

## Stages

1. Setup: packages, folders, Input Actions, player controller, Cinemachine camera, test scene.
2. DimensionManager, DimensionObject, overlap-damage rule, switch effect.
3. Interactables and events: door, switch, pressure plate, key, checkpoint, exit.
4. Health, hazards, enemies (EnemyData, max 2 types), respawn.
5. Save/load and GameManager with LevelData.
6. UI (menu, pause, HUD), AudioManager, sound hooks.
7. At least five playable levels with a placement checklist; every puzzle solvable by switching. Starts with 7.0, an art pass (spirit, parallax, fog and lights, menu art), then 7.1, a level kit (prefabs and a template scene), then one step per level, polish and a Windows build. Every level mixes street, cutaway building and underground areas in one scene: 1 apartment and metro entrance, 2 corner shop and metro station, 3 school and sewer, 4 hospital and service tunnels, 5 all of them combined. Metro and sewer areas are underground paths or small caves where a key or an orb is found.
8. Optional: chunk-based level generator.

Target: stages 1-2 on Nov 23-24, 3 on Nov 25, 4 on Nov 26, 5 on Nov 27, 6 on Nov 28, art pass and level kit on Nov 28-29, levels 1-5 on Nov 30-Dec 4, polish and build on Dec 5-7, buffer to Dec 11.

## graphify

This project has a knowledge graph at graphify-out/ with god nodes, community structure, and cross-file relationships.

Rules:
- For codebase questions, first run `graphify query "<question>"` when graphify-out/graph.json exists. Use `graphify path "<A>" "<B>"` for relationships and `graphify explain "<concept>"` for focused concepts. These return a scoped subgraph, usually much smaller than GRAPH_REPORT.md or raw grep output.
- If graphify-out/wiki/index.md exists, use it for broad navigation instead of raw source browsing.
- Read graphify-out/GRAPH_REPORT.md only for broad architecture review or when query/path/explain do not surface enough context.
- After modifying code, run `graphify update .` to keep the graph current (AST-only, no API cost).
