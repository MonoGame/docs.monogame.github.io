---
title: "Chapter 23: Completing the Game"
description: "Finalize game mechanics by updating our current demo into a snake-like inspired game."
---

In [Chapter 22](../22_snake_game_mechanics/index.md) we implemented the core mechanics of a snake-like game by creating the [`Slime`](../22_snake_game_mechanics/index.md#the-slime-class), [`Bat`](../22_snake_game_mechanics/index.md#the-bat-class) and [`GameController`](../22_snake_game_mechanics/index.md#the-gamecontroller-class) classes.  While these classes handle the foundational gameplay, a complete game needs additional elements to provide player feedback, manage game states, and create a polished experience.

In this chapter, you will:

- Create a dedicated UI class to manage the UI for the game scene.
- Implement pause and game over screens with appropriate controls.
- Refactor the `GameScene` class to coordinate all game elements.
- Add game state management to handle playing, paused, and game over conditions
- Implement input buffering to improve control responsiveness
- Connect all elements to create a complete, playable game.

## The GameSceneUI Class

Currently, the `GameScene` class contains the methods for initializing and creating the pause menu.  However, now that we have a defined condition for game over, we need to create a game-over menu as well.  To do this, we will take the opportunity to refactor the current code and pull the UI-specific code into its own class.

In the *UI* directory of the game project, create a new file named *GameSceneUI* and add the following initial code:

[!code-csharp[](./snippets/gamesceneui/definition.cs)]

This code establishes the foundation for our `GameSceneUI` class, which inherits from Gum's `ContainerRuntime` class. This inheritance means our UI class is itself a UI container that can hold and manage other UI elements. We've included all necessary using statements for MonoGame, Gum UI components, and our library references.

Let's build out this class by adding each section in sequence. Follow the order below to create the complete UI management system for our game scene.

> [!NOTE]
> You may see compiler errors as you add these sections one by one. This is expected because some parts of the code will reference fields, properties, or methods that we haven't added yet. Once all sections are in place, these errors will resolve.

### GameSceneUI Fields

Add the following fields to the `GameSceneUI` class:

[!code-csharp[](./snippets/gamesceneui/fields.cs)]

Let's break down what each of these fields is responsible for:

- `s_scoreFormat`: A string format template used to display the player's score with leading zeros.
- `_uiSoundEffect`: Stores the sound effect played for UI interactions like button clicks and focus changes.
- `_pausePanel`: The panel containing the UI elements shown when the game is paused.
- `_resumeButton`: A reference to the resume button, allowing us to set focus on it when the pause panel is shown.
- `_gameOverPanel`: The panel containing the UI elements shown when a game over occurs.
- `_retryButton`: A reference to the retry button, allowing us to set focus to it when the game over panel is shown.
- `_scoreText`: The text display showing the player's current score.

### GameSceneUI Events

After the fields, add the following events to the `GameSceneUI` class:

[!code-csharp[](./snippets/gamesceneui/events.cs)]

These events allow the `GameSceneUI` class to notify the `GameScene` when important UI actions occur:

- `ResumeButtonClick`: Triggered when the player clicks the Resume button on the pause panel.
- `QuitButtonClick`: Triggered when the player clicks the Quit button on either panel.
- `RetryButtonClick`: Triggered when the player clicks the Retry button on the game over panel.

### GameSceneUI Constructor

Add the following constructor to the `GameSceneUI` class after the events:

[!code-csharp[](./snippets/gamesceneui/constructor.cs)]

This constructor initializes all UI components:

1. Set the container to fill the entire screen.
2. Adds itself to Gum's root element.
3. Loads necessary assets (sound effect and texture atlas).
4. Creates and adds child elements in the correct order.

### GameSceneUI UI Creation Methods

To keep the code more organized, we will create separate functions to build the individual UI elements that will be managed by the `GameSceneUI` class.

#### Creating the Score Text

First, we will add a method to create a `TextRuntime` element that is used to display the player's score.  Add the following method to the `GameSceneUI`  after the constructor:

[!code-csharp[](./snippets/gamesceneui/createscoretext.cs)]

#### Creating the Pause Panel

Next, we will add a method to create a `Panel` element that is shown when the game is paused, including the "Resume" and "Quit" buttons.  Add the following method to the `GameSceneUI` class after the `CreateScoreText` method:

[!code-csharp[](./snippets/gamesceneui/createpausepanel.cs)]

#### Creating the Game Over Panel

Finally, we will add a method to create a `Panel` element that is shown when a game over occurs, including the "Retry" and "Quit" buttons.  Add the following method to the `GameSceneUI` class after the `CreatePausePanel` method:

[!code-csharp[](./snippets/gamesceneui/creategameoverpanel.cs)]

Both the pause panel and the game over panel use event handlers for their buttons.  Let's add those next.

### GameSceneUI Event Handlers

After the `CreateGameOverPanel` method, add the following method to the `GameSceneUI` class:

[!code-csharp[](./snippets/gamesceneui/eventhandlers.cs)]

These event handlers provide audio feedback and appropriate UI updates when buttons are clicked or UI elements receive focus.

### GameSceneUI Public Methods

Finally, add the following public methods to the `GameSceneUI` class after the `OnElementGotFocus` method:

[!code-csharp[](./snippets/gamesceneui/publicmethods.cs)]

These public methods provide the interface for the `GameScene` to:

- Update the score display.
- Show or hide the pause menu.
- Show or hide the game over menu.
- Update and draw the UI components.

With the `GameSceneUI` class complete, we now have a fully encapsulated UI system that can handle displaying game information (score), providing feedback for game states (pause, game over), and processing user interactions (button clicks). This separation of UI logic from game logic will make our codebase much easier to maintain and extend.

Now that we have all our specialized components ready, let's refactor the GameScene class to coordinate between them and manage the overall game flow.

## Refactoring The GameScene Class

Now that we have created the encapsulated [`Slime`](../22_snake_game_mechanics/index.md#the-slime-class), [`Bat`](../22_snake_game_mechanics/index.md#the-bat-class), and [`GameSceneUI`](#the-gamesceneui-class) classes, we can refactor the `GameScene` class to leverage these new components.  This will make our code more maintainable and allow us to focus on the game logic within the scene itself.  We will rebuild the `GameScene` class to coordinate the interactions between the components.

In the *Scenes* directory of the DungeonSlime project (your main game project), open the *GameScene.cs* file and replace the code with the following initial code:

[!code-csharp[](./snippets/gamescene/definition.cs)]

This code provides the foundation for our refactored `GameScene` class. We have included all the necessary using statements to reference our new game object classes and UI components. The class will now focus on managing the game state and coordinating between our specialized component classes rather than implementing all the functionality directly.

The `GameScene` class now contains the following key fields:

- `GameState`: An enum that defines the different states that the game can be in (playing, paused, or game over).
- `_slime`: A reference to the slime (snake-like player character) instance.
- `_bat`: A reference to the bat (food) instance.
- `_tilemap`: The tilemap that defines the level layout.
- `_roomBounds`: A rectangle defining the playable area within the walls.
- `_collectSoundEffect`: The sound effect played when the slime eats a bat.
- `_score`: Tracks the player's current score.
- `_ui`: A reference to the game scene UI component.
- `_state`: The current state of the game represented by the `GameState` enum.

Now we will add the various methods needed to complete the `GameScene` class. Add each section in the sequence presented below. This will build up the scene's functionality step by step.

> [!NOTE]
> As with previous classes, you might encounter compiler errors until all sections are in place. These errors will be resolved once all components of the class have been added.

### GameScene Initialize Method

To set up the scene, add the following `Initialize` method after the fields in te `GameScene` class:

[!code-csharp[](./snippets/gamescene/initialize.cs)]

This method sets up the initial state of the game scene:

1. Disables the "exit on escape" behavior so we can use the escape key for pausing.
2. Calculate the playable area within the tilemap walls.
3. Subscribes to the slime's body collision event to detect when the player collides with itself triggering a game over state.
4. Initialize the UI components.
5. Set up a new game.

### GameScene InitializeUI Method

The `Initialize` method we just added calls a method to initialize the user interface for the scene.  Let's add that method now.  Add the following method after the `Initialize` method in the `GameScene` class:

[!code-csharp[](./snippets/gamescene/initializeui.cs)]

This method creates the UI components and subscribes to its events to respond to button clicks.

### GameScene UI Event Handlers

In the `InitializeUI` method we just added, we subscribe to the events from the `GameSceneUI` class that are triggered when buttons are clicked.  Now we need to add those methods that would be called when the events are triggered.  Add the following methods to the `GameScene` class after the `InitializeUI` method:

[!code-csharp[](./snippets/gamescene/eventhandlers.cs)]

These methods respond to the UI events:

- `OnResumeButtonClicked`: Resumes the game from a paused state.
- `OnRetryButtonClicked`: Restarts the game after a game over.
- `OnQuitButtonClicked`: Quits the game by returning to the title scene.

### GameScene InitializeNewGame Method

In the `Initialize` method we added above, it also makes a call to an `InitializeNewGame` method.  Let's add this now.  Add the following method to the `GameScene` class after the `OnQuitButtonClicked` method:

[!code-csharp[](./snippets/gamescene/initializenewgame.cs)]

This method will:

1. Position the slime in the center of the map.
2. Initialize the slime with its starting position and movement stride.
3. Randomize the bat's velocity and position it away from the slime.
4. Reset the player's score.
5. Set the game state to "Playing".

### GameScene LoadContent Method

Next, we need to add the method to load game assets for the scene.  Add the following method to the `GameScene` class after the `InitializeNewGame` method:

[!code-csharp[](./snippets/gamescene/loadcontent.cs)]

This method loads all necessary assets for the game scene:

1. The texture atlas containing the sprite graphics
2. The tilemap that defines the level layout.
3. The animated sprites for the slime and bat.
4. Sound effects for the bat bouncing and collecting.

### GameScene Update Method

Next, to update the scene add the following method to the `GameScene` class after the `LoadContent` method:

[!code-csharp[](./snippets/gamescene/update.cs)]

This method updates the scene in each frame to:

1. Always update the UI, regardless of game state.
2. Return early if the game is over.
3. Check for pause input and toggle the pause state if needed.
4. Return early if the game is paused.
5. Update the slime and bat.
6. Check for collisions between the game objects.

### GameScene CollisionChecks Method

In the `Update` method we just added, it makes a call to a `CollisionChecks` method to handle the collision detection and response.  Let's add that method now.  Add the following method to the `GameScene` class after the `Update` method:

[!code-csharp[](./snippets/gamescene/collisionchecks.cs)]

This method checks for three types of collisions:

1. Slime-Bat collision: The slime "eats" the bat, gains points, grows, and the bat respawns.
2. Slime-Wall collision: Triggers a game over if the slime hits a wall.
3. Bat-Wall collision: Causes the bat to bounce off the walls.

### GameScene PositionBatAwayFromSlime Method

The `CollisionCheck` method makes a call to `PositionBatAwayFromSlime`.  Previously, when we needed to set the position of the bat when it respawns, we simply chose a random tile within the tilemap to move it to.  By choosing a completely random location, it could be on top fo the head segment of the slime, forcing an instant collision, or it could spawn very close to the head segment, which adds not challenge for the player.

To ensure the bat appears in a random, but strategic location, we can instead set it to position away from the slime on the opposite side of the room.  Add the following method to the `GameScene` class after the `CollisionCheck` method:

[!code-csharp[](./snippets/gamescene/positionbatawayfromslime.cs)]

This method positions the bat after it's been eaten:

1. Determines which wall (top, bottom, left, or right) is furthest from the slime.
2. Places the bat near that wall, making it more challenging for the player to reach.

### GameScene Event Handler and Game State Methods

Next, we will add some of the missing methods being called from above that handle game events and state changes.  Add the following methods to the `GameScene` class after the `PositionBatAwayFromSlime` method:

[!code-csharp[](./snippets/gamescene/statechanges.cs)]

These methods handle specific game events:

- `OnSlimeBodyCollision`: Called when the slime collides with itself, triggering a game over.
- `TogglePause`: Switches between paused and playing states.
- `GameOver`: Called when a game over condition is met, showing the game over UI.

### GameScene Draw Method

Finally, we need a method to draw the scene.  Add the following method to the `GameScene `class after the `GameOver` method.

[!code-csharp[](./snippets/gamescene/draw.cs)]

This method handles drawing the scene by:

1. Clearing the screen.
2. Drawing the tilemap as the background.
3. Drawing the slime and bat sprites.
4. Drawing the UI elements on top.

By refactoring our game into these encapsulated components, we have created a more maintainable codebase with a clear separation of concerns:

- The `Slime` class handles snake-like movement and growth.
- The `Bat` class manages its movement and bouncing.
- The `GameSceneUI` class manages all UI components.
- The `GameScene` class coordinates between these components and manages the game state.

This architecture makes it easier to add new features or fix bugs, as changes to one component are less likely to affect others.

## Adding Input Buffering to the Slime Class

The game at this point is now playable.  If you test it out though, you may notice a small issue with inputs.  In games where movement updates happen at fixed intervals that are less frequent than input polling, inputs can sometimes feel unresponsive, expecially when trying to make multiple inputs in succession.

For instance, if a player wants to navigate a tight corner by pressing up and then immediately left, pressing these keys in rapid succession often results in only the second input being registered.  When this happens, the slime will only turn left without first moving upward, missing the intended two-part movement completely.  This occurs because the second input overwrites the first one before the game has a change to process it, leading to frustrating gameplay where the slime does not respond to the player's complete sequence of commands.

### Understanding Input Buffering

Input buffering is a technique used in game development to temporarily store player inputs that cannot be immediately processed. Instead of discarding these inputs, they are placed in a queue and processed in order when the game is ready to handle them.

In our snake-like game, the slime moves at fixed intervals rather than continuously. This creates a disconnect between when the player presses a button (which is checked every frame) and when the game can actually respond (which happens on a less frequent movement cycle). Without input buffering:

- Players must time their inputs perfectly to align with the game's update cycle.
- Rapid inputs are lost because only the last input is remembered.
- The game feels unresponsive during quick turns.

A well-implemented input buffer gives players a more forgiving and responsive experience by:

1. Storing inputs that arrive between movement updates.
2. Preserving the order of inputs for more predictable behavior.
3. Creating a sense that the game is actually listening to the player.

The size of an input buffer is an important design decision. If it is too small, players still might feel the game isn't responsive enough during complex sequences. If it is too large, the game might feel like it is playing itself as it works through the backlog of buffered commands.

For our snake-style game, a buffer size of two is typically ideal - enough to handle quick two-direction turns (like up-then-left to navigate a corner) without letting players queue too many moves ahead.

### Implementing Input Buffering in the Slime Class

Let's modify our `Slime` class to include an input buffer system. First, we will add the necessary fields to store our input queue.  In the *GameObjects* directory of the *DungeonSlime* project (your main game project), open the *Slime.cs* file and add the folliwing fields after the `_sprite` field:

[!code-csharp[](./snippets/slime/fields.cs)]

> [!NOTE]
> The [`Queue<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.queue-1?view=net-9.0>) is a First In, First Out (FIFO) collection in C#. When you add items with `Enqueue()`, they join the end of the line, and when you retrieve items with `Dequeue()`, you always get the oldest item (the one at the front of the line). Think of it like people waiting in line - the first person to arrive is the first one served.
>
> This contrasts with a [`Stack<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.stack-1?view=net-9.0>), which follows Last In, First Out (LIFO) behavior, where the most recently added item is the first one retrieved.

This queue will store the directional vectors (up, down, left, right) that we will apply to the slime's movement in the order they were received.

Next, we need to initialize this queue.  In the `Slime` class, locate the `Initialize` method and and update it to the following:

[!code-csharp[](./snippets/slime/initialize.cs?highlight=30-31)]

Next, we need to update the input handling method to store the inputs in the queue instead of immediately overwritting the `_nextDirection` field.  In the `Slime` class, locate the `HandleInput` method and update it to the following

[!code-csharp[](./snippets/slime/handleinput.cs?hightlight=3,22-38)]

1. The `potentialNewDirection` is now given the initial value of [**Vector2.Zero**](xref:Microsoft.Xna.Framework.Vector2.Zero).
2. A check is made to see if the player has pressed a direction key and if the input buffer is not already at maximum capacity.
3. If a new direction key is pressed and the buffer has space:
   1. The validation is made using [**Vector2.Dot**](xref:Microsoft.Xna.Framework.Vector2.Dot(Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Vector2)) just like before to ensure it is a valid direction
   2. If it is a valid direciton, then it is added to the queue.

Finally, we need to modifiy how we apply the movement direction during the movement update cycle.  In the `Slime` class, locate the `Move` method and update it to the following:

[!code-csharp[](./snippets/slime/move.cs?highlight=3-7)]

The key change here is that we now dequeue a direction from the input buffer rather than directly using the `_nextDirection` value. This ensures we process inputs in the order they were received, preserving the player's intent.

With these changes in place, our game now supports input buffering. This small enhancement improves how the game feels to play, particularly when making rapid directional changes.

Players will notice:

- When navigating a corner, they can quickly press up followed by left (or any other valid combination), and both inputs will be respected
- The game feels more responsive since it remembers inputs between movement update cycles
- Complex maneuvers are easier to execute since timing is more forgiving

The difference might seem subtle, but it significantly reduces frustration during gameplay.

## Putting It All Together

With all of these components now in place, our Dungeon Slime game has transformed from a simple demo built on learning MonoGame concepts into a complete snake-like game experience.  The player controls the slime that moves through the dungeon, consuming bats to grow longer.  If the slime collides with the wall or its own body, the game ends.

Let's see how it all looks and plays:

| ![Figure 23-1: Gameplay demonstration of the completed Dungeon Slime game showing the snake-like slime growing as it eats bats and a game over when colliding with the wall ](./videos/gameplay.webm) |
| :---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: |
|             **Figure 23-1: Gameplay demonstration of the completed Dungeon Slime game showing the snake-like slime growing as it eats bats and a game over when colliding with the wall**             |

1. The game starts with a single slime segment in the center of the room.
2. The player controls the direction of the slime by using the keyboard (arrow keys or WASD) or by using a game pad (DPad or left thumbstick).
3. The slime moves at regular intervals, creating a grid-based movement pattern.
4. When the slime eats a bat, it grows longer by adding a new segment to its tail.
5. The bat respawns at a strategic location after being eaten.
6. The player's score increases with each bat consumed.
7. If the slime collides with a wall or its own body, the game over panel appears.
8. On the game over panel, the player can choose to retry or return to the title scene.

With these mechanics implemented, Dungeon Slime is now a complete game with clear objectives, escalating difficulty, and a game feedback loop.

## Conclusion

In this chapter, we have transformed our technical demo into a complete game by integrating UI systems with game mechanics. We have accomplished several important goals:

- Created a dedicated [`GameSceneUI`](#the-gamesceneui-class) class to manage the game's user interface.
- Implemented pause and game over screens that provide clear feedback to the player.
- Refactored the `GameScene` class to coordinate all game components.
- Added game state management to handle different gameplay conditions.
- Enhanced player control through input buffering for more responsive gameplay
- Connected all of the elements to create a complete playable game.

The refactoring process we undertook demonstrates an important game development principle: separating concerns into specialized components makes code more maintainable and easier to extend. The `Slime` class manages snake-like behavior, the `Bat` class handles movement and collision response, and the `GameSceneUI` class encapsulates all UI-related functionality.

## Test Your Knowledge

1. How does the game handle different states (playing, paused, game over), and why is this state management important?

    :::question-answer
    The game uses an enum (`GameState`) to track its current state and implements different behavior based on that state:

    - During the `Playing` state, the game updates all objects and checks for collisions
    - During the `Paused` state, the game shows the pause menu and stops updating game objects
    - During the `GameOver` state, the game shows the game over menu and prevents further gameplay

    This state management is important because it:

    - Prevents inappropriate updates during non-gameplay states
    - Creates a clear flow between different game conditions
    - Simplifies conditional logic by using explicit states rather than multiple boolean flags
    - Makes the game's behavior more predictable and easier to debug

    :::

2. Why is it important to position the bat away from the slime after it's been eaten rather than at a completely random location?

    :::question-answer
    Positioning the bat away from the slime after it's been eaten rather than at a completely random location is important because:

    - It prevents unfair situations where the bat might spawn right on top of the slime causing an immediate collision
    - It creates a more strategic gameplay experience by forcing the player to navigate toward the bat
    - It ensures the player faces an appropriate level of challenge that increases as the slime grows longer
    - It prevents potential frustration from random spawns that might be either too easy or too difficult to reach
    - It creates a more balanced and predictable game experience while still maintaining variety
    :::

3. What problem does input buffering solve and how does our implementation address it?

    :::question-answer
    Input buffering solves the timing disconnect between when players press buttons and when the game can actually process those inputs in games with fixed movement cycles. Without buffering, inputs that occur between movement cycles are lost, especially when players make rapid sequential inputs like navigating corners.

    Our implementation addresses this by:

    - Using a queue data structure to store up to two directional inputs
    - Processing inputs in First-In-First-Out order to preserve the player's intended sequence
    - Validating each input against the previous one to prevent impossible movements
    :::
