---
title: Animating a Sprite
description: Demonstrates how to animate a sprite from a texture using a custom class.
---

# Animating a Sprite

Demonstrates how to animate a sprite from a texture using a custom class.

The example assumes the texture you are loading contains four frames of the same size in a texture whose size is 256×64 pixels, and uses a class named **AnimatedTexture**, which is included with the sample.

## Drawing an Animated Sprite

1. In your game's constructor, create an instance of the **AnimatedTexture** class.
   This example uses (0,0) as the origin of the texture, no rotation, a scale of 2, and a depth of 0.5.

    ```csharp
    private AnimatedTexture SpriteTexture;
    private const float Rotation = 0;
    private const float Scale = 2.0f;
    private const float Depth = 0.5f;
    public Game1()
    {
        ...
        SpriteTexture = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
    
        // Set device frame rate to 30 fps.
        TargetElapsedTime = TimeSpan.FromSeconds(1 / 30.0);
    }
    ```

2. Load one or more textures to provide the image data for the animation. 
   In this example, the **AnimatedTexture** class loads a single texture and divides it into frames of animation. It uses the last parameter to determine how many frames to draw each second. In this case, it draws four frames at two frames per second (fps).

    ```csharp
    private Viewport viewport;
    private Vector2 shipPos;
    private const int Frames = 4;
    private const int FramesPerSec = 2;
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);
    
        // "shipanimated" is the name of the sprite asset in the project.
        SpriteTexture.Load(Content, "shipanimated", Frames, FramesPerSec);
        viewport = graphics.GraphicsDevice.Viewport;
        shipPos = new Vector2(viewport.Width / 2, viewport.Height / 2);
    }
    ```

3. In your game's [Game.Update](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Update_Microsoft_Xna_Framework_GameTime_) method, determine which animation frame to display.

    ```csharp
    protected override void Update(GameTime gameTime)
    {
        ...
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
    
        // TODO: Add your game logic here.
        SpriteTexture.UpdateFrame(elapsed);
        base.Update(gameTime);
    }
    ```

    This is handled by AnimatedTexture's **UpdateFrame** method, which takes the elapsed seconds between updates as a parameter.

    ```csharp
    // class AnimatedTexture
    public void UpdateFrame(float elapsed)
    {
        if (Paused)
            return;
        TotalElapsed += elapsed;
        if (TotalElapsed > TimePerFrame)
        {
            Frame++;
            // Keep the Frame between 0 and the total frames, minus one.
            Frame = Frame % framecount;
            TotalElapsed -= TimePerFrame;
        }
    }
    ```

4. In your game's [Game.Draw](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Draw_Microsoft_Xna_Framework_GameTime_) method, call [SpriteBatch.Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_Draw_Microsoft_Xna_Framework_Graphics_Texture2D_Microsoft_Xna_Framework_Vector2_Microsoft_Xna_Framework_Color_) on the **AnimatedTexture** object.

    ```csharp
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
    
        // TODO: Add your drawing code here
        spriteBatch.Begin();
        SpriteTexture.DrawFrame(spriteBatch, shipPos);
        spriteBatch.End();
    
        base.Draw(gameTime);
    }
    ```

    **AnimatedTexture** draws the sprite using the subrectangle of the texture that contains the desired animation.

    ```csharp
    // class AnimatedTexture
    public void DrawFrame(SpriteBatch batch, Vector2 screenPos)
    {
        DrawFrame(batch, Frame, screenPos);
    }
    public void DrawFrame(SpriteBatch batch, int frame, Vector2 screenPos)
    {
        int FrameWidth = myTexture.Width / framecount;
        Rectangle sourcerect = new Rectangle(FrameWidth * frame, 0,
            FrameWidth, myTexture.Height);
        batch.Draw(myTexture, screenPos, sourcerect, Color.White,
            Rotation, Origin, Scale, SpriteEffects.None, Depth);
    }
    ```

## See Also

### Tasks

[Drawing a Sprite](HowTo_Draw_A_Sprite.md)

#### Concepts

[What Is a Sprite?](../../whatis/graphics/WhatIs_Sprite.md)

#### Reference

[SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch)
[SpriteBatch.Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_Draw_Microsoft_Xna_Framework_Graphics_Texture2D_Microsoft_Xna_Framework_Vector2_Microsoft_Xna_Framework_Color_)
[Texture2D](xref:Microsoft.Xna.Framework.Graphics.Texture2D)

---

© 2012 Microsoft Corporation. All rights reserved.

© 2023 The MonoGame Foundation.
