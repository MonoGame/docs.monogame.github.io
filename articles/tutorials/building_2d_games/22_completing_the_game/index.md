---
title: "Chapter 22: Completing the Game"
description: "Finalize game mechanics by updating our current demo into a snake-like inspired game."
---

In the previous chapters, we've built all the fundamental systems needed for our game: [graphics](../07_optimizing_texture_rendering/index.md), [input](../11_input_management/index.md), [collision detection](../12_collision_detection/index.md), [audio](../15_audio_controller/index.md), [scene management](../17_scenes/index.md), and a [user interface](../19_user_interface/index.html).   Now it's time to transform our demo into a complete experience by implementing classic snake game mechanics. Before we do that, we first need to define what mechanics make a snake game.

In this chapter, you will:

- Understand the core mechanics that define a classic snake-like game.
- Learn how to implement grid-based movement with timed intervals.
- Create a segmented character that grows when collecting objects.
- Implement game state management for playing, paused, and game over conditions.
- Encapsulate related fields and properties into game components.
- Build a complete game loop with clear objectives and increasing difficulty.

> [!NOTE]
> This chapter will not focus much on MonoGame itself, but rather our implementation of the mechanics to transform our current game into a snake-like game.

## Understanding Snake Game Mechanics

In a classic snake-like game, the mechanics follow a set of simple but engaging rules:

1. The player controls a snake by telling it to move in one of four cardinal directions (up, down, left, and right).
2. The snake cannot reverse into itself, only moving forward or perpendicular to its current direction.
3. The actual movement of the snake occurs at regular timed intervals, creating a grid-based movement pattern.
4. When the snake eats food, it grows longer by adding a new segment to its tail.
5. If the snake collides with a wall or its own body, the game ends.

The mechanics create an increasingly challenging experience as the snake grows longer, requiring planning and reflexes to avoid collision.

### Directions

In snake, players input a cardinal direction (up, down, left, or right), to indicate which direction the snake will move during the next movement cycle.  When direction input occurs, it must be checked against the current direction to determine if the move is valid.

For example, if the snake is moving to the right, an invalid input would allow a player to move it to the left.  Doing so would cause the head of the snake to reverse direction and immediately collide with the first body segment. This means the only valid inputs are those where the next direction would be the same as the current direction or perpendicular to the current direction.

| ![Figure 22-1: An example snake with four segments, the head segment highlighted in orange, moving to the right.  Arrows show that the only valid movements for the head segment are up or down (perpendicular) or to continue to the right.](./images/snake_directions.svg) |
| :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: |
|                **Figure 22-1: An example snake with four segments, the head segment highlighted in orange, moving to the right.  Arrows show that the only valid movements for the head segment are up or down (perpendicular) or to continue to the right.**                |

### Movement Cycle

Instead of moving every update frame as a directional input is being pressed, the snake instead only moves during regular timed intervals.  A timer is used to determine how much time has passed since the last movement cycle, and when it reaches a set threshold, the next movement cycle occurs.  During this movement cycle, the snake should move forward in the direction that was input by the player between the last and current movement cycles.  This creates the grid-based movement system typically found in snake games.

There are various methods for handling the movement, such as iterating through each segment of the snake and updating the position of that segment to move forward.  Methods such as this though are wasteful, since visually the only parts of the snake that move on the screen are the head and the tail.  

Instead, a more common approach is to:

1. Make a copy of the head segment.

    | ![Figure 22-2: From a snake with four segments, a copy of the head segment is made, represented by the orange block](./images/snake_copy_head.svg) |
    | :------------------------------------------------------------------------------------------------------------------------------------------------: |
    |                **Figure 22-2 From a snake with four segments, a copy of the head segment is made, represented by the orange block**                |

2. Update the properties of the copy so that it is positioned where the original head segment would have moved to
3. Insert the copy at the front of the segment collection.

    | ![Figure 22-3: The copy of the head segment, represented by the orange block, is inserted at the front of the segment collection as the new head, which now makes it five segments (one too many)](./images/snake_insert_head.svg) |
    | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: |
    |                **Figure 22-3: The copy of the head segment, represented by the orange block, is inserted at the front of the segment collection as the new head, which now makes it five segments (one too many)**                 |

4. Remove the tail segment.

    | ![Figure 22-4: The tail segment of the snake is removed, bringing it back to the original four segments, giving the illusion that the entire snake moved forward ](./images/snake_remove_tail.svg) |
    | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: |
    |                 **Figure 22-4: The tail segment of the snake is removed, bringing it back to the original four segments, giving the illusion that the entire snake moved forward**                 |

By doing this, no matter how many segments the snake body has, we only ever need to update two of them, the head and the tail.

### Growth

