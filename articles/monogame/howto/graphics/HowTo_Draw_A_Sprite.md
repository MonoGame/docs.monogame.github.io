---
title: Drawing a Sprite
description: Demonstrates how to draw a sprite by using the SpriteBatch class
---

# Drawing a Sprite

Demonstrates how to draw a sprite by using the [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) class.

## To draw a sprite on screen

1. Derive a class from [Game](xref:Microsoft.Xna.Framework.Game).
2. Define a [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) object as a field on your game class.
3. In your [Game.LoadContent](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_LoadContent) method, construct the [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) object, passing the current graphics device.
4. Load the textures that will be used for drawing sprites in [Game.LoadContent](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_LoadContent).
    In this case, the example uses the **Content** member to load a texture from the MonoGame Framework Content Pipeline. The texture must be in the project, with the same name passed to [ContentManager.Load](xref:Microsoft.Xna.Framework.Content.ContentManager#Microsoft_Xna_Framework_Content_ContentManager_Load__1_System_String_).

    ```csharp
    private Texture2D SpriteTexture;
    private Rectangle TitleSafe;
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);
        SpriteTexture = Content.Load<Texture2D>("ship");
        TitleSafe = GetTitleSafeArea(.8f);
    }
    ```

5. In the overridden [Game.Draw](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Draw_Microsoft_Xna_Framework_GameTime_) method, call [GraphicsDevice.Clear](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice#Microsoft_Xna_Framework_Graphics_GraphicsDevice_Clear_Microsoft_Xna_Framework_Color_).
6. After [GraphicsDevice.Clear](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice#Microsoft_Xna_Framework_Graphics_GraphicsDevice_Clear_Microsoft_Xna_Framework_Color_), call [SpriteBatch.Begin](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_Begin_Microsoft_Xna_Framework_Graphics_SpriteSortMode_Microsoft_Xna_Framework_Graphics_BlendState_Microsoft_Xna_Framework_Graphics_SamplerState_Microsoft_Xna_Framework_Graphics_DepthStencilState_Microsoft_Xna_Framework_Graphics_RasterizerState_Microsoft_Xna_Framework_Graphics_Effect_System_Nullable_Microsoft_Xna_Framework_Matrix__) on your [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) object.
7. Create a [Vector2](xref:Microsoft.Xna.Framework.Vector2) to represent the screen position of the sprite.
8. Call [SpriteBatch.Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_Draw_Microsoft_Xna_Framework_Graphics_Texture2D_Microsoft_Xna_Framework_Vector2_Microsoft_Xna_Framework_Color_) on your [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) object, passing the texture to draw, the screen position, and the color to apply.
9. Use [Color.White](xref:Microsoft.Xna.Framework.Color) to draw the texture without any color effects.
10. When all the sprites have been drawn, call [SpriteBatch.End](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_End) on your [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) object.

    ```csharp
    protected override void Draw(GameTime gameTime)
    {
        graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
        spriteBatch.Begin();
        Vector2 pos = new Vector2(TitleSafe.Left, TitleSafe.Top);
        spriteBatch.Draw(SpriteTexture, pos, Color.White);
        spriteBatch.End();
        base.Draw(gameTime);
    }
    ```

## See Also

### Concepts

[What Is a Sprite?](../../whatis/graphics/WhatIs_Sprite.md)

### Reference

[SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch)  
[SpriteBatch.Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_Draw_Microsoft_Xna_Framework_Graphics_Texture2D_Microsoft_Xna_Framework_Vector2_Microsoft_Xna_Framework_Color_)
[Texture2D](xref:Microsoft.Xna.Framework.Graphics.Texture2D)  

---

© 2012 Microsoft Corporation. All rights reserved.

© 2023 The MonoGame Foundation.
