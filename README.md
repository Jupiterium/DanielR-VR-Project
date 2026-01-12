# VR Application Development Report


## 1. Introduction
The main objective of this continuous assessment was to design and prototype a Virtual Reality application that would incorporate the techniques outlined in the tutorials (Unity XR tutorials) of this module. 
The project itself then offers a physics-based traversal experince in a virtual 3D space. The main goal is to reach the dirigible after collecting energy cores and using them to interact with the glowing beacons.
<table border="0">
  <tr>
    <td><img src="https://github.com/user-attachments/assets/2d33d47b-211d-4a65-93aa-86c8713f2a47" width="300"></td>
  </tr>
</table>


## 2. Technical Achievements
- The project was developed using **Unity 2022.3.62f1** version and the **XR Interaction Toolkit**.
> **Make sure to use the same Unity version in order to run the application smoothly.**

### 2.1 Physics-Based Locomotion (Swinging)
> *Relevant Tutorial Topic: VR Locomotion*

The core technical feature of the game is the custom swinging system. 

It's controlled by the `Swing.cs` script attached to the hand controllers, which interacts directly with Unity's physics engine. This implementation demonstrates several advanced technical elements outlined in the project rubric:

* **Vector Math & Raycasting:** When the trigger is pressed, the script fires a `Physics.Raycast` along the controller's forward vector. A specific Layer Mask ("Swingable") ensures players can only attach to valid surfaces.
    * Wall Repulsion: To prevent clipping/getting stuck in objects , a safety cushion was implemented using vector math. In `FixedUpdate`, a raycast checks the player's velocity vector, and if a wall is detected within 2 meters, a repulsive force is applied along the surface normal to gently push the player away. This can be adjusted, but it creates that parallel to a wall flying feeling like in recent Spider-Man games.
* **Rigidbody Simulation:** Upon a valid hit, the script dynamically adds a **`ConfigurableJoint`** to the player's Rigidbody.
    * The `connectedAnchor` is set to the `raycastHit.point`.
    * The `LinearLimit` is calculated using `Vector3.Distance` between the hand and hit point, creating a tight connection.
* **Web Retraction System:** Holding the controller's grab button applies a directional `AddForce` towards the anchor point while simultaneously reducing the joint’s `LinearLimit`. This adds more flexibility and responsiveness, improving the traversal.

<table border="0">
  <tr>
    <td><img src="https://github.com/user-attachments/assets/f85a16fc-84e3-4138-803c-aec1c43024e2" width="400"></td>
    <td><img src="https://github.com/user-attachments/assets/f35c1c4f-63ca-447f-b398-8cce66b26ffc" width="500"></td>
    <td><img src="https://github.com/user-attachments/assets/2201e7d6-a8e5-4287-853f-43b859170137" width="400"></td>
  </tr>
  
  <tr>
    <td align="center">Flowchart of the Swing System</td>
    <td align="center">Swing.cs in the inspector</td>
    <td align="center">You can see that each hand gets its own Swing controller</td>
  </tr>
</table>


### 2.2 Object Interaction & Events
> *Relevant Tutorial Topic: Sockets, Grabbable Objects, Activation Events*

I utilized the **XR Socket Interactor** event system to create a modular interaction architecture.

* **Socket Logic:** Each Beacon is equipped with a socket that accepts specific interaction layers (for different energy cores).
* **Event Binding:** Instead of hard-coding references, I utilized the `Select Entered` event in the inspector to trigger the `SocketOccupied()` method in the Game Manager.
* **State Locking:** A key implementation detail was preventing players from "spamming" the same socket to increment the counter. I achieved this by having the socket disable its own `socketActive` boolean via the `OnSelectEntered` event. This effectively locks the energy core in place permanently, and provides some visual feedback.

