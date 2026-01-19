# ⚔️ Modular 2D Action-Controller System

A robust, component-based architecture for Unity 2D projects. This system focuses on **decoupled logic**, ensuring that movement, combat, and AI systems function independently for maximum scalability.

## 🚀 Technical Highlights

### 🕹️ Advanced Player Controller
- **Physics-Based Movement:** Custom implementation of "Better Jump" logic for responsive, snappier gameplay.
- **Modular Abilities:** Independent scripts for **Wall Jump**, **Dashing**, and **Interactions** to avoid "God Object" scripts.
- **Health System:** Includes invincibility frames (iframes) with visual feedback (sprite flashing) and knockback physics.

### 🧠 State-Driven Enemy AI
- **Distance-Based Logic:** Enemies automatically switch between **Wander** and **Chase** states based on player proximity.
- **Combat Feedback:** Integrated stun routines and attack cooldowns to balance gameplay difficulty.

## 📁 Project Structure
- **/Core:** Central vital systems (Health, Stats).
- **/Movement:** Specialized physics and locomotion scripts.
- **/Actions:** Combat and interaction logic.
- **/Enemies:** Base and specialized AI behaviors.

---
*Developed as part of my 6th Semester B.Tech CSE Portfolio.*
