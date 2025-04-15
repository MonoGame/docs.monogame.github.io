---
title: "Chapter 20: Completing the Game"
description: "Finalize game mechanics by updating our current demo into a snake-like inspired game."
---

In the previous chapters, we've built all the fundamental systems needed for our game: [graphics](../07_optimizing_texture_rendering/index.md), [input](../11_input_management/index.md), [collision detection](../12_collision_detection/index.md), [audio](../15_audio_controller/index.md), [scene management](../17_scenes/index.md), and a [user interface](../19_user_interface/index.html).   Now it's time to transform our game into a complete experience by implementing classic snake game mechanics; where the slime grows longer as it consumes bats, creating a challenging and engaging gameplay loop.

In this chapter you will:

- Implement a slime segment system to create a growing snake-like chain
- Create a movement system that updates on timed intervals
- Implement proper collision detection between the slime's head, body, and walls
- Add game over conditions when the slime collides with walls or itself
- Create a game controller for simplified input management
- Update the visual representation to smoothly interpolate between positions
- Implement retry functionality to restart the game after losing

> [!NOTE]
> This chapter will not focus much on MonoGame itself, but rather our implementation of the mechanics for the snake game.

Let's start by understanding the core mechanics of a snake-style game and how we will implement them in our Dungeon Slime project.

## Understanding Snake Game Mechanics

The classic snake game follows a simple but engaging set of rules:

1. The players controls a snake that moves in four cardinal directions (up, down, left, right).
2. The snake moves at regular intervals, creating a grid-based movement pattern.
3. When the snake eats food, it grows longer by adding a new segment to its tail.
4. If the snake collides with a wall or its own body, the game ends.

These mechanics create an increasingly challenging experience as the snake grows longer, requiring planning and reflexes to avoid collisions.

For our Dungeon Slime game, we will adapt these mechanics to fit our existing game structure:

- The slime will be a chain of connected segments.
- The player will control the direction of the head segment.
- The bat will serve as "food" that the slime consumes to grow.
- Collisions with walls or the slime's own body will trigger a game over state.

## The SlimeSegment Struct

To implement snake-like behavior, we first need a way to represent each segment of the slime chain.  We will create a dedicated struct to store the position and movement data for each segment.

Create a new file named *SlimeSegment.cs* in the root of the game project and add the following code:

[!code-csharp[](./snippets/slime_segment.cs)]

This struct contains the fields to track:

- `At`: The current position of the segment.
- `To`: The target position the segment is moving toward.
- `Direction`: The normalized vector representing the direction the segment is moving in.
- `ReverseDirection`: A computed property that returns the opposite of the segment's direction.

By tracking both the current and target positions, we can implement smooth visual movement between grid positions, creating more fluid animations than the traditional stutter movement seen in classic snake games.  We will explore how we do this later in this chapter.

## The GameController Class

Currently, we have two methods dedicated to handling input for our snake mechanics (`CheckKeyboardInput` and `CheckGamePadInput`).  Both of these methods essentially perform the same logic across different input devices.  This presents an opportunity to improve our code.

To simplify input handling for our snake mechanics, we will create a dedicated class that consolidates these input methods, providing a unified input profile for our game.  This pattern is widely used in game development to separate the "what" (game actions) from the "how" (specific input devices and buttons).

Create a new file named *GameController.cs* in the root of the game project and add the following code:

[!code-csharp[](./snippets/game_controller.cs)]

This controller provides methods for each game action (moving in four directions, pausing, and confirming actions), translating inputs from both the keyboard and game pad to create a consistent control scheme.

By implementing an abstraction layer such as this class, it offers benefits such as:

1. **Abstraction**: It separates game actions from physical inputs, making code more readable and maintainable.
2. **Consolidation**: All input handling is unified in one place, rather than spread across multiple methods.
3. **Extensibility**: Adding a new input device or changing key bindings only requires changes in one place.
4. **Clarity**: game code expresses intent clearly with methods like `MoveUp` instead of device specific checks like `keyboard.WasKeyJustPressed(Keys.Up)`.

