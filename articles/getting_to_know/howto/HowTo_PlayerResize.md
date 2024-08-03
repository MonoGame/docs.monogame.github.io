---
title: How to resize a Game
description: Demonstrates how to handle the resizing of the active game window.
---

# Adding Window Resizing Functionality

## To add player window resizing to a game

1. Derive a class from [Game](xref:Microsoft.Xna.Framework.Game).

2. Set [Game.GameWindow.AllowUserResizing](xref:Microsoft.Xna.Framework.GameWindow.AllowUserResizing) to **true**.

3. Add an event handler for the **ClientSizeChanged** event of [Game.Window](xref:Microsoft.Xna.Framework.Game.Window).

    ```csharp
    this.Window.AllowUserResizing = true;
    this.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
    ```

4. Implement a method to handle the **ClientSizeChanged** event of [Game.Window](xref:Microsoft.Xna.Framework.Game.Window).

    ```csharp
    void Window_ClientSizeChanged(object sender, EventArgs e)
    {
        // Make changes to handle the new window size.            
    }
    ```

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
