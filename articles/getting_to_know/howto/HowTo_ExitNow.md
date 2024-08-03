---
title: How to exit a Game Immediately
description: Demonstrates how to exit a game in response to user input.
---

# Exiting a Game Immediately

Demonstrates how to exit a game in response to user input.

## Exiting a Game Without Finishing the Current Update

> **Note* some platforms react differently to **Game.Exit**, so be sure to test on a device!

### To exit the game loop without running any remaining code in the update handler

1. Derive a class from [Game](xref:Microsoft.Xna.Framework.Game).

2. Create a method that checks [KeyboardState.IsKeyDown](xref:Microsoft.Xna.Framework.Input.KeyboardState) for the state of the **ESC** key.

3. If the ESC key has been pressed, call [Game.Exit](xref:Microsoft.Xna.Framework.Game.Exit) and return **true**.

    ```csharp
    bool checkExitKey(KeyboardState keyboardState, GamePadState gamePadState)
    {
        // Check to see whether ESC was pressed on the keyboard 
        // or BACK was pressed on the controller.
        if (keyboardState.IsKeyDown(Keys.Escape) ||
            gamePadState.Buttons.Back == ButtonState.Pressed)
        {
            Exit();
            return true;
        }
        return false;
    }
    ```

4. Call the method in **Game.Update**, and return from **Update** if the method returned **true**.

    ```csharp
    GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
    KeyboardState keyboardState = Keyboard.GetState();
    
    // Check to see if the user has exited
    if (checkExitKey(keyboardState, gamePadState))
    {
        base.Update(gameTime);
        return;
    }
    ```

5. Create a method to handle the **Game.Exiting** event.

    The **Exiting** event is issued at the end of the tick in which [Game.Exit](xref:Microsoft.Xna.Framework.Game.Exit) is called.

    ```csharp
    void Game1_Exiting(object sender, EventArgs e)
    {
        // Add any code that must execute before the game ends.
    }
    ```

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
