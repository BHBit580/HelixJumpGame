# Unity 3D Stack Fall Game

A 3D hyper-casual game developed in Unity. The project is inspired by popular games like "Helix Jump," where the player controls a rotating platform to guide a bouncing ball to the bottom.

## About The Project

This game demonstrates core Unity development concepts with a focus on creating scalable and maintainable code. The player rotates a helix of platforms to help a bouncing ball find gaps to fall through. Colliding with safe platforms allows the ball to continue, while hitting danger zones ends the game.

## Key Features

* **Dynamic Gameplay**: Control the helix rotation with a simple drag input to guide the automatically bouncing ball through the tower.
* **Progressive Difficulty**: The game automatically increases its challenge as the player descends. The number of dangerous tiles and the gaps between platforms adjust based on how many stacks have been cleared.
* **Scoring & Persistence**: Features a complete scoring system that tracks the current score and saves the highest score locally between sessions using a JSON file.
* **Event-Driven Architecture**: Built using a decoupled architecture with ScriptableObject-based event channels (`VoidEventChannelSO`) to manage communication between different systems like the player, UI, and game managers.
* **Smooth Animations**: Utilizes the DOTween library to create smooth and polished UI feedback and camera movements.

## Built With

* Unity Engine
* C#
* Unity's New Input System
* DOTween (Animation Library)
