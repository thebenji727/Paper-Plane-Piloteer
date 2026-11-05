# Parcel Plane – Game Design Document

**Working Title:** Parcel Plane  
**Version:** 1.0 (Assignment Draft)  
**Date:** November 4, 2026  
**Author:** Rhonas, the Indomitable  
**Engine:** Unity 6 (C# Scripting)  
**Target Platform:** PC (Windows) with Keyboard + Mouse  

## 1. Concept and Pitch

**Elevator Pitch:**  
Fold a paper plane on a city rooftop, launch it into the wind, and guide it across rooftops to deliver packages to balconies and landing pads before it stalls or crashes.

**Genre:** Physics-based arcade delivery / precision flight puzzle.  
**Target Player:** Casual players who enjoy short, skillful sessions of Paper Toss, Angry Birds physics, Tiny Wings, or simple flight games. Ideal for 5–10 minute sessions.  
**Similar Games:** “It’s like Paper Toss meets Tiny Wings meets a rooftop delivery sim.” Players get the satisfying launch of Paper Toss, the continuous flight feel of Tiny Wings, and the goal-oriented delivery loop of casual courier games.

The fantasy is simple and tactile: you are a skilled paper-plane courier who turns ordinary notebook paper into precision delivery vehicles. Success feels like nailing a perfect throw across a classroom, but scaled to city rooftops.

## 2. Framework Analysis (MDA)

**Mechanics (the rules and systems):**  
- Paper plane has a Rigidbody with custom lift, drag, and stall behavior based on angle of attack and speed.  
- Player chooses or lightly customizes fold type (Dart = fast/unstable, Glider = slow/stable, Stunt = high maneuverability).  
- Launch phase: aim with mouse, set power with hold, release to throw.  
- Mid-flight control: limited inputs that apply brief force or adjust control surfaces (nose up/down, bank left/right). Controls are “bursty” rather than continuous throttle so the plane always feels like paper.  
- Delivery zones are trigger colliders on rooftops/balconies. Soft landing (low speed + correct orientation) scores higher than a crash landing.  
- Wind zones and thermal updrafts as environmental forces.  
- Package integrity: rough landings or collisions reduce a “condition” meter; zero condition fails the delivery.

**Dynamics (emergent player behavior):**  
Players quickly learn to trade altitude for speed (dive then pull up), ride wind currents, and choose fold types based on distance versus precision. Near-misses with obstacles create tension. Successful long glides produce “flow” moments where the player barely corrects the path. Failed throws encourage immediate retry with adjusted aim or fold.

**Aesthetics (desired player experience):**  
Primary: **Challenge** (skillful aiming and mid-air corrections) and **Sensation** (the physical feeling of a light paper plane riding air). Secondary: **Discovery** (finding better wind routes or fold combinations) and light **Fantasy** (being the best rooftop courier in the city). The game should feel light, playful, and satisfying rather than stressful or punishing.

## 3. Core Gameplay Loop

1. **Select** – Choose package destination and paper fold type from the rooftop menu.  
2. **Aim & Launch** – Mouse aims direction and elevation; hold to charge power; release to throw.  
3. **Fly & Correct** – Guide the plane with short control inputs while managing speed and altitude.  
4. **Deliver** – Hit the target zone with as soft a landing as possible.  
5. **Score & Retry/Next** – Receive stars and tips based on accuracy, condition, and time. Unlock better paper or new folds.  

What keeps the player looping is the immediate feedback of a good throw, the visible improvement in technique, and the simple joy of watching a paper plane fly farther or more precisely than last time. A simple diagram of the loop:

```
[Select Fold & Target] → [Aim & Launch] → [Mid-flight Control] → [Land / Deliver] → [Score & Progress] → (repeat)
```

## 4. Controls (Keyboard + Mouse, PC)

| Input | Action |
|-------|--------|
| Mouse Move | Aim direction and elevation during launch phase |
| Left Mouse Hold | Charge throw power |
| Left Mouse Release | Launch plane |
| W / Up Arrow | Brief nose-up pitch correction (lift burst) |
| S / Down Arrow | Brief nose-down dive |
| A / Left Arrow | Bank left |
| D / Right Arrow | Bank right |
| Space | Optional short boost (if unlocked; limited uses) |
| R | Restart current throw |
| Esc | Pause / Menu |
| Mouse Click (post-flight) | Confirm score / next package |

Gamepad support is Nice-to-have (left stick aim, right stick or face buttons for corrections).

## 5. Look and Feel

**Art Style:** Low-poly 3D with clean, slightly stylized paper textures. Buildings are simple geometric forms with flat colors and minimal detail so the paper plane remains the visual focus.  
**Color Palette:** Warm paper whites and kraft browns for the plane; soft blues and grays for the city; bright accent colors (yellow, teal) for target zones and UI.  
**Camera:** Third-person follow camera that trails the plane, with a slight lag and ability to look around mid-flight. Launch phase uses a fixed over-the-shoulder view from the rooftop.  
**UI/HUD:** Minimal. Launch phase shows power meter and target distance. Flight shows speed, altitude relative to target, and package condition bar. Score appears large and punchy on delivery.  
**Audio Direction:** Light paper rustle and whoosh on launch and flight, soft impact sounds on landing, gentle wind ambient. Upbeat, lo-fi electronic or acoustic track that stays out of the way.

Reference mood: clean low-poly city rooftops at golden hour, paper planes with visible fold lines, satisfying soft landings.

## 6. Game World and Content

**Setting:** A compact stylized city of low-rise and mid-rise buildings connected by rooftops. Multiple starting rooftops unlock as the player progresses.  
**Levels / Areas:**  
- Tutorial Rooftop (short distances, no wind)  
- Downtown Block (medium distances, light wind, one thermal)  
- High-Rise District (longer glides, stronger crosswinds)  
- Night Market (optional, denser obstacles, colored lights)  

**Characters:** The player is an unseen courier. Optional simple NPC silhouettes on balconies waiting for packages (non-interactive).  
**Enemies / Obstacles:** None in the traditional sense. Obstacles include chimneys, antennas, laundry lines, birds (static or simple path), and strong wind walls.  
**Items:** Packages of varying weight (affects flight), different paper types (affects durability and aerodynamics), collectible stickers that are purely cosmetic.  
**Story:** Light framing only. You are the newest courier for “Skyfold Deliveries.” Completing deliveries earns tips that unlock better paper and new starting locations. No heavy narrative.

## 7. Win/Lose Conditions and Progression

**Session Start:** Player stands on a rooftop with a stack of packages.  
**Win Condition (per throw):** Package lands inside the target zone with condition > 0. Stars awarded (1–3) based on landing softness, remaining condition, and time.  
**Lose Condition (per throw):** Plane crashes outside the zone, package condition reaches zero, or plane stalls and falls short. Immediate retry available.  
**Progression:** Completing deliveries with 2+ stars earns currency. Currency unlocks new fold types, paper skins, and later starting rooftops. Difficulty increases through longer distances, stronger wind, and tighter landing zones rather than damage. A full “shift” of 5–8 deliveries can be a session goal.

## 8. Technical Scope

**Engine & Version:** Unity 6 (or Unity 2022.3 LTS if preferred for stability). Universal Render Pipeline.  
**Language:** C#.  
**Target Platform:** Windows PC (builds also tested in Editor).  
**Key Systems:**  
- Custom or modified Rigidbody aerodynamic model for paper plane (lift coefficient based on AoA, stall, drag).  
- Simple wind and thermal volume forces.  
- Trigger-based delivery scoring.  
- Basic UI with Unity UI Toolkit or uGUI.  

**Assets & Tools Planned:**  
- Low-poly building kit (Kenney or similar free assets, credited).  
- Paper textures generated or from free sources.  
- Audio from free packs (e.g., freesound.org, credited).  
- No paid assets required for Must-have scope.  

**Scope Honesty:** The Must-have version is a single playable rooftop with one fold type, basic flight physics, and one target. Everything else is layered on top.
