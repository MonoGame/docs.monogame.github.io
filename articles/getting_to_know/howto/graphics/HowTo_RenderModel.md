---
title: How to render a Model using a Basic Effect
description: Demonstrates how to load and render a model using the MonoGame Content Pipeline.
requireMSLicense: true
---

## Overview

This example has three main parts:

- Importing and processing the model
- Drawing the resultant managed object as a model with full lighting effect in the game
- Enabling movement of the model with a game pad.

## Requirements

> It is assumed that an existing project is loaded in MonoGame. In this example, the project is called "CPModel".

## Adding a Model

Follow the steps in [How to add content](HowTo_GameContent_Add.md) in order to add a Model to your project.

For this example, use the [Fuel Carrier model](https://github.com/MonoGame/MonoGame.Samples/raw/3.8.2/FuelCell/FuelCell.Core/Content/Models/fuelcarrier.fbx) file and its corresponding [diffuse texture](https://github.com/MonoGame/MonoGame.Samples/raw/3.8.2/FuelCell/FuelCell.Core/Content/Models/ShipDiffuse.tga).  But you can use any model file you wish, and set the **Content Importer** to `FBX Importer - MonoGame` and the **Content Processor** is `Model - MonoGame` (which should be the default for an FBX) and Save the solution.

The remaining parts render the model and add some user control of the model. All code modifications for this part occur within the game1.cs file of the game project.

## Rendering the model

1. Open the `game1.cs` file.
2. Modify the **Game1** class by adding the following code at the beginning of the declaration.

    ```csharp
    private Model gameShip;
    ```

    This member holds the ship model.

3. Modify the **LoadContent** method by adding the following code.

    ```csharp
    gameShip = Content.Load<Model>("fuelcarrier");
    ```

    This code loads the model into the `gameShip` member (using [Load](xref:Microsoft.Xna.Framework.Content.ContentManager)).

4. Create a new **private** method (called **DrawModel**) in the **Game1** class by adding the following code before the existing [Game.Draw](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Draw_Microsoft_Xna_Framework_GameTime_) method.

    ```csharp
    private void DrawModel(Model m)
    {
        Matrix[] transforms = new Matrix[m.Bones.Count];
        float aspectRatio = GraphicsDevice.Viewport.AspectRatio;
        m.CopyAbsoluteBoneTransformsTo(transforms);
        Matrix projection =
            Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
            aspectRatio, 1.0f, 10000.0f);
        Matrix view = Matrix.CreateLookAt(new Vector3(0.0f, 50.0f, zoom),
            Vector3.Zero, Vector3.Up);

        foreach (ModelMesh mesh in m.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.EnableDefaultLighting();

                effect.View = view;
                effect.Projection = projection;
                effect.World = gameWorldRotation *
                    transforms[mesh.ParentBone.Index] *
                    Matrix.CreateTranslation(position);
            }
            mesh.Draw();
        }
    }
    ```

    This code sets up the lighting effects for each sub-mesh of the model. The `gameWorldRotation` and `Zoom` variables are used for player control. This functionality is added later.

    > This render code is designed for only those models with a [BasicEffect](xref:Microsoft.Xna.Framework.Graphics.BasicEffect) (set in the `MGCB` editor as the `defaultEffect` property for the model). For custom effects, the inner `for-each` loop should be changed to use the [Effect](xref:Microsoft.Xna.Framework.Graphics.Effect) class instead of the [BasicEffect](xref:Microsoft.Xna.Framework.Graphics.BasicEffect) class. In addition, you must use [EffectParameter](xref:Microsoft.Xna.Framework.Graphics.EffectParameter) objects to manually set the world, view, and projection matrices.

5. Modify the **Game1.Draw** method by replacing the following code `**// TODO: Add your drawing code here` with the following code:

    ```csharp
    DrawModel(gameShip);
    ```

    This initializes the model's effects before the model is rendered.

6. Save the solution.

At this point, the rendering code for the model is complete, but the user control code still needs implementation.

> [!IMPORTANT]
> The project will not run as this point as there is more to add.

## Moving the model

To begin moving the model, we need to add some controls and User input.

1. Modify the **Game1** class by adding the following code after the `gameShip` declaration.

    ```csharp
    private Vector3 position = Vector3.One;
    private float zoom = 2500;
    private float rotationY = 0.0f;
    private float rotationX = 0.0f;
    private Matrix gameWorldRotation;
    float speed = 10f;
    ```

    These members store the current position, zoom, and rotation values. In addition, the `gameWorldRotation` is updated by the `UpdateGamePad` method below.

2. Add a private method (called **UpdateGamePad**) before the call to [Game.Update](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Update_Microsoft_Xna_Framework_GameTime_).

    ```csharp
    private void UpdateGamePad()
    {
        GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
        KeyboardState keyState = Keyboard.GetState();

        // Gamepad controls
        position.X += gamePadState.ThumbSticks.Left.X * speed;
        position.Y += gamePadState.ThumbSticks.Left.Y * speed;
        zoom += gamePadState.ThumbSticks.Right.Y * speed;
        rotationY += gamePadState.ThumbSticks.Right.X * speed;
        if (gamePadState.Buttons.RightShoulder == ButtonState.Pressed)
        {
            rotationX += 1.0f * speed;
        }
        else if (gamePadState.Buttons.LeftShoulder == ButtonState.Pressed)
        {
            rotationX -= 1.0f * speed;
        }

        // Keyboard controls
        if (keyState.IsKeyDown(Keys.A)) { position.X += 1.0f * speed; }
        else if (keyState.IsKeyDown(Keys.D)) { position.X -= 1.0f * speed; }
        if (keyState.IsKeyDown(Keys.W)) { zoom += 1.0f * speed; }
        else if (keyState.IsKeyDown(Keys.S)) { zoom -= 1.0f * speed; }
        if (keyState.IsKeyDown(Keys.E)) { rotationY += 1.0f * speed; }
        else if (keyState.IsKeyDown(Keys.Q)) { rotationY -= 1.0f * speed; }

        if (keyState.IsKeyDown(Keys.Right)) { position.Y += 1.0f * speed; }
        else if (keyState.IsKeyDown(Keys.Left)) { position.Y -= 1.0f * speed; }
        if (keyState.IsKeyDown(Keys.Up)) { rotationX += 1.0f * speed; }
        else if (keyState.IsKeyDown(Keys.Down)) { rotationX -= 1.0f * speed; }

        gameWorldRotation =
            Matrix.CreateRotationX(MathHelper.ToRadians(rotationX)) *
            Matrix.CreateRotationY(MathHelper.ToRadians(rotationY));
    }
    ```

    This code implements an exit method for the game (pressing the **A** button on a GamePad), and updates the position members with the current input of the game controller.

3. Modify the **Update** method by adding a call to `UpdateGamePad`, before the call to [Game.Update](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Update_Microsoft_Xna_Framework_GameTime_).

    ```csharp
    UpdateGamePad();
    ```

    This code updates the state of the position variables with the latest input.

4. Save the solution.

Development is complete so you are ready to build and run the game. Control the ship location with the game pad, and exit by pressing the **A** button.

## See Also

- [Adding Content to a Game](../content_pipeline/HowTo_GameContent_Add.md)
- [Using Input](../input/index.md)

### Concepts

- [What Is a Configurable Effect?](../../whatis/graphics/WhatIs_3DRendering.md)
- [What Is a Configurable Effect?](../../whatis/graphics/WhatIs_ConfigurableEffect.md)

### Reference

- [BasicEffect](xref:Microsoft.Xna.Framework.Graphics.BasicEffect)
- [Matrix](xref:Microsoft.Xna.Framework.Matrix)
- [GamePadState](xref:Microsoft.Xna.Framework.Input.GamePadState)
- [KeyboardState](xref:Microsoft.Xna.Framework.Input.KeyboardState)