## Implementing Snake Mechanics in the Game Scene

Now we will update our `GameScene` class to implement the snake-like behavior for our slime.  First, open the *GameScene.cs* file and add the following fields:

[!code-csharp[](./snippets/game_scene_fields.cs)]

These fields will:

- Track the list of segments that make up our slime chain.
- Store the next direction to apply to the head segment.
- Handle timing for the grid-based movement updates.
- Reference our game controller for input handling.

### Updating the Initialize Method

Next, we need to update the `Initialize` method.  Since the game will feature a "Retry" button on the game over menu, we need to separate the initialization concerns between initializing the state of the scene and initializing a new game.  First, update the `Initialize` method to the following:

[!code-csharp[](./snippets/game_scene_initialize.cs)]

The key changes here are that the controller is now initialized in this method, and we have removed all new game initialization logic, which we will add next in the new `InitializeNewGame` method.

### Adding the InitializeNewGameMethod

The `InitializeNewGame` method will provide the functionality of setting the initial state of a new game to be played.  Add the following method to the `GameScene` class:

[!code-csharp[](./snippets/game_scene_initialize_new_game.cs)]

This method:

- Creates a new list to store the slime segments.
- Places the initial head segment at the center of the tile map.
- Sets the direction for the head and stores it as the next direction.
- Sets up the bat's position and velocity.
- Resets the timer and score.

### Implementing the AssignRandomBatPosition Method

Previously, when we needed to set the position of the bat when it re-spawns, we simply chose  a random tile within the tile map to move it too.  By choosing a completely random location, it could spawn very close to the head segment of the slime, forcing an instant collision, or it could spawn very close to the head segment, which adds no challenge for the player.

To ensure the bat appears in a strategic location away from the head segment of the slime, we will create a method that positions the bat on the opposite side of the room.

Add the following method to the `GameScene` class:

[!code-csharp[](./snippets/game_scene_assign_random_bat_position.cs)]

This method:

1. Calculates the center of the room.
2. Determines which direction the slime's head segment is relative to the room center.
3. Places the bat on the opposite side of the room.
4. Ensures the bat is properly positioned within the room bounds.

### Implementing the CheckInput Method

We will replace our existing input handling methods with a new `CheckInput` method that uses the game controller.

First remove both the `CheckKeyboardInput` method and the `CheckGamePadInput` method.  Next, add the following method to the `GameScene` class:

[!code-csharp[](./snippets/game_scene_check_input.cs)]

This method:

1. Checks if the player has triggered the pause action, and if so pauses the game.
2. Determines if the player is attempting to change directions instead of directly moving the slime.  This direction change will be applied during the movement update.
3. Uses [**Vector2.Dot**](xref:Microsoft.Xna.Framework.Vector2.Dot(Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Vector2)) to prevent the slime from reversing into itself causing an immediate collision and game over state.
4. Updates the next direction if the requested direction is valid.

> [!NOTE]
> The dot product measure how much two vectors point in the same direction.  It is:
>
> - Positive if they are pointing in the same direction.
> - Negative if they are pointing in opposite directions.
> - Zero when they are perpendicular.
>
> By using dot product here, this effectively implements the classic snake game rule that you can't turn 180° and crash into yourself.

### Implementing Slime Movement

To handle the timed, grid-based movement of our slime chain, we will add an `UpdateSlimeMovement` method.  Add the following method to the `GameScene` class:

[!code-csharp[](./snippets/game_scene_update_slime_movement.cs)]

This method:

1. Creates a new head segment in the current direction.
2. Inserts the new head segment at the front of the chain and removes the last segment.
3. Checks for collisions with walls or the slime's own body.
4. Triggers a game over state if a collision is detected.

