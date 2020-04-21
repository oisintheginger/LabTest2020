# LabTest2020
 OisinFitz Game Engines 2 Lab Test
 
 
 # Basic Functionality
 
Car class has two steering behaviours, Seek and Arrive which are weighted. Each generates a force, and that is applied to the car. The traffic light generator instantiates 'Traffic Lights' in a circle around itself and adds them to a public list. Each 'Traffic Light' has a trafficLight component, which stores its state in a public enum, which can be: Green, Red, Yellow. Each traffic light checks its own state and change conditions. It is not done by the generator. 
 
The Car takes the public list of traffic lights and screens through them for any active lights(Traffic lights in the Green State). It   then adds them to a separate lsit of the Active lights. This Active lights list is continuously updated to remove deactivate lights and   add newly activated lights.

The Car generates a random integer to pick from the list of active lights to set as its steering behaviour target.

For extra bits I added a basic camera movement, which rotates and object at the center of the screen, which the camera is childed to. I also added a colour changing background and a trail renderer with music for fun.

# Camera Controls
- Scroll wheel = Zoom IN/OUT
- W/S = Move Up/Down 
- A/D = Move Left/Right
