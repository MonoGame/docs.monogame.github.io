---
title: Tinting a Sprite
description: Demonstrates how to tint a sprite using a Color value.
requireMSLicense: true
---

## Overview

Tinting sprites is an easy way to either animate a sprite (when it takes damage) or even to create different characters of different colors.  It is quick and efficient to do and all you need is the color to tine with and a single change to your `SpriteBatch` draw call.

## Drawing a Tinted Sprite

1. Follow the procedures of [Drawing a Sprite](HowTo_Draw_A_Sprite.md).
2. In the [Game.Update](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Update_Microsoft_Xna_Framework_GameTime_) method, determine the color to tint the sprite.

   In this example, the position of the mouse determines the Red, Green, values to apply to the sprite, the blue is fixed for simplicity.

    ```csharp
    protected Color tint;
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        MouseState mouse = Mouse.GetState();
        tint = new Color(mouse.X % 255, mouse.Y % 255, 255);

        base.Update(gameTime);
    }
    ```

3. In the [Game.Draw](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Draw_Microsoft_Xna_Framework_GameTime_) method, pass the color value created in [Game.Update](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Update_Microsoft_Xna_Framework_GameTime_) to [SpriteBatch.Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_Draw_Microsoft_Xna_Framework_Graphics_Texture2D_Microsoft_Xna_Framework_Vector2_Microsoft_Xna_Framework_Color_).

    ```csharp
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        _spriteBatch.Draw(spriteTexture, spritePosition, tint);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
    ```

4. When all of the sprites have been drawn, call [SpriteBatch.End](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_End) on your [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) object.

    Moving the mouse around the screen will change the color of the Tint that is applied to the sprite / texture each frame.  Alternatively, try switching the input to something else, or for a challenge, animate the tint to make it look like the character is taking damage.

## See Also

- [Drawing a Sprite](HowTo_Draw_A_Sprite.md)

### Concepts

- [What Is a Sprite?](../../whatis/graphics/WhatIs_Sprite.md)

### Reference

- [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch)
- [SpriteBatch.Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_Draw_Microsoft_Xna_Framework_Graphics_Texture2D_Microsoft_Xna_Framework_Vector2_Microsoft_Xna_Framework_Color_)
- [Texture2D](xref:Microsoft.Xna.Framework.Graphics.Texture2D)
- [Color](xref:Microsoft.Xna.Framework.Color)

(Character by `upklyak` from [FreePik](https://www.freepik.com/free-vector/cartoon-alien-character-animation-sprite-sheet_33397951.htm))
