# COMP4501-Project  
A term project made by Johnson, Kayla, and Ryan for COMP4501 at Carleton University.  
  
# Assets  
Wizard - https://assetstore.unity.com/packages/3d/characters/creatures/fantasy-monster-wizard-demo-103037  
Gambler Cat - https://assetstore.unity.com/packages/3d/characters/creatures/gambler-cat-20897  
Dog Knight - https://assetstore.unity.com/packages/3d/characters/animals/dog-knight-pbr-polyart-135227  
Cat - https://sketchfab.com/3d-models/toon-cat-free-b2bd1ee7858444bda366110a2d960386  
Deer druid - https://sketchfab.com/3d-models/deer-druid-9f962da38a59441680ba3cfaf7f84f22  
Armadillo - https://www.turbosquid.com/3d-models/armadillo-andean-model-1732536#  
Health potion - https://sketchfab.com/3d-models/stylized-health-potion-downloadable-e8a09eb24da645c0b5a36f6004fbd67c  
Magic crystals - https://assetstore.unity.com/packages/3d/props/stylized-crystal-77275  
Magic effects - https://assetstore.unity.com/packages/vfx/particles/spells/magic-effects-bundle-247933  
Golem - https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/mini-legion-rock-golem-pbr-hp-polyart-94707  
Rocks - https://assetstore.unity.com/packages/3d/props/exterior/low-poly-styled-rocks-43486  
Forest - https://assetstore.unity.com/packages/3d/environments/landscapes/free-low-poly-nature-forest-205742  
More Forest - https://assetstore.unity.com/packages/3d/environments/landscapes/low-poly-simple-nature-pack-162153  
Health Bar - https://www.youtube.com/watch?v=BLfNP4Sc_iA  
Rune Stones - https://sketchfab.com/3d-models/low-poly-fantasy-rune-stone-b78539a01fce41fea5d4ad1d1afd7b83  
Damage Numbers and Text Pop-ups - https://www.youtube.com/watch?v=iD1_JczQcFY  

# Dependencies  
Blender (https://www.blender.org/download/) is required to compile and run the project  

# Controls
Camera Movement - WASD keys or mouse movement to control the camera.  
Unit Selection - Left click on an allied unit or hold left click and drag over multiple allied units to select.  
Unit De-selection - Left click on an allied unit to deselect all units except the clicked unit, left click on something other than an allied unit (EX: terrain) to deselect all units.  
Unit Movement - Right click on terrain with allied unit(s) selected to command them to move to the right-clicked point.  
Unit Attacks - Right click on an enemy unit with allied unit(s) selected to command them to attack the enemy unit. Units will automatically attack an enemy at a frequency based on their attack speed once commanded to attack.  
Unit Gathering - Right click on a resource (crystals/potions) with allied unit(s) selected to command them to harvest said resource, increasing a unit's count of that resource.  
Consume Potion - If a selected unit has a potion available and is missing health, press the F key to consume the potion and restore the units health.  
Blink/Teleport - For the Cat Hero, when selected press Q to teleport to the location of the cursor. This ability has a cooldown. More hero abilities will be implemented for all heroes.
Speed/damage buff - For the Dog Knight Hero, when selected press Q to increase speed and damage for 3 seconds. This ability has a cooldown. More hero abilities will be implemented for all heroes. 
Take Damage - Press G while a hero is selected to have that hero take a single point of damage. For testing. If the unit is idle, flinch animation will play.  

# Advanced Prototype Requirements
## Animation
There is a DogKnight unit on the east of the map that has the Hermite Spline implemented for movement.  
The movement is coded in the SplineMovement script and implemented in the SplineUnit script.  
As we will not be using this type of movement for our final submission, this is the only unit with that type of movement.  

## Character Animation
The GamblerCat, Wizard, and DogKnight each have a different idle, moving, taking damage, and attacking animation.  
The Golem has an idle and taking damage animation.  
The idle animation plays when the unit is still.  
The moving animation plays when the unit is moving.
The attacking animation plays with the unit is attacking.  
The taking damage animation plays when the unit takes damage and no other animations (other than idle) are happening. This can be tested by pressing G with a unit selected, or by attacking the golem in the middle of the map.  

## Physics and Steering
The cats have been moved to a large and blank landmass to the south of the map. This is so that they can demonstrate their wander behaviour properly.  
For testing the behaviour it is recommended that the tester pauses the game via unity controls and selects Cat Mage (2), the leader of the flock.  
They can then command that specific cat to move around and the other cats will automatically follow once it reaches the manual destination and starts to make decisions for the flock.  
Every time the cat leader reaches a destination, it will wander with the other cats in a new destination.  
All cats are using flock behaviours (alignment, avoidance, cohesion) at all times, but their influences are not particularly strong.  
When the cats arrive at their destination it is possible the swarm may obstruct the leader from being close enough to 'complete' the journey and select a new place for the cats to wander - 
to fix this the cats stray from the group automatically once they arrive at their destination until the cat leader gives them a new place to go.  
Side note, on cat swarm island specifically the raycasting for commands is not very consistent. If the cats are not responsive to commands, just keep clicking around in places until they are.  
