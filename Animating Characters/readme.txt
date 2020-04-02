demo1 https://youtu.be/MJRVbVTnfj8
demo2 https://youtu.be/OERJQkUz7jY
demo3 https://youtu.be/n-mtBLXe4pg

Part2: 
User's guide: 
Use WSAD, space, left ctrl, and mouse wheel to control the camera. 
The character has three states. 
When you click the character with left click at the first time, it changes to walk mode with speed 1. 
Then if you click the character again with the left click, it changes to run mode with speed 2. 
If you click the character the third time, it will back to unselected mode. 
The you can use your mouse right click the ground for destination.


Design: 
The prefab of the agents has a capsules collider and navmesh agent with a sub character object. 
There is a agent controller script just like assignment b1 on the agents to control the navmeshagents.
There is a player controller script to control the locomotion of the character.
It will detact the location difference between the character and the nextpoint to update the animation parameters.

Part3:
User's guide:
Same as Part2

Design: 
There will be two lists in the controller script to determine which agents will walk or run.
The breaking mechanics will calculate the average distance between selected agents and the destination. 
I will also calculate the distance between each agents and destiantion, so there are two breaking mechanics to ensure all of them will break.