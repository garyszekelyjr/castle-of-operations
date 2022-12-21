# Castle of Operations

## Members
    - Uday Patel
    - Gary Szekely
    - Navya Ravavarapu

## Scripts

    GameManager.cs
        Controller for the game manager, controls and gives information about the state of the game for the objects to use

    Player.cs
        Controller for the player, allows for movement, attacking, and other functionalty neseccsay for the player

    Mob.cs
        Controller for the mobs, allows for random movmement, path finding to the player, and attacking.

    Hallway.cs
        Generates each hallway and places the specfic mobs.

    Exit.cs
        Serves as the exit point for the player within each scene (except Start).

    HUDController.cs
        Controls the heads up display.

    PauseMenu.cs
        Controls the pause menu.

    OpponentDeter.cs

## Work Distribution
    ### Uday Patel
        - Scripts
            - Player.cs
            - GameManager.cs
            - HUDController.cs
            - Mob.cs
            - PauseMenu.cs

        - Other 
            - Music
            - Combat
            - Enemy Animation Controllers, Movement, & Path Finding, Character Controllers
            - Player Camera, Animation Controllers, & Movement Controller
            - Random Aestetics in scenes (lighting, fonts, objects, materials)
            - Boss Animation Controllers, Character Controllers


    ### Gary Szekely
        - Scripts
            - Player.cs (hot fixes for combat)
            - GameManager.cs (Level flow and functions for buttons)
            - Mob.cs (hot fixes for combat)
            - Hallway.cs
            - Exit.cs
        
        - Scenes
            - Start
            - Tutorial
            - Hallways

        - Other
            - Maze generation w/ Prim's algorithm
            - Mob spawning
            - One-shot audio


    ### Navya Ravavarapu
        - Scripts
            - Player.cs
            - Mob.cs
            - OpponentDeter.cs 
        - Scenes
            - Addition Arena 
            - Subtraction Arena
            - Multiplication Arena
            - Division arena 
        - Other 
            - Combat
            - Enemy Animators and Movement
            - Player Camera Controller
            - Aesthetics of Arena

## External
    Hero Model/Animtations: 
    https://assetstore.unity.com/packages/3d/characters/humanoids/rpg-tiny-hero-duo-pbr-polyart-225148

    Slime and Shell Model/Animations:
    https://assetstore.unity.com/packages/3d/characters/creatures/rpg-monster-duo-pbr-polyart-157762

    Chest and Beholder Model/Animations:
    https://assetstore.unity.com/packages/3d/characters/creatures/rpg-monster-partners-pbr-polyart-168251

    Font: OldLondon

    Music:
    https://www.chosic.com/download-audio/24210/
    https://www.chosic.com/download-audio/45434/

    Material:
    https://assetstore.unity.com/packages/2d/textures-materials/wood/stylized-wood-textures-213607
