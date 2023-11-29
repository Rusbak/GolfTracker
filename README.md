# **GolfTracker**

## Overview of the Game
The idea of the game is Wii Golf. But with the addition of having to walk to the ball yourself as well as figuring out what club to use, just as in real life.

### Main parts of the game
- **Player** - the player can be moved around using the directional keys(WASD or arrows), rotated around using the mouse, and swing the golf club using space bar
- **Camera** - there are two cameras, both attached to the player object. Switch between them using scroll wheel
- **Flag** - as in real life the flag prefab consists of various shaped colliders. Though the hole is is a cylinder above ground, using the "isTrigger" property instead. The hole cylinder has a tag that is used for tracking contact with the ball.
- **Shot Slider** - this slider tells the user how far the selected golf club can shoot a "perfect" shot. When space bar is held down, the slider goes up and tells the user how far the ball will go if they let go of space bar at that point. The slider has a gradient color, from red to green to red again. The color should signify whether it is a "good" or a "bad" shot, meaning whether or not the length is within the set ranges for the selected club. Unfortunately bad shots are not penalized and works just like good shots, instead of giving the trajectory of the ball some curve or other bad stuff.
- **Ball Detector** - the ball detector is a transparent green cylinder attached to the player object. It uses the "isTrigger" property to detect contact with the golf ball. When in contact, the player is put into "shoot mode", meaning the player is rotated 90 degrees and aligned next to the golf ball
- **Golf Ball** - before the golf ball is shot, it's trajectory is calculated based based on parameters, such as: the distance it should cover(from shot slider), initial angle(from selected golf club), and golf ball object current rotation
- **Ground** - the ground has a box collider, allowing for the player to walk on it. It also has a ground layer tag. When in contact with the golf ball, the ball is frozen in position and rotation, and then placed above the ground in the RayCast's hit point

### Game Features
- The user can pick between 26 golf clubs, that each have different ranges and shooting angle
- The distance of the shot is based on the selected golf club and the time taken to shoot the ball (space bar)
- The game keeps track of what hole is being played and how many shots has been taken

## Running the Game
1. Download Unity2021.3.9f1
2. Download or Clone this project
3. The game requires Mouse and Keyboard

## Project Parts
### Scripts
- **CalculateShot** - information about the different golf clubs, the color of the slider, and time for spacebar held down
- **PlayerMovement** - Everything related to the player. Camera movement/switching, movement, animation, current hole/shots and some other GUI elements
- **tteessttiinngg** - Everything related to the golf ball. Collision with the flag, player and ground. The trajectory that the ball will fly in. And alternative shoot mode for putting, based on distance travelled, rather than collision

### Time Management
Remember to write this part!! and commit/push
|**Things Done**|**Hours Taken**|
|--------------------------------------------|-----------|
|Setting up the Unity Project|0.5|
|CalculateShot Script|5.5|
|PlayerMovement Script|5|
|tteessttiinngg Script (golf ball Script)|10|
|Connections between Scripts and other scripts/game objects|1.5|
|Player Prefab + Textures|2.5|
|Other Prefabs|0.5|
|Animations|3|
|Debugging Animations|2.5|
|Adding Finished Unity Project to GitHub Repository|1|
|Writing ReadMe File|1|
|**Total Time**|**33 hours**|


## References
[Distances based on data from local golf club](https://www.halsgolf.dk/gaester/18-huls-bane/baneguide/)

### Textures from [Textures.com](https://www.textures.com/):
[Worn Rubber(handle of golf club)](https://www.textures.com/download/3DScans0154/132411)

[Stone Surface with Indents(golf ball)](https://www.textures.com/download/3DScans0078/128149)

[Cotton Fabric(shirt)](https://www.textures.com/download/PBR0291/134373)

[Denim Fabric(pants)](https://www.textures.com/download/PBR0292/134374)

[Cashmere Fabric(hair)](https://www.textures.com/download/PBR0290/134372)
