---
title: Tinting a Sprite
description: Demonstrates how to tint a sprite using a Color value.
---

# Tinting a Sprite

Demonstrates how to tint a sprite using a [Color](xref:Microsoft.Xna.Framework.Color) value.

## Drawing a Tinted Sprite

1. Follow the procedures of [Drawing a Sprite](HowTo_Draw_A_Sprite.md).
2. In the [Game.Update](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Update_Microsoft_Xna_Framework_GameTime_) method, determine how to tint the sprite.

   In this example, the value of the game pad thumbsticks determine the Red, Green, Blue, and Alpha values to apply to the sprite.

    ```csharp
    protected Color tint;
    protected override void Update(GameTime gameTime)
    {
        ...
        GamePadState input = GamePad.GetState(PlayerIndex.One);
        tint = new Color(GetColor(input.ThumbSticks.Left.X),
            GetColor(input.ThumbSticks.Left.Y),
            GetColor(input.ThumbSticks.Right.X),
            GetColor(input.ThumbSticks.Right.Y));
    
        base.Update(gameTime);
    }
    ```

3. In the [Game.Draw](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Draw_Microsoft_Xna_Framework_GameTime_) method, pass the color value created in [Game.Update](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Update_Microsoft_Xna_Framework_GameTime_) to [SpriteBatch.Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_Draw_Microsoft_Xna_Framework_Graphics_Texture2D_Microsoft_Xna_Framework_Vector2_Microsoft_Xna_Framework_Color_).

    ```csharp
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
    
        spriteBatch.Begin();
        spriteBatch.Draw(SpriteTexture, position, tint);
        spriteBatch.End();
    
        base.Draw(gameTime);
    }
    ```

4. When all of the sprites have been drawn, call [SpriteBatch.End](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_End) on your [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) object.

## See Also

### Tasks

[Drawing a Sprite](HowTo_Draw_A_Sprite.md)

#### Concepts

[What Is a Sprite?](../../whatis/graphics/WhatIs_Sprite.md)

#### Reference

[SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch)
[SpriteBatch.Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_Draw_Microsoft_Xna_Framework_Graphics_Texture2D_Microsoft_Xna_Framework_Vector2_Microsoft_Xna_Framework_Color_)[Texture2D](xref:Microsoft.Xna.Framework.Graphics.Texture2D)
[Color](xref:Microsoft.Xna.Framework.Color)

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
