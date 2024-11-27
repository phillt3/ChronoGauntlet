Names: Phillip Tracy, Jacob Rashoff, Youssef Mourad
Email: ptracy7@gatech.edu, jrashoff3@gatech.edu, ymourad3@gatech.edu
Canvas Account Name: Phillip Andrew Tracy, Jacob Martin Rashoff, Youssef Mohamad Mourad
Starting Scene File: Assets/Scenes/MenuScenes/MenuScreen

How To Play
- To begin playing, press "Start Game" on the main menu.
- Basic Controls: WASD keys to walk, SPACE TO JUMP, MOUSE to aim, LEFTMOUSEBTN to shoot
- Advanced Controls: With Dash Enhancement: SHIFT and walk in any direction to dash, With Double Jump Enhancement: Press SPACE twice to jump twice while in air
- Must defeat all enemies and/or pick up all enhancement before time runs out (seen in top right corner) to unlock the next arena. These are marked by red and yellow waypoints respectively
- Exit current arena after completing objectives by touching obelisk, marked by purple waypoint.
- To pause press ESC

Where to Observe Technology Requirements
- 3D Game Feel Game: Start menu, reset and replay through pause menu, game win screen to communicate success of beating the game, clearly defined goals via waypoints, timer, and dialog boxes

- Precursors to Fun Gameplay: Waypoints convey goals, player has freedom in order and style of completing objectives that lead to different gameplay variations via what pickups or enemies they go for first, trigger buttons and use special movement capabilities to solve puzzles, enemy AI, etc

- 3D Character with Real-Time Control: Character control and movement are expanded upon specifically with dash and double jump features to enhance gameplay, has engaging animation, low latency, and responsive sound design. Controls are simple and easy to pickup but with the ability to improve skills.

- 3D World with Physics and Spatial Simulation: All custom built maps, graphically and auditorily well represented, appropriate boundaries through use of trees, rocks, invisible colliders, and abysses with fall detection.
	- Environmental Physics Interactions: buttons open doors, animated platforms and doors, 	enemies react to the physics of being hit by a fast moving object (laser)

- Real-time NPC Steering Behaviors / AI: Multiple AI states (idle, following, attacking), smooth steering, fluid movement and animation

- Polish: Start Menu UI with credits page, can pause and quit game, no debug output visible, can exit software any time. UI themes are consistent, audio polish, unified graphics and aesthetic with sound themes matching current level, no glitches or easily escapable worlds, no obvious edge of the world or bottomless pit unless purposefully designed to contain that.

Known Issues
- With the dash ability, the player can find ways to teleport through objects that they should otherwise not be able to.
- A design decision was made early on to not use root motion for the player character to better expand upon the dash, double jump, and aiming mechanics.
- Because of the above, the player's footing can sometimes look like it is sliding. This was adjusted through manual properties in the scripts to look better.
- The AI relies more on independent variations, placement, and magnitude over advanced behaviors to create exciting gameplay.
- The context surrounding the story is only communicated through dialog and the environments themselves.
- Hit detection for the most part works but on occasion laser hits are not detected.

Manifest

Phillip Tracy
1. Actions Performed
- Player design, movement, effects, health, animation, behavior, shooting, and scripting
- Created all levels (assets, order, puzzles, enemy placement, skybox, colors, animations, etc)
- Added all Audio (music and all sound design)
- Implemented Tip boxes and question mark pickups
- Implemented all movement and combat enhancements (assets, scripting, animation, behavior, placement)
- Created the Credit Game Menu, the game completion scene, and polished the start menu and pause menu
- Puzzles and trigger orbs (design, creation, and scripting)
- Added larger enemy AI and integrated the rest of the AI into the main project
- End level obelisks (design, placement, animation, and scripting)
- Moving platforms, doors, and other animated objects
- Created waypoint markers (design, implementation, scripting)
- Polished game controller to handle increasingly complex level logic
- Timer logic
- End level logic

2. Assets Utilized:
-3D Character Sci-Fi Robots Bundle
-Basic Motions Free
-Cartoon FX Remaster Free
-Collectables Sound Effects Pack
-Customizable Skybox
-Free Casual Game SFX Pack
-Hana Pixel Font
-Handpainted Grass & Ground Textures
-Logic Blox Models
-Low_Poly Nature
-Low-Poly Simple Nature Pack
-Low Poly tropical Island Lite
-Rocks and terrains Pack - Low Poly
-Simple Low Poly Nature Pack

3. C# script files
-/Assets/Scripts/GameController/
EndLevel
EndLevelSoundEvent
Timer

- All scripts in /Assets/Scripts/Pickups/

- All scripts in /Assets/Scripts/Player/

- /Assets/Scripts/TriggerSphere/GlowOnHit

- All scripts in /Assets/Scripts/Tutorial/

- /Assets/Scripts/Waypoints/WaypointSystem

Yousseff Mourad
1. Actions Performed
- Enemy AI (assets, design, behavior, scripting)
- GameController implementation to handle high level game logic

2. Assets Utilized:
- RPG Monster DUO PBR Polyart

3. C# script files
-/Assets/Scripts/GameController/GameController

- All scripts in /Assets/AI/Scripts/

Jacob Rashoff
1. Actions Performed
- GUI (initial Main Menu, Pause Menu)

2. No Assets utilized

3. C# script files
-/Assets/Scripts/GameMenu/MenuScript



