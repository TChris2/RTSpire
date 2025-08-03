# Version 0.1.5 Options Update - 8/2/2025
## **UI General:**
- Redid how UI is handled.
    - Instead of instantiating UI when needed, instead needed UI for a scene/level is stored in one parent object.
- Redid existing buttons so they are navigable through both keyboard or controller.
- Updates to font materials.
- Changed Health UI Animator to now only use one object which switches between the different images (RTNormal, RTHurt, RTDead).
## **Pause Screen Menu:**
- Accessed by pressing the esc key or start on controller.
    - Pressing esc key will also resume the game if on the pause screen or close the sub menu you are currently in.
- Contains:
    - Resume Game:
        - Resumes game.
    - Options Menu:
        - Opens option menu.
    - Hub:
        - Does not have functionality currently.
    - Main Menu:
        - Goes back to main menu
    - Exit Game:
        - Exits game.
    - ### **Options Menu:**
        - Implemented a scroll bar script from https://gist.github.com/spireggs/2b8221a2961f2352542fad8f9a9dc07e to allow for smoother movement in the option menus.
        - **Audio Settings:**
            - Volume sliders for Master, Music, SFX, and Tumble Clips.
            - Slider to adjust Yap Rate.
            - Slider to adjust Min and Max Yap Delay.
            - Added option to disable enemy voice clips from playing.
            - Added option to enable/disable specific clips or use presets.
                - Option to have disabled clips to be replaced by Youtube.
        - **Game Settings:**
            - Full Screen option.
            - Camera options:
                - Camera Rotation Sensitivity
                - Camera Zoom Sensitivity
                - Inverted Controls
            - Option to disable and reenable specific Tumble designs.
                - Option to have disabled designs to be replaced by Youtube censors.
## General:
- Any ongoing sound effects not tied to the player’s death animations are paused during player death animation.
- Existing RT voice clips have been organized into folders and the “clip#” has been removed from the file names.
- Set target frame rate of 60.
- Game window can now be adjusted to any window size and adjusts the screen’s size accordingly.
    - Implemented from Max O’Didily’s aspect ratio script Unity How to Lock Camera View to an Aspect Ratio and Add Black Bars.
- 57 new RT voice clips have been added to the enemy voice clip pool.
- RT voice clip & Tumble designs storage has been reworked for easier scalability.
- All sprites have been reorganized into sprite sheets.
    - All sprites now use sprite renderers instead of just materials.
## Health Muffins:
- Added fade in and out animations.
- Added check which stops the muffin from healing the player if at max health.
- Increased range of bobbing animation.
- Added green heal hint overlay effect & heal used animations.
## TMachines:
- Added additional check to prevent enemies from spawning on a destroyed TMachine.
- Changed when TMachines can be hit again.
- Added hit hint overlay effect animation to TMachines.
- Changed when the destroy machine counter is updated alongside when the win state is applied.
- Combined EnemySpawn.cs code into TMachineState.cs.
    - Removed EnemySpawn.cs from TMachines.
## Player:
- Added invincibility overlay effect animation during player invincibility period.
- Added heal overlay effect animation when player gets healed.
- Combined both attack hitboxes into one object.
- Attack hitboxes are now static.
- Holding the zoom button will now continually zoom in or out.
## Cupcake:
- Added fade out animation to Cupcake.
## Enemies:
- Changes odds for when scream sfx can play for enemies.
- Added death fade animation to enemies.
## Animations General:
- Added hit effect to player, enemies, and TMachines.
- Added fade in/out animations to Cupcake, Muffins, & Enemies.