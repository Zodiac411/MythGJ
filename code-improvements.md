# Dragon Airlines - Code Improvements

## Priority Improvements

### Already addressed
- `PlayerController` now has helper methods for wing colour updates and tilt-damage handling.
- `MainMenu` now loads `SampleScene` by name.
- `EnemySpawner` now routes spawn decisions through wave strategy objects and wave profiles.

### 1. Split the player controller
`PlayerController` should not own movement, tilt visuals, and health/damage handling all at once. Separate it into:
- movement
- visual feedback
- life or damage state

That would make the arcade loop easier to tune and maintain.

### 2. Replace hardcoded wave logic
`EnemySpawner` should keep moving toward data-driven wave definitions instead of branching directly on progression. That would make future difficulty tuning much simpler.
The current profile-backed strategy is a step in that direction, and a scriptable wave asset would be the natural next layer if the game grows.

### 3. Introduce pooling
Passenger, enemy, and cloud objects are good candidates for object pooling because they are repeatedly created and destroyed. Pooling would reduce runtime overhead and improve consistency.

### 4. Decouple damage reactions
The current damage flow appears to connect player health directly to passenger spawning. That is a neat effect, but it would be cleaner if damage emitted an event and the passenger system listened for it.

### 5. Use named scene routing
`MainMenu` should use named scene constants or a small scene service instead of hardcoded scene indices. This avoids accidental breakage when scenes are reordered.

## Secondary Cleanups
- Add comments around the health-to-passenger conversion rule
- Give enemy movement modes clearer names if more patterns are added
- Make cloud movement configurable from a shared data asset
- Normalize file and class naming for long-term readability
