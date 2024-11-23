## About
Aim Zone is a 3D FPS game set in a shooting range environment, designed to test players' aiming skills. The game features three difficulty levels: Easy, Normal, and Hard, catering to players of all skill levels. A scoring system tracks performance based on the number of targets destroyed, encouraging players to improve their accuracy and reaction time. Additionally, the game includes immersive animations, such as weapon breath and sprint animations, enhancing the overall gameplay experience with a realistic and engaging touch.

<tbody>
    <tr>
      <td><img src="https://github.com/Swiper0/Swiper0/blob/main/GIF/AimZoneDemo.gif"/></td>
    </tr>
  
<br>

## Scripts and Features
Scripts:
|  Script       | Description                                                  | Development Time |
| ------------------- | ------------------------------------------------------------ | -------------- |
| `Enemy.cs` | Handles the enemy's health, damage, and destruction. It flashes red when hit, triggers a death event when health reaches zero, updates the score, and destroys the enemy after its lifetime expires. | ≈ 3 hours |
| `ModeController.cs` | **Handles** the mode selection in the game. It allows the player to choose between Easy, Normal, or Hard modes using number keys 1, 2, or 3 before the game starts. Once a mode is selected, it updates the mode in the `TargetSpawner` and displays the selected mode on the screen. After the game starts, mode selection is disabled. | ≈ 4 hours |
| `Models.cs`  | Handles the player and weapon settings. PlayerSettingsModel configures movement, sensitivity, speed modifiers, and jumping. WeaponSettingsModel defines weapon damage, sway effects, and movement-related sway. Enums like PlayerStance and WeaponFireType set the player’s stance and weapon firing modes. | ≈ 4 hours |
| `PlayerControll.cs`  | **Handles** the player's movement, aiming, stance, and other controls. The `PlayerControll` script uses input from the player to move the character, adjust view sensitivity, crouch, sprint, lean, and aim. It also manages gravity, jumping, and falling, as well as updating the player’s weapon state. The script saves and loads player settings (such as sensitivity and stance) using `PlayerPrefs`. The `Shooting` and `Leaning` features are also handled by listening to input and adjusting the player's state accordingly. | ≈ 6 hours |
| `ScoreManager.cs`  | Handles the player's score by tracking it with a singleton pattern. It includes methods to add to the score (AddScore(int amount)) and reset it (ResetScore()). | ≈ 3 hours |
| `ScoreUI.cs`  | Handles the display of the player's score on the UI. It updates the scoreText every frame with the current score from ScoreManager. | ≈ 1 hours |
| `SensitivityController.cs`  | Handles player's sensitivity settings. It listens for the minus (-) key to decrease sensitivity, and the plus (+) to increase sensitivity. | ≈ 1 hours |
| `StartStopController.cs`  | Handles in this context refers to how the StartStopController script manages (handles) the game start and stop processes through two methods: StartGame to begin target spawning and StopGame to stop target spawning, both controlled by the TargetSpawner. | ≈ 1 hours |
| `TargetSpawner.cs`  | Handles target spawning in the game. It starts and stops spawning with StartSpawning() and StopSpawning(). It adjusts the target's lifetime based on the game mode set via SetMode(). In Update(), it spawns new targets until the max limit is reached, and resets the score at the start of the game. The SpawnTarget() method creates targets at random positions. | ≈ 4 hours |
| `WeaponController.cs`  | Handles weapon control, including rotation, sway, and animation based on player input. Manages shooting mechanics, such as raycasting for hits and interacting with enemies or targets. Controls weapon sway when moving or aiming, adjusting weapon position and behavior. Handles fire rate, sound, and visual effects like line rendering for shooting. | ≈ 5 hours |
| `etc`  |  | ≈ 10 hours |


Post Processing used:
- Vignette
- Color Grading
- Ambient Occlusion

The game has:
- Player Movement
- Weapon Sprint Animation
- Shooting Mechanics
- Lean Camera
- Sensitivity Controller
- Difficulty Selector
- Weapon Breath Animation
- Crouch System
- Scoring System
- Post Processing 

<br>

## Game controls
The following controls are bound in-game, for gameplay and testing.

| Key Binding       | Function          |
| ----------------- | ----------------- |
| W,A,S,D       | Standard movement  |
| Mouse1       | Shoot  |
| Mouse2       | Aim / Scope  |
| Q,E       | Lean |
| Shift       | Sprint  |
| Ctrl      | Crouch |
| - / _      | Decrease sensitivity  |
| = / +       | Increase sensitivity  |
| 1,2,3       | Difficulty mode  |

<br>

## Notes
this game is developed in **Unity Editor 2022.3.7f1**

Asset used:
- Weapon : https://ng1994.itch.io/realistic-mp5-submachine-gun-3d-model
- Sound : https://www.soundsnap.com/warfare_weapon_sub_machine_gun_automatic_9mm_heckler_and_koch_mp5_single_shot_distant_perspectiv2
- Skybox : https://assetstore.unity.com/packages/2d/textures-materials/sky/skybox-series-free-103633
- Target icon : https://www.pngwing.com/en/free-png-bnkmk
- Font : https://www.dafont.com/hello-jerry.font

Texture materials :
- https://www.poliigon.com/texture/fine-rocky-dirt-ground-texture/7006
- https://www.poliigon.com/texture/slip-match-oak-wood-veneer-texture-beige/7760
- https://www.poliigon.com/texture/plain-white-ceramic-texture/5212
- https://www.poliigon.com/texture/reclaimed-running-bond-brick-texture-whitewashed-orange/7787
- https://www.poliigon.com/texture/ash-wood-board-texture-light-beige/4186

Number icons :
- https://www.iconexperience.com/g_collection/icons/?icon=keyboard_key_1&style=standard
- https://www.iconexperience.com/g_collection/icons/?icon=keyboard_key_2&style=standard
- https://www.iconexperience.com/g_collection/icons/?icon=keyboard_key_3&style=standard
