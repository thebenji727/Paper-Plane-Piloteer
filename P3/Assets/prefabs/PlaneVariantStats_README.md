# Plane Variant Stats Reference

Reference for the fields on `PlaneVariant` ScriptableObjects (used by `PaperPlaneController`).

All values are floats unless noted. The **Suggested range** column is based on the five example variants (Dart Straight, Glider Distance, Climber High, Stunt Agile, Cargo Heavy) plus a bit of headroom for experimentation. You can go outside these ranges, but the feel may become extreme or unstable.

## Identity

| Field | Type | Suggested Range | What it does |
|-------|------|-----------------|--------------|
| `displayName` | string | — | Name shown in UI / debug. |
| `description` | string | — | Short flavor text (Inspector only). |

## Physics Tuning

| Field | Suggested Range | Default | What it does | Tuning notes |
|-------|-----------------|---------|--------------|--------------|
| `mass` | 0.3 – 1.6 | 0.5 | Rigidbody mass. Higher mass = more inertia, harder to accelerate or turn, drops faster. | Dart ~0.8, Glider ~0.4, Cargo ~1.4. |
| `liftCoefficient` | 0.3 – 2.5 | 1.2 | Strength of the lift force (proportional to speed² and angle of attack). Higher = climbs more and glides farther. | Low for straight darts (0.45), high for gliders (2.1) and climbers (1.8). |
| `dragCoefficient` | 0.15 – 0.7 | 0.35 | Air resistance opposing velocity. Lower = flies farther and keeps speed longer. | Gliders use the lowest (~0.18); agile/stunt planes use higher (~0.55). |
| `angularDamping` | 0.5 – 5.0 | 1.5 | How quickly rotational velocity dies. Higher = more stable, flies straighter, less wobble. | High on darts and cargo (3.5–4.0), low on agile planes (0.6). |
| `climbBias` | -1.0 – 3.0 | 0 | Constant extra upward force. Positive helps climbers; negative makes a plane drop faster. | Climber High uses +2.5; Cargo Heavy uses –0.5. |
| `directionalStability` | 0.5 – 8.0 | 2.0 | How strongly the plane tries to point its nose along its velocity vector. Higher = straighter flight, less drifting or looping. | Dart uses the highest (6); Stunt Agile uses the lowest (0.8). |

## Control Feel

| Field | Suggested Range | Default | What it does | Tuning notes |
|-------|-----------------|---------|--------------|--------------|
| `controlAuthority` | 3 – 16 | 8 | Multiplier on mid-flight pitch/bank torque (WASD / arrows). Higher = more responsive to player input. | Stunt Agile is highest (14); cargo and darts are lower (4–5). |
| `stallAngle` | 15 – 40 | 25 | Maximum angle of attack (degrees) before lift cuts out. Higher = more forgiving nose-up attitude before stalling. | Agile and climber planes use higher values (32–35); darts use lower (18). |

## Optional

| Field | Suggested Range | Default | What it does |
|-------|-----------------|---------|--------------|
| `launchPowerMultiplier` | 0.7 – 1.4 | 1.0 | Scales the launch speed applied by the slingshot (if your `SlingShot` script reads `PaperPlaneController.GetLaunchPowerMultiplier()`). |

## Quick Tuning Cheatsheet

| Goal | Adjust |
|------|--------|
| Flies almost straight / minimal arch | ↓ `liftCoefficient`, ↑ `directionalStability`, ↑ `angularDamping` |
| Longest distance / best glide | ↑ `liftCoefficient`, ↓ `dragCoefficient`, moderate `mass` |
| Climbs higher | ↑ `liftCoefficient` and/or ↑ `climbBias` |
| More agile / precise control | ↑ `controlAuthority`, ↓ `directionalStability`, ↓ `angularDamping` |
| Heavy, stable drop | ↑ `mass`, ↓ `liftCoefficient`, ↑ `angularDamping`, optional negative `climbBias` |
| More forgiving stalls | ↑ `stallAngle` |

## Example Variants (from PlaneVariantFactory)

| Variant | mass | lift | drag | angDamp | climb | stability | control | stall |
|---------|------|------|------|---------|-------|-----------|---------|-------|
| Dart Straight | 0.80 | 0.45 | 0.22 | 3.5 | 0.0 | 6.0 | 5 | 18 |
| Glider Distance | 0.40 | 2.10 | 0.18 | 2.0 | 0.3 | 3.0 | 6 | 28 |
| Climber High | 0.45 | 1.80 | 0.32 | 1.2 | 2.5 | 1.5 | 9 | 32 |
| Stunt Agile | 0.35 | 1.00 | 0.55 | 0.6 | 0.0 | 0.8 | 14 | 35 |
| Cargo Heavy | 1.40 | 0.70 | 0.48 | 4.0 | –0.5 | 4.0 | 4 | 20 |

These values are starting points that work well with a typical slingshot launch. Duplicate a variant asset and tweak from there.

## Notes

- All aerodynamic forces are applied in `FixedUpdate` on the `PaperPlaneController`.
- `PaperPlaneController` reads these values at startup and applies them to the Rigidbody (`mass`, `drag`, `angularDrag`, etc.).
- Compatible with Unity 2022.3 (uses the older `velocity` / `drag` / `angularDrag` property names).
