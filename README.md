# ML Agents Arena

### Abstract
For our final project we implemented an artificial intelligence with the help of the ML Agents package of Unity. The agent learned on a basic level how to avoid the incoming enemies spawned into the arena environment as well as picking up ammo packages to shoot the enemies.
Link to a YouTube Video: [https://youtu.be/sN2epvBnmQk]

### Description
For the realization of the project we used the Unity game engine as well as the package “ML Agents”. The basic idea was to create an environment for the agent that looks like an arena where enemies randomly spawn from all sides which the enemy has to avoid or kill. So the main goal of the agent is to survive as long as possible. In order to learn, we defined actions, observations (with raycasting) and rewards to our agent. By connecting to TensorFlow, the agent requests decisions and he can then perform the actions given by TensorFlow’s neural net. After training for a certain amount of time, a brain is created with the learned behavior.
The terrain was added later on with the help of Unity’s terrain tools package. Using timeline and CineMachine, we created a short dolly track showcase video (see YouTube link).

### References
- Unity's ML agents: https://github.com/Unity-Technologies/ml-agents
