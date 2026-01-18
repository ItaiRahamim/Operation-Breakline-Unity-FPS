# 🔫 Operation Breakline - Unity FPS

![Project Preview](https://github.com/user-attachments/assets/a095822e-07b1-47c0-b627-a9730fb47d42)

## 🌟 Overview
**Operation Breakline** is an intense First-Person Shooter (FPS) game built with Unity 🎮. Experience fast-paced combat, smart enemy AI, destructible environments 💥, and an engaging level progression system.

## ✨ Features

### ⚔️ Core Gameplay
- **FPS Movement & Combat 🏃‍♂️**: Responsive player controller with smooth shooting mechanics.
- **Weapon System 🎯**: Precision aiming with crosshair (`Red Crosshar.png`) and realistic bullet physics (`Bullet.cs`).
- **Health System ❤️**: Manage your health with UI indicators and survive by finding MedKits 💊.

### 🤖 Enemies & AI
- **Enemy AI 👾**: Intelligent enemies that track and engage the player (`EnemyScript.cs`).
- **Kill Counter ☠️**: Keep track of every defeated enemy during your mission.

### 🌍 Environment & Interaction
- **Destructible Objects 📦**: Watch the world crumble! Objects break upon impact or when hitting the ground (`BreakOnHit.cs`, `BreakOnGroundHit.cs`).
- **Level Progression 🚀**: Seamlessly move between levels and complete missions (`LevelEndTrigger.cs`, `MissionCompleteManager.cs`).

### 🔄 Game Flow
- **Main Menu 🏠**: Start your operation.
- **Mission Complete 🏆**: Victory summary screen after completing objectives.

## 🛠️ Technical Details
- Built with **Unity** 🧊.
- Powered by the **New Input System** 🎮.
- **HDRP** (High Definition Render Pipeline) for stunning visuals 👁️.

## 📜 Scripts Breakdown
- **👤 Player**: `PlayerController`, `PlayerHealth`, `PlayerHealthUI`
- **⚔️ Combat**: `Bullet`, `KillCounter`
- **👾 Enemies**: `EnemyScript`
- **💊 Items**: `MedKit`
- **⚙️ System**: `MainMenuScript`, `LevelEndTrigger`, `MissionCompleteManager`, `CameraSwitcher`

## 🎮 How to Play
1. **Move 🕹️**: W, A, S, D
2. **Aim 🎯**: Mouse
3. **Shoot 🔥**: Left Mouse Button
4. **Interact/Pickups ✋**: Walk over MedKits
