---
title: Detecting input from a GamePad
description: The code in this topic describes how to detect input on a GamePad.
---

# Detecting input from a GamePad

By using [GamePad.GetState](xref:Microsoft.Xna.Framework.Input.GamePad#Microsoft_Xna_Framework_Input_GamePad_GetState_System_Int32_) a game can determine which buttons are being held down. A game often needs to detect when a user has pressed or released a button. For example, there is the case of an action title that requires users to press and release keys in rapid succession. The example uses a cached [GamePadState](xref:Microsoft.Xna.Framework.Input.GamePadState) object to determine if buttons were pressed or released in a given frame.

Unlike GamePads however, multiple GamePads can be connected to a computer or console at the same time, so the [GamePad.GetState](xref:Microsoft.Xna.Framework.Input.GamePad#Microsoft_Xna_Framework_Input_GamePad_GetState_System_Int32_) call requires an Index parameter for which controller is being polled.  You also need to query the system the game is currently on for its [GamePad.MaximumGamePadCount](xref:Microsoft.Xna.Framework.Input.GamePad#Microsoft_Xna_Framework_Input_GamePad_MaximumGamePadCount) to determine how many controllers are supported and how many need to be polled for each frame.  Also unlike GamePads, GamePads can be disconnected (especially if the battery dies) at any time and most consoles require you to validate this to avoid player issues.

Depending on game design, there may be times when checking for a button press needs to occur more frequently, and other times it does not. It is possible in the case of very fast button presses that more than one key press could occur within one frame. In such a case, the last button press is returned. Writing code that checks as often as possible for button presses is the best way to handle this case.

## Types of GamePad input

Most GamePads include a variety of different input options, including (but not limited to)

* Thumbsticks - providing ranged motion in two axis.
* Buttons (including buttons on the Thumbsticks) - Digital on/off buttons (similar to keyboard keys)
* Triggers - providing ranged motion in a singular axis.
* Touchpads - in some advanced controllers (such as the PlayStation Dual Shock controller) include a small touchpad.

Additionally, most controllers also support haptic feedback (vibration) in the controller, which is different depending on the controller being used and for which system.

> Joysticks also work the same as GamePads, but use their own [Joystick](xref:Microsoft.Xna.Framework.Input.Joystick) and [JoystickState](xref:Microsoft.Xna.Framework.Input.JoystickState) classes. Operationally however, they work the same as GamePads.

## Detecting input changes on a GamePad

1. Declare a [GamePadState](xref:Microsoft.Xna.Framework.Input.GamePadState) object to hold the last known GamePad state (in this example, the **oldState** object).

2. Assign this object a value in your constructor.

3. Call [GamePad.GetState](xref:Microsoft.Xna.Framework.Input.GamePad#Microsoft_Xna_Framework_Input_GamePad_GetState_System_Int32_) to retrieve the current GamePad state (in this example, the **newState** object).

4. Compare the values in your **newState** object to the values in the **oldState** object.

    Buttons pressed in the **newState** object that were not pressed in the **oldState** object were pressed during this frame. Conversely, buttons pressed in the **oldState** object that are not pressed in the **newState** object were released during this frame.

    > For Thumbsticks and Triggers, it is not necessary to compare to the previous value unless you also need to calculate the difference, e.g. Was the variable controller moved fast or slow.  Reading just the current value is usually enough.

5. Update **oldState** object to the **newState** object before leaving **Update**.

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GamePadInput
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        GamePadState oldState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            base.Initialize();
            oldState = GamePad.GetState(PlayerIndex.One);
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            UpdateInput();

            base.Update(gameTime);
        }

        private void UpdateInput()
        {
            GamePadState newState = GamePad.GetState(PlayerIndex.One);

            // Is the A Button down?
            if (newState.IsButtonDown(Buttons.A))
            {
                if (!oldState.IsButtonDown(Buttons.A))
                {
                    // If not down last update, the button has just been pressed.
               }
            }
            else if (oldState.IsButtonDown(Buttons.A))
            {
                // Button was down last update, but not down now, so it has just been released.
            }

            // Which direction is the right thumbstick being moved?
            Vector2 direction = newState.ThumbSticks.Right;

            // How much is the left trigger being squeezed?
            float leftTriggerAmount = newState.Triggers.Left;

            // Update saved state.
            oldState = newState;
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
```

The above sample demonstrates sampling just the first connected controller, to support multiple controllers, you will need to sample from all connected controllers (as well as managing their connected state in case one is disconnected) and use an array of [GamePadState](xref:Microsoft.Xna.Framework.Input.GamePadState) to maintain the cache of all controllers.

> P.S. Most mobiles these days can support Bluetooth GamePads, so make sure you also support them if you intend to ship your game on mobile.

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