The next core mechanic for a snake-like game is the growth of the snake.  Typically, somewhere in the game is an objective for the snake to eat, such as an apple.  When the head of the snake reaches this objective, a new segment is added to the snake's body, making it longer, the player's score is increased, and a new objective is spawned at a random location within the game.

This mechanic also acts as an artificial difficulty curve for the game.  As the body of the snake grows longer, it requires planning to avoid creating situations where the snake becomes trapped by its own body making it impossible to avoid a collision.

### Game Over

The challenge in a snake-like game is to avoid colliding with either a wall or another segment of the snake body.  Doing so will cause a game over condition as the snake can no longer continue moving forward.

## Implementing Snake-Like Mechanics In Our Game

Now that we have a foundational set of rules in place for the mechanics of a snake-like game, let's implement them into the current demo we've been building up. Our game will adapt these mechanics to fit them into our existing game structure:

- Instead of a snake, we will use the slime and create a chain of slime segments that follow the slime at the front.
- The player will control the direction of the slime segment
- The bat will serve as the objective to acquire to grow the slime segment.
- Collisions with either the walls of the room or a slime segment will result in a game over state.

As we implement these mechanics, we're also going to be creating classes that encapsulate the properties and functions of existing implementations in the game scene, such as the slime and the bat.  For example, currently, the game scene tracks fields for the `AnimatedSprite` and the position of the slime, as well as updating, input handling, and drawing the slime.  These can be moved into their dedicated classes encapsulating the functionality and also cleaning up the code in the game scene that has grown quite large.

### The GameController Class

Currently, we have two methods dedicated to handling input in the game scene, `CheckKeyboardInput` and `CheckGamePadInput`.  Both of these methods essentially perform the same logic across different input devices. This presents an opportunity to improve our code.

To simplify input handling for the game, we can create a dedicated class that consolidates the input methods, providing a unified input profile for the game.  This pattern is widely used in game development to separate the "what" (game actions) from the "how" (specific input devices and buttons).

In the *DungeonSlime* project (your main game project), create a new file named *GameController.cs* in the root directory and add the following code:

[!code-csharp[](./snippets/gamecontroller.cs)]

The `GameController` class implements an important design pattern in game development known as the "Input Abstraction" or "Command" pattern.  This pattern separates what happens in the game (the actions) from how players trigger those actions (the inputs).

This separation provides several benefits, including:

1. **Input Device Independence**: The game logic doesn't need to know which input device the player is using. Whether they're playing with a keyboard, gamepad, or touch screen, the game only cares that a "move up" action was triggered, not which specific button or key caused it.
2. **Simplified Input Handling**: Instead of checking multiple input combinations throughout the codebase, game objects can simply ask "Should I move up?" through a clean API call.
3. **Easy Rebinding**: If you want to add key rebinding features, you only need to modify the `GameController` class, not every piece of code that uses input.
4. **Consistent Input Logic**: The rules for determining if an action occurred (like checking if a button was just pressed version being held down) are defined in one place.
5. **Cross-Platform Compatibility**: When porting to different platforms with different input methods, you only need to update the `GameController` class to map the new input devices to your existing game actions.

By implementing this pattern in our game, we are not only making our current input handling cleaner, but we are also establishing a foundation that would make it easier to add features like input customization or support for new input devices in the future.

With our input handling system in place, now we can turn our attention to implementing the core mechanics of our snake-like game. First, we need to create a structure that will represent each segment of the slime's body.

### The SlimeSegment Struct

Next, we will need to implement a structure that can represent each segment of the slime.  This structure will store the position and movement data for each segment.

In the DungeonSlime project (your main game project), create a new directory named *GameObjects*. We will be putting all of our code related to the objects within the game here. Next, create a new file named *SlimeSegment.cs* inside the *GameObjects* directory you just created and add the following code:

[!code-csharp[](./snippets/slimesegment.cs)]

This structure contains fields to track:

- `At`: The current position of the segment.
- `To`: The position the segment will move to during the next movement cycle if it is the head segment.
- `Direction`: A normalized vector representing the direction the segment is moving in.
- `ReverseDirection`: A computed property that returns the opposite of the `Direction` property.

> [!NOTE]
> We are implementing this as a struct rather than a class because SlimeSegment is a small, simple data container with value semantics. Structs are more efficient for small data structures since they're allocated on the stack rather than the heap, reducing garbage collection overhead. Since our game will potentially create many segments as the snake grows, using a struct can provide better performance, especially when we will be copying segment data during movement operations.

