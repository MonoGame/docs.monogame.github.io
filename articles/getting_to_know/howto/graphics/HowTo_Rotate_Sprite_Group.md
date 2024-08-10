---
title: Rotating a Group of Sprites
description: Demonstrates how to rotate a group of sprites around a single point.
requireMSLicense: true
---

## Overview

Sometimes either an individual character is made up of many sprites/textures, or you simply want to arrange a group of sprites in a grid and have them move as one.  The process is simple enough and simply an extension of what is covered in [HowTo Rotate a Sprite](HowTo_Rotate_Sprite.md).

## Drawing a Rotated Group of Sprites

1. Follow the steps of [Drawing a Sprite](HowTo_Draw_A_Sprite.md).

2. Create one array of [Vector2](xref:Microsoft.Xna.Framework.Vector2) variables that represent the un-rotated positions of the sprites and another to hold the rotated values.

    ```csharp
    private Vector2[] myVectors;
    private Vector2[] drawVectors;
    protected override void Initialize()
    {
        myVectors = new Vector2[9];
        drawVectors = new Vector2[9];
    
        base.Initialize();
    }
    ```

3. After loading the sprite, calculate the positions of the un-rotated group of sprites based on the sprite's size.

    ```csharp
    private Texture2D spriteTexture;
    private Vector2 spriteOrigin;
    private Vector2 spritePosition;
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        spriteTexture = Content.Load<Texture2D>("Character");
        viewport = _graphics.GraphicsDevice.Viewport;
        spriteOrigin.X = spriteTexture.Width / 2;
        spriteOrigin.Y = spriteTexture.Height / 2;
        spritePosition.X = viewport.Width / 2;
        spritePosition.Y = viewport.Height / 2;

        // Populate the `myVectors` array with a grid of Vector2 positions for the character to draw.  These will then be rotated around the center of the screen.
        int i = 0;
        while (i < 9)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    myVectors[i] = new Vector2(x * 200, y * 200); // Assign positions
                    i++;
                }
            }
        }
    }
    ```

4. In your [Game.Update](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Update_Microsoft_Xna_Framework_GameTime_) method, copy the un-rotated vectors and determine the screen position around which all the sprites will rotate.

    ```csharp
    private float rotationAngle = 0f;
    private Matrix rotationMatrix = Matrix.Identity;
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // The time since Update was called last.
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // TODO: Add your game logic here.
        rotationAngle += elapsed;
        float circle = MathHelper.Pi * 2;
        rotationAngle %= circle;

        // Copy and rotate the sprite positions.
        drawVectors = (Vector2[])myVectors.Clone();
    
        RotatePoints(ref spritePosition, rotationAngle, ref drawVectors);

        base.Update(gameTime);
    }
    ```

    Transform each vector using a rotation matrix created for the rotation angle.

5. To rotate around the origin, transform each vector relative to the origin by subtracting the origin vector.

6. Add the origin vector to the transformed vector to create the final rotated vector.

    ```csharp
    private static void RotatePoints(ref Vector2 origin, float radians, ref Vector2[] Vectors)
    {
        Matrix myRotationMatrix = Matrix.CreateRotationZ(radians);

        for (int i = 0; i < 9; i++)
        {
            // Rotate relative to origin.
            Vector2 rotatedVector =
                Vector2.Transform(Vectors[i] - origin, myRotationMatrix);

            // Add origin to get final location.
            Vectors[i] = rotatedVector + origin;
        }
    }
    ```

7. Draw each sprite using the rotated vectors as screen locations.

    ```csharp
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        for (int i = 0; i < drawVectors.Length; i++)
        {
            _spriteBatch.Draw(spriteTexture, drawVectors[i], null,
                Color.White, rotationAngle, spriteOrigin, 1.0f,
                SpriteEffects.None, 0f);
        }
        _spriteBatch.End();

        base.Draw(gameTime);
    }
    ```

8. When all the sprites have been drawn, call [SpriteBatch.End](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_End).

    This results in the Grid of characters that are drawn being rotated `Together` around a position in the center of the screen, instead of each sprite rotating individually.

## See Also

- [Drawing a Sprite](HowTo_Draw_A_Sprite.md)  

### Concepts

- [What Is a Sprite?](../../whatis/graphics/WhatIs_Sprite.md)

### Reference

- [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch)
- [SpriteBatch.Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_Draw_Microsoft_Xna_Framework_Graphics_Texture2D_Microsoft_Xna_Framework_Vector2_Microsoft_Xna_Framework_Color_)
- [Texture2D](xref:Microsoft.Xna.Framework.Graphics.Texture2D)
