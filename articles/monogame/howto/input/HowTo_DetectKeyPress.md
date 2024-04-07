---
title: Detecting a Key Press
description: The code in this topic describes how to detect a key press or release on the keyboard.
---

# Detecting a Key Press

By using [GetState](xref:Microsoft.Xna.Framework.Input.Keyboard.GetState) a game can determine which keys are being held down. A game often needs to detect when a user has pressed or released a key. For example, there is the case of an action title that requires users to press and release keys in rapid succession. The example uses a cached [KeyboardState](xref:Microsoft.Xna.Framework.Input.KeyboardState) object to determine if keys were pressed or released in a given frame.

Depending on game design, there may be times when checking for a key press needs to occur more frequently, and other times it does not. It is possible in the case of very fast key presses that more than one key press could occur within one frame. In such a case, the last key press is returned. Writing code that checks as often as possible for key presses is the best way to handle this case.

## Detecting a Key Press or Release

1. Declare a [KeyboardState](xref:Microsoft.Xna.Framework.Input.KeyboardState) object to hold the last known keyboard state (in this example, the **oldState** object).

2. Assign this object a value in your constructor.

3. Call [GetState](xref:Microsoft.Xna.Framework.Input.Keyboard.GetState) to retrieve the current keyboard state (in this example, the **newState** object).

4. Compare the values in your **newState** object to the values in the **oldState** object.

    Keys pressed in the **newState** object that were not pressed in the **oldState** object were pressed during this frame. Conversely, keys pressed in the **oldState** object that are not pressed in the **newState** object were released during this frame.

5. Update **oldState** object to the **newState** object before leaving **Update**.

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Keypress
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        KeyboardState oldState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            base.Initialize();
            oldState = Keyboard.GetState();
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
            KeyboardState newState = Keyboard.GetState();

            // Is the SPACE key down?
            if (newState.IsKeyDown(Keys.Space))
            {
                if (!oldState.IsKeyDown(Keys.Space))
                {
                    // If not down last update, key has just been pressed.
                }
            }
            else if (oldState.IsKeyDown(Keys.Space))
            {
                // Key was down last update, but not down now, so it has just been released.
            }

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

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
