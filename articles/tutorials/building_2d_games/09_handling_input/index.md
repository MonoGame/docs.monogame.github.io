---
title: "Chapter 09: Handling Input"
description: "Learn how to handle keyboard, mouse, and gamepad input in MonoGame."
---

When you play a game, you need ways to control what's happening; using a keyboard or gamepad to control a character or clicking the mouse to navigate a menu, MonoGame helps us handle all these different types of controls through dedicated input classes:

- [**Keyboard**](xref:Microsoft.Xna.Framework.Input.Keyboard): Detects which keys are being pressed.
- [**Mouse**](xref:Microsoft.Xna.Framework.Input.Mouse): Tracks mouse movement, button clicks, and scroll wheel use.
- [**GamePad**](xref:Microsoft.Xna.Framework.Input.GamePad): Manages controller input like button presses and joystick movement.

> [!NOTE]
> MonoGame also supports touch and accelerometer input for mobile devices, however this tutorial focuses on desktop input handling using keyboard, mouse, and gamepad controls.

Each of these input types has a `GetState` method that, when called, checks what is happening with that device at that moment. Think of it like taking a snapshot; when you call `GetState`, MonoGame looks at that exact moment to see which buttons are pressed, where the mouse is, or how the controller is being used.

In this chapter you will, we will learn how to use each of these dedicated input classes to handle player input.

## Keyboard Input

The keyboard is often the primary input device for PC games, used for everything from character movement to menu navigation. MonoGame provides the [**Keyboard**](xref:Microsoft.Xna.Framework.Input.Keyboard) class to handle keyboard input, making it easy to detect which keys are being pressed at any time.  Calling [**Keyboard.GetState**](xref:Microsoft.Xna.Framework.Input.Keyboard.GetState) will retrieve the current state of the keyboard as a [**KeyboardState**](xref:Microsoft.Xna.Framework.Input.KeyboardState) struct.

### KeyboardState Struct

The [**KeyboardState**](xref:Microsoft.Xna.Framework.Input.KeyboardState) struct contains methods that can be used to determine if a keyboard key is currently down or up:

| Method                                                                                                                | Description                                                              |
|-----------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------|
| [**IsKeyDown(Keys)**](xref:Microsoft.Xna.Framework.Input.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys)) | Returns `true` if the specified key is down; otherwise, returns `false`. |
| [**IsKeyUp(Keys)**](xref:Microsoft.Xna.Framework.Input.KeyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys))     | Returns `true` if the specified key is up; otherwise, returns `false`.   |

For example, if we wanted to see if the Space key is down, you could use the following:

```cs
if(Keyboard.GetState().IsKeyDown(Keys.Space))
{
    // The space key is down, so do something.
}
```

### Implementing Keyboard Input

Let's implement keyboard controls to move our slime sprite around the screen.  Open the *Game1.cs* file and perform the following:

1. First, add the following fields to track the position of the slime and movement speed:

    ```cs
    private Vector2 _slimePosition;
    private const float MOVEMENT_SPEED = 5.0f;
    ```

2. Next, in [**Update**](xref:Microsoft.Xna.Framework.Game.Update(Microsoft.Xna.Framework.GameTime)), check if the up, down, left, or right arrow keys are pressed, and if any of them are, adjust the slime's position.  Add the following just before the call to `base.Update`:

    ```cs
    KeyboardState keyboardState = Keyboard.GetState();
    
    if(keyboardState.IsKeyDown(Keys.Up))
    {
        _slimePosition.Y -= MOVEMENT_SPEED;
    }
    
    if(keyboardState.IsKeyDown(Keys.Down))
    {
        _slimePosition.Y += MOVEMENT_SPEED;
    }
    
    if(keyboardState.IsKeyDown(Keys.Left))
    {
        _slimePosition.X -= MOVEMENT_SPEED;
    }
    
    if(keyboardState.IsKeyDown(Keys.Right))
    {
        _slimePosition.X += MOVEMENT_SPEED;
    }
    ```

    > [!TIP]
    > Notice we store the keyboard state in a variable instead of calling [**Keyboard.GetState**](xref:Microsoft.Xna.Framework.Input.Keyboard.GetState) multiple times. This is more efficient and ensures consistent input checking within a single frame.

    > [!NOTE]
    > Remember that the Y-axis increases downward in MonoGame's coordinate system. This is why we subtract from Y to move up and add to Y to move down.

