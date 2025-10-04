# Bounce The Egg

Bounce The Egg is a physics-based puzzle/arcade game built in Unity.

## Overview
In Bounce The Egg, players draw lines in the game world to guide a ball through various levels. The drawn lines act as physical objects, allowing the ball to bounce, roll, or be redirected to reach goals, avoid obstacles, or complete level-specific objectives.

## Gameplay Features
- **Draw Lines:** Players can draw lines of limited length. These lines have physical properties (colliders, bounciness) and interact with the ball and environment.
- **Ball Physics:** The ball starts stationary. When the game begins, gravity is enabled and the ball moves, bouncing off drawn lines and level elements.
- **Multiple Levels:** The game includes multiple levels, each with unique layouts and challenges.
- **Win/Loss Conditions:** Players win by achieving the level's objective (e.g., guiding the ball to a goal) and can lose if the ball falls out of bounds or fails the objective.
- **Retry/Next Level:** After completing or failing a level, players can retry or proceed to the next challenge.

## Project Structure
- **Assets/Scripts/Mangers:** Core game management scripts (GameManger, DrawManger, LevelManger, etc.)
- **Assets/Scripts/game Logic:** Ball behavior, level logic, and interfaces.
- **Assets/Prefabs & Scenes:** Level layouts, prefabs for game objects, and Unity scenes.

## Getting Started
1. Open the project in Unity.
2. Open a scene (e.g., `game.unity` or `Splash.unity`).
3. Play the scene to start drawing lines and guiding the ball!

## Requirements
- Unity (version compatible with the project files)