> [!NOTE]
> By inserting a new head segment at the front of the chain and removing the last segment this creates the illusion of the entire chain moving forward as one unit, even though we're only actually moving the head forward and removing the tail.

### Updating Bat Collision and Growth

To handle collision detection between the head segment of the slime and the bat and the growth response, we will add a new method called `CheckSlimeAndBatCollisions`.  Add the following method to the `GameScene` class:

[!code-csharp[](./snippets/game_scene_check_slime_and_bat_collision.cs)]

This method:

1. Checks for a collision between the slime's head and the bat.
2. When a collision occurs it:
   1. Adds a new segment to the end of the slime chain.
   2. Repositions the bat to a new random location and assigns it a new random velocity.
   3. Updates the player's score and plays the sound effect for auditory feedback.

### Implementing the MoveBat Method

Another key component is the autonomous movement of the bat. Unlike the slime, which moves based on player input in a grid-like pattern, the bat moves continuously around the screen, bouncing off walls when it reaches them.  Add the following method to the `GameScene` class:

[!code-csharp[](./snippets/game_scene_move_bat.cs)]

This method:

1. Calculates the potential new position for the bat based on its current velocity.
2. Creates a circular boundary for collision detection.
3. Checks if the bat would move outside the room bounds in either direction (wall collision).
4. If a collision is detected, calculates the normal vector of the collided wall.
5. Uses [**Vector2.Reflect**](xref:Microsoft.Xna.Framework.Vector2.Reflect(Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Vector2)) to bounce the bat's velocity off the wall naturally.
6. Plays a sound effect to provide audio feedback for the bounce.
7. Updates the bat's position.

The continuous movement of the bat contrasts with the grid-based movement of the slime, creating different gameplay dynamics for the player to consider.  This makes catching the bat challenging without requiring any complex behaviors.

### Updating the Game Loop

Now that we've separated the logic that occurs during the main game loop, we need to update the main game loop to incorporate these new mechanics.  Update the `UpdateGame` method in the `GameScene` class to the following:

[!code-csharp[](./snippets/game_scene_update_game.cs)]

This updated method:

1. Updates the sprite animations for the slime and the bat.
2. Processes the player input.
3. Handles collision detection and response between the head segment of the slime and the bat.
4. Manages the timed movement update for the slime chain.
5. Updates the bat's position.

### Updating the Draw Method

Finally, we need to update the draw method to draw the slime chain with smooth interpolation between positions.  Update the `Draw` method in the `GameScene` class to the following:

[!code-csharp[](./snippets/game_scene_draw.cs)]

This updated method:

1. Calculates an interpolation factor based on the current tick timer.
2. Renders each segment of the slime chain at a position smoothly interpolated between its source and target positions.  This creates fluid visual movement instead of the traditional stutter movement seen in classic snake games.

> [!NOTE]
> [**Vector2.Lerp**](xref:Microsoft.Xna.Framework.Vector2.Lerp(Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Vector2,System.Single)) performs a linear interpolation between two Vector2 points, creating a smooth transition from start to end based on an amount parameter.  The formula is:
>
> $\text{result} = \text{start} + (\text{end} - \text{start}) \cdot \text{amount}$
>
> Where the amount parameter ranges from $0.0$ (returns the start position) to $1.0$ (returns the end position).  Values between $0.0$ and $1.0$ give positions along the straight line between start and end.
>
> In our game, this is used to create a visual fluid movement illusion by interpolating between a segment's current position (`At`) and its target position (`To`) based on the elapsed time, rather than having the segments abruptly jump from one grid position to the next after each movement update.

## Implementing Game Over Retry Functionality

To allow players to restart after a game over, we need to update the game over menu's confirm action.  In the `CreateGameOverMenu` method for the `GameScene` class, make the following updates:

[!code-csharp[](./snippets/game_scene_create_game_over_menu.cs)]

This update now handles the confirm action for the game over menu's "Retry" button.  When chosen, it reinitializes the game so the player can try again.

