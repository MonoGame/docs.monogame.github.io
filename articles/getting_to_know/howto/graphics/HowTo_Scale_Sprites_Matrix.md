---
title: Scaling Sprites Based On Screen Size
description: Demonstrates how to scale sprites using a matrix that is created based on the viewport width.
requireMSLicense: true
---

## Overview

Ensuring that the textures you draw to the screen are always scaled the same regardless of screen resolution is key, especially when targeting so many different devices with varying screen sizes.  The key to handling this is to generate a scaling matrix based on your "designed" screen size and then applying that to the `SpriteBatch` calls you make when drawing to the screen.  This is crucial for Screen UI / UX for example.

To demonstrate this, we will design a view in a default resolution of `800 x 600` and define a scaling matrix that works on the same ratio.

> [!NOTE]
> If you intend to work with multiple different scaling resolutions, you will need to define different scaling matrix's appropriately, if you allow the user to redefine what resolution the game runs in.

## Scaling Sprites Based on Screen Size

1. Set the [PreferredBackBufferHeight](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.PreferredBackBufferHeight) and [PreferredBackBufferWidth](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.PreferredBackBufferWidth) properties of [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager) in the `Game` constructor to set the default screen size of your game.

    ```csharp
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferHeight = 600,
            PreferredBackBufferWidth = 800
        };
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }
    ```

2. Set up some variables to hold the debug textures we will draw, and then in your [Game.LoadContent](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_LoadContent) method, initialize them:

    ```csharp
    private Viewport viewport;
    private Vector2[] scalingSpritePositions;
    private Texture2D squareTexture;
    private Vector2 spriteOrigin;
    private Matrix spriteScaleMatrix;
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        viewport = _graphics.GraphicsDevice.Viewport;
        squareTexture = CreateTexture(GraphicsDevice, 50, 50);
        spriteOrigin = new Vector2(squareTexture.Width / 2, squareTexture.Height / 2);

        scalingSpritePositions = new Vector2[4];
        scalingSpritePositions[0] = new Vector2(25, 25);
        scalingSpritePositions[1] = new Vector2(25, viewport.Height - 25);
        scalingSpritePositions[2] = new Vector2(viewport.Width - 25, 25);
        scalingSpritePositions[3] = new Vector2(viewport.Width - 25, viewport.Height - 25);

        UpdateScaleMatrix();

        base.LoadContent();
    }
    ```

3. To help make the debug texture `squareTexture` easier, we add a little helper function to just generate the texture rather than import it through the Content Pipeline, as follows:

    ```csharp
    public static Texture2D CreateTexture(GraphicsDevice device, int width, int height)
    {
        // Initialize a texture
        Texture2D texture = new Texture2D(device, width, height);

        // The array holds the color for each pixel in the texture
        Color[] data = new Color[width * height];
        for (int pixel = 0; pixel < data.Length; pixel++)
        {
            data[pixel] = Color.White;
        }

        // Set the color
        texture.SetData(data);
        return texture;
    }
    ```

    > [!NOTE]
    > This is a really handy reusable function to create simple textures at runtime.

4. Next, we create another method to calculate the Scaling Matrix using [Matrix.CreateScale](xref:Microsoft.Xna.Framework.Matrix#Microsoft_Xna_Framework_Matrix_CreateScale_System_Single_) based on the current Graphics Device's resolution, using our original `800x600` as a reference scale.

   This matrix should be recreated any time the resolution of the [GraphicsDevice](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice) changes.

    > [!NOTE]
    > Because you are scaling sprites, you should use only the x and y parameters to create the scaling matrix. Scaling the depth of sprites can result in their depth shifting above 1.0. If that happens, they will not render.

    ```csharp
    private void UpdateScaleMatrix()
    {
        // Default resolution is 800x600; scale sprites up or down based on
        // current viewport
        float screenScale = _graphics.GraphicsDevice.Viewport.Width / 800f;
        // Create the scale transform for Draw. 
        // Do not scale the sprite depth (Z=1).
        spriteScaleMatrix = Matrix.CreateScale(screenScale, screenScale, 1);
    }
    ```

5. To allow you to quickly change the current resolution, in your [Game.Update](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Update_Microsoft_Xna_Framework_GameTime_) method add the following.

   This example uses the `A` and `B` keys to switch between two resolutions.

    ```csharp
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.ApplyChanges();
            UpdateScaleMatrix();
        }
        if (Keyboard.GetState().IsKeyDown(Keys.B))
        {
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.ApplyChanges();
            UpdateScaleMatrix();
        }
        base.Update(gameTime);
    }
    ```

6. In your [Game.Draw](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Draw_Microsoft_Xna_Framework_GameTime_) method, call [SpriteBatch.Begin](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_Begin_Microsoft_Xna_Framework_Graphics_SpriteSortMode_Microsoft_Xna_Framework_Graphics_BlendState_Microsoft_Xna_Framework_Graphics_SamplerState_Microsoft_Xna_Framework_Graphics_DepthStencilState_Microsoft_Xna_Framework_Graphics_RasterizerState_Microsoft_Xna_Framework_Graphics_Effect_System_Nullable_Microsoft_Xna_Framework_Matrix__), passing the scaling matrix created in [Game.LoadContent](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_LoadContent).

7. Finally, draw your scene normally, then call [SpriteBatch.End](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_End).

    All of the sprites you draw will be scaled according to the matrix.

    ```csharp
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Initialize the batch with the scaling matrix
        _spriteBatch.Begin(transformMatrix: spriteScaleMatrix);
        // Draw a sprite at each corner
        for (int i = 0; i < scalingSpritePositions.Length; i++)
        {
            _spriteBatch.Draw(squareTexture, scalingSpritePositions[i], null, Color.White,
                0f, spriteOrigin, 1f, SpriteEffects.None, 0f);
        }
        _spriteBatch.End();

        base.Draw(gameTime);
    }
    ```

## See Also

- [Drawing a Sprite](HowTo_Draw_A_Sprite.md)

### Concepts

- [What Is a Sprite?](../../whatis/graphics/WhatIs_Sprite.md)

### Reference

- [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch)
- [SpriteBatch.Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_Draw_Microsoft_Xna_Framework_Graphics_Texture2D_Microsoft_Xna_Framework_Vector2_Microsoft_Xna_Framework_Color_)
- [Texture2D](xref:Microsoft.Xna.Framework.Graphics.Texture2D)
- [Matrix.CreateScale](xref:Microsoft.Xna.Framework.Matrix#Microsoft_Xna_Framework_Matrix_CreateScale_System_Single_)