3. Finally, in [**Draw**](xref:Microsoft.Xna.Framework.Game.Draw(Microsoft.Xna.Framework.GameTime)), update the position of the slime when it is rendered by using the `_slimePosition` value:

    ```cs
    _slime.Draw(_spriteBatch, _slimePosition);
    ```

Running the game now, you can move the slime sprite around using the arrow keys on the keyboard. Try it out!

## Mouse Input

The mouse is often the secondary input device for PC games, used for various actions from camera movement to interacting with menus and objects.  MonoGame provides the [**Mouse**](xref:Microsoft.Xna.Framework.Input.Mouse) class to handle mouse input, making it easy to detect which buttons are pressed, the position of the mouse cursor, and the value of the scroll wheel.  Calling [**Mouse.GetState**](xref:Microsoft.Xna.Framework.Input.Mouse.GetState) will retrieve the current state of the mouse as a [**MouseState**](xref:Microsoft.Xna.Framework.Input.MouseState) struct.

### MouseState Struct

The [**MouseState**](xref:Microsoft.Xna.Framework.Input.MouseState) struct contains properties that can be used to determine the state of the mouse buttons, the mouse position, and the scroll wheel value:

| Property                                                                               | Type                                                              | Description                                                                                                             |
|----------------------------------------------------------------------------------------|-------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------|
| [**LeftButton**](xref:Microsoft.Xna.Framework.Input.MouseState.LeftButton)             | [**ButtonState**](xref:Microsoft.Xna.Framework.Input.ButtonState) | Returns the state of the left mouse button.                                                                             |
| [**MiddleButton**](xref:Microsoft.Xna.Framework.Input.MouseState.MiddleButton)         | [**ButtonState**](xref:Microsoft.Xna.Framework.Input.ButtonState) | Returns the state of the middle mouse button.  This is often the button when pressing the scroll wheel down as a button |
| [**Position**](xref:Microsoft.Xna.Framework.Input.MouseState.Position)                 | [**Point**](xref:Microsoft.Xna.Framework.Point)                   | Returns the position of the mouse cursor relative to the bounds of the game window.                                     |
| [**RightButton**](xref:Microsoft.Xna.Framework.Input.MouseState.RightButton)           | [**ButtonState**](xref:Microsoft.Xna.Framework.Input.ButtonState) | Returns the state of the right mouse button.                                                                            |
| [**ScrollWheelValue**](xref:Microsoft.Xna.Framework.Input.MouseState.ScrollWheelValue) | `int`                                                             | Returns the **cumulative** scroll wheel value since the start of the game                                               |
| [**XButton1**](xref:Microsoft.Xna.Framework.Input.MouseState.XButton1)                 | [**ButtonState**](xref:Microsoft.Xna.Framework.Input.ButtonState) | Returns the state of the first extended button on the mouse.                                                            |
| [**XButton2**](xref:Microsoft.Xna.Framework.Input.MouseState.XButton2)                 | [**ButtonState**](xref:Microsoft.Xna.Framework.Input.ButtonState) | Returns the state of the second extended button on the mouse.                                                           |

> [!NOTE]
> [**ScrollWheelValue**](xref:Microsoft.Xna.Framework.Input.MouseState.ScrollWheelValue) returns the cumulative value of the scroll wheel since the start of the game, not how much it moved since the last update.  To determine how much it moved between one update and the next, you would need to compare it with the previous frame's value.  We'll discuss comparing previous and current frame values for inputs in the next chapter.

Unlike keyboard input which uses [**IsKeyDown(Keys)**](xref:Microsoft.Xna.Framework.Input.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys))/[**IsKeyUp(Keys)**](xref:Microsoft.Xna.Framework.Input.KeyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys)) methods mouse buttons return a [**ButtonState**](xref:Microsoft.Xna.Framework.Input.ButtonState):

- [**ButtonState.Pressed**](xref:Microsoft.Xna.Framework.Input.ButtonState): The button is being held down.
- [**ButtonState.Released**](xref:Microsoft.Xna.Framework.Input.ButtonState): The button is not being pressed.

For example, if we wanted to see if the left mouse button is down, you could use the following

