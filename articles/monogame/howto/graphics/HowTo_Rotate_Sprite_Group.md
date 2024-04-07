---
title: Rotating a Group of Sprites
description: Demonstrates how to rotate a group of sprites around a single point.
---

# Rotating a Group of Sprites

Demonstrates how to rotate a group of sprites around a single point.

## Drawing a Rotated Group of Sprites

1. Follow the steps of [Drawing a Sprite](HowTo_Draw_A_Sprite.md).

2. Create one set of [Vector2](xref:Microsoft.Xna.Framework.Vector2) objects that represents the unrotated positions of the sprites and one set to hold the rotated values.

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

3. After loading the sprite, calculate the positions of the unrotated group of sprites based on the sprite's size.

    ```csharp
    private Texture2D SpriteTexture;
    private Vector2 origin;
    private Vector2 screenpos;
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);
    
        SpriteTexture = Content.Load<Texture2D>("ship");
        origin.X = SpriteTexture.Width / 2;
        origin.Y = SpriteTexture.Height / 2;
        Viewport viewport = graphics.GraphicsDevice.Viewport;
        screenpos.X = viewport.Width / 2;
        screenpos.Y = viewport.Height / 2;
    }
    ```

4. In your [Game.Update](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Update_Microsoft_Xna_Framework_GameTime_) method, copy the unrotated vectors and determine the screen position around which all the sprites will rotate.

    ```csharp
    private float RotationAngle = 0f;
    private Matrix rotationMatrix = Matrix.Identity;
    protected override void Update(GameTime gameTime)
    {
        ...
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
    
        RotationAngle += elapsed;
        float circle = MathHelper.Pi * 2;
        RotationAngle = RotationAngle % circle;
    
        // Copy and rotate the sprite positions.
        drawVectors = (Vector2[])myVectors.Clone();
    
        RotatePoints(ref screenpos, RotationAngle, ref drawVectors);
    
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
    private void DrawPoints()
    {
        // Draw using manually rotated vectors
        spriteBatch.Begin();
        for (int i = 0; i < drawVectors.Length; i++)
            spriteBatch.Draw(SpriteTexture, drawVectors[i], null,
                Color.White, RotationAngle, origin, 1.0f,
                SpriteEffects.None, 0f);
        spriteBatch.End();
    }
    ```

8. When all the sprites have been drawn, call [SpriteBatch.End](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_End).

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