> [!IMPORTANT]
> Structs work best with value types (like int, float, Vector2); using reference types in structs can cause boxing operations that negate the performance benefits. For more information on structs, refer to the [Structure Types - C# Reference](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/struct) documentation on Microsoft Learn.

By tracking both the current (`At`) and target (`To`) positions, we can implement smooth visual movement between grid positions, creating more fluid animations than the traditional stutter step movement seen in classic snake games.  We will explore this concept a little further in this chapter.

### The Slime Class

Next, let's implement a class to encapsulate the properties and functionality of our snake-like slime. In the *GameObjects* directory of the DungeonSlime project (your main game project), create a new file named *Slime.cs* and add the following initial code:

[!code-csharp[](./snippets/slime/definition.cs)]

This code sets up the basic structure for our `Slime` class. We've added the necessary using statements to access MonoGame's framework components and placed the class in the `DungeonSlime.GameObjects` namespace to keep our code organized. The empty class will serve as our foundation, and we'll build it up piece by piece in the following sections.

Now, we'll add several components to this class in sequence. Each section below should be added to the `Slime` class in the order presented. As we go through each part, the class will gradually take shape to handle all the snake-like behavior we need.

> [!NOTE]
> When adding these sections one by one, you may see compiler errors until all sections are in place. This is normal, as some parts of the code will reference fields or methods that haven't been added yet. Once all sections are complete, these errors will resolve.

#### Slime Fields

Add the following fields to the `Slime` class:

[!code-csharp[](./snippets/slime/fields.cs)]

Let's break down what each of these fields is responsible for:

- `s_movementTime`: This constant represents how long the slime waits between movement cycles (300ms). This creates the classic snake game's grid-based movement feel, where the snake moves at regular intervals rather than continuously.
- `_movementTime`: This field accumulates elapsed time until it reaches the movement threshold. When it does, the slime moves one grid cell and the timer resets.
- `_movementProgress`: This normalized value (0-1) represents progress between movement ticks and is used for visual interpolation. It allows us to smoothly animate the slime's movement between grid positions.
- `_nextDirection`: This stores the direction that will be applied to the head segment during the next movement cycle.
- `_stride`: This represents the total number of pixels the head segment should move during movement cycles.
- `_segments`: This collection holds all the `SlimeSegment` structures that make up the slime's body. The first segment is the head, and the rest form the trailing body.
- `_sprite`: This stores the `AnimatedSprite` that is used to draw each segment of the slime.

These fields implement core snake-like mechanics - the timed interval movement, direction control, and the segmented body that forms the snake.

#### Slime Events

Next, add the following events to the `Slime` class after the fields:

[!code-csharp[](./snippets/slime/events.cs)]

This event will allow the `Slime` class to notify the game scene when the head of the slime collides with another segment, triggering a game over.

#### Slime Constructor

After the events, add the following constructor to the `Slime` class:

[!code-csharp[](./snippets/slime/constructor.cs)]

This is a simple constructor that requires the slime to be given the `AnimatedSprite` that will be used to draw each of the slime segments.

#### Slime Initialization

Add the following `Initialization` method to the `Slime` class after the constructor:

[!code-csharp[](./snippets/slime/initialize.cs)]

With this method, we can initialize, or reset the state of slime.  It:

- Instantiates a new segment collection.
- Creates the initial head segment and positions it at the specific `startingPosition`.
- Sets the initial direction to be to the right.
- Initialize the movement timer to zero.

#### Slime Input Handling

Next, let's add the `HandleInput` method to process player input. Add the following method after the `Initialize` method:

[!code-csharp[](./snippets/slime/handleinput.cs)]

This method will:

1. Determine if the player is attempting to change directions instead of directly moving the slime.  This direction change will be applied later during the movement cycle update.
2. Uses [**Vector2.Dot**](xref:xref:Microsoft.Xna.Framework.Vector2.Dot(Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Vector2)) to prevent the slime from reversing into itself, causing an immediate collision and game over state.
3. Updates the `_nextDirection` value only if the direction input is valid.

> [!NOTE]
> The dot product measures how much two vectors point in the same direction. It is:
>
> - Positive if they are pointing in the same direction.
> - Negative if they are pointing in opposite directions.
> - Zero when they are perpendicular.
>
> By using dot product here, this effectively implements the classic snake-like game rule that you cannot turn 180° into yourself.

#### Slime Movement Cycle

To handle the snake-like movement cycle of the slime, we will create a method called `Move`.  Add the following method to the `Slime` class after the `HandleInput` method:

[!code-csharp[](./snippets/slime/move.cs)]

This method performs the core snake-like movement cycle logic by:

1. Copying the value of the current head segment.
2. Updating the copy's position (`At`) to where the head was moving to and updating the position it is moving to (`To`).
3. Insert the copy into the front of the segment collection and remove the tail.
4. Check if the head is now in the same position as any body segments, which would cause a collision and trigger a game over.

> [!NOTE]
> By inserting a new head segment at the front of the chain and removing the last segment, this creates the illusion of the entire chain moving forward as one, even though we're only actually moving the head forward and removing the tail.
>
> This follows the common snake movement pattern as discussed in the [Understanding Snake Game Mechanics: Movement Cycle](#movement-cycle) section above.

#### Slime Growth

To handle the snake-like growth of the slime, we'll create a method called `Grow`.  Add the following method to the `Slime` class after the `Move` method:

[!code-csharp[](./snippets/slime/grow.cs)]

Let's break down how this method works:

1. First it creates a copy of the current tail value.
2. It then adjusts the values of the copy so that it is now positioned behind the current tail by using the `ReverseDirection` value of the tail.
3. Finally, it inserts this new tail into the segments collection.

#### Slime Update

With most of the core snake-like mechanics now added to the `Slime` class within their own methods, let's add the `Update` method.  Add the following method to the `Slime` class after the `Grow` method:

[!code-csharp[](./snippets/slime/update.cs)]

This update method:

1. Updates the slime's `AnimatedSprite` to ensure the sprite animations occur.
2. Calls `HandleInput` to check for player input
3. Increments the movement timer by the amount of time that has elapsed between the game's update cycles.
4. Performs a check to see if the movement timer has accumulated more time than the threshold to perform a movement cycle update.  If it has then
   1. The movement timer is reduced by the threshold time.
   2. The `Move` method is called to perform a movement cycle update.
5. Finally, the movement progress amount is calculated by dividing the number of seconds accumulated for the movement timer by the number of seconds for the threshold.  This gives us a normalized value between 0.0 and 1.0 that we can use for visual interpolation for fluid movement.

> [!TIP]
> In games, frame rates can vary based on system performance, causing inconsistent update intervals.  If we simply reset the movement timer to zero after each movement cycle, we would lose any excess time that accumulated beyond the movement threshold.
>
> For example:
>
> - Our movement threshold is 300ms.
> - The game runs at 60fps (16.67ms per frame).
> - After 18 frames, we have accumulated 300.06ms.
> - If we reset to zero, we lose 0.06ms.
> - Over time, these small losses can add up and cause inconsistent movement.
>
> By subtracting the threshold instead of resetting to zero, we "bank" the excess time (0.06ms in this example) for the next movement cycle.  This ensures that:
>
> 1. Movement happens exactly at the intended frequency, maintaining consistent game speed.
> 2. The visual smoothness of movement remains intact even if the game occasionally drops frames.
> 3. Players experience the same game timing regardless of their hardware's performance.
>
> This technique is standard practice in game development, especially for timing-sensitive mechanics like rhythmic games, animations, and movement systems.  It is a simple solution that significantly improves gameplay consistency.

#### Slime Draw

We also need a method to handle drawing the slime and all of its segments.  Add the following `Draw` method after the `Update` method to the `Slime` class:

[!code-csharp[](./snippets/slime/draw.cs)]

This draw method iterates each segment of the slime and calculates the visual position to draw each segment at by performing linear interpolation (lerp) to determine the position of the segment between its current position (`At`) and the position it is moving to (`To`) based on the `_movementProgress` calculation.

> [!NOTE]
> [**Vector2.Lerp**](xref:Microsoft.Xna.Framework.Vector2.Lerp(Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Vector2,System.Single)) performs a linear interpolation between two vectors, creating a smooth transition from start to end based on an amount parameter.  The formula is:
>
> $\text{result} = \text{start} + (\text{end} - \text{start}) \cdot \text{amount}$
>
> Where the amount parameter ranges from 0.0 (returns the start position) to 1.0 (returns the end position).  Values between 0.0 and 1.0 give positions along the straight line between start and end.
>
> In our game, this is used to create a visual fluid movement illusion by interpolating between a segment's current position (`At`) and its target position (`To`) based on the elapsed time, rather than having the segments abruptly jump from one grid position to the next after each movement update.

#### Slime Bounds

For the game scene to detect collisions between the slime and other elements (walls or the bat), we need a method to calculate the current collision bounds. Add the following method to the `Slime` class after the `Draw` method:

[!code-csharp[](./snippets/slime/getbounds.cs)]

This method takes the current head segment (the first segment in our collection), calculates its visual position using linear interpolation, and then creates a `Circle` value to represent its collision boundary. Using the interpolated position ensures that collision detection aligns with what the player sees on screen.

> [!NOTE]
> We only need collision bounds for the slime's head for interactions with the bat and walls, as this matches the classic snake game mechanic where only the head's collisions matter for gameplay. For detecting collisions between the head and body segments, we use a simpler position-based check in the `Move` method since those positions are always aligned to the grid.

With all these methods in place, our Slime class now fully implements the snake-like mechanics we need. It handles movement on a grid, prevents invalid direction changes, detects self-collisions, and provides smooth visual movement between grid positions. This encapsulation allows us to manage all slime-related behavior in one place while exposing only the necessary interfaces to the game scene.

Now that we have our player-controlled character implemented, let's create the object that the slime will try to collect; the bat.

### The Bat Class

In the *GameObjects* directory of the DungeonSlime project (your main game project), create a new file named *Bat.cs* and add the following initial code:

[!code-csharp[](./snippets/bat/definition.cs)]

This code establishes the foundation for our `Bat` class. We have included the necessary using statements for MonoGame components, audio functionality, and our library references. The class is placed in the same `DungeonSlime.GameObjects` namespace as our Slime class to maintain a consistent organization.

Now we will build this class step by step, adding all the functionality needed for the bat to serve as the collectible object in our game. Add each of the following sections to the `Bat` class in the order they are presented.

> [!NOTE]
> As with the Slime class, you may encounter compiler errors until all sections are in place. These errors will be resolved once all components of the class have been added.

#### Bat Fields

Add the following fields to the `Bat` class:

[!code-csharp[](./snippets/bat/fields.cs)]

Let's break down what each of these fields is responsible for:

- `MOVEMENT_SPEED`: This constant represents the factor to multiply the velocity vector by to determine how fast the bat is moving.
- `_velocity`: A vector that defines the direction and how much in that direction to update the position of the bat each update cycle.
- `_sprite`: This stores the `AnimatedSprite` that is used to draw the bat.
- `_bounceSoundEffect`: This store the [**SoundEffect**](xref:Microsoft.Xna.Framework.Audio.SoundEffect) to play when the bat is told to bounce.

#### Bat Properties

Next, add the following property to the `Bat` class after the fields:

[!code-csharp[](./snippets/bat/properties.cs)]

This property exposes the position of the bat so it can be used for calculations in the game scene when determining where to place the bat after the slime eats it.

#### Bat Constructor

After the property, dd the following constructor to the `Bat` class:

[!code-csharp[](./snippets/bat/constructor.cs)]

This is a simple constructor that requires the bat to be given the `AnimatedSprite` that will be used to draw the bat and the [**SoundEffect**](xref:Microsoft.Xna.Framework.Audio.SoundEffect) to be played when the bat bounces off a wall.

#### Bat Randomize Velocity

Currently, we have the `AssignRandomVelocity` method in the `GameScene` that we call to randomize the velocity of the bat after it has been eaten by the slime.  Let's take this method out of the `GameScene` class and put it into the `Bat` class itself.  Add the following method to the `Bat` class after the constructor:

[!code-csharp[](./snippets/bat/randomizevelocity.cs)]

#### Bat Bounce

We are also going to take the logic from the `GameScene` class that bounces the bat off the walls and move it into a dedicated method in the `Bat` class.  Add the following method to the `Bat` class after the `RandomizeVelocity` method:

[!code-csharp[](./snippets/bat/bounce.cs)]

This method only takes a single parameter, the [normal vector](../12_collision_detection/index.md#bounce-collision-response) of the surface the bat is bouncing against.  Based on the X and Y components of the normal vector, we can determine which wall the bat bounced against and adjust the position of the bat so that it doesn't stick to the wall.

#### Bat Bounds

Similar to the [`Slime` class](#slime-bounds), for the game scene to detect collision between the bat and other elements, we need a method to calculate the current collision bounds of the bat.  Add the following method to the `Bat` class after the `Bounce` method:

[!code-csharp[](./snippets/bat/getbounds.cs)]

#### Bat Update

The `Bat` class will also need to be updated.  Add the following `Update` method to the `Bat` class after the `GetBounds` method:

[!code-csharp[](./snippets/bat/update.cs)]

This method simply updates the bat's `AnimatedSprite` to ensure animations occur and adjusts the position of the bat based on the current velocity.

> [!NOTE]
> The continuous movement of the bat contrasts with the grid-based interval movement of the slime, creating different gameplay dynamics for the player to consider.  This makes catching the bat challenging without requiring any complex behaviors.

#### Bat Draw

Finally, we need a method to draw the bat.  Add the following `Draw` method to the `Bat` class after the `Update` method:

[!code-csharp[](./snippets/bat/draw.cs)]

This method simply draws the bat's `AnimatedSprite` at the bat's current position.

With the `Bat` class complete, we've now encapsulated all the behavior needed for the collectible element in our game. The bat moves continuously around the screen and can bounce off walls, adding a twist on the classic snake-like mechanic by creating a target for the player to chase.

### The GameSceneUI Class

Currently, the `GameScene` class contains the methods for initializing and creating the pause menu.  However, now that we have a defined condition for game-over, we need to create a game-over menu as well.  To do this, we could create the game over menu directly inside the `GameScene` class the same way we did with the pause menu, or we can take this as another opportunity to refactor the code and pull the UI-specific code into its own class.

In the *UI* directory of the game project, create a new file named *GameSceneUI* and add the following initial code:

[!code-csharp[](./snippets/gamesceneui/definition.cs)]

With our `Slime` and `Bat` classes now complete, we've successfully encapsulated the core gameplay objects. Now we need to address another important aspect of our game: the user interface elements that communicate with the player.

Currently, the `GameScene` class contains the methods for initializing and creating the pause menu. However, now that we have a defined condition for game over, we need to create a game over menu as well. To do this, we could create the game over menu directly inside the `GameScene` class the same way we did with the pause menu, or we can take this as another opportunity to refactor the code and pull the UI-specific code into its own class.

In the *UI* directory of the DungeonSlime project (your main game project), create a new file named *GameSceneUI.cs* and add the following initial code:

[!code-csharp[](./snippets/gamesceneui/definition.cs)]

This code establishes the foundation for our `GameSceneUI` class, which inherits from Gum's `ContainerRuntime` class. This inheritance means our UI class is itself a UI container that can hold and manage other UI elements. We've included all necessary using statements for MonoGame, Gum UI components, and our library references.

Let's build out this class by adding each section in sequence. Follow the order below to create the complete UI management system for our game scene.

> [!NOTE]
> You may see compiler errors as you add these sections one by one. This is expected because some parts of the code will reference fields, properties, or methods that we haven't added yet. Once all sections are in place, these errors will resolve.

#### GameSceneUI Fields

Add the following fields to the `GameSceneUI` class:

[!code-csharp[](./snippets/gamesceneui/fields.cs)]

Let's break down what each of these fields is responsible for:

- `s_scoreFormat`: A string format template used to display the player's score with leading zeros.
- `_uiSoundEffect`: Stores the sound effect played for UI interactions like button clicks and focus changes.
- `_overlay`: A semi-transparent overlay that darkens the game screen when a menu is displayed.
- `_pausePanel`: The panel containing the UI elements shown when the game is paused.
- `_resumeButton`: A reference to the resume button, allowing us to set focus on it when the pause panel is shown.
- `_gameOverPanel`: The panel containing the UI elements shown when a game over occurs.
- `_retryButton`: A reference to the retry button, allowing us to set focus to it when the game over panel is shown.
- `_scoreText`: The text display showing the player's current score.

#### GameSceneUI Events

After the fields, add the following events to the `GameSceneUI` class:

[!code-csharp[](./snippets/gamesceneui/events.cs)]

These events allow the `GameSceneUI` class to notify the `GameScene` when important UI actions occur:

- `ResumeButtonClick`: Triggered when the player clicks the Resume button on the pause panel.
- `QuitButtonClick`: Triggered when the player clicks the Quit button on either panel.
- `RetryButtonClick`: Triggered when the player clicks the Retry button on the game over panel.

#### GameSceneUI Constructor

Add the following constructor to the `GameSceneUI` class after the events:

[!code-csharp[](./snippets/gamesceneui/constructor.cs)]

This constructor initializes all UI components:

1. Set the container to fill the entire screen.
2. Adds itself to Gum's root element.
3. Loads necessary assets (sound effect and texture atlas).
4. Creates and adds child elements in the correct order.

#### GameSceneUI UI Creation Methods

Next add the methods to create the various UI elements that are managed by the `GameSceneUI` class.  Add the following method after the constructor:

[!code-csharp[](./snippets/gamesceneui/createscoretext.cs)]

This method takes creates a `TextRuntime` element that we can use to display the player's score and returns it back.  After this method, add the following method:

[!code-csharp[](./snippets/gamesceneui/createoverlay.cs)]

This method creates a `ColoredRectangleRuntime` element that is semi-transparent and returns it back.  This will be used to dim the game when the pause and game over panels are shown.  After this method, add the following method:

[!code-csharp[](./snippets/gamesceneui/createpausepanel.cs)]

This method builds the `Panel` that is shown hen the game is paused, including the "Resume" and "Quit" buttons, then returns it back. Finally, after this method, add the following method:

[!code-csharp[](./snippets/gamesceneui/creategameoverpanel.cs)]

This method builds the `Panel` that is shown when a game over occurs, including the "Retry" and "Quit" buttons.

Both the pause panel and the game over panel use event handlers for their buttons.  Let's add those next.

#### GameSceneUI Event Handlers

After the `CreateGameOverPanel` method, add the following method to the `GameSceneUI` class:

[!code-csharp[](./snippets/gamesceneui/eventhandlers.cs)]

These event handlers provide audio feedback and appropriate UI updates when buttons are clicked or UI elements receive focus.

#### GameSceneUI Public Methods

Finally, add the following public methods to the `GameSceneUI` class after the `OnElementGotFocus` method:

[!code-csharp[](./snippets/gamesceneui/publicmethods.cs)]

These public methods provide the interface for the `GameScene` to:

- Update the score display.
- Show or hide the pause menu.
- Show or hide the game over menu.
- Update and draw the UI components.

With the `GameSceneUI` class complete, we now have a fully encapsulated UI system that can handle displaying game information (score), providing feedback for game states (pause, game over), and processing user interactions (button clicks). This separation of UI logic from game logic will make our codebase much easier to maintain and extend.

Now that we have all our specialized components ready, let's refactor the GameScene class to coordinate between them and manage the overall game flow.

### The GameScene Class

Now that we have created the encapsulated [`Slime`](#the-slime-class), [`Bat`](#the-bat-class), and [`GameSceneUI`](#the-gamesceneui-class) classes, we can refactor the `GameScene` class to leverage these new components.  This will make our code more maintainable and allow us to focus on the game logic within the scene itself.  We will rebuild the `GameScene` class to coordinate the interactions between the components.

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

Now we'll add the various methods needed to complete the `GameScene` class. Add each section in the sequence presented below. This will build up the scene's functionality step by step.

> [!NOTE]
> As with previous classes, you might encounter compiler errors until all sections are in place. These errors will be resolved once all components of the class have been added.

#### GameScene Initialize Method

To set up the scene, add the following `Initialize` method after the fields in te `GameScene` class:

[!code-csharp[](./snippets/gamescene/initialize.cs)]

This method sets up the initial state of the game scene:

1. Disables the "exit on escape" behavior so we can use the escape key for pausing.
2. Calculate the playable area within the tilemap walls.
3. Subscribes to the slime's body collision event to detect when the player collides with itself triggering a game over state.
4. Initialize the UI components.
5. Set up a new game.

#### GameScene InitializeUI Method

The `Initialize` method we just added calls a method to initialize the user interface for the scene.  Let's add that method now.  Add the following method after the `Initialize` method in the `GameScene` class:

[!code-csharp[](./snippets/gamescene/initializeui.cs)]

This method creates the UI components and subscribes to its events to respond to button clicks.

#### GameScene UI Event Handlers

In the `InitializeUI` method we just added, we subscribe to the events from the `GameSceneUI` class that are triggered when buttons are clicked.  Now we need to add those methods that would be called when the events are triggered.  Add the following methods to the `GameScene` class after the `InitializeUI` method:

[!code-csharp[](./snippets/gamescene/eventhandlers.cs)]

These methods respond to the UI events:

- `OnResumeButtonClicked`: Resumes the game from a paused state.
- `OnRetryButtonClicked`: Restarts the game after a game over.
- `OnQuitButtonClicked`: Quits the game by returning to the title scene.

#### GameScene InitializeNewGame Method

In the `Initialize` method we added above, it also makes a call to an `InitializeNewGame` method.  Let's add this now.  Add the following method to the `GameScene` class after the `OnQuitButtonClicked` method:

[!code-csharp[](./snippets/gamescene/initializenewgame.cs)]

This method will:

1. Position the slime in the center of the map.
2. Initialize the slime with its starting position and movement stride.
3. Randomize the bat's velocity and position it away from the slime.
4. Reset the player's score.
5. Set the game state to "Playing".

#### GameScene LoadContent Method

Next, we need to add the method to load game assets for the scene.  Add the following method to the `GameScene` class after the `InitializeNewGame` method:

[!code-csharp[](./snippets/gamescene/loadcontent.cs)]

This method loads all necessary assets for the game scene:

1. The texture atlas containing the sprite graphics
2. The tilemap that defines the level layout.
3. The animated sprites for the slime and bat.
4. Sound effects for the bat bouncing and collecting.

#### GameScene Update Method

Next, to update the scene add the following method to the `GameScene` class after the `LoadContent` method:

[!code-csharp[](./snippets/gamescene/update.cs)]

This method updates the scene in each frame to:

1. Always update the UI, regardless of game state.
2. Return early if the game is over.
3. Check for pause input and toggle the pause state if needed.
4. Return early if the game is paused.
5. Update the slime and bat.
6. Check for collisions between the game objects.

#### GameScene CollisionChecks Method

In the `Update` method we just added, it makes a call to a `CollisionChecks` method to handle the collision detection and response.  Let's add that method now.  Add the following method to the `GameScene` class after the `Update` method:

[!code-csharp[](./snippets/gamescene/collisionchecks.cs)]

This method checks for three types of collisions:

1. Slime-Bat collision: The slime "eats" the bat, gains points, grows, and the bat respawns.
2. Slime-Wall collision: Triggers a game over if the slime hits a wall.
3. Bat-Wall collision: Causes the bat to bounce off the walls.

#### GameScene PositionBatAwayFromSlime Method

The `CollisionCheck` method makes a call to `PositionBatAwayFromSlime`.  Previously, when we needed to set the position of the bat when it respawns, we simply chose a random tile within the tilemap to move it to.  By choosing a completely random location, it could be on top fo the head segment of the slime, forcing an instant collision, or it could spawn very close to the head segment, which adds not challenge for the player.

To ensure the bat appears in a random, but strategic location, we can instead set it to position away from the slime on the opposite side of the room.  Add the following method to the `GameScene` class after the `CollisionCheck` method:

[!code-csharp[](./snippets/gamescene/positionbatawayfromslime.cs)]

This method positions the bat after it's been eaten:

1. Determines which wall (top, bottom, left, or right) is furthest from the slime.
2. Places the bat near that wall, making it more challenging for the player to reach.

#### GameScene Event Handler and Game State Methods

Next, we will add some of the missing methods being called from above that handle game events and state changes.  Add the following methods to the `GameScene` class after the `PositionBatAwayFromSlime` method:

[!code-csharp[](./snippets/gamescene/statechanges.cs)]

These methods handle specific game events:

- `OnSlimeBodyCollision`: Called when the slime collides with itself, triggering a game over.
- `TogglePause`: Switches between paused and playing states.
- `GameOver`: Called when a game over condition is met, showing the game over UI.

#### GameScene Draw Method

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

## Putting It All Together

With all of these components now in place, our Dungeon Slime game has transformed from a simple demo built on learning MonoGame concepts into a complete snake-style game experience.  The player controls the slime that moves through the dungeon, consuming bats to grow longer.  If the slime collides with the wall or its own body, the game ends.

Let's see how it all looks and plays:

| ![Figure 22-5: Gameplay demonstration of the completed Dungeon Slime game showing the snake-like slime growing as it eats bats and a game over when colliding with the wall ](./videos/gameplay.webm) |
| :---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: |
|             **Figure 22-5: Gameplay demonstration of the completed Dungeon Slime game showing the snake-like slime growing as it eats bats and a game over when colliding with the wall**             |

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

In this chapter, we've transformed our technical demo into a complete game by implementing classic snake-like mechanics. We've learned how to:

- Create a grid-based movement system with timed intervals.
- Build a segmented character that grows when consuming objectives.
- Implement collision detection for walls and self-collision.
- Manage game states for playing, pausing, and game over conditions.
- Encapsulate functionality into reusable, maintainable components.

By applying these mechanics, our Dungeon Slime game now offers players a clear objective (grow the slime by eating bats), an increasing challenge (avoiding collision with walls and the growing body), and a complete game loop with scoring and restart options.

The refactoring process we undertook demonstrates an important game development principle: separating concerns into specialized components makes code more maintainable and easier to extend. The `Slime` class manages snake-like behavior, the `Bat` class handles movement and collision response, and the `GameSceneUI` class encapsulates all UI-related functionality.

## Test Your Knowledge

1. Why must a snake-like game prevent the player from reversing direction?

    :::question-answer
    Preventing reverse movement is necessary because it would cause an immediate collision between the snake's head and the first body segment.
    :::

2. How does the movement cycle for a snake work, and why is it more efficient than updating each segment individually?

    :::question-answer
    The snake movement cycle works by:

    1. Creating a copy of the head segment.
    2. Positioning the copy one grid cell ahead in the current direction
    3. Inserting this copy at the front of the segment collection
    4. Removing the last segment.

    This approach is more efficient because it only requires manipulating two segments (adding a new head and removing the tail) regardless of how long the snake becomes, rather than iterating through and updating every segment individually.
    :::

3. What are the benefits of encapsulating game objects like the `Slime` and `Bat` into their classes?

    :::question-answer
    Encapsulating game objects into dedicated classes provides several benefits:

    - Clearer separation of concerns where each class manages its functionality.
    - Better code organization making it easier to find and modify specific behaviors.
    - Reduced complexity in the main game scene, which now coordinates between components.
    - Improved maintainability as changes to one component are less likely to affect others.
    - Enhanced reusability of code for future projects or different game modes.

    :::

4. How does the implementation use [**Vector2.Lerp**](xref:Microsoft.Xna.Framework.Vector2.Lerp(Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Vector2,System.Single)) to create smooth visual movement, and why is this important?

     :::question-answer
     The implementation uses [**Vector2.Lerp**](xref:Microsoft.Xna.Framework.Vector2.Lerp(Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Vector2,System.Single)) to interpolate between a segment's current position (`At`) and its target position (`To`) based on a normalized movement progress value. This creates smooth visual movement by rendering the segments at intermediate positions between grid points rather than abruptly jumping from one grid position to the next. This is important because it provides more fluid animation while maintaining the logical grid-based movement, enhancing the visual quality of the game without changing the core mechanics.
     :::
