# Space Shooting Mobile - Prototype

* Game Name: Space Shooting Mobile
* Author: Hugo Uchoas Borges
* Unity Version: 2022.3.10f1

* Game Demo Video: 
* [![CS50G Final Assignment](http://img.youtube.com/vi/q-JlsB4Uz2E/0.jpg)](https://youtu.be/q-JlsB4Uz2E)

## Controls

* W or ArrowUp: Up
* S or ArrowDown: Down
* A or ArrowLeft: Left
* D or ArrowRight: Right
* Space: Shooting
* Return or Escape: Pause Game
* R: Resume Game

## Where to Play

You can play my assignment by:
* Opening it in Unity 2022.3.10f1
* Compiling the project yourself
* Extracting the already built game in *./Output/cs50_space_shooter.rar*
* Via Itch here: [itch - edx-space-shooting](https://hugouchoasborges.itch.io/edx-space-shooting) -- This webgl version doesn't have an exit button

## Game Features

* Object Pooling -- Used for explosion particles, enemies and background elements
* FSM with an easy-to-use Custom Editor interface
* Tag Logging -- I can log stuff using some tags so I can enable\disable logs based on Tags (Custom Editor)
* Enemies Spawning with difficulty progression and pooling
* Dynamic Background -- Created by spawning (and pooling) star particles and background meteors
* Custom build system -- In order to version each update and simplify the build process to multiple platforms

Now I'm going to describe some infra stuff I created in order to handle FSM, Building, Spawning, Pooling and Sounds

### Project Structure Overall

![filesystem_00](./Git_Media/filesystem_00.png?raw=true "File System")

### Scripts Structure

Runtime Scripts
![filesystem_01](./Git_Media/filesystem_01.png?raw=true "File System")


Editor Scripts
![filesystem_02](./Git_Media/filesystem_02.png?raw=true "File System")

### Finite State Machine (FSM)

I've created an FSM system with a CustomEditor interface so I can register\remove elements via interface:
* FSM Controllers
* FSM States
* FSM Events

![fsm_00](./Git_Media/fsm_00.png?raw=true "FSM Custom Editors")

Altering the entries list seem in this picture would also change the ENUM related to it, recompiling the code

![fsm_01](./Git_Media/fsm_01.png?raw=true "FSM Events window --> Code")

To use this, simply add a **FSMStateController** component to any GameObject, register its states and you're ready to go

![fsm_02](./Git_Media/fsm_02.png?raw=true "Player FSMStateController")

Existing states in this project

* Application: PREPARING, IDLE, MENU, GAME, PAUSED, PLAYER_SELECTOR, GAME_OVER
* Background: IDLE
* Player: PREPARING, IDLE, GAME, PLAYER_SELECTOR, PAUSED
* Enemy:PREPARING, IDLE
* Input: IDLE
* Gun: IDLE
* MenuController: IDLE, MENU, GAME, PAUSED, PLAYER_SELECTOR, GAME_OVER


### Pooling System

I've created a simple pooling system so I can recycle common elements spawning in the scene
* Enemies
* Explosions
* Guns

- ![pooling_system_00](./Git_Media/pooling_system.gif?raw=true "Enemies Spawning")

### Enemy Spawning and difficulty progression

The enemy spawning system has 3 configurable via inspector areas:
* Spawn area - Where the enemies can be spawned
* Destination area - Where the spawned enemies can head to
* Destroy area - The area all enemies should keep inside. Going outside of it will re-enqueue the enemy to the pool

- ![enemy_spawning_00](./Git_Media/enemy_spawning_00.png?raw=true "Enemies Spawning")

### Tag Logging

Using the same infra code from FSM Custom Editor, I've created a simple but elegant Logging System

- ![logging_00](./Git_Media/logging_00.png?raw=true "Logging")

### Custom Build System

Lastly, I've created a build system that can manage versions and create a CHANGELOG.md for each build

- ![build_00](./Git_Media/build_00.png?raw=true "Build System")