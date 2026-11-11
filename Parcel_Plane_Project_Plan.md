# Parcel Plane – GitHub Project Plan

**Repository:** (Your Unity repo for Parcel Plane)  
**Project Name:** Parcel Plane Development  
**Created:** November 4, 2026  


## Milestones (create under Issues → Milestones)

| Milestone | Due Date | Description |
|-----------|----------|-------------|
| Prototype | Nov 18, 2026 | Core loop playable: launch paper plane, basic flight, hit a target |
| Alpha | Dec 2, 2026 | All Must-have features implemented |
| Beta | Dec 10, 2026 | Content complete, major bugs fixed, playable start-to-finish |
| Final Build | Dec 15, 2026 | Polished, documented, ready for submission |


**Issue: Basic Paper Plane Rigidbody Setup**  
Milestone: Prototype  
Scope: Must have  
Estimate: 3 hrs  
Description: Create a simple paper plane mesh (or placeholder) with Rigidbody. Zero default drag.  
Acceptance Criteria:  
- [ ] Plane exists in scene with Rigidbody  
- [ ] Falls under gravity when launched  
- [ ] No Unity default drag interfering  

**Issue: Simple Aerodynamic Forces (Lift & Drag)**  
Milestone: Prototype  
Scope: Must have  
Estimate: 4 hrs  
Description: Script that applies lift and drag based on forward speed and angle of attack.  
Acceptance Criteria:  
- [ ] Plane can glide a short distance  
- [ ] Higher speed or better AoA produces more lift  
- [ ] Plane stalls and drops if too slow or steep  

**Issue: Mouse Aim & Launch System**  
Milestone: Prototype  
Scope: Must have  
Estimate: 3 hrs  
Description: From rooftop, mouse controls aim direction. Hold left mouse to charge power, release to apply launch velocity.  
Acceptance Criteria:  
- [ ] Player can aim up/down/left/right  
- [ ] Power meter visible  
- [ ] Release launches plane with correct direction and force  

**Issue: Basic Mid-Flight Pitch & Bank Controls**  
Milestone: Prototype  
Scope: Must have  
Estimate: 3 hrs  
Description: WASD or arrows apply short force bursts for pitch and bank.  
Acceptance Criteria:  
- [ ] W/S change pitch  
- [ ] A/D bank the plane  
- [ ] Controls feel like corrections, not continuous flight  

**Issue: Delivery Target Trigger & Scoring**  
Milestone: Prototype  
Scope: Must have  
Estimate: 2 hrs  
Description: Target zone collider. Detect soft vs hard landing. Display simple score.  
Acceptance Criteria:  
- [ ] Landing in zone counts as success  
- [ ] Crash outside or high-speed impact counts as fail  
- [ ] Score appears after flight  

**Issue: Single Tutorial Rooftop Level**  
Milestone: Alpha  
Scope: Must have  
Estimate: 3 hrs  
Description: One starting rooftop, one target balcony, simple low-poly buildings.  
Acceptance Criteria:  
- [ ] Level loads and is playable end-to-end  
- [ ] Player can complete a delivery  

**Issue: Minimal HUD (Power, Condition, Speed)**  
Milestone: Alpha  
Scope: Must have  
Estimate: 2 hrs  
Description: Basic UI elements for launch power and flight feedback.  
Acceptance Criteria:  
- [ ] Power meter during aim  
- [ ] Condition or success indicator after landing  

**Issue: Restart & Simple Menu**  
Milestone: Alpha  
Scope: Must have  
Estimate: 2 hrs  
Description: R restarts current throw. Esc pauses. Main menu scene or panel.  
Acceptance Criteria:  
- [ ] Can restart without reloading scene  
- [ ] Can return to menu  

### Should Have

**Issue: Wind Zones**  
Milestone: Alpha  
Scope: Should have  
Estimate: 3 hrs  
Description: Volume colliders that apply directional force.  
Acceptance Criteria:  
- [ ] Plane is pushed by wind  
- [ ] Wind strength tunable  

**Issue: Two Fold Types (Dart & Glider)**  
Milestone: Alpha  
Scope: Should have  
Estimate: 3 hrs  
Description: Different mass, drag, and lift coefficients.  
Acceptance Criteria:  
- [ ] Player can select fold before launch  
- [ ] Noticeable flight difference  

**Issue: Package Condition Meter**  
Milestone: Beta  
Scope: Should have  
Estimate: 2 hrs  
Description: Rough landings reduce condition; zero fails delivery.  
Acceptance Criteria:  
- [ ] Condition decreases on hard impact  
- [ ] Zero condition fails the throw  

**Issue: Star Rating (1–3)**  
Milestone: Beta  
Scope: Should have  
Estimate: 2 hrs  
Description: Stars based on landing softness and remaining condition.  
Acceptance Criteria:  
- [ ] 1–3 stars displayed after successful delivery  

**Issue: Second Level (Downtown Block)**  
Milestone: Beta  
Scope: Should have  
Estimate: 4 hrs  
Description: Longer distance, one wind zone.  
Acceptance Criteria:  
- [ ] Level selectable and completable  

### Nice to Have

**Issue: Thermal Updrafts**  
Milestone: Beta  
Scope: Nice to have  
Estimate: 2 hrs  

**Issue: Cosmetic Paper Skins**  
Milestone: Final Build  
Scope: Nice to have  
Estimate: 2 hrs  

**Issue: Simple Audio (Launch, Wind, Landing)**  
Milestone: Beta  
Scope: Nice to have  
Estimate: 2 hrs  

**Issue: Camera Polish (Smooth Follow + Look)**  
Milestone: Final Build  
Scope: Nice to have  
Estimate: 3 hrs  

**Issue: Gamepad Support**  
Milestone: Final Build  
Scope: Nice to have  
Estimate: 3 hrs  

### Risks (label with `risk`)

**Issue: Paper Plane Physics Too Unstable**  
Milestone: Prototype  
Scope: Must have  
Estimate: 2 hrs  
Label: risk  
Description: Custom aero may be hard to tune.  
Fallback: Fall back to simple velocity + drag script or Unity’s built-in forces with heavy tuning.  
Decision Date: Nov 12, 2026  

**Issue: Art Assets Delay Progress**  
Milestone: Alpha  
Scope: Should have  
Estimate: 1 hr  
Label: risk  
Description: Low-poly buildings or plane model take longer than expected.  
Fallback: Use Kenney free assets or primitive shapes for the entire project.  
Decision Date: Nov 20, 2026  

**Issue: Mid-Flight Controls Feel Unresponsive**  
Milestone: Prototype  
Scope: Must have  
Estimate: 1 hr  
Label: risk  
Description: Bursty controls may feel bad.  
Fallback: Switch to continuous force application or simpler “tap to correct” system.  
Decision Date: Nov 15, 2026  

## Tracking Evidence

1. Create the GitHub Project.  
2. Add all issues above.  
3. Set Scope, Estimate, Milestone, and Target date on every card.  
4. Enable Board view (Todo / In Progress / Done) and Roadmap view.  
5. Make the project visible to the instructor.  
6. Submit the project link + screenshot of the Board view.  