<table border="0">
  <tr>
    <td><img src="https://github.com/user-attachments/assets/5730e83e-a519-4566-95de-c2440ab6785c" width="400"></td>
    <td><img src="https://github.com/user-attachments/assets/2391ca44-a22a-45dd-87ca-52d76901026a" width="400"></td>
    <td><img src="https://github.com/user-attachments/assets/c144b153-23de-49e9-ad30-3d45d0864fcd" width="400"></td>
    <td><img src="https://github.com/user-attachments/assets/dfd8373d-32ca-4508-b2fe-673bf1655e12" width="400"></td>
  </tr>
  
  <tr>
    <td align="center">The glowing beacon</td>
    <td align="center">Socket itself</td>
    <td align="center">Using Socket's Interactable Events for different actions</td>
    <td align="center">The layout in the hierarchy</td>
  </tr>
</table>


<table border="0">
  <tr>
    <th colspan="3" align="center"><h4>Energy Cores</h4></th>
  </tr>

  <tr>
    <td><img src="https://github.com/user-attachments/assets/d2b7268f-9326-4509-8b52-bf0b8a97f219" width="200"></td>
    <td><img src="https://github.com/user-attachments/assets/e46590c3-aa1e-4e1d-b599-2cd727362c1f" width="200"></td>
    <td><img src="https://github.com/user-attachments/assets/aa47784a-b0fb-4b24-9849-743d361bad0a" width="200"></td>
  </tr>
</table>

### 2.3 Scene Management & Game Loop
> *Relevant Tutorial Topic: VR Project Setup*

After testing the game with multiple people and some discussions with the lecturer, a tutorial level became a consideration. Hence, the Unity’s `SceneManagement` API was used to handle game flow in a better, more user friendly way.
* **Tutorial Scene:** A small, straight environment designed to teach the player how to use the swinging mechanic and interact with sockets without getting distracted by other entities.
* **Main Game Scene:** Upon completing the tutorial, the player transitions to the main city. This separation ensures the player is competent with the controls before facing the main challenge in a bigger space.

<table border="0">
  <tr>
    <th colspan="5" align="Left"><h4>Tutorial Scene (you pass it by getting to the opposite side)</h4></th>
  </tr>
  
  <tr>
    <td><img src="https://github.com/user-attachments/assets/108e84fe-3fd4-4ef5-bdbf-d08237acebdc" width="500"></td>
     <td><img src="https://github.com/user-attachments/assets/2963e1e1-9908-4976-803e-8ab7e7639bcc" width="500"></td>
     <td><img src="https://github.com/user-attachments/assets/f0f56dc0-5d65-46f6-93f6-e9d060369ae6" width="500"></td>
     <td><img src="https://github.com/user-attachments/assets/d6ef622a-2def-493e-b4e7-80db0f89d63c" width="500"></td>
     <td><img src="https://github.com/user-attachments/assets/1fcfcbd7-4d35-4cc9-9b92-4ac603aaf226" width="500"></td>
  </tr>
</table>

<table border="0">
  <tr>
    <th colspan="4" align="Left"><h4>Main Game Scene</h4></th>
  </tr>
  
  <tr>
    <td><img src="https://github.com/user-attachments/assets/e956d001-21a2-4832-966a-9f5074a4a832" width="300"></td>
     <td><img src="https://github.com/user-attachments/assets/9857a457-6655-4a36-b4fb-398a5567393d" width="300"></td>
  </tr>
</table>

The game state is managed by a `GameManager2.cs` singleton. This script tracks the activation status of the three beacons and handles the transition to the completion state, which activates an **XR UI Canvas** and triggers the dirigible launch sequence (a video clip).


### 2.4 User Interface & Feedback
> *Relevant Tutorial Topics: UI/UX, Audio, Haptics*

* **Audio & Haptics:** Custom sound effects in the Swing system are played when webs are shot, as well as the spatial background music and sound effects for improved immersion. Haptics were also added to the controllers upon using grabbables and sockets, which too improves the UX.
* **Visual Feedback:** The Swing's `LineRenderer` is used to visualize the web. Its colour dynamically changes from white to red when the player is actively retracting the web. Additionally, **Particle System** was used and attached to the beacons to create vertical beams of light, guiding the player to their objectives from across the map. External/custom assets were also used to build the environment.
* **UI:** The UI elemets were built via the **XR UI Canvas** for better immersion. The instructions and other screens exist physically in the world rather than as a screen-space overlay, which helps maintain the player's sense of presence.