## Putting It All Together

With all of these components in place, our Dungeon Slime game is now a complete snake-style game experience.  The player controls the slime that moves through the dungeon, consuming bats to grow longer.  If the slime collides with the wall or its own body, the game ends.

Let's see how it all looks and plays:

| ![Figure 20-1: Gameplay demonstration of the completed Dungeon Slime game showing the snake-like slime growing as it eats bats and a game over when colliding with the wall ](./videos/gameplay.webm) |
| :---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: |
|             **Figure 20-1: Gameplay demonstration of the completed Dungeon Slime game showing the snake-like slime growing as it eats bats and a game over when colliding with the wall**             |

1. The game starts with a single slime segment in the center of the screen.
2. The player controls the direction of the slime by using keyboard arrow or WASD keys, or by using a game pad DPad or left thumbstick.
3. The slime moves at regular intervals, creating a grid-based movement pattern.
4. When the slime eats a bat, it grows longer by adding a new segment to its tail.
5. The bat re-spawns at a strategic location after being eaten.
6. The player's score increases with each bat consumed.
7. If the slime collides with a wall or its own body; the game over menu appears.
8. On the game over menu, the player can choose to retry or return to the title scene.

With these mechanics in implemented, Dungeon Slime is now a complete game with clear objectives, escalating difficulty, and a game feedback loop.

## Conclusion

In this chapter, we've transformed our game from a simple collision demo into a complete snake-style game experience.  We've implemented a slime chain with multiple segments, timed movement updates, strategic bat positioning, collision detection and response, and smooth visual interpolation between positions.

The key techniques we've learned include:

- Creating a hierarchal data structure for the slime chain.
- Implementing grid-based movement with timed updates.
- Using interpolation for smooth visual movement.
- Creating strategic spawn logic for game objects.
- Implementing game over conditions with retry functionality.

These mechanics create a challenging and engaging game loop that offers increasing difficulty as the player progresses.  The slime chain grows longer with each bat consumed, making navigation more difficult and creating a natural difficulty curve.  Players can enjoy the classic snake gameplay with modern visual touches, all built on the MonoGame framework we've been developing throughout this tutorial series.

However the learning doesn't stop here.  In the next chapter, we will look at polishing up the game by implementing an input buffer and adding shader effects.

## Test Your Knowledge

1. How does the slime chain grow when the slime eats a bat?

   :::question-answer
   When the slime eats a bat, a new segment is added at the end of the chain.  This is done by getting the current tail segment, creating a new segment positioned behind it (using the `ReverseDirection` value), and adding it to the slime segments list.  This creates the growing snake effect where the chain gets longer with each bat consumed.
   :::

2. How does [**Vector2.Dot**](xref:Microsoft.Xna.Framework.Vector2.Dot(Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Vector2)) work and how is it used to validate the direction changes.

    :::question-answer
   [**Vector2.Dot**](xref:Microsoft.Xna.Framework.Vector2.Dot(Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Vector2)) calculates the dot product of two given vectors which results in a value that defines how much to vectors point in the same direction:

   - A positive value indicates they are pointing in the same direction.
   - A negative value indicates they are pointing in opposite directions.
   - A zero value indicates they are perpendicular.

    By calculating the dot product between the potential new direction and the current direction, we can determine if the player is trying to move in a direction that would cause the slime to collide with the neck segment.
    :::

3. How is a visual smooth movement achieved when drawing even though the movement of the slime chain happens at timed intervals?

  :::question-answer
  The smooth movement is achieved through linear interpolation with the [**Vector2.Lerp**](xref:Microsoft.Xna.Framework.Vector2.Lerp(Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Vector2,System.Single)) method.  An interpolation factor is calculated based on the current tick timer divided by the total tick time.  Then each slime segment is drawn at a position that is interpolated between where it is at and where it is moving to based on the calculated interpolation factor.
  :::
