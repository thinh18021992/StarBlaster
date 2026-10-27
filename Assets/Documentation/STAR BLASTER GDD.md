STAR BLASTER
GAME DESIGN DOCUMENT
Genre: Shoot 'em up (STG) / Top-down Arcade Shooter
Platform: Mobile (Android/iOS) & PC / WebGL
1.	Core Loop & Game Flow
+ Target: Players control a spaceship to attack, dodge incoming bullet hells from endless enemy waves, collect power-ups to enhance firepower, and achieve the highest score possible before being destroyed.
+ Game flow: Main Menu ➔ Gameplay (Combat / Dodge / Upgrade) ➔ Game
Over & Score Summary ➔ Retry / Main Menu
 
GAME FLOW

2.	Core Mechanics & Control:
Player Movement:
+ Controls: Supports both Touch/Drag (Mobile) and Mouse Cursor (PC/Editor). Ship position updates smoothly based on pointer position.
+ Screen Bounds: Movement constrained within the active Camera Viewport using Mathf.Clamp to prevent the player from moving off-screen or too far up.
Shoot & Weapon:
Shooter & Weapon Systems:
+ Auto-fire Mechanism: Firing is continuously active upon holding touch or mouse press.
+ Cooldown & Anti-Spam: Fire rate is governed by an automated timer (nextFireTime / Coroutine) to enforce a strictly controlled attack frequency, neutralizing fast-click spamming.
Enemy Spawning & Behavior:
+ Pathing: Enemy waves spawn dynamically and follow pre-configured Waypoints (PathToFollow). The pathing system is modular, allowing easy addition, removal, or modification of trajectories.
+ Combat AI: Enemies fire projectiles based on assigned fire rates with randomized time variance (fireRateVariance) to create dynamic, organic bullet patterns.
+ Scoring: Points are awarded instantly upon defeating each enemy type.
3.	System & Economy
Power-up System:
Power-ups spawn inside falling Space Asteroids after set intervals. Shooting and destroying an asteroid instantly grants a permanent or stackable buff to the player:
🔴 Red Asteroid: Attack Damage Boost (+Damage)
🟡 Yellow Asteroid: Fire Rate Boost (- Fire Rate)
🟢 Green Asteroid: Max HP Restore / Increase (+HP)
Dynamic Difficulty Scaling (DDS):
Game difficulty scales over time. Every fixed time interval ($T_{scale}$), enemy attributes (Max HP, Fire Rate, Movement Speed, and Damage) are multiplied by a growth factor to maintain high engagement and challenge.
4.	User Interface
Main Menu Screen: Title, Start Game button (Scene transition), Exit button (ApplicationQuit).
Game HUD: Real-time Player Health Bar (HP), Current Score counter, and Pause Button.
Pause Overlay Window: Freezes time scale (Time.timeScale = 0), provides Resume and Quit to Menu actions.
Game Over Screen: Displays final calculated score, Retry button (Reload Gameplay Scene), and Main Menu button.