## 3. Technical Challenges & Solutions
### 3.1 Defining the Locomotion Type
* **Challenge:** The main question was on how to achieve high-speed, vertical traversal for a Spider-Man style game using standard Unity XR tools.
* **Thoughts:** Initial attempt was to utilize the provided `ClimbInteractable` system from the toolkit. It wasn't exactly suitable as it restricted movement to surface contact, preventing gap traversal. Similarly, the `ContinuousMoveProvider` lacked the physics capability to simulate momentum, resulting in a floating, weightless feeling.
* **Solution:** Adapting those locomotions was too complex for my understanding, so I chose a custom physics approach. That's where the `Swing.cs` script was developed to handle swinging physics independently using `ConfigurableJoints`, allowing for proper momentum-based swinging. In the end, it was used in combination with the given `MoveProvider` (for walking). That would cause an issue initially where the two would interfere with each other, but I simply disabled the provider whenever swinging was active.

<table border="0">
  <tr>
    <th colspan="4" align="Left"><h4>Continuous Turn and Move Providers with the Swing (hands) system.</h4></th>
  </tr>
  
  <tr>
    <td><img src="https://github.com/user-attachments/assets/5bf31f1d-84b9-41c8-bafd-866f74169868" width="500"></td>
     <td><img src="https://github.com/user-attachments/assets/fc376227-9ce7-4d88-9668-e9f0835392aa" width="400"></td>
  </tr>
</table>

### 3.2 Physics Tuning & Motion Sickness
* **Challenge:** Physics-based VR locomotion caused motion sickness due to jarring acceleration and bouncy movement during playtests with multiple peopele.
* **Thoughts:** Default Unity Rigidbody settings allowed for infinite acceleration, while the `ConfigurableJoint` spring settings were too elastic, causing too much of a rubber band effect.
* **Solution:** The solution was rather simple, where I had adjust the **Mass** and **LinearDrag** on the Rigidbody of the player, as well as playing around with the **Cushion Force** and **Distance** on the Swing system.

<table border="0">
  <tr>
    <td><img src="https://github.com/user-attachments/assets/4ec5e6d1-6abd-41f0-89bf-51992c571540" width="500"></td>
     <td><img src="https://github.com/user-attachments/assets/b3c33c3b-4efe-40e2-8988-45dfb279f710" width="500"></td>
  </tr>

 <tr>
    <td align="center">Swing settings</td>
    <td align="center">Rigidbody settings</td>
  </tr>
</table>

### 4. Limitations & Future Improvements
Currently, the web functions as a straight line connection between a hand and the anchor point. It creates a static tether that clips through buildings rather than wrapping around them physically. In other words, the web attached to a moving object does not follow it respectively. I attempted to solve this by updating the anchor point dynamically, but that didn't really work. The solution is more complex and was out of scope, so I decided to stick with static environment. 

However, that's where future improvements can be made and hence new features can be added such as drones, that will have a specific followed paths around the city, and the player could use them as another way to traverse by attaching web to them. This could then also be followed with assisted aiming to help the player attach to these moving objects easier (also based on playtest reviews).

There are some other limitations, but rather minor, like objects grabbed being interfered with the player's collider, causing movement issues. A future fix would be assigning grabbables on another layer that ignores collisions. 

Some other future improvements could possibly include: 
* Better immersion via more diverse audio and haptic feedbacks, specifically like a "swooshing" sfx during a swinging motion.
* More environmental challenges, like turrets, lazers, saws, etc.

## 5. Reflection
I have definitely enjoyed the process of making this project. Having different ideas in the beginnig, and then choosing something that wasn't easy to understand straightaway was interesting. 
Tutorials were probably the part where most learning outcome evolved for me. I learned a lot of new information about VR development, but I think most significantly was that I got more comfortable with it overall and more positive in my likings for it. 
One thing I would try and use more though in the future are the provided systems and options in the XR Toolkit. I initially was creating my own heavy scripts, but towards the end of the project I learned how flexible some given systems already are. **Interactable Events** for various actions were probably the biggest ones for me. The gave me a ton of new ideas, and that's something I will keep in mind for future development as well.