```cs
if(Mouse.GetState().LeftButton == ButtonState.Pressed)
{
    // The left button is down, so do something.
}
```

### Implementing Mouse Input

Let's implement mouse controls to move the bat sprite around the screen to the point that the cursor is clicked at.  Open *Game1.cs* and perform the following:

1. First, add the following field to track the position of the bat:

    ```cs
    private Vector2 _batPosition;
    ```

2. Next, in [**Initialize**](xref:Microsoft.Xna.Framework.Game.Initialize) set the initial position of the bat to where it is currently drawn, 10px to the right of the slime.  Add the following **after** the call to `base.Initialize()`

    ```cs
    _batPosition = new Vector2(_slime.Width + 10, 0);
    ```

    > [!IMPORTANT]
    > Notice that we set the value of the bat position **after** the call to `base.Initialize`.  Recall from Chapter 03 in the [Content Loading](../03_the_game1_file/index.md#content-loading), that the [**LoadContent**](xref:Microsoft.Xna.Framework.Game.LoadContent) method is called during the `base.Initialize()` call.  Since we are creating the slime sprite inside of [**LoadContent**](xref:Microsoft.Xna.Framework.Game.LoadContent) we need to ensure it's been created before we can use the `Width` property of the slime to set the position of the bat in [**Initialize**](xref:Microsoft.Xna.Framework.Game.Initialize).
    >
    > We could have just as easily set the bat's position inside the [**LoadContent**](xref:Microsoft.Xna.Framework.Game.LoadContent) method after creating the slime, but I wanted to demonstrate the importance of the call order relationship between [**Initialize**](xref:Microsoft.Xna.Framework.Game.Initialize) and [**LoadContent**](xref:Microsoft.Xna.Framework.Game.LoadContent)

3. Next, in [**Update**](xref:Microsoft.Xna.Framework.Game.Update(Microsoft.Xna.Framework.GameTime)), check if the left mouse button is pressed, and if so, adjust the bat's position to the position of the mouse cursor.  Add the following just before the call to `base.Update`:

    ```cs
    MouseState mouseState = Mouse.GetState();
    
    if(mouseState.LeftButton == ButtonState.Pressed)
    {
        _batPosition = mouseState.Position.ToVector2();
    }
    ```

4. Finally, in [**Draw**](xref:Microsoft.Xna.Framework.Game.Draw(Microsoft.Xna.Framework.GameTime)), update the position of the bat when it is rendered by using the `_batPosition` value:

```cs
_bat.Draw(_spriteBatch, _batPosition);
```

Running the game now, you can move the bat sprite around by clicking the left mouse button on the game screen and it will move to that position.  Try it out!

> [!NOTE]
> When the bat moves to the position of the mouse cursor, notice that it does so relative to the upper-left corner of the bat sprite and not the center of the sprite.  This is because the `Origin` of the bat sprite is [**Vector2.Zero**](xref:Microsoft.Xna.Framework.Vector2.Zero) (upper-left) corner by default.

## Gamepad Input

Gamepads are often used as a primary input for a game or an alternative for keyboard and mouse controls.  MonoGame provides the [**GamePad**](xref:Microsoft.Xna.Framework.Input.GamePad) class to handle gamepad input, making it easy to detect which buttons are pressed and the value of the joysticks. Calling [**GamePad.GetState**](xref:Microsoft.Xna.Framework.Input.GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex)) will retrieve the state of the gamepad as a [**GamePadState**](xref:Microsoft.Xna.Framework.Input.GamePadState) struct.  Since multiple gamepads can be connected, you will need to supply a [**PlayerIndex**](xref:Microsoft.Xna.Framework.PlayerIndex) value to specify which gamepad state to retrieve.

### GamePadState Struct

The [**GamePadState**](xref:Microsoft.Xna.Framework.Input.GamePadState) struct and properties that can be used to get the state of the buttons, dpad, triggers, and joysticks:

