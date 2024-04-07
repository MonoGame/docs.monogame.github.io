---
title: How to display Multiple Screens with Viewports
description: Demonstrates how to use viewports to display different scenes simultaneously using two cameras.
---

# Displaying Multiple Screens with Viewports

Demonstrates how to use viewports to display different scenes simultaneously using two cameras.

![A Split screen Example](../images/graphics_split_screen.png)

## To create multiple screens

1. In your [LoadContent](xref:Microsoft.Xna.Framework.Game) method, create two new [Viewport](xref:Microsoft.Xna.Framework.Graphics.Viewport) objects to define the two new "split" regions of the screen.

    > In this example, the screen is split in half vertically.

    ```csharp
    Viewport defaultViewport;
    Viewport leftViewport;
    Viewport rightViewport;
    Matrix projectionMatrix;
    Matrix halfprojectionMatrix;

    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);
    
        defaultViewport = GraphicsDevice.Viewport;
        leftViewport = defaultViewport;
        rightViewport = defaultViewport;
        leftViewport.Width = leftViewport.Width / 2;
        rightViewport.Width = rightViewport.Width / 2;
        rightViewport.X = leftViewport.Width;
    ```

2. Create a projection matrix to fit each new viewport.

    In this case, because the screen is split in half, only one new projection matrix is necessary. It has the same settings as the 4:3 full screen projection matrix, except the aspect ratio is now 2:3.

    ```csharp
        projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.PiOver4, 4.0f / 3.0f, 1.0f, 10000f);

        halfprojectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.PiOver4, 2.0f / 3.0f, 1.0f, 10000f);
    }
    ```

3. In your Game [Game.Draw](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Draw_Microsoft_Xna_Framework_GameTime_) method, assign one of the viewports to draw as the [GraphicsDevice](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice)[Viewport](xref:Microsoft.Xna.Framework.Graphics.Viewport).

4. Draw your scene as normal, using the camera (or view matrix) associated with this perspective along with the proper projection matrix.

    ```csharp
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Viewport = defaultViewport;
        GraphicsDevice.Clear(Color.CornflowerBlue);
    
        GraphicsDevice.Viewport = leftViewport;
        DrawScene(gameTime, Camera1.ViewMatrix, halfprojectionMatrix);
    ```

5. After drawing the first scene, assign the other viewport to the [Viewport](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice.Viewport) property.

6. Draw your scene again with the associated camera or view matrix, and the proper projection matrix.

    ```csharp
        GraphicsDevice.Viewport = rightViewport;
        DrawScene(gameTime, Camera2.ViewMatrix, halfprojectionMatrix);
    
        base.Draw(gameTime);
    
    }
    ```

## See Also

### Concepts

[What Is a Viewport?](../../whatis/graphics/WhatIs_Viewport.md)  

#### Reference

[GraphicsDevice.Viewport](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice)  
[Viewport Structure](xref:Microsoft.Xna.Framework.Graphics.Viewport)  

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
