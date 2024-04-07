---
title: Making a Scrolling Background
description: Demonstrates how to draw a scrolling background sprite using the SpriteBatch class
---

# Making a Scrolling Background

Demonstrates how to draw a scrolling background sprite using the [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) class.

## Drawing a Scrolling Background Sprite

1. Create the game class.

2. Load resources as described in the procedures of [Drawing a Sprite](HowTo_Draw_A_Sprite.md).

3. Load the background texture.

    ```csharp
    private ScrollingBackground myBackground;
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);
        myBackground = new ScrollingBackground();
        Texture2D background = Content.Load<Texture2D>("starfield");
        myBackground.Load(GraphicsDevice, background);
    }
    ```

4. Determine the size of the background texture and the size of the screen.

   The texture size is determined using the [Height](xref:Microsoft.Xna.Framework.Graphics.Texture2D.Height) and [Width](xref:Microsoft.Xna.Framework.Graphics.Texture2D.Width) properties, and the screen size is determined using the [Viewport](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice.Viewport) property on the graphics device.

5. Using the texture and screen information, set the origin of the texture to the center of the top edge of the texture, and the initial screen position to the center of the screen.

    ```csharp
    // class ScrollingBackground
    private Vector2 screenpos, origin, texturesize;
    private Texture2D mytexture;
    private int screenheight;
    public void Load( GraphicsDevice device, Texture2D backgroundTexture )
    {
        mytexture = backgroundTexture;
        screenheight = device.Viewport.Height;
        int screenwidth = device.Viewport.Width;
        // Set the origin so that we're drawing from the 
        // center of the top edge.
        origin = new Vector2( mytexture.Width / 2, 0 );
        // Set the screen position to the center of the screen.
        screenpos = new Vector2( screenwidth / 2, screenheight / 2 );
        // Offset to draw the second texture, when necessary.
        texturesize = new Vector2( 0, mytexture.Height );
    }
    ```

6. To scroll the background, change the screen position of the background texture in your [Game.Update](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Update_Microsoft_Xna_Framework_GameTime_) method.

   This example moves the background down 100 pixels per second by increasing the screen position's Y value.

    ```csharp
    protected override void Update(GameTime gameTime)
    {
        ...
        // The time since Update was called last.
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
    
        // TODO: Add your game logic here.
        myBackground.Update(elapsed * 100);
    
        base.Update(gameTime);
    }
    ```

    The Y value is kept no larger than the texture height, making the background scroll from the bottom of the screen back to the top.

    ```csharp
    public void Update( float deltaY )
    {
        screenpos.Y += deltaY;
        screenpos.Y = screenpos.Y % mytexture.Height;
    }
    // ScrollingBackground.Draw
    ```

7. Draw the background using the origin and screen position calculated in [Game.LoadContent](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_LoadContent) and [Game.Update](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Update_Microsoft_Xna_Framework_GameTime_).

    ```csharp
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
    
        spriteBatch.Begin();
        myBackground.Draw(spriteBatch);
        spriteBatch.End();
    
        base.Draw(gameTime);
    }
    ```

    In case the texture doesn't cover the screen, another texture is drawn. This subtracts the texture height from the screen position using the **texturesize** vector created at load time. This creates the illusion of a loop.

    ```csharp
    public void Draw( SpriteBatch batch )
    {
        // Draw the texture, if it is still onscreen.
        if (screenpos.Y < screenheight)
        {
            batch.Draw( mytexture, screenpos, null,
                 Color.White, 0, origin, 1, SpriteEffects.None, 0f );
        }
        // Draw the texture a second time, behind the first,
        // to create the scrolling illusion.
        batch.Draw( mytexture, screenpos - texturesize, null,
             Color.White, 0, origin, 1, SpriteEffects.None, 0f );
    }
    ```

## See Also

### Tasks

[Drawing a Sprite](HowTo_Draw_A_Sprite.md)  
[Drawing a Masked Sprite over a Background](HowTo_Draw_Sprite_Background.md)  

#### Concepts

[What Is a Sprite?](../../whatis/graphics/WhatIs_Sprite.md)

#### Reference

[SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch)  
[SpriteBatch.Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_Draw_Microsoft_Xna_Framework_Graphics_Texture2D_Microsoft_Xna_Framework_Vector2_Microsoft_Xna_Framework_Color_)
[Texture2D](xref:Microsoft.Xna.Framework.Graphics.Texture2D)  

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
