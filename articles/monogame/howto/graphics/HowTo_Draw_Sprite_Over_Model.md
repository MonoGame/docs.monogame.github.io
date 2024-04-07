---
title: Drawing a Sprite Over a Model
description: Demonstrates how to draw a sprite so that it obscures a model. In this example, we are drawing an animated sprite representing an explosion over the current screen position of a 3D model.
---

# Drawing a Sprite Over a Model

Demonstrates how to draw a sprite so that it obscures a model. In this example, we are drawing an animated sprite representing an explosion over the current screen position of a 3D model.

For this sample, the camera is a standard arc ball camera, implemented by camera.cs. The 3D model file is a simple ring, implemented by ring16b.x. The animated explosion sprite is implemented by explosion.dds. These files can be found in the complete sample. See [Animating a Sprite](HowTo_Animate_Sprite.md) for an example of the **AnimatedTexture** class.

## Drawing a Sprite Over a 3D Model

1. In your [Game.Update](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Update_Microsoft_Xna_Framework_GameTime_) method, handle the input to move your camera, then call **UpdateFrame** on the **AnimatedTexture**.

     ```csharp
     GamePadState PlayerOne = GamePad.GetState(PlayerIndex.One);
    
    // Move the camera using thumbsticks
    MoveCamera(PlayerOne);
    
    // Start or stop the animated sprite using buttons
    if (PlayerOne.Buttons.A == ButtonState.Pressed)
        explosion.Play();
    if (PlayerOne.Buttons.B == ButtonState.Pressed)
        explosion.Stop();
    
    // Update the animated sprite
    explosion.UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
    ```

2. Use [BoundingSphere.CreateMerged](xref:Microsoft.Xna.Framework.BoundingSphere#Microsoft_Xna_Framework_BoundingSphere_CreateMerged_Microsoft_Xna_Framework_BoundingSphere_Microsoft_Xna_Framework_BoundingSphere_) to create a [BoundingSphere](xref:Microsoft.Xna.Framework.BoundingSphere) that contains all the **BoundingSphere** values for each [ModelMesh](xref:Microsoft.Xna.Framework.Graphics.ModelMesh) in the [Model](xref:Microsoft.Xna.Framework.Graphics.Model).

3. Use [Viewport.Project](xref:Microsoft.Xna.Framework.Graphics.Viewport#Microsoft_Xna_Framework_Graphics_Viewport_Project_Microsoft_Xna_Framework_Vector3_Microsoft_Xna_Framework_Matrix_Microsoft_Xna_Framework_Matrix_Microsoft_Xna_Framework_Matrix_) to find the center point of that sphere, which is the center of the model in screen coordinates.

    ```csharp
    // Create a total bounding sphere for the mesh
    BoundingSphere totalbounds = new BoundingSphere();
    foreach (ModelMesh mesh in Ring.Meshes)
    {
        totalbounds = BoundingSphere.CreateMerged(totalbounds,
            mesh.BoundingSphere);
    }
    
    // Project the center of the 3D object to the screen, and center the
    // sprite there
    Vector3 center = GraphicsDevice.Viewport.Project(totalbounds.Center,
        projectionMatrix, Camera1.ViewMatrix, Matrix.Identity);
    explosionpos.X = center.X;
    explosionpos.Y = center.Y;
    ```

4. Take the **BoundingSphere** for the model and use it to create a [BoundingBox](xref:Microsoft.Xna.Framework.BoundingBox) with [BoundingBox.CreateFromSphere](xref:Microsoft.Xna.Framework.BoundingBox#Microsoft_Xna_Framework_BoundingBox_CreateFromSphere_Microsoft_Xna_Framework_BoundingSphere_).

5. Use [Viewport.Project](xref:Microsoft.Xna.Framework.Graphics.Viewport#Microsoft_Xna_Framework_Graphics_Viewport_Project_Microsoft_Xna_Framework_Vector3_Microsoft_Xna_Framework_Matrix_Microsoft_Xna_Framework_Matrix_Microsoft_Xna_Framework_Matrix_) to find the corner of the box farthest from the center and use the return value to scale the sprite appropriately.

    ```csharp
    // Create a bounding box from the bounding sphere, 
    // and find the corner that is farthest away from 
    // the center using Project
    BoundingBox extents = BoundingBox.CreateFromSphere(totalbounds);
    float maxdistance = 0;
    float distance;
    Vector3 screencorner;
    foreach (Vector3 corner in extents.GetCorners())
    {
        screencorner = GraphicsDevice.Viewport.Project(corner,
        projectionMatrix, Camera1.ViewMatrix, Matrix.Identity);
        distance = Vector3.Distance(screencorner, center);
        if (distance > maxdistance)
            maxdistance = distance;
    }
    
    // Scale the sprite using the two points (the sprite is 
    // 75 pixels square)
    explosion.Scale = maxdistance / 75;
    ```

6. In your [Game.Draw](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Draw_Microsoft_Xna_Framework_GameTime_) method, draw the [Model](xref:Microsoft.Xna.Framework.Graphics.Model) normally, and then draw the animated sprite using the position calculated in [Game.Update](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Update_Microsoft_Xna_Framework_GameTime_).

    ```csharp
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
    
        //Draw the model, a model can have multiple meshes, so loop
        foreach (ModelMesh mesh in Ring.Meshes)
        {
            //This is where the mesh orientation is set, as well as 
            //our camera and projection
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.EnableDefaultLighting();
                effect.World = Matrix.Identity *
                    Matrix.CreateRotationY(RingRotation) *
                    Matrix.CreateTranslation(RingPosition);
                effect.View = Camera1.ViewMatrix;
                effect.Projection = projectionMatrix;
            }
            //Draw the mesh, will use the effects set above.
            mesh.Draw();
        }
    
        // Draw the sprite over the 3D object
        //            spriteBatch.Begin(SpriteBlendMode.AlphaBlend,
        //                SpriteSortMode.Deferred, SaveStateMode.SaveState);
        spriteBatch.Begin();
        explosion.DrawFrame(spriteBatch, explosionpos);
        spriteBatch.End();
    
        base.Draw(gameTime);
    }
    ```

## See Also

### Concepts

[What Is a Sprite?](../../whatis/graphics/WhatIs_Sprite.md)
[What Is Color Blending?](../../whatis/graphics/WhatIs_ColorBlending.md)

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
