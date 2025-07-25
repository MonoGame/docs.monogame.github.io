---
title: "Chapter 11: Input Management"
description: "Learn how to create an input management system to handle keyboard, mouse, and gamepad input, including state tracking between frames and creating a reusable framework for handling player input."
---

In [Chapter 10](../10_handling_input/index.md), you learned how to handle input from various devices like keyboard, mouse, and gamepad. While checking if an input is currently down works well for continuous actions like movement, many game actions should only happen once when an input is first pressed; think firing a weapon or jumping. To handle these scenarios, we need to compare the current input state with the previous frame's state to detect when an input changes from up to down.

In this chapter you will:

- Learn the difference between an input being down versus being pressed
- Track input states between frames
- Create a reusable input management system
- Simplify handling input across multiple devices

We will start by understanding the concept of input state changes and how we can detect them.

## Understanding Input States

When handling input in games, there are two key scenarios we need to consider:

- An input is being held down (like holding a movement key).
- An input was just pressed for one frame (like pressing a jump button).

Now, we will look at the difference using keyboard input as an example. With our current implementation, we can check if a key is down using [**KeyboardState.IsKeyDown**](xref:Microsoft.Xna.Framework.Input.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys)):

[!code-csharp[](./snippets/key_down_every_frame.cs)]

However, many game actions should not repeat while a key is held. For instance, if the Space key makes your character jump, you probably do not want them to jump repeatedly just because the player is holding the key down. Instead, you want the jump to happen only on the first frame when Space is pressed.

To detect this "just pressed" state, we need to compare two states:

1. Is the key down in the current frame?
2. Was the key up in the previous frame?

If both conditions are true, we know the key was just pressed.  If we were to modify the above code to track the previous keyboard state it would look something like this:

[!code-csharp[](./snippets/compare_previous_state.cs)]

If you need to know the inverse state, when the key was just released, then it is simply a matter of switching the checking of the states, for example, is the key up this frame and was it down in the previous frame.

This same concept applies to mouse buttons and gamepad input as well. Any time you need to detect a "just pressed" or "just released" state, you will need to compare the current input state with the previous frame's state.

So far, we have only been working with our game within the `Game1.cs` file.  This has been fine for the examples given.  Over time, as the game grows, we are going to have a more complex system set up with different scenes, and each scene will need a way to track the state of input over time.  We could do this by creating a lot of variables in each scene to track this information, or we can use object-oriented design concepts to create a reusable `InputManager` class to simplify this for us.

Before we create the `InputManager` class, we should first create classes for the keyboard, mouse, and gamepad that encapsulates the information about those inputs, which will then be exposed through the `InputManager`.  

To get started, create a new folder called `Input` in the *MonoGameLibrary* project.  We will put all of our input related classes here.

## The KeyboardInfo Class

We will start our input management system by creating a class to handle keyboard input. The `KeyboardInfo` class will encapsulate all keyboard-related functionality, making it easier to:

- Track current and previous keyboard states
- Detect when keys are pressed or released
- Check if keys are being held down

In the `Input` folder of the *MonoGameLibrary* project, add a new file named `KeyboardInfo.cs` with this initial structure:

