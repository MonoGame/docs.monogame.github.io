---
title: "Chapter 10: Input Management"
description: "Learn how to create an input management system to handle keyboard, mouse, and gamepad input, including state tracking between frames and creating a reusable framework for handling player input."
---

In [Chapter 09](../09_handling_input/index.md), you learned how to handle input from various devices like keyboard, mouse, and gamepad. While checking if an input is currently down works well for continuous actions like movement, many game actions should only happen once when an input is first pressed; think firing a weapon or jumping. To handle these scenarios, we need to compare the current input state with the previous frame's state to detect when an input changes from up to down.

In this chapter you will:

- Learn the difference between an input being down versus being pressed
- Track input states between frames
- Create a reusable input management system
- Simplify handling input across multiple devices

Let's start by understanding the concept of input state changes and how we can detect them.

## Understanding Input States

When handling input in games, there are two key scenarios we need to consider:

- An input is being held down (like holding a movement key).
- An input was just pressed for one frame (like pressing a jump button).

Let's look at the difference using keyboard input as an example. With our current implementation, we can check if a key is down using [**KeyboardState.IsKeyDown**](xref:Microsoft.Xna.Framework.Input.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys)):

```cs
KeyboardState keyboardState = Keyboard.GetState();

if (keyboardState.IsKeyDown(Keys.Space))
{
    // This runs EVERY frame the space key is held down
}
```

However, many game actions shouldn't repeat while a key is held. For instance, if the Space key makes your character jump, you probably don't want them to jump repeatedly just because the player is holding the key down. Instead, you want the jump to happen only on the first frame when Space is pressed.

To detect this "just pressed" state, we need to compare two states:

1. Is the key down in the current frame?
2. Was the key up in the previous frame?

If both conditions are true, we know the key was just pressed.  If we were to modify the above code to track the previous keyboard state it would look something like this:

```cs
private KeyboardState _previousKeyboardState;

protected override void Update(GameTime gameTime)
{
    KeyboardState keyboardState = Keyboard.GetState();

    if (keyboardState.IsKeyDown(Keys.Space) && _previousKeyboardState.IsKeyUp(Keys.Space))
    {
        // This will only run on the first frame Space is pressed and will not
        // happen again until it has been released and then pressed again.
    }

    _previousKeyboardState = keyboardState;
    base.Update(gameTime);
}
```

This same concept applies to mouse buttons and gamepad input as well. Any time you need to detect a "just pressed" or "just released" state, you'll need to compare the current input state with the previous frame's state.

So far, we've only been working with our game within the *Game1.cs* file.  This has been fine for the examples given.  Overtime, as the game grows, we're going to have a more complex system setup with different scenes, and each scene will need a way to track the state of input over time.  We could do this by creating a lot of variables in each scene to track this information, or we can use object-oriented design concepts to create a reusable `InputManager` class to simplify this for us. 

Before we create the `InputManager` class, let's first create classes for the keyboard, mouse, and gamepad that encapsulates the information about those inputs which will then be exposed through the `InputManager`.  

To get started, create a new directory called *Input* in the *MonoGameLibrary* project.  We'll put all of our input related classes here.

## The KeyboardInfo Class

Let's start our input management system by creating a class to handle keyboard input. The `KeyboardInfo` class will encapsulate all keyboard-related functionality, making it easier to:

- Track current and previous keyboard states
- Detect when keys are pressed or released
- Check if keys are being held down

In the *Input* directory of the *MonoGameLibrary* project, add a new file named *KeyboardInfo.cs* with this initial structure:

```cs
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Input;

public class KeyboardInfo
{

}
```

### KeyboardInfo Properties

To detect changes in keyboard input between frames, we need to track both the previous and current keyboard states. Add these properties to the `KeyboardInfo` class:

```cs
/// <summary>
/// Gets the state of keyboard input during the previous update cycle.
/// </summary>
public KeyboardState PreviousState { get; private set; }

/// <summary>
/// Gets the state of keyboard input during the current input cycle.
/// </summary>
public KeyboardState CurrentState { get; private set; }
```

> [!NOTE]
> These properties use a public getter but private setter pattern. This allows other parts of the game to read the keyboard states if needed, while ensuring only the `KeyboardInfo` class can update them.

### KeyboardInfo Constructor

The `KeyboardInfo` class needs a constructor to initialize the keyboard states. Add this constructor:

