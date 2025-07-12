# 🎮 Unity 3D Stack Fall Game

A 3D hyper-casual game developed in Unity, inspired by popular titles like **Helix Jump**. In this game, players rotate a helix tower to help a bouncing ball descend through gaps while avoiding deadly platforms.

[⬇️ Download Windows Build (.zip)](https://github.com/BHBit580/HelixJumpGame/releases/download/V/WindowsBuild.zip)

---

## 🧩 About The Project

This game showcases core Unity gameplay systems, scalable architecture, and responsive input handling. The objective is to descend as far as possible through a series of procedurally generated platforms, using simple input to rotate the helix structure.

---

## 🎮 Key Features

- **🌀 Dynamic Helix Rotation**  
  Rotate the tower with simple drag input to align gaps for the falling ball.

- **⚙️ Procedurally Generated Levels**  
  Platforms are generated dynamically with random gaps and danger tiles to keep gameplay fresh.

- **🔥 Progressive Difficulty**  
  As the player descends, the game becomes harder with narrower gaps and more danger tiles.

- **💾 Score Tracking & Persistence**  
  Keeps track of the current score and highest score locally using a JSON file for save data.

- **📣 Event-Driven Architecture**  
  Built using Unity’s `ScriptableObject`-based event channels to maintain clean, decoupled code.

- **🎞️ Smooth Animations**  
  Uses the DOTween library for fluid UI transitions and camera animations.

---

## 🛠️ Built With

- **Unity Engine**
- **C#**
- **DOTween** – For smooth tween animations
- **Unity's New Input System**
- **ScriptableObject Event Channels**

---

## 🚀 Getting Started (Source Code)

If you want to build or modify the game:

1. Clone the repo:
   ```bash
   git clone https://github.com/BHBit580/HelixJumpGame.git
