# Dragon Airlines

## What This Project Is
MythGJ is a Unity game-jam project called Dragon Airlines. It looks like a 2D flying arcade game with a main menu, enemy waves, cloud movement, and passenger-like loss mechanics tied to the player’s health.

The main scenes are:
- `Assets/Scenes/MainMenu.unity`
- `Assets/Scenes/SampleScene.unity`

### Recent cleanup
`PlayerController` now separates wing-colour updates and tilt-damage handling into helpers, and the menu loads the gameplay scene by name instead of build index.
`EnemySpawner` now uses wave strategies plus serialized wave profiles, which makes spawn progression easier to reason about than the older nested branching.

## How The Code Works
The codebase is centered on movement, waves, and visual feedback.

### Menu
`MainMenu` opens the main gameplay scene and likely also handles simple credits/menu UI toggles.

### Player movement and health
`PlayerController` is the central player script. It appears to manage:
- horizontal and vertical movement
- tilt or rotation feedback
- wing color changes
- health

When the player takes damage, the game seems to remove one passenger or life unit from the board.

### Passenger system
`PassengerSpawner` creates a grid or group of passengers that visually reflect the player’s health. `Passenger` animates a passenger falling away through movement, rotation, scaling, and fade-out before being destroyed.

### Enemy waves
`EnemySpawner` manages repeated waves and escalates enemy patterns over time. `EnemyMovement` provides the actual motion patterns and collision damage behavior.

### Background and atmosphere
`CloudScript` and `CloudMove` make the background scroll and keep the scene feeling alive.

## Main Design Traits
- This is a compact arcade project with a very readable core loop.
- Most logic lives in a few scripts, so the project is easy to understand but could become hard to extend without refactoring.
- Health is represented visually through passenger loss, which is a nice thematic mechanic.

## Evidence Used
- `Assets/Scripts/MainMenu.cs`
- `Assets/Scripts/JJ/PlayerController.cs`
- `Assets/Scripts/Chris/PassengerSpawner.cs`
- `Assets/Scripts/Chris/Passenger.cs`
- `Assets/Scripts/Chris/EnemySpawner.cs`
- `Assets/Scripts/Chris/EnemyMovement.cs`
- `Assets/Scripts/CloudScript.cs`
- `Assets/Scripts/CloudMove.cs`