```cs
/// <summary>
/// Creates a new KeyboardInfo 
/// </summary>
public KeyboardInfo()
{
    PreviousState = new KeyboardState();
    CurrentState = Keyboard.GetState();
}
```

The constructor:

- Creates an empty state for `PreviousState` since there is no previous input yet
- Gets the current keyboard state as our starting point for `CurrentState`

This initialization ensures we have valid states to compare against in the first frame of our game, preventing any potential null reference issues when checking for input changes.

### KeyboardInfo Methods

The `KeyboardInfo` class needs methods both for updating states and checking key states. Let's start with our update method:

```cs
/// <summary>
/// Updates the state information about keyboard input.
/// </summary>
public void Update()
{
    PreviousState = CurrentState;
    CurrentState = Keyboard.GetState();
}
```

> [!NOTE]
> Each time `Update` is called, the current state becomes the previous state, and we get a fresh current state. This creates our frame-to-frame comparison chain.

Next, we'll add methods to check various key states:

```cs
/// <summary>
/// Returns a value that indicates if the specified key is currently down.
/// </summary>
/// <param name="key">The key to check.</param>
/// <returns>true if the specified key is currently down; otherwise, false.</returns>
public bool IsKeyDown(Keys key)
{
    return CurrentState.IsKeyDown(key);
}

/// <summary>
/// Returns a value that indicates whether the specified key is currently up.
/// </summary>
/// <param name="key">The key to check.</param>
/// <returns>true if the specified key is currently up; otherwise, false.</returns>
public bool IsKeyUp(Keys key)
{
    return CurrentState.IsKeyUp(key);
}

/// <summary>
/// Returns a value that indicates if the specified key was just pressed on the current frame.
/// </summary>
/// <param name="key">The key to check.</param>
/// <returns>true if the specified key was just pressed on the current frame; otherwise, false.</returns>
public bool WasKeyJustPressed(Keys key)
{
    return CurrentState.IsKeyDown(key) && PreviousState.IsKeyUp(key);
}

/// <summary>
/// Returns a value that indicates if the specified key was just released on the current frame.
/// </summary>
/// <param name="key">The key to check.</param>
/// <returns>true if the specified key was just released on the current frame; otherwise, false.</returns>
public bool WasKeyJustReleased(Keys key)
{
    return CurrentState.IsKeyUp(key) && PreviousState.IsKeyDown(key);
}
```

These methods serve two distinct purposes.  For checking continuous states:

- `IsKeyDown`: Returns true as long as a key is being held down.
- `IsKeyUp`: Returns true as long as a key is not being pressed.

And for detecting state changes:

- `WasKeyJustPressed`: Returns true only on the frame when a key changes from up-to-down.
- `WasKeyJustReleased`: Returns true only on the frame when a key changes from down-to-up.

> [!TIP]
> Use continuous state checks (`IsKeyDown`/`IsKeyUp`) for actions that should repeat while a key is held, like movement. Use single-frame checks (`WasKeyJustPressed`/`WasKeyJustReleased`) for actions that should happen once per key press, like jumping or shooting.

That's it for the `KeyboardInfo` class, let's move on to mouse input next.

## MouseButton Enum

