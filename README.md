# VR Application Development Report


## 1. Introduction
The main objective of this continuous assessment was to design and prototype a Virtual Reality application that would incorporate the techniques outlined in the tutorials (Unity XR tutorials) of this module. 
The project itself then offers a physics-based traversal experince in a virtual 3D space. The main goal is to reach the dirigible after collecting energy cores and using them to interact with the glowing beacons.


## 2. Technical Achievements
- The project was developed using **Unity 2022.3.62f1** version and the **XR Interaction Toolkit**. __The architecture prioritizes custom physics scripting over standard asset store solutions to meet the technical achievement criteria.__
> **Make sure to use the same Unity version in order to run the application smoothly.**


### 2.1 Physics-Based Locomotion (Swinging)
> *Relevant Tutorial Topic: VR Locomotion*

The core technical feature of the game is the custom swinging system. 

The system is controlled by a `Swing.cs` script attached to the hand controllers, which interacts directly with Unity's physics engine. This implementation demonstrates several advanced technical elements outlined in the project rubric:

* **Vector Math & Raycasting:** When the trigger is pressed, the script fires a `Physics.Raycast` along the controller's forward vector. A specific Layer Mask ("Swingable") ensures players can only attach to valid surfaces.
    * *Wall Repulsion:* To prevent clipping/getting stuck in objects , I implemented a safety cushion using vector mathematics. In `FixedUpdate`, a raycast checks the player's velocity vector, and if a wall is detected within 2 meters, a repulsive force is applied along the surface normal to gently push the player away. This can be adjusted, but it creates that parallel to a wall flying feeling like in recent Spider-Man games.
* **Rigidbody Simulation:** Upon a valid hit, the script dynamically adds a **`ConfigurableJoint`** to the player's Rigidbody.
    * The `connectedAnchor` is set to the `raycastHit.point`.
    * The `LinearLimit` is calculated using `Vector3.Distance` between the hand and hit point, creating a taut tether.
* **Web Retraction System:** Holding the controller's grab button applies a directional `AddForce` towards the anchor point while simultaneously reducing the joint’s `LinearLimit`. This adds more flexibility and responsiveness, improving the traversal.


### 2.2 Object Interaction & Events
> *Relevant Tutorial Topic: Sockets, Grabbable Objects, Activation Events*

I utilized the **XR Socket Interactor** event system to create a modular interaction architecture.

* **Socket Logic:** Each Beacon is equipped with a socket that accepts specific interaction layers (Red, Blue, Green).
* **Event Binding:** Instead of hard-coding references, I utilized the `Select Entered` event in the inspector to trigger the `SocketOccupied()` method in the Game Manager.
* **State Locking:** A key implementation detail was preventing players from "spamming" the same socket to increment the counter. I achieved this by having the socket disable its own `socketActive` boolean via the `OnSelectEntered` event. This effectively locks the energy core in place permanently, providing visual feedback that the task is complete while securing the game state.


### 2.3 Scene Management & Game Loop
> *Relevant Tutorial Topic: VR Project Setup*

After testing the game with multiple people and some discussions the lecturer, a tutorial level became a consideration. Hence, the Unity’s `SceneManagement` API was used to handle game flow in a better, more user friendly way.
* **Tutorial Scene:** A small, straight enclosed environment designed to teach the player how to use the swinging mechanic and interact with sockets without getting distracted by other entities.
* **Main Game Scene:** Upon completing the tutorial, the player transitions to the main city. This separation ensures the player is competent with the controls before facing the main challenge in a bigger space.

The game state is managed by a `GameManager2` singleton. This script tracks the activation status of the three beacons and handles the transition to the "Win State," which activates a World Space UI canvas and triggers the dirigible launch sequence (a video clip).


### 2.4 User Interface & Feedback
> *Relevant Tutorial Topics: UI/UX, Audio, Haptics*

* **Audio & Haptics:** Custom sound effects in the Swing system are played when webs are shot, as well as the spatial background music and sound effects for improved immersion. Haptics were also added to the controllers upon using grabbables and sockets, which too improves the UX.
* **Visual Feedback:** The Swing's `LineRenderer` is used to visualize the web. Its colour dynamically changes from white to red when the player is actively retracting the web. Additionally, Particle Systems using Additive materials were attached to the beacons to create vertical beams of light, guiding the player to their objectives from across the map.
* **UI:** The UI elemets were built via the **XR UI Canvas** for better immersion. The instructions and other screens exist physically in the world rather than as a screen-space overlay, which helps maintain the player's sense of presence.

## 3. Technical Challenges & Solutions
...to do...

### 4. Limitations & Future Work
While functional, the swinging mechanic currently relies on a fixed-length tether. If a player attaches to a point too low, they may drag on the ground. A future iteration would implement a "reel-in" mechanic (dynamically reducing the `LinearLimit` of the joint) to allow players to pull themselves upwards, adding verticality to the movement. Additionally, implementing an audio mixer to duck background ambience during key narrative events would improve the overall polish.

## 5. Reflection
The project successfully meets the technical requirements by implementing a complex locomotion system distinct from the standard provider. The specific focus on the `Swing.cs` script demonstrated how `ConfigurableJoints` can be manipulated at runtime to create compelling VR movement.
