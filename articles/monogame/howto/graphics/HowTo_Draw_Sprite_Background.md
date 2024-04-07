---
title: Drawing a Masked Sprite over a Background
description: Demonstrates how to draw a foreground and background sprite using the SpriteBatch class, where only part of the foreground sprite masks the background.
---

# Drawing a Masked Sprite over a Background

Demonstrates how to draw a foreground and background sprite using the [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) class, where only part of the foreground sprite masks the background.

The foreground sprite in this example must include masking information.

## Drawing a Foreground and Background Sprite

1. Open the `Game1` class, and load your content as described in the procedures of [Drawing a Sprite](HowTo_Draw_A_Sprite.md).

    ```csharp
    private Vector2 ViperPos;  // Position of foreground sprite on screen
    private Viewport viewport;
    SpriteBatch spriteBatch;
    Texture2D ShipTexture;
    Texture2D StarTexture;
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);
    
        StarTexture = Content.Load<Texture2D>("starfield");
        ShipTexture = Content.Load<Texture2D>("ship");
        viewport = graphics.GraphicsDevice.Viewport;
    
        ViperPos.X = viewport.Width / 2;
        ViperPos.Y = viewport.Height - 100;
    }
    ```

2. In [Game.Draw](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Draw_Microsoft_Xna_Framework_GameTime_) method of your game class, call [SpriteBatch.Begin](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_Begin_Microsoft_Xna_Framework_Graphics_SpriteSortMode_Microsoft_Xna_Framework_Graphics_BlendState_Microsoft_Xna_Framework_Graphics_SamplerState_Microsoft_Xna_Framework_Graphics_DepthStencilState_Microsoft_Xna_Framework_Graphics_RasterizerState_Microsoft_Xna_Framework_Graphics_Effect_System_Nullable_Microsoft_Xna_Framework_Matrix__) for the [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch).

3. Specify [BlendState.None](xref:Microsoft.Xna.Framework.Graphics.BlendState).
   This will tell the [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) to ignore alpha color values when drawing sprites. By default, the z-order of sprites is the order in which they are drawn.

4. Call the [Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) method, passing in the `StarTexture`.  Then call [SpriteBatch.End](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_End).

    ```csharp
    public override void Draw (GameTime game)
    {
        spriteBatch.Begin(BlendState.None);
        spriteBatch.Draw (StarTexture);
        spriteBatch.End();
    }
    ```

5. Underneath the code we wrote previously, call [SpriteBatch.Begin](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_Begin_Microsoft_Xna_Framework_Graphics_SpriteSortMode_Microsoft_Xna_Framework_Graphics_BlendState_Microsoft_Xna_Framework_Graphics_SamplerState_Microsoft_Xna_Framework_Graphics_DepthStencilState_Microsoft_Xna_Framework_Graphics_RasterizerState_Microsoft_Xna_Framework_Graphics_Effect_System_Nullable_Microsoft_Xna_Framework_Matrix__) for the [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) again.

6. This time, specify [BlendState.AlphaBlend](xref:Microsoft.Xna.Framework.Graphics.BlendState).

    This will cause pixels on the sprite with an alpha value less than 255 to become progressively transparent based on the magnitude of the alpha value. An alpha of 0 will make the pixel completely transparent. Calling [SpriteBatch.Begin](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_Begin_Microsoft_Xna_Framework_Graphics_SpriteSortMode_Microsoft_Xna_Framework_Graphics_BlendState_Microsoft_Xna_Framework_Graphics_SamplerState_Microsoft_Xna_Framework_Graphics_DepthStencilState_Microsoft_Xna_Framework_Graphics_RasterizerState_Microsoft_Xna_Framework_Graphics_Effect_System_Nullable_Microsoft_Xna_Framework_Matrix__) with no parameters causes [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) to default to [BlendState.AlphaBlend](xref:Microsoft.Xna.Framework.Graphics.BlendState).

7. Call the [Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) method, passing the `ShipTexture`, `ViperPos` and finally [Color.White](xref:Microsoft.Xna.Framework.Color) . Then call [SpriteBatch.End](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_End).

    ```csharp
    public override void Draw (GameTime game)
    {
        spriteBatch.Begin(BlendState.None);
        spriteBatch.Draw (StarTexture);
        spriteBatch.End();
        
        spriteBatch.Begin(BlendState.AlphaBlend);
        spriteBatch.Draw (ShipTexture, ViperPos, Color.White);
        spriteBatch.End();
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