Recall from the [Mouse Input](../09_handling_input/index.md#mouse-input) section of the previous chapter that the [**MouseState**](xref:Microsoft.Xna.Framework.Input.MouseState) struct provides button states through properties rather than methods like `IsButtonDown`/`IsButtonUp`. To keep our input management API consistent across devices, we'll create a `MouseButton` enum that lets us reference mouse buttons in a similar way to how we use [**Keys**](xref:Microsoft.Xna.Framework.Input.Keys) for keyboard input and [**Buttons**](xref:Microsoft.Xna.Framework.Input.Buttons) for gamepad input.

In the *Input* directory of the *MonoGameLibrary* project, add a new file named *MouseButton.cs* with the following code:

```cs
namespace MonoGameLibrary.Input;

public enum MouseButton
{
    Left,
    Middle,
    Right,
    XButton1,
    XButton2
}
```

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
  
Let's create this class in the *Input* directory of the *MonoGameLibrary* project. Add a new file named *MouseInfo.cs* with the following initial structure:

```cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Input;

public class MouseInfo
{

}
```

### MouseInfo Properties

The `MouseInfo` class needs properties to track both mouse states and provide easy access to common mouse information. Let's add these properties.

First, we need properties for tracking mouse states:

```cs
/// <summary>
/// The state of mouse input during the previous update cycle.
/// </summary>
public MouseState PreviousState { get; private set; }

/// <summary>
/// The state of mouse input during the current update cycle.
/// </summary>
public MouseState CurrentState { get; private set; }
```

Next, we'll add properties for handling cursor position:

```cs
/// <summary>
/// Gets or Sets the current position of the mouse cursor in screen space.
/// </summary>
public Point Position
{
    get => CurrentState.Position;
    set => SetPosition(value.X, value.Y);
}

/// <summary>
/// Gets or Sets the current x-coordinate position of the mouse cursor in screen space.
/// </summary>
public int X
{
    get => CurrentState.X;
    set => SetPosition(value, CurrentState.Y);
}

/// <summary>
/// Gets or Sets the current y-coordinate position of the mouse cursor in screen space.
/// </summary>
public int Y
{
    get => CurrentState.Y;
    set => SetPosition(CurrentState.X, value);
}

/// <summary>
/// Gets a value that indicates if the mouse cursor moved between the previous and current frames.
/// </summary>
public bool WasMoved => CurrentState.X != PreviousState.X || CurrentState.Y != PreviousState.Y;
```

> [!NOTE]
> The position properties use a `SetPosition` method that we'll implement later. This method will handle the actual cursor positioning on screen.

These properties provide different ways to work with the cursor position:

- `Position`: Gets/sets the cursor position as a [**Point**](xref:Microsoft.Xna.Framework.Point).
- `X`: Gets/sets just the horizontal position.
- `Y`: Gets/sets just the vertical position.

Next, we'll add properties for determining if the mouse cursor moved between game frames and if so how much:

```cs
/// <summary>
/// Gets the difference in the mouse cursor position between the previous and current frame.
/// </summary>
public Point PositionDelta => CurrentState.Position - PreviousState.Position;

/// <summary>
/// Gets the difference in the mouse cursor x-position between the previous and current frame.
/// </summary>
public int XDelta => CurrentState.X - PreviousState.X;

/// <summary>
/// Gets the difference in the mouse cursor y-position between the previous and current frame.
/// </summary>
public int YDelta => CurrentState.Y - PreviousState.Y;

/// <summary>
/// Gets a value that indicates if the mouse cursor moved between the previous and current frames.
/// </summary>
public bool WasMoved => PositionDelta != Point.Zero;
```

The properties provide different ways of detecting mouse movement between frames:

- `PositionDelta`: Gets how much the cursor moved between frames as a [**Point**](xref:Microsoft.Xna.Framework.Point).
- `XDelta`: Gets how much the cursor moved horizontally between frames.
- `YDelta`: Gets how much the cursor moved vertically between frames.
- `WasMoved`: Indicates if the cursor moved between frames.

Finally, we'll add properties for handling the scroll wheel:

```cs
/// <summary>
/// Gets the cumulative value of the mouse scroll wheel since the start of the game.
/// </summary>
public int ScrollWheel => CurrentState.ScrollWheelValue;

/// <summary>
/// Gets the value of the scroll wheel between the previous and current frame.
/// </summary>
public int ScrollWheelDelta => CurrentState.ScrollWheelValue - PreviousState.ScrollWheelValue;
```

The scroll wheel properties serve different purposes:

- `ScrollWheel`: Gets the total accumulated scroll value since game start.
- `ScrollWheelDelta`: Gets the change in scroll value just in this frame.

> [!TIP]
> Use `ScrollWheelDelta` when you need to respond to how much the user just scrolled, rather than tracking the total scroll amount.

### MouseInfo Constructor

The `KeyboardInfo` class needs a constructor to initialize the mouse states. Add this constructor:

```cs
/// <summary>
/// Creates a new MouseInfo.
/// </summary>
public MouseInfo()
{
    PreviousState = new MouseState();
    CurrentState = Mouse.GetState();
}
```

The constructor:

- Creates an empty state for `PreviousState` since there is no previous input yet.
- Gets the current mouse state as our starting point for `CurrentState`.

This initialization ensures we have valid states to compare against in the first frame of our game, preventing any potential null reference issues when checking for input changes.

### MouseInfo Methods

The `MouseInfo` class needs methods for updating states, checking button states, and setting the cursor position. Let's start with our update method:

```cs
/// <summary>
/// Updates the state information about mouse input.
/// </summary>
public void Update()
{
    PreviousState = CurrentState;
    CurrentState = Mouse.GetState();
}
```

Next, we'll add methods to check various button states:

```cs
/// <summary>
/// Returns a value that indicates whether the specified mouse button is currently down.
/// </summary>
/// <param name="button">The mouse button to check.</param>
/// <returns>true if the specified mouse button is currently down; otherwise, false.</returns>
public bool IsButtonDown(MouseButton button)
{
    switch (button)
    {
        case MouseButton.Left:
            return CurrentState.LeftButton == ButtonState.Pressed;
        case MouseButton.Middle:
            return CurrentState.MiddleButton == ButtonState.Pressed;
        case MouseButton.Right:
            return CurrentState.RightButton == ButtonState.Pressed;
        case MouseButton.XButton1:
            return CurrentState.XButton1 == ButtonState.Pressed;
        case MouseButton.XButton2:
            return CurrentState.XButton2 == ButtonState.Pressed;
        default:
            return false;
    }
}

/// <summary>
/// Returns a value that indicates whether the specified mouse button is current up.
/// </summary>
/// <param name="button">The mouse button to check.</param>
/// <returns>true if the specified mouse button is currently up; otherwise, false.</returns>
public bool IsButtonUp(MouseButton button)
{
    switch (button)
    {
        case MouseButton.Left:
            return CurrentState.LeftButton == ButtonState.Released;
        case MouseButton.Middle:
            return CurrentState.MiddleButton == ButtonState.Released;
        case MouseButton.Right:
            return CurrentState.RightButton == ButtonState.Released;
        case MouseButton.XButton1:
            return CurrentState.XButton1 == ButtonState.Released;
        case MouseButton.XButton2:
            return CurrentState.XButton2 == ButtonState.Released;
        default:
            return false;
    }
}

/// <summary>
/// Returns a value that indicates whether the specified mouse button was just pressed on the current frame.
/// </summary>
/// <param name="button">The mouse button to check.</param>
/// <returns>true if the specified mouse button was just pressed on the current frame; otherwise, false.</returns>
public bool WasButtonJustPressed(MouseButton button)
{
    switch (button)
    {
        case MouseButton.Left:
            return CurrentState.LeftButton == ButtonState.Pressed && PreviousState.LeftButton == ButtonState.Released;
        case MouseButton.Middle:
            return CurrentState.MiddleButton == ButtonState.Pressed && PreviousState.MiddleButton == ButtonState.Released;
        case MouseButton.Right:
            return CurrentState.RightButton == ButtonState.Pressed && PreviousState.RightButton == ButtonState.Released;
        case MouseButton.XButton1:
            return CurrentState.XButton1 == ButtonState.Pressed && PreviousState.XButton1 == ButtonState.Released;
        case MouseButton.XButton2:
            return CurrentState.XButton2 == ButtonState.Pressed && PreviousState.XButton2 == ButtonState.Released;
        default:
            return false;
    }
}

/// <summary>
/// Returns a value that indicates whether the specified mouse button was just released on the current frame.
/// </summary>
/// <param name="button">The mouse button to check.</param>
/// <returns>true if the specified mouse button was just released on the current frame; otherwise, false.</returns>F
public bool WasButtonJustReleased(MouseButton button)
{
    switch (button)
    {
        case MouseButton.Left:
            return CurrentState.LeftButton == ButtonState.Released && PreviousState.LeftButton == ButtonState.Pressed;
        case MouseButton.Middle:
            return CurrentState.MiddleButton == ButtonState.Released && PreviousState.MiddleButton == ButtonState.Pressed;
        case MouseButton.Right:
            return CurrentState.RightButton == ButtonState.Released && PreviousState.RightButton == ButtonState.Pressed;
        case MouseButton.XButton1:
            return CurrentState.XButton1 == ButtonState.Released && PreviousState.XButton1 == ButtonState.Pressed;
        case MouseButton.XButton2:
            return CurrentState.XButton2 == ButtonState.Released && PreviousState.XButton2 == ButtonState.Pressed;
        default:
            return false;
    }
}
```

These methods serve two distinct purposes. For checking continuous states:


- `IsKeyDown`: Returns true as long as a key is being held down.
- `IsKeyUp`: Returns true as long as a key is not being pressed.

And for detecting state changes:

- `WasKeyJustPressed`: Returns true only on the frame when a key changes from up-to-down.
- `WasKeyJustReleased`: Returns true only on the frame when a key changes from down-to-up.

> [!NOTE]
> Each method uses a switch statement to check the appropriate button property from the [**MouseState**](xref:Microsoft.Xna.Framework.Input.MouseState) based on which `MouseButton` enum value is provided. This provides a consistent API while handling the different button properties internally.

Finally, we need a method to handle setting the cursor position:

```cs
/// <summary>
/// Sets the current position of the mouse cursor in screen space and updates the CurrentState with the new position.
/// </summary>
/// <param name="x">The x-coordinate location of the mouse cursor in screen space.</param>
/// <param name="y">The y-coordinate location of the mouse cursor in screen space.</param>
public void SetPosition(int x, int y)
{
    Mouse.SetPosition(x, y);
    CurrentState = new MouseState(
        x,
        y,
        CurrentState.ScrollWheelValue,
        CurrentState.LeftButton,
        CurrentState.MiddleButton,
        CurrentState.RightButton,
        CurrentState.XButton1,
        CurrentState.XButton2
    );
}
```

> [!TIP]
> Notice that after setting the position, we immediately update the `CurrentState`. This ensures our state tracking remains accurate even when manually moving the cursor.

That's it for the `MouseInfo` class, next we'll move onto gamepad input.

## The GamePadInfo Class

To manage gamepad input effectively, we need to track both current and previous states, is the gamepad still connected, as well as provide easy access to the thumbstick values, trigger values, and button states. The `GamePadInfo` class will encapsulate all of this functionality, making it easier to:

- Track current and previous gamepad states.
- Check if the gamepad is still connected.
- Track the position of the left and right thumbsticks.
- Check the values of the left and right triggers.
- Detect when gamepad buttons are pressed or released.
- Check if gamepad buttons are being held down.
- Start and Stop vibration of a gamepad.

Let's create this class in the *Input* directory of the *MonoGameLibrary* project. Add a new file named *GamePadInfo.cs* with the following initial structure:

```cs
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Input;

public class GamePadInfo
{

}
```

### GamePadInfo Properties

We use vibration in gamepads to provide haptic feedback to the player.  The [**GamePad**](xref:Microsoft.Xna.Framework.Input.GamePad) class provides the [**SetVibration**](xref:Microsoft.Xna.Framework.Input.GamePad.SetVibration(Microsoft.Xna.Framework.PlayerIndex,System.Single,System.Single)) method to tell the gamepad to vibrate, but it does not provide a timing mechanism for it if we wanted to only vibrate for a certain period of time.  Add the following private field to the `GamePadInfo` class:

```cs
private TimeSpan _vibrationTimeRemaining = TimeSpan.Zero;
```

Recall from the [previous chapter](../09_handling_input/index.md#gamepad-input) that a [**PlayerIndex**](xref:Microsoft.Xna.Framework.PlayerIndex) value needs to be supplied when calling [**Gamepad.GetState**](xref:Microsoft.Xna.Framework.Input.GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex)).   Doing this returns the state of the gamepad connected at that player index.  So we'll need a property to track the player index this gamepad info is for.

```cs
/// <summary>
/// Gets the index of the player this gamepad is for.
/// </summary>
public PlayerIndex PlayerIndex { get; }
```

To detect changes in the gamepad input between frames, we need to track both the previous and current gamepad states.  Add these properties to the `GamePadInfo` class:

```cs
/// <summary>
/// Gets the state of input for this gamepad during the previous update cycle.
/// </summary>
public GamePadState PreviousState { get; private set; }

/// <summary>
/// Gets the state of input for this gamepad during the current update cycle.
/// </summary>
public GamePadState CurrentState { get; private set; }
```

There are times that a gamepad can disconnect for various reasons; being unplugged, bluetooth disconnection, or battery dying are just some examples.  To track if the gamepad is connected, add the following property:

```cs
/// <summary>
/// Gets a value that indicates if this gamepad is currently connected.
/// </summary>
public bool IsConnected => CurrentState.IsConnected;
```

The values of the thumbsticks and triggers can be accessed through the `CurrentState`.  However, instead of having to navigate through multiple property chains to get this information, add the following properties to get direct access to the values:

```cs
/// <summary>
/// Gets the value of the left thumbstick of this gamepad.
/// </summary>
public Vector2 LeftThumbStick => CurrentState.ThumbSticks.Left;

/// <summary>
/// Gets the value of the right thumbstick of this gamepad.
/// </summary>
public Vector2 RightThumbStick => CurrentState.ThumbSticks.Right;

/// <summary>
/// Gets the value of the left trigger of this gamepad.
/// </summary>
public float LeftTrigger => CurrentState.Triggers.Left;

/// <summary>
/// Gets the value of the right trigger of this gamepad.
/// </summary>
public float RightTrigger => CurrentState.Triggers.Right;
```

### GamePadInfo Constructor

The `GamePadInfo` class needs a constructor to initialize the gamepad states.  Add this constructor

```cs
/// <summary>
/// Creates a new GamePadInfo for the gamepad connected at the specified player index.
/// </summary>
/// <param name="playerIndex">The index of the player for this gamepad.</param>
public GamePadInfo(PlayerIndex playerIndex)
{
    PlayerIndex = playerIndex;
    PreviousState = new GamePadState();
    CurrentState = GamePad.GetState(playerIndex);
}
```

This constructor

- Requires a [**PlayerIndex**](xref:Microsoft.Xna.Framework.PlayerIndex) value which is stored and will be used to get the states for the correct gamepad
- Creates an empty state for `PreviousState` since there is no previous state yet.
- Gets the current gamepad state as our starting `CurrentState`.

This initialization ensures we have valid states to compare against in the first frame of our game, preventing any potential null reference issues when checking for input changes.

### GamePadInfo Methods

The `GamePadInfo` class needs methods for updating states, checking button states, and controlling vibration. Let's start with our update method:

```cs
/// <summary>
/// Updates the state information for this gamepad input.
/// </summary>
/// <param name="gameTime"></param>
public void Update(GameTime gameTime)
{
    PreviousState = CurrentState;
    CurrentState = GamePad.GetState(PlayerIndex);

    if (_vibrationTimeRemaining > TimeSpan.Zero)
    {
        _vibrationTimeRemaining -= gameTime.ElapsedGameTime;

        if (_vibrationTimeRemaining <= TimeSpan.Zero)
        {
            StopVibration();
        }
    }
}
```

> [!NOTE]
> Unlike keyboard and mouse input, the gamepad update method takes a [**GameTime**](xref:Microsoft.Xna.Framework.GameTime) parameter. This allows us to track and manage timed vibration effects.

Next, we'll add methods to check various button states:

```cs
/// <summary>
/// Returns a value that indicates whether the specified gamepad button is current down.
/// </summary>
/// <param name="button">The gamepad button to check.</param>
/// <returns>true if the specified gamepad button is currently down; otherwise, false.</returns>
public bool IsButtonDown(Buttons button)
{
    return CurrentState.IsButtonDown(button);
}

/// <summary>
/// Returns a value that indicates whether the specified gamepad button is currently up.
/// </summary>
/// <param name="button">The gamepad button to check.</param>
/// <returns>true if the specified gamepad button is currently up; otherwise, false.</returns>
public bool IsButtonUp(Buttons button)
{
    return CurrentState.IsButtonUp(button);
}

/// <summary>
/// Returns a value that indicates whether the specified gamepad button was just pressed on the current frame.
/// </summary>
/// <param name="button"><The gamepad button to check./param>
/// <returns>true if the specified gamepad button was just pressed on the current frame; otherwise, false.</returns>
public bool WasButtonJustPressed(Buttons button)
{
    return CurrentState.IsButtonDown(button) && PreviousState.IsButtonUp(button);
}

/// <summary>
/// Returns a value that indicates whether the specified gamepad button was just released on the current frame.
/// </summary>
/// <param name="button"><The gamepad button to check./param>
/// <returns>true if the specified gamepad button was just released on the current frame; otherwise, false.</returns>
public bool WasButtonJustReleased(Buttons button)
{
    return CurrentState.IsButtonUp(button) && PreviousState.IsButtonDown(button);
}
```

These methods serve two distinct purposes. For checking continuous states:

- `IsButtonDown`: Returns true as long as a button is being held down.
- `IsButtonUp`: Returns true as long as a button is not being pressed.

And for detecting state changes:

- `WasButtonJustPressed`: Returns true only on the frame when a button changes from up-to-down.
- `WasButtonJustReleased`: Returns true only on the frame when a button changes from down-to-up.

Finally, we'll add methods for controlling gamepad vibration:

```cs
/// <summary>
/// Sets the vibration for all motors of this gamepad.
/// </summary>
/// <param name="strength">The strength of the vibration from 0.0f (none) to 1.0f (full).</param>
/// <param name="time">The amount of time the vibration should occur.</param>
public void SetVibration(float strength, TimeSpan time)
{
    _vibrationTimeRemaining = time;
    GamePad.SetVibration(PlayerIndex, strength, strength);
}

/// <summary>
/// Stops the vibration of all motors for this gamepad.
/// </summary>
public void StopVibration()
{
    GamePad.SetVibration(PlayerIndex, 0.0f, 0.0f);
}
```

The vibration methods provide control over the gamepad's haptic feedback:

- `SetVibration`: Starts vibration at the specified strength for a set duration.
- `StopVibration`: Immediately stops all vibration.

> [!TIP]
> When setting vibration, you can specify both the strength (`0.0f` to `1.0f`) and duration. The vibration will automatically stop after the specified time has elapsed, so you don't need to manage stopping it manually.

That's it for the `GamePadInfo` class.  Next, let's create the actual input manager.

## The InputManager Class

Now that we have classes to handle keyboard, mouse, and gamepad input individually, we can create a centralized manager class to coordinate all input handling. The `InputManager` class will be static, providing easy access to all input states from anywhere in our game.

In the *Input* directory of the *MonoGameLibrary* project, add a new file named *InputManager.cs* with this initial structure:

```cs
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Input;

public static class InputManager
{

}
```

### InputManager Properties

The InputManager class needs properties to access each type of input device. Add these properties:

```cs
/// <summary>
/// Gets the state information of keyboard input.
/// </summary>
public static KeyboardInfo Keyboard { get; private set; }

/// <summary>
/// Gets the state information of mouse input.
/// </summary>
public static MouseInfo Mouse { get; private set; }

/// <summary>
/// Gets the state information of a gamepad.
/// </summary>
public static GamePadInfo[] GamePads { get; private set; }
```

> [!NOTE]
> The `GamePads` property is an array because MonoGame supports up to four gamepads simultaneously. Each gamepad is associated with a PlayerIndex (0-3).

### InputManager Methods

First, we need a method to initialize our input devices:

```cs
/// <summary>
/// Initializes this input manager.
/// </summary>
public static void Initialize()
{
    Keyboard = new KeyboardInfo();
    Mouse = new MouseInfo();

    GamePads = new GamePadInfo[4];
    for (int i = 0; i < 4; i++)
    {
        GamePads[i] = new GamePadInfo((PlayerIndex)i);
    }
}
```

Next, we'll add a method to update all input states:

```cs
/// <summary>
/// Updates the state information for the keyboard, mouse, and gamepad inputs.
/// </summary>
/// <param name="gameTime">A snapshot of the timing values for the current frame.</param>
public static void Update(GameTime gameTime)
{
    Keyboard.Update();
    Mouse.Update();

    for (int i = 0; i < 4; i++)
    {
        GamePads[i].Update(gameTime);
    }
}
```

> [!TIP]
> By centralizing input updates in the `InputManager`, we ensure all input states are updated consistently each frame. You only need to call `InputManager.Update` once in your game's [**Update**](xref:Microsoft.Xna.Framework.Game.Update(Microsoft.Xna.Framework.GameTime)) method.

### Implementing the InputManager Class

Now that we have our input management system complete, let's update our game to use it. Instead of tracking input states directly, we'll use the `InputManager` to handle all our input detection. Open *Game1.cs* and make the following changes:

Let's update the input code in our game now to instead use the `InputManager` class to manage tracking input states which inputs are active. Open the *Game1.cs* file and perform the following:

1. First we need to set up the `InputManager`.  In [**Initialize**](xref:Microsoft.Xna.Framework.Game.Initialize), add this initialization code just before `base.Initialize()`:

    ```cs
    InputManager.Initialize();
    ```

2. Next, in [**Update**](xref:Microsoft.Xna.Framework.Game.Update(Microsoft.Xna.Framework.GameTime)), we need to ensure input states are updated each frame.  Add the following as the first line of code inside the [**Update**](xref:Microsoft.Xna.Framework.Game.Update(Microsoft.Xna.Framework.GameTime)) method:

    ```cs
    InputManager.Update(gameTime);
    ```

3. Next, remove the `if` statement in [**Update**](xref:Microsoft.Xna.Framework.Game.Update(Microsoft.Xna.Framework.GameTime)) that checks for the gamepad back button or the keyboard escape key being pressed and then exits the game.

4. Finally, update the game controls to use the `InputManager`.  Replace the `HandleKeyboardInput`, `HandleMouseInput` and `HandleGamePadInput` methods with the following:

    ```cs
    private void HandleKeyboardInput()
    {
        if (InputManager.Keyboard.IsKeyDown(Keys.Escape))
        {
            Exit();
        }
        if (InputManager.Keyboard.IsKeyDown(Keys.Up))
        {
            _slimePosition.Y -= MOVEMENT_SPEED;
        }
        if (InputManager.Keyboard.IsKeyDown(Keys.Down))
        {
            _slimePosition.Y += MOVEMENT_SPEED;
        }
        if (InputManager.Keyboard.IsKeyDown(Keys.Left))
        {
            _slimePosition.X -= MOVEMENT_SPEED;
        }
        if (InputManager.Keyboard.IsKeyDown(Keys.Right))
        {
            _slimePosition.X += MOVEMENT_SPEED;
        }
    }

    private void HandleMouseInput()
    {
        if (InputManager.Mouse.WasButtonJustPressed(MouseButton.Left))
        {
            _batPosition = InputManager.Mouse.Position.ToVector2();
        }
    }

    private void HandleGamepadInput()
    {
        GamePadInfo gamePadOne = InputManager.GamePads[(int)PlayerIndex.One];

        if(gamePadOne.IsButtonDown(Buttons.Back))
        {
            Exit();
        }

        if (gamePadOne.IsButtonDown(Buttons.A))
        {
            _slimePosition.X += gamePadOne.LeftThumbStick.X * 1.5f * MOVEMENT_SPEED;
            _slimePosition.Y -= gamePadOne.LeftThumbStick.Y * 1.5f * MOVEMENT_SPEED;
            gamePadOne.SetVibration(1.0f, TimeSpan.FromSeconds(0.5f));
        }
        else
        {
            _slimePosition.X += gamePadOne.LeftThumbStick.X * MOVEMENT_SPEED;
            _slimePosition.Y -= gamePadOne.LeftThumbStick.Y * MOVEMENT_SPEED;
        }
    }
    ```

The key improvements in this implementation are:

1. Centralized Input Management:
    - All input is now accessed through the `InputManager`.
    - Input states are automatically tracked between frames.
    - No need to manually store previous states.

2. Improved Input Detection:
    - Mouse movement now only triggers on initial click using `WasButtonJustPressed`.
    - Gamepad vibration is handled through `SetVibration` with automatic duration.
    - Thumbstick values are easily accessed through `LeftThumbStick` property.

> [!NOTE]
> Using `WasButtonJustPressed` instead of `IsButtonDown` for the mouse control means the bat only moves when you first click, not continuously while holding the button. This gives you more precise control over movement.

Running the game now, you will be able to control it the same as before, only now we're using our new `InputManager` class instead.

## Conclusion

In this chapter, you learned how to:

- Detect the difference between continuous and single-frame input states.
- Create classes to manage different input devices.
- Build a centralized `InputManager` to coordinate all input handling that is:
  - Reusable across different game projects
  - Easy to maintain and extend
  - Consistent across different input devices

## Test Your Knowledge

1. What's the difference between checking if an input is "down" versus checking if it was "just pressed"?

   <details>
   <summary>Question 1 Answer</summary>

   > "Down" checks if an input is currently being held, returning true every frame while held. "Just pressed" only returns true on the first frame when the input changes from up to down, requiring comparison between current and previous states.
   </details><br />

2. Why do we track both current and previous input states?

   <details>
   <summary>Question 2 Answer</summary>

   > Tracking both states allows us to detect when input changes occur by comparing the current frame's state with the previous frame's state. This is essential for implementing "just pressed" and "just released" checks.
   </details><br />

3. What advantage does the `InputManager` provide over handling input directly?

   <details>
   <summary>Question 3 Answer</summary>

   > The `InputManager` centralizes all input handling, automatically tracks states between frames, and provides a consistent API across different input devices. This makes the code more organized, reusable, and easier to maintain.
   </details><br />
