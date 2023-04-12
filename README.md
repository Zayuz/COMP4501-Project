# COMP4501-Project  
A term project made by Johnson, Kayla, and Ryan for COMP4501 at Carleton University.  
  
# Assets  
Wizard - https://assetstore.unity.com/packages/3d/characters/creatures/fantasy-monster-wizard-demo-103037  
Gambler Cat - https://assetstore.unity.com/packages/3d/characters/creatures/gambler-cat-20897  
Dog Knight - https://assetstore.unity.com/packages/3d/characters/animals/dog-knight-pbr-polyart-135227  
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
Camera Movement - Use the WASD keys or mouse movement to control the camera. Holding SHIFT will speed up the camera movement.  
Unit Selection - Left click on an allied unit or hold left click and drag over multiple allied units to select.  
Unit De-selection - Left click on an allied unit to deselect all units except the clicked unit, left click on something other than an allied unit (EX: terrain) to deselect all units.  
Unit Movement - Right click on terrain with allied unit(s) selected to command them to move to the right-clicked point.  
Unit Attacks - Right click on an enemy unit with allied unit(s) selected to command them to attack the enemy unit. Units will automatically attack an enemy at a frequency based on their attack speed once commanded to attack.  
Unit Gathering - Right click on a resource (crystals/potions) with allied unit(s) selected to command them to harvest said resource, increasing a unit's count of that resource.  
Consume Potion - If a selected unit has a potion available and is missing health, press the F key to consume the potion and restore the units health.  
Blink/Teleport - For the Cat Hero, when selected press Q to teleport to the location of the cursor. This ability has a cooldown.  
Speed/Damage buff - For the Dog Knight Hero, when selected press Q to increase speed and damage for 3 seconds. This ability has a cooldown.  
Heal - For the Wizard hero, when selected press Q on self or an ally to heal them in a burst. This ability has a cooldown.  

# Gameplay
To win the game, destroy the enemy tree. You will lose if your tree is destroyed, so protect it at all costs.  
The runic shield stones increase the defense of their teams' tree. If it is killed, the tree will become weaker.  
The shields also spawn wisps when you have enough summoning crystals. These wisps will attack the enemy tree, but can be intercepted and killed before they reach the tree.  
The Golem in the center of the map will drop a mushroom upon being killed which, when picked up, fully heals and buffs the character who picked it up.  
Units will respawn shortly after dying, but killing units will provide an opening to attack their shield and tree.  
Items will also respawn shortly after collecting them.  
