---
title: How to resize a Game
description: Demonstrates how to handle the resizing of the active game window.
requireMSLicense: true
---

## Overview

On Desktop, it is normally possible for the user to change how the game window is sized, either by moving from full-screen to windowed, or by altering the resolution of their screen.  In these cases an event is fired to enable you to handle these changes.

> [!NOTE]
> Ideally, your games drawing should always take account of the Aspect Ratio and dimensions of the displayed screen, regardless of device, else content may not be always drawn where you expect it to.

## To handle player window resizing to a game

1. Derive a class from [Game](xref:Microsoft.Xna.Framework.Game).

2. Set [Game.GameWindow.AllowUserResizing](xref:Microsoft.Xna.Framework.GameWindow#Microsoft_Xna_Framework_GameWindow_AllowUserResizing) to **true**.

3. Add an event handler for the **ClientSizeChanged** event of [Game.Window](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Window).

    ```csharp
    this.Window.AllowUserResizing = true;
    this.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
    ```

4. Implement a method to handle the **ClientSizeChanged** event of [Game.Window](xref:Microsoft.Xna.Framework.GameWindow).

    ```csharp
    void Window_ClientSizeChanged(object sender, EventArgs e)
    {
        // Make changes to handle the new window size.            
    }
    ```

## See Also

- [How to articles for the Graphics Pipeline](index.md)

### Concepts

- [What Is 3D Rendering?](../whatis/graphics/WhatIs_3DRendering.md)
- [What Is a Back Buffer?](../whatis/graphics/WhatIs_BackBuffer.md)
- [What Is a Viewport?](../whatis/graphics/WhatIs_Viewport.md)

### Reference

- [GameWindow](xref:Microsoft.Xna.Framework.GameWindow)
- [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager)
