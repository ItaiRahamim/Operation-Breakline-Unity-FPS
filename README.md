# Operation Breakline - Unity FPS

![Project Preview](https://github.com/user-attachments/assets/a095822e-07b1-47c0-b627-a9730fb47d42)

## Overview
Operation Breakline is a First-Person Shooter (FPS) game built with Unity. It features combat mechanics, enemy AI, destructible environments, and a level progression system.

## Features

### Core Gameplay
- **FPS Movement & Combat**: Responsive player controller with shooting mechanics.
- **Weapon System**: Includes crosshair aiming (`Red Crosshar.png`) and bullet physics (`Bullet.cs`).
- **Health System**: Player health management with UI indicators and MedKits for healing.

### Enemies & AI
- **Enemy AI**: Enemies that track and engage the player (`EnemyScript.cs`).
- **Kill Counter**: Tracks the number of defeated enemies during the mission.

### Environment & Interaction
- **Destructible Objects**: Elements that break upon impact or when hitting the ground (`BreakOnHit.cs`, `BreakOnGroundHit.cs`).
- **Level Progression**: Triggers for ending levels and completing missions (`LevelEndTrigger.cs`, `MissionCompleteManager.cs`).

### Game Flow
- **Main Menu**: Start screen and navigation.
- **Mission Complete**: Summary screen after finishing objectives.

## Technical Details
- Built with **Unity**.
- Uses the **New Input System**.
- **HDRP** (High Definition Render Pipeline) resources detected.

## Scripts Breakdown
- **Player**: `PlayerController`, `PlayerHealth`, `PlayerHealthUI`
- **Combat**: `Bullet`, `KillCounter`
- **Enemies**: `EnemyScript`
- **Items**: `MedKit`
- **System**: `MainMenuScript`, `LevelEndTrigger`, `MissionCompleteManager`, `CameraSwitcher`

## How to Play
1. **Move**: W, A, S, D
2. **Aim**: Mouse
3. **Shoot**: Left Mouse Button
4. **Interact/Pickups**: Walk over MedKits
