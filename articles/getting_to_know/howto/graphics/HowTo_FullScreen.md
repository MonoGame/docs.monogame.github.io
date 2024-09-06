---
title: How to create a Full-Screen Game
description: Demonstrates how to start a game in full-screen mode.
requireMSLicense: true
---

## Overview

By default, MonoGame will render in a Window pre-set to the default (800 x 480) resolution.  If you instead want to render to the full screen, then is it as simple as flipping a switch and the renderer will use the full dimensions of the targeted display device.

> [!NOTE]
> Rendering to the full screen does NOT change the resolution that the game will be drawn in, that is something as a game developer you have to control.  This is because the resolution the game draws at will have a direct impact on the content you are rendering, so you need to best control what gets drawn and how.

## To create a full-screen game

1. Derive a class from [Game](xref:Microsoft.Xna.Framework.Game).

1. After creating the [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager), set its [PreferredBackBufferWidth](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.PreferredBackBufferWidth) and [PreferredBackBufferHeight](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.PreferredBackBufferHeight) to the desired screen height and width.

    > [!NOTE]
    > Check the [What Is 3D Rendering?](../../whatis/graphics/WhatIs_3DRendering.md) guide on the various ways the [GraphicsDevice](xref:Microsoft.Xna.Framework.GraphicsDevice) and [Back Buffer?](../../whatis/graphics/WhatIs_BackBuffer.md) can be initialized.

1. Set [IsFullScreen](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.IsFullScreen) to **true**.

    ```csharp
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        // Setup up the default resolution for the project
        _graphics.PreferredBackBufferWidth = 800;
        _graphics.PreferredBackBufferHeight = 480;

        // Runs the game in "full Screen" mode using the set resolution
        _graphics.IsFullScreen = true;

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }
    ```

## See Also

- [How to articles for the Graphics Pipeline](index.md)

### Concepts

- [What Is 3D Rendering?](../../whatis/graphics/WhatIs_3DRendering.md)
- [What Is a Back Buffer?](../../whatis/graphics/WhatIs_BackBuffer.md)
- [What Is a Viewport?](../../whatis/graphics/WhatIs_Viewport.md)

### Reference

- [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager)