| Property                                                                       | Type                                                                            | Description                                                                                                                                                                                                                        |
|--------------------------------------------------------------------------------|---------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [**Buttons**](xref:Microsoft.Xna.Framework.Input.GamePadState.Buttons)         | [**GamePadButtons**](xref:Microsoft.Xna.Framework.Input.GamePadButtons)         | Returns a struct that identifies which buttons on the controller are pressed.                                                                                                                                                      |
| [**DPad**](xref:Microsoft.Xna.Framework.Input.GamePadState.DPad)               | [**GamePadDPad**](xref:Microsoft.Xna.Framework.Input.GamePadDPad)               | Returns a struct that identifies which directions on the DPad are pressed.                                                                                                                                                         |
| [**IsConnected**](xref:Microsoft.Xna.Framework.Input.GamePadState.IsConnected) | `bool`                                                                          | Returns a value that indicates whether the controller is connected.                                                                                                                                                                |
| [**ThumbSticks**](xref:Microsoft.Xna.Framework.Input.GamePadState.ThumbSticks) | [**GamePadThumbSticks**](xref:Microsoft.Xna.Framework.Input.GamePadThumbSticks) | Returns a struct that contains the direction of each thumbstick.  Each thumbstick (left and right) are represented as a [**Vector2**](xref:Microsoft.Xna.Framework.Vector2) value between `-1.0f` and `1.0` for the x- and y-axes. |
| [**Triggers**](xref:Microsoft.Xna.Framework.Input.GamePadState.Triggers)       | [**GamePadTriggers**](xref:Microsoft.Xna.Framework.Input.GamePadTriggers)       | Returns a struct that contains the value of each trigger. Each trigger (left and right) are represented as a `float` value between `0.0f`, meaning not pressed, and `1.0f`, meaning fully pressed.                                 |

#### Buttons

The [**GamePadState.Buttons**](xref:Microsoft.Xna.Framework.Input.GamePadState.Buttons) property returns a [**GamePadButtons**](xref:Microsoft.Xna.Framework.Input.GamePadButtons) struct that can be used to identify which buttons on the controller are pressed.  This struct contains the following properties:

| Property                                                                             | Type                                                              | Description                                   |
|--------------------------------------------------------------------------------------|-------------------------------------------------------------------|-----------------------------------------------|
| [**A**](xref:Microsoft.Xna.Framework.Input.GamePadButtons.A)                         | [**ButtonState**](xref:Microsoft.Xna.Framework.Input.ButtonState) | Returns the state of the A button             |
| [**B**](xref:Microsoft.Xna.Framework.Input.GamePadButtons.B)                         | [**ButtonState**](xref:Microsoft.Xna.Framework.Input.ButtonState) | Returns the state of the B button             |
| [**Back**](xref:Microsoft.Xna.Framework.Input.GamePadButtons.Back)                   | [**ButtonState**](xref:Microsoft.Xna.Framework.Input.ButtonState) | Returns the state of the Back button          |
| [**BigButton**](xref:Microsoft.Xna.Framework.Input.GamePadButtons.BigButton)         | [**ButtonState**](xref:Microsoft.Xna.Framework.Input.ButtonState) | Returns the state of the BigButton button     |
| [**LeftShoulder**](xref:Microsoft.Xna.Framework.Input.GamePadButtons.LeftShoulder)   | [**ButtonState**](xref:Microsoft.Xna.Framework.Input.ButtonState) | Returns the state of the LeftShoulder button  |
| [**LeftStick**](xref:Microsoft.Xna.Framework.Input.GamePadButtons.LeftStick)         | [**ButtonState**](xref:Microsoft.Xna.Framework.Input.ButtonState) | Returns the state of the LeftStick button     |
| [**RightShoulder**](xref:Microsoft.Xna.Framework.Input.GamePadButtons.RightShoulder) | [**ButtonState**](xref:Microsoft.Xna.Framework.Input.ButtonState) | Returns the state of the RightShoulder button |
| [**RightStick**](xref:Microsoft.Xna.Framework.Input.GamePadButtons.RightStick)       | [**ButtonState**](xref:Microsoft.Xna.Framework.Input.ButtonState) | Returns the state of the RightStick button    |
| [**Start**](xref:Microsoft.Xna.Framework.Input.GamePadButtons.Start)                 | [**ButtonState**](xref:Microsoft.Xna.Framework.Input.ButtonState) | Returns the state of the Start button         |
| [**X**](xref:Microsoft.Xna.Framework.Input.GamePadButtons.X)                         | [**ButtonState**](xref:Microsoft.Xna.Framework.Input.ButtonState) | Returns the state of the X button             |
| [**Y**](xref:Microsoft.Xna.Framework.Input.GamePadButtons.Y)                         | [**ButtonState**](xref:Microsoft.Xna.Framework.Input.ButtonState) | Returns the state of the Y button             |

