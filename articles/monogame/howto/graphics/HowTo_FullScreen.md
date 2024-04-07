---
title: How to create a Full-Screen Game
description: Demonstrates how to start a game in full-screen mode.
---

# Creating a Full-Screen Game

Demonstrates how to start a game in full-screen mode.

## Creating a Full-Screen Game

### To create a full-screen game

1. Derive a class from [Game](xref:Microsoft.Xna.Framework.Game).

2. After creating the [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager), set its [PreferredBackBufferWidth](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.PreferredBackBufferWidth) and [PreferredBackBufferHeight](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.PreferredBackBufferHeight) to the desired screen height and width.

3. Set [IsFullScreen](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.IsFullScreen) to **true**.

    ```csharp
    this.graphics.PreferredBackBufferWidth = 480;
    this.graphics.PreferredBackBufferHeight = 800;
    
    this.graphics.IsFullScreen = true;
    ```

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