[!code-csharp[](./snippets/keyboardinfo.cs#declaration)]

### KeyboardInfo Properties

To detect changes in keyboard input between frames, we need to track both the previous and current keyboard states. Add these properties to the `KeyboardInfo` class:

[!code-csharp[](./snippets/keyboardinfo.cs#members)]

> [!NOTE]
> These properties use a public getter but private setter pattern. This allows other parts of the game to read the keyboard states if needed, while ensuring only the `KeyboardInfo` class can update them.

### KeyboardInfo Constructor

The `KeyboardInfo` class constructor needs to initialize the keyboard states.

Add this constructor:

[!code-csharp[](./snippets/keyboardinfo.cs#ctors)]

The constructor:

- Creates an empty state for `PreviousState` since there is no previous input yet
- Gets the current keyboard state as our starting point for `CurrentState`

This initialization ensures we have valid states to compare against in the first frame of our game, preventing any potential null reference issues when checking for input changes.

### KeyboardInfo Methods

The `KeyboardInfo` class needs methods both for updating states and checking key states. First, we will start with our update method:

[!code-csharp[](./snippets/keyboardinfo.cs#methods_update)]

> [!NOTE]
> Each time `Update` is called, the current state becomes the previous state, and we get a fresh current state. This creates our frame-to-frame comparison chain.

Next, we will add methods to check various key states:

[!code-csharp[](./snippets/keyboardinfo.cs#methods_keystate)]

These methods serve two distinct purposes.  For checking continuous states:

- `IsKeyDown`: Returns true as long as the specified key is being held down.
- `IsKeyUp`: Returns true as long as the specified key is not being pressed.

And for detecting state changes:

- `WasKeyJustPressed`: Returns true only on the frame when the specified key changes from up-to-down.
- `WasKeyJustReleased`: Returns true only on the frame when the specified key changes from down-to-up.

> [!TIP]
> Use continuous state checks (`IsKeyDown`/`IsKeyUp`) for actions that should repeat while a key is held, like movement. Use single-frame checks (`WasKeyJustPressed`/`WasKeyJustReleased`) for actions that should happen once per key press, like jumping or shooting.

That's it for the `KeyboardInfo` class, now we can move on to mouse input next.

## MouseButton Enum

Recall from the [Mouse Input](../10_handling_input/index.md#mouse-input) section of the previous chapter that the [**MouseState**](xref:Microsoft.Xna.Framework.Input.MouseState) struct provides button states through properties rather than methods like `IsButtonDown`/`IsButtonUp`. To keep our input management API consistent across devices, we will create a `MouseButton` enum that lets us reference mouse buttons in a similar way to how we use [**Keys**](xref:Microsoft.Xna.Framework.Input.Keys) for keyboard input and [**Buttons**](xref:Microsoft.Xna.Framework.Input.Buttons) for gamepad input.

In the `Input` folder of the *MonoGameLibrary* project, add a new file named `MouseButton.cs` with the following code:

[!code-csharp[](./snippets/mousebutton.cs)]

> [!NOTE]
> Each enum value corresponds directly to a button property in MouseState:
>
> - `Left`: Maps to [**MouseState.LeftButton**](xref:Microsoft.Xna.Framework.Input.MouseState.LeftButton).
> - `Middle`: Maps to [**MouseState.MiddleButton**](xref:Microsoft.Xna.Framework.Input.MouseState.MiddleButton).
> - `Right`: Maps to [**MouseState.RightButton**](xref:Microsoft.Xna.Framework.Input.MouseState.RightButton).
> - `XButton1`: Maps to [**MouseState.XButton1**](xref:Microsoft.Xna.Framework.Input.MouseState.XButton1).
> - `XButton2`: Maps to [**MouseState.XButton2**](xref:Microsoft.Xna.Framework.Input.MouseState.XButton2).

## The MouseInfo Class

To manage mouse input effectively, we need to track both current and previous states, as well as provide easy access to mouse position, scroll wheel values, and button states. The `MouseInfo` class will encapsulate all of this functionality, making it easier to:

- Track current and previous mouse states.
- Track the mouse position.
- Check the change in mouse position between frames and if it was moved.
- Track scroll wheel changes.
- Detect when mouse buttons are pressed or released
- Check if mouse buttons are being held down
  
To get started, in the `Input` folder of the *MonoGameLibrary* project, create a new file named `MouseInfo.cs` with the following initial structure:

[!code-csharp[](./snippets/mouseinfo.cs#declaration)]

### MouseInfo Properties

The `MouseInfo` class needs properties to track both mouse states and provide easy access to common mouse information. Add the following properties to the `MouseInfo` class:

First, we need properties for tracking mouse states:

[!code-csharp[](./snippets/mouseinfo.cs#properties_states)]

Next, we will add properties for handling cursor position:

[!code-csharp[](./snippets/mouseinfo.cs#properties_position)]

> [!NOTE]
> The position properties use a `SetPosition` method that we will implement later. This method will handle the actual cursor positioning on screen.

These properties provide different ways to work with the cursor position:

- `Position`: Gets/sets the cursor position as a [**Point**](xref:Microsoft.Xna.Framework.Point).
- `X`: Gets/sets just the horizontal position.
- `Y`: Gets/sets just the vertical position.

Next, we will add properties for determining if the mouse cursor moved between game frames and if so how much:

[!code-csharp[](./snippets/mouseinfo.cs#properties_position_delta)]

The properties provide different ways of detecting mouse movement between frames:

- `PositionDelta`: Gets how much the cursor moved between frames as a [**Point**](xref:Microsoft.Xna.Framework.Point).
- `XDelta`: Gets how much the cursor moved horizontally between frames.
- `YDelta`: Gets how much the cursor moved vertically between frames.
- `WasMoved`: Indicates if the cursor moved between frames.

Finally, we will add properties for handling the scroll wheel:

[!code-csharp[](./snippets/mouseinfo.cs#properties_scrollwheel)]

The scroll wheel properties serve different purposes:

- `ScrollWheel`: Gets the total accumulated scroll value since game start.
- `ScrollWheelDelta`: Gets the change in scroll value just in this frame.

> [!TIP]
> Use `ScrollWheelDelta` when you need to respond to how much the user just scrolled, rather than tracking the total scroll amount.

### MouseInfo Constructor

The `MouseInfo` class constructor needs to initialize the mouse states.

Add this constructor:

[!code-csharp[](./snippets/mouseinfo.cs#ctors)]

The constructor:

- Creates an empty state for `PreviousState` since there is no previous input yet.
- Gets the current mouse state as our starting point for `CurrentState`.

This initialization ensures we have valid states to compare against in the first frame of our game, preventing any potential null reference issues when checking for input changes.

### MouseInfo Methods

The `MouseInfo` class needs methods for updating states, checking button states, and setting the cursor position. Add the following method to the `MouseInfo` class:

[!code-csharp[](./snippets/mouseinfo.cs#methods_update)]

Next, we will add methods to check various button states:

[!code-csharp[](./snippets/mouseinfo.cs#methods_buttonstate)]

These methods serve two distinct purposes. For checking continuous states:

- `IsButtonDown`: Returns true as long as the specified button is being held down.
- `IsButtonUp`: Returns true as long as the specified button is not being pressed.

And for detecting state changes:

- `WasButtonJustPressed`: Returns true only on the frame when the specified button changes from up-to-down.
- `WasButtonJustReleased`: Returns true only on the frame when the specified button changes from down-to-up.

> [!NOTE]
> Each method uses a switch statement to check the appropriate button property from the [**MouseState**](xref:Microsoft.Xna.Framework.Input.MouseState) based on which `MouseButton` enum value is provided. This provides a consistent API while handling the different button properties internally.

Finally, we need a method to handle setting the cursor position:

[!code-csharp[](./snippets/mouseinfo.cs#methods_setposition)]

> [!TIP]
> Notice that after setting the position, we immediately update the `CurrentState`. This ensures our state tracking remains accurate even when manually moving the cursor.

That's it for the `MouseInfo` class, next we will move onto gamepad input.

## The GamePadInfo Class

To manage gamepad input effectively, we need to track both current and previous states, is the gamepad still connected, as well as provide easy access to the thumbstick values, trigger values, and button states. The `GamePadInfo` class will encapsulate all of this functionality, making it easier to:

- Track current and previous gamepad states.
- Check if the gamepad is still connected.
- Track the position of the left and right thumbsticks.
- Check the values of the left and right triggers.
- Detect when gamepad buttons are pressed or released.
- Check if gamepad buttons are being held down.
- Start and Stop vibration of a gamepad.

To get started, in the `Input` folder of the *MonoGameLibrary* project, create a new file named `GamePadInfo.cs` with the following initial structure:

[!code-csharp[](./snippets/gamepadinfo.cs#declaration)]

### GamePadInfo Properties

We use vibration in gamepads to provide haptic feedback to the player.  The [**GamePad**](xref:Microsoft.Xna.Framework.Input.GamePad) class provides the [**SetVibration**](xref:Microsoft.Xna.Framework.Input.GamePad.SetVibration(Microsoft.Xna.Framework.PlayerIndex,System.Single,System.Single)) method to tell the gamepad to vibrate, but it does not provide a timing mechanism for it if we wanted to only vibrate for a certain period of time.  Add the following private field to the `GamePadInfo` class:

[!code-csharp[](./snippets/gamepadinfo.cs#member_fields)]

If you recall from the [previous chapter](../10_handling_input/index.md#gamepad-input), a [**PlayerIndex**](xref:Microsoft.Xna.Framework.PlayerIndex) value needs to be supplied when calling [**Gamepad.GetState**](xref:Microsoft.Xna.Framework.Input.GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex)).   Doing this returns the state of the gamepad connected at that player index.  So we will need a property to track the player index this gamepad info is for.

[!code-csharp[](./snippets/gamepadinfo.cs#properties_playerindex)]

To detect changes in the gamepad input between frames, we need to track both the previous and current gamepad states.  Add these properties to the `GamePadInfo` class:

[!code-csharp[](./snippets/gamepadinfo.cs#properties_state)]

There are times that a gamepad can disconnect for various reasons; being unplugged, bluetooth disconnection, or battery dying are just some examples.  To track if the gamepad is connected, add the following property:

[!code-csharp[](./snippets/gamepadinfo.cs#properties_connected)]

The values of the thumbsticks and triggers can be accessed through the `CurrentState`.  However, instead of having to navigate through multiple property chains to get this information, add the following properties to get direct access to the values:

[!code-csharp[](./snippets/gamepadinfo.cs#properties_thumbsticks_triggers)]

### GamePadInfo Constructor

The `GamePadInfo` class constructor needs to initialize the gamepad states.

Add this constructor:

[!code-csharp[](./snippets/gamepadinfo.cs#ctors)]

This constructor

- Requires a [**PlayerIndex**](xref:Microsoft.Xna.Framework.PlayerIndex) value which is stored and will be used to get the states for the correct gamepad
- Creates an empty state for `PreviousState` since there is no previous state yet.
- Gets the current gamepad state as our starting `CurrentState`.

This initialization ensures we have valid states to compare against in the first frame of our game, preventing any potential null reference issues when checking for input changes.

### GamePadInfo Methods

The `GamePadInfo` class needs methods for updating states, checking button states, and controlling vibration. Add the following method to the `GamePadInfo` class:

[!code-csharp[](./snippets/gamepadinfo.cs#methods_update)]

> [!NOTE]
> Unlike keyboard and mouse input, the gamepad update method takes a [**GameTime**](xref:Microsoft.Xna.Framework.GameTime) parameter. This allows us to track and manage timed vibration effects.

Next, we will add methods to check various button states:

[!code-csharp[](./snippets/gamepadinfo.cs#methods_buttonstate)]

These methods serve two distinct purposes. For checking continuous states:

- `IsButtonDown`: Returns true as long as a button is being held down.
- `IsButtonUp`: Returns true as long as a button is not being pressed.

And for detecting state changes:

- `WasButtonJustPressed`: Returns true only on the frame when a button changes from up-to-down.
- `WasButtonJustReleased`: Returns true only on the frame when a button changes from down-to-up.

Finally, we will add methods for controlling gamepad vibration:

[!code-csharp[](./snippets/gamepadinfo.cs#methods_vibration)]

The vibration methods provide control over the gamepad's haptic feedback:

- `SetVibration`: Starts vibration at the specified strength for a set duration.
- `StopVibration`: Immediately stops all vibration.

> [!TIP]
> When setting vibration, you can specify both the strength (`0.0f` to `1.0f`) and duration. The vibration will automatically stop after the specified time has elapsed, so you do not need to manage stopping it manually.

That's it for the `GamePadInfo` class.  Next, we can create the actual input manager.

## The InputManager Class

Now that we have classes to handle keyboard, mouse, and gamepad input individually, we can create a centralized manager class to coordinate all input handling.

In the `Input` folder of the *MonoGameLibrary* project, add a new file named `InputManager.cs` with this initial structure:

[!code-csharp[](./snippets/inputmanager.cs#declaration)]

### InputManager Properties

The `InputManager` class needs properties to access each type of input device. Add these properties:

[!code-csharp[](./snippets/inputmanager.cs#properties)]

> [!NOTE]
> The `GamePads` property is an array because MonoGame supports up to four gamepads simultaneously. Each gamepad is associated with a PlayerIndex (0-3).

### InputManager Constructor

The constructor for the `InputManager` initializes the keyboard, mouse, and gamepad states.

Add the following constructor:

[!code-csharp[](./snippets/inputmanager.cs#ctors)]

### InputManager Methods

The `Update` method for the `InputManager` calls update for each device, so that they can update their internal states.

[!code-csharp[](./snippets/inputmanager.cs#methods)]

## Implementing the InputManager Class

Now that we have our input management system complete, we will update our game to use it.  We will do this in two steps:

1. First, update the `Core` class to add the `InputManager` globally.
2. Update the `Game1` class to use the global input manager from `Core`.

### Updating the Core Class

The `Core` class serves as our base game class, so we will update it to add and expose the `InputManager` globally.  Open the *Core.cs* file in the *MonoGameLibrary* project and update it to the following:

[!code-csharp[](./snippets/core.cs?highlight=5-6,39-47,91-92,106–107,110–121)]

The key changes to the `Core` class are:

1. Added the `using MonoGameLibrary.Input;` directive to access the `InputManager` class.
2. Added a static `Input` property to provide global access to the input manager.
3. Added a static `ExitOnEscape` property to set whether the game should exit when the Escape key on the keyboard is pressed.
4. In the `Core` constructor, `ExitOnEscape` is set to true by default to mirror how the default MonoGame `Game1` class template has this functionality by default.
5. In `Initialize` the input manager is created.
6. Added an override for the `Update` method where:
   1. The input manager is updated
   2. A check is made to see if `ExitOnEscape` is true and if the Escape keyboard key is pressed.

### Updating the Game1 Class

Now we can update our `Game1` class to use the new input management system through the `Core` class.  Open `Game1.cs` in the game project and update it to the following:

[!code-csharp[](./snippets/game1.cs?highlight=1,7,73,79,85,91,97,105,110,113,117,123,125-126,131,137,143,149)]

The key changes to the `Game1` class are:

1. In [**Update**](xref:Microsoft.Xna.Framework.Game.Update(Microsoft.Xna.Framework.GameTime)), the check for the gamepad back button or keyboard escape key being pressed was removed.  This is now handled by the `ExitOnEscape` property and the `Update` method of the `Core` class.
2. In `CheckKeyboardInput` and `CheckGamepadInput`, instead of getting the keyboard and gamepad states and then using the states, calls to check those devices are now done through the input.

Running the game now, you will be able to control it the same as before, only now we are using our new `InputManager` class instead.

| ![Figure 11-1: The slime moving around based on device input](./videos/input-moving-slime.webm) |
|:-----------------------------------------------------------------------------------------------:|
|                 **Figure 11-1: The slime moving around based on device input**                  |

## Conclusion

In this chapter, you accomplished the following:

- Detect the difference between continuous and single-frame input states.
- Create classes to manage different input devices.
- Build a centralized `InputManager` to coordinate all input handling that is:
  - Reusable across different game projects
  - Easy to maintain and extend
  - Consistent across different input devices
- Integrate the input system into the `Core` class for global access.
- Update the game to use the new input management system.

## Test Your Knowledge

1. What is the difference between checking if an input is "down" versus checking if it was "just pressed"?

    :::question-answer
    "Down" checks if an input is currently being held, returning true every frame while held. "Just pressed" only returns true on the first frame when the input changes from up to down, requiring comparison between current and previous states.
    :::

2. Why do we track both current and previous input states?

    :::question-answer
    Tracking both states allows us to detect when input changes occur by comparing the current frame's state with the previous frame's state. This is essential for implementing "just pressed" and "just released" checks.
    :::

3. What advantage does the `InputManager` provide over handling input directly?

    :::question-answer
    The `InputManager` centralizes all input handling, automatically tracks states between frames, and provides a consistent API across different input devices. This makes the code more organized, reusable, and easier to maintain.
    :::