> [!NOTE]
> Recall from [Chapter 01](../01_what_is_monogame/index.md) that MonoGame is a implementation the XNA API.  Since XNA was originally created for making games on Windows PC and Xbox 360, the names of the gamepad buttons match those of an Xbox 360 controller.

Like with the [mouse input](#mousestate-struct), each of these buttons are represented by a [**ButtonState**](xref:Microsoft.Xna.Framework.Input.ButtonState) enum value.  For instance, if you wanted to check if the A button is being pressed you could do the following:

```cs
if(GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
{
    // Button A is pressed, do something.
}
```

#### DPad

The [**DPad**](xref:Microsoft.Xna.Framework.Input.GamePadState.DPad)  property returns a [**GamePadDPad**](xref:Microsoft.Xna.Framework.Input.GamePadDPad) struct that can be used to identify which DPad buttons on the controller are pressed. This struct contains the following properties:

| Property                                                         | Type                                                              | Description                                 |
|------------------------------------------------------------------|-------------------------------------------------------------------|---------------------------------------------|
| [**Down**](xref:Microsoft.Xna.Framework.Input.GamePadDPad.Down)  | [**ButtonState**](xref:Microsoft.Xna.Framework.Input.ButtonState) | Returns the state of the DPad Down button.  |
| [**Left**](xref:Microsoft.Xna.Framework.Input.GamePadDPad.Down)  | [**ButtonState**](xref:Microsoft.Xna.Framework.Input.ButtonState) | Returns the state of the DPad Left button.  |
| [**Right**](xref:Microsoft.Xna.Framework.Input.GamePadDPad.Down) | [**ButtonState**](xref:Microsoft.Xna.Framework.Input.ButtonState) | Returns the state of the DPad Right button. |
| [**Up**](xref:Microsoft.Xna.Framework.Input.GamePadDPad.Down)    | [**ButtonState**](xref:Microsoft.Xna.Framework.Input.ButtonState) | Returns the state of the DPad Up Button.    |

Like with the [Buttons](#buttons), these also return a [**ButtonState**](xref:Microsoft.Xna.Framework.Input.ButtonState) enum value to represent the state of the DPad button.  For instance, if you wanted to check if the DPad up button is being pressed, you could do the following:

```cs
if(GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed)
{
    // DPad down is pressed, do something.
}
```

#### Thumbsticks

The [**ThumbSticks**](xref:Microsoft.Xna.Framework.Input.GamePadState.ThumbSticks) property returns a [**GamePadThumbSticks**](xref:Microsoft.Xna.Framework.Input.GamePadThumbSticks) struct that can be used to retrieve the values of the left and right thumbsticks.  This struct contains the following properties:

| Property                                                                 | Type                                                | Description                                    |
|--------------------------------------------------------------------------|-----------------------------------------------------|------------------------------------------------|
| [**Left**](xref:Microsoft.Xna.Framework.Input.GamePadThumbSticks.Left)   | [**Vector2**](xref:Microsoft.Xna.Framework.Vector2) | The direction the left thumbstick is pressed.  |
| [**Right**](xref:Microsoft.Xna.Framework.Input.GamePadThumbSticks.Right) | [**Vector2**](xref:Microsoft.Xna.Framework.Vector2) | The direction the right thumbstick is pressed. |

The thumbstick values are represented as a [**Vector2**](xref:Microsoft.Xna.Framework.Vector2) value:

- X-axis: A value between `-1.0f` (pushed fully to the left) and `1.0f` (pushed fully to the right).
- Y-axis: A value between `-1.0f` (pushed fully downward) and `1.0f` (pushed fully upward).

For example, if you wanted to move a sprite using the left thumbstick, you could do the following

```cs
Vector2 leftStick = GamePad.GetState(PlayerIndex.One).Thumbsticks.Left;
leftStick.Y *= -1.0f;
sprite.Position += leftStick;
```

> [!IMPORTANT]
> When the Y-Axis is fully pushed downward, its value is `-1.0f`.  Recall from [Chapter 05](../05_working_with_textures/index.md#drawing-a-texture) that MonoGame uses a coordinate system where the Y values **increase** moving down. This is directly opposite of the value represented by the thumbstick.  To resolve this, the common approach is to multiply the Y value of the thumbstick by `-1.0f`.  This is why it is done in the example above.

#### Triggers

The [**Triggers**](xref:Microsoft.Xna.Framework.Input.GamePadState.Triggers) property returns a [**GamePadTriggers**](xref:Microsoft.Xna.Framework.Input.GamePadTriggers) struct that can be used to retrieve the values of the left and right triggers. This struct contains the following properties:

| Property                                                              | Type    | Description                    |
|-----------------------------------------------------------------------|---------|--------------------------------|
| [**Left**](xref:Microsoft.Xna.Framework.Input.GamePadTriggers.Left)   | `float` | The value of the left trigger. |
| [**Right**](xref:Microsoft.Xna.Framework.Input.GamePadTriggers.Right) | `float` | The value of the left trigger. |

The trigger values are represented as a float value between `0.0f` (not pressed) to `1.0f` (fully pressed). The triggers on a gamepad, however, can be either *analog* or *digital* depending the gamepad manufacturer.  For gamepads with *digital* triggers, the value will always be either `0.0f` or `1.0f`, as a digital trigger does not register values in between based on the amount of pressure applied to the trigger.  

For example, if we were creating a racing game, the right trigger could be used for acceleration like the following:

```cs
float acceleration = GamePad.GetState(PlayerIndex.One).Triggers.Right;
```

### GamePadState Methods

The [**GamePadState**](xref:Microsoft.Xna.Framework.Input.GamePadState) struct also contains two methods that can be used to get information about the device's inputs as either being up or down:

| Method                                                                                                                           | Description                                                                                                                                                                                                                                                                                                           |
|----------------------------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [**IsButtonDown(Buttons)**](xref:Microsoft.Xna.Framework.Input.GamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons)) | Returns a value that indicates whether the specified button is down.  Multiple [**Buttons**](xref:Microsoft.Xna.Framework.Input.Buttons) values can be given using the bitwise OR `|` operator.  When multiple buttons are given, the return value indicates if all buttons specified are down, not just one of them. |
| [**IsButtonUp(Buttons)**](xref:Microsoft.Xna.Framework.Input.GamePadState.IsButtonUp(Microsoft.Xna.Framework.Input.Buttons))     | Returns a value that indicates whether the specified button is up.  Multiple [**Buttons**](xref:Microsoft.Xna.Framework.Input.Buttons) values can be given using the bitwise OR `|` operator.  When multiple buttons are given, the return value indicates if all buttons specified are up, not just one of them.     |

You can use the [**IsButtonDown(Buttons)**](xref:Microsoft.Xna.Framework.Input.GamePadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons)) and [**IsButtonUp(Buttons)**](xref:Microsoft.Xna.Framework.Input.GamePadState.IsButtonUp(Microsoft.Xna.Framework.Input.Buttons)) methods to get the state of all buttons, including the DPad.  The following is a complete list of all of the [**Buttons**](xref:Microsoft.Xna.Framework.Input.Buttons) enum values:

- [**Buttons.A**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.B**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.Back**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.BigButton**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.DPadDown**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.DPadLeft**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.DPadRight**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.DPadUp**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.LeftShoulder**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.LeftStick**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.LeftThumbstickDown**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.LeftThumbstickLeft**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.LeftThumbstickRight**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.LeftThumbstickUp**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.LeftTrigger**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.None**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.RightShoulder**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.RightStick**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.RightStickDown**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.RightStickLeft**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.RightStickRight**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.RightStickUp**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.RightTrigger**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.Start**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.X**](xref:Microsoft.Xna.Framework.Input.Buttons)
- [**Buttons.Y**](xref:Microsoft.Xna.Framework.Input.Buttons)


> [!CAUTION]
> While you can use these methods to get the state of any of these button inputs, the state will only tell you if it is being pressed or released.  For the actual joystick values and trigger values, you would need to use the properties instead.

For example, if we wanted to check if the A button on the the first gamepad is pressed, you could use the following:

```cs
if(GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A))
{
    // The A button is pressed, do something.
}
```

### Implementing GamePad Input

Let's implement gamepad controls as an alternative method of moving the slime sprite around.  Open the *Game1.cs* file and perform the following:

1. In [**Update**](xref:Microsoft.Xna.Framework.Game.Update(Microsoft.Xna.Framework.GameTime)), use the value of the left thumbstick to adjust the sprite's position. Since the value of the thumbstick is a range between `-1.0f` and `1.0f`, we can multiply those values by the `MOVEMENT_SPEED`.  This will make the slime move slower or faster depending on how far in the direction the thumbstick is pushed.  Add the following just before the `base.Update` call:

```cs
GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
_slimePos.X += gamePadState.ThumbSticks.Left.X * MOVEMENT_SPEED;
_slimePos.Y -= gamePadState.ThumbSticks.Left.Y * MOVEMENT_SPEED;
```

Running the game now, you can move the slime sprite around using the left thumbstick on your gamepad.  Try it out!  Notice that the more you push the thumbstick in a particular direction, the faster the slime moves up to the movement speed cap.

### GamePad Vibration

Another thing we can do with a gamepad is tell it to vibrate.  To do this, the [**GamePad**](xref:Microsoft.Xna.Framework.Input.GamePad) class has a [**SetVibration**](xref:Microsoft.Xna.Framework.Input.GamePad.SetVibration(Microsoft.Xna.Framework.PlayerIndex,System.Single,System.Single)) method that requires the player index, and the speed of the left and right vibration motors.  The speed can be any value from `0.0f` (no vibration) to `1.0f` (full vibration).  

Let's adjust the current code so that when the A button is pressed on the gamepad, it gives a slight speed boost to the slime as it moves.  When moving with a speed boost, we can apply vibration to the gamepad as feedback to the player.  Update the code you just added to the following:

```cs
GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

if (gamePadState.Buttons.A == ButtonState.Pressed)
{
    _slimePos.X += gamePadState.ThumbSticks.Left.X * 1.5f * MOVEMENT_SPEED;
    _slimePos.Y -= gamePadState.ThumbSticks.Left.Y * 1.5f * MOVEMENT_SPEED;
    GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
}
else
{
    _slimePos.X += gamePadState.ThumbSticks.Left.X * MOVEMENT_SPEED;
    _slimePos.Y -= gamePadState.ThumbSticks.Left.Y * MOVEMENT_SPEED;
    GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
}
```

Running the game now, when you press the A button, the slime sprite will move slightly faster and you can feel the vibration. Try it out!

## Conclusion

In this chapter, you learned how to:

- Handle keyboard input to detect key presses.
- Handle mouse input including button clicks and cursor position.
- Work with gamepad controls including buttons, thumbsticks, and vibration.
- Implement movement controls using different input methods.
- Consider controller-specific details like coordinate systems and analog vs digital input.

In the next chapter, we'll learn how to track previous input states to handle single-press events and implement an input management system to simplify some of the complexity of handling input.

## Test Your Knowledge

1. Why do we store the result of GetState() in a variable instead of calling it multiple times?

   <details>
   <summary>Question 1 Answer</summary>
   
   > Storing the state in a variable is more efficient and ensures consistent input checking within a frame. Each GetState() call polls the device, which can impact performance if called repeatedly.
   </details><br />

2. What's the main difference between how keyboard and mouse/gamepad button states are checked?

   <details>
   <summary>Question 2 Answer</summary>
   
   > Keyboard input uses IsKeyUp/IsKeyDown methods, while mouse and gamepad buttons return a ButtonState enum value (Pressed or Released).
   </details><br />

3. When using thumbstick values for movement, why do we multiply the Y value by -1?

   <details>
   <summary>Question 3 Answer</summary>
   
   > The thumbstick Y-axis values (-1.0f down to 1.0f up) are inverted compared to MonoGame's screen coordinate system (Y increases downward). Multiplying by -1 aligns the thumbstick direction with screen movement.
   </details><br />

4. What's the difference between analog and digital trigger input on a gamepad?

   <details>
   <summary>Question 4 Answer</summary>
   
   > Analog triggers provide values between 0.0f and 1.0f based on how far they're pressed, while digital triggers only report 0.0f (not pressed) or 1.0f (pressed). This affects how you handle trigger input in your game.
   </details><br />