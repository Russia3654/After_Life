# Graph Report - After LIfe  (2026-11-23)

## Corpus Check
- 8 files · ~32,254 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 75 file(s) not represented in the graph (top: .meta 33, .asset 33, .unity 3)

## Summary
- 22 nodes · 22 edges · 5 communities (1 shown, 4 thin omitted)
- Extraction: 100% EXTRACTED · 0% INFERRED · 0% AMBIGUOUS
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `20bad338`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- The Other Side (Unity project "After LIfe")
- PlayerController.cs
- PlayerController

## God Nodes (most connected - your core abstractions)
1. `PlayerController` - 11 edges
2. `The Other Side (Unity project "After LIfe")` - 6 edges
3. `AfterLife.Player` - 1 edges
4. `How we work (rules)` - 1 edges
5. `Code style (AAA, not junior)` - 1 edges
6. `Game design (agreed)` - 1 edges
7. `Stages` - 1 edges
8. `graphify` - 1 edges

## Surprising Connections (you probably didn't know these)
- `PlayerController` --references--> `Rigidbody2D`  [EXTRACTED]
  Assets/Scripts/Player/PlayerController.cs →   _Bridges community 2 → community 3_

## Import Cycles
- None detected.

## Communities (5 total, 4 thin omitted)

### Community 0 - "The Other Side (Unity project "After LIfe")"
Cohesion: 0.29
Nodes (6): Code style (AAA, not junior), Game design (agreed), graphify, How we work (rules), Stages, The Other Side (Unity project "After LIfe")

## Knowledge Gaps
- **6 isolated node(s):** `AfterLife.Player`, `How we work (rules)`, `Code style (AAA, not junior)`, `Game design (agreed)`, `Stages` (+1 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 15 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **4 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `PlayerController` connect `PlayerController` to `PlayerController.cs`, `.Awake`, `.Update`?**
  _High betweenness centrality (0.395) - this node is a cross-community bridge._
- **What connects `AfterLife.Player`, `How we work (rules)`, `Code style (AAA, not junior)` to the rest of the system?**
  _6 weakly-connected nodes found - possible documentation gaps or missing edges._