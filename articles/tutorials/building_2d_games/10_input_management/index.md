---
title: "Chapter 10: Input Management"
description: TODO WRITE DESCRIPTION
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

Let's look at the difference using keyboard input as an example. With our current implementation, we can check if a key is down using [**KeyboardState.IsKeyDown**]():

```cs
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

The `KeyboardInfo` class will be the first one we create.  This class will track the previous and current state of keyboard input and provide methods of checking the state of keys on the keyboard. In the *Input* directory of the *MonoGameLibrary* project, add a new file named *KeyboardInfo.cs*.  Add the following code for the foundation fo the `KeyboardInfo` class:

```cs
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Input;

public class KeyboardInfo
{

}
```

### KeyboardInfo Properties

The `KeyboardInfo` class will need two properties to track the previous and current state of keyboard input.  Add the following properties:

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

Both of these properties are get-only, so they are still accessible outside of the input management system we're creating if needed, but setting the values for each remains private within the class itself.

### KeyboardInfo Constructor

The `KeyboardInfo` class will only need a single default constructor.  Add the following constructor:

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

This creates a new `KeyboardInfo` instance and sets the initial state values by setting the `PreviousState` to a new empty [**KeyboardState**]() and the `CurrentState` to the state of the keyboard at that moment.

### KeyboardInfo Methods

First, the `KeyboardInfo` class needs a method to update the `PreviousState` and `CurrentState` properties.  Add the following method:

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

With this, each time `Update` is called, whatever the current state is will get cached into the `PreviousState` property and then the `CurrentState` property will be updated with the most recent state.

Next, the `KeyboardInfo` class will need methods to retrieve the state of key inputs.  Add the following methods:

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

These methods handle different aspects of keyboard input:

- `IsKeyDown` and `IsKeyUp` are simple wrappers for the existing methods offered by the current state.  They will return true for as long as the key is held down or is up, respectively.
- `WasKeyJustPressed` and `WasKeyJustReleased` are used to check for frame specific key state changes.  They only return true on the specific frame when a key state occurs; either from up-to-down (pressed) or down-to-up (released).

The difference is that the first pair tracks continuous states wile the second pair tracks frame-to-frame changes.  This is useful for distinguishing between holding a key down and tapping a key. 

## The MouseInfo Class

The `MouseInfo` class will track the previous and current state of mouse input, properties for getting the position and scroll wheel values, and methods for checking the state of mouse buttons.  





