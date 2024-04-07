---
title: Creating a Custom Effect with Texturing
description: Demonstrates how to use a custom effect and a texture to render a 3D object.
---

# Creating a Custom Effect with Texturing

Demonstrates how to use a custom effect and a texture to render a 3D object.

## Using a custom effect and a texture

### To use a custom effect and a texture

1. Create a vertex declaration that contains a position and a texture coordinate.

2. Create a vertex buffer from the vertex declaration.

    ``` csharp
    vertexDeclaration = new VertexDeclaration(new VertexElement[]
        {
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
        }
    );
    
    vertexBuffer = new VertexBuffer(
        graphics.GraphicsDevice,
        vertexDeclaration,
        number_of_vertices,
        BufferUsage.None
        );
    ```

3. Create a custom effect using the [Effect](xref:Microsoft.Xna.Framework.Graphics.Effect) class.

4. Load the [Effect](xref:Microsoft.Xna.Framework.Graphics.Effect) object using the [ContentManager.Load\<Effect\>](xref:Microsoft.Xna.Framework.Content.ContentManager) method to load the .fx file.

5. Load a [Texture2D](xref:Microsoft.Xna.Framework.Graphics.Texture2D) object using the [ContentManager.Load\<Texture2D\>](xref:Microsoft.Xna.Framework.Content.ContentManager)) method to load the asset.

6. Call [SetValue](xref:Microsoft.Xna.Framework.Graphics.Effect) to initialize each effect parameter using the corresponding game property.

7. Initialize the [CurrentTechnique](xref:Microsoft.Xna.Framework.Graphics.Effect) to a technique that exists in the .fx file.

    ``` csharp
    effect = Content.Load<Effect>("ReallySimpleTexture");
    
    Texture2D texture = Content.Load<Texture2D>("XNA_pow2");
    
    effect.Parameters["WorldViewProj"].SetValue(worldViewProjection);
    effect.Parameters["UserTexture"].SetValue(texture);
    
    effect.CurrentTechnique = effect.Techniques["TransformAndTexture"];
    ```

8. Render the effect.

9. Set the [RasterizerState](xref:Microsoft.Xna.Framework.Graphics.RasterizerState) property to turn culling off so that all primitives will be drawn regardless of the order of the vertices.

10. Loop through each pass in the effect calling [DrawPrimitives](https://msdn.microsoft.com/en-us/library/m:microsoft.xna.framework.graphics.graphicsdevice.drawprimitives\(microsoft.xna.framework.graphics.primitivetype%2csystem.int32%2csystem.int32\)).

    ``` csharp
    graphics.GraphicsDevice.Clear(Color.SteelBlue);
    
    RasterizerState rasterizerState = new RasterizerState();
    rasterizerState.CullMode = CullMode.None;
    graphics.GraphicsDevice.RasterizerState = rasterizerState;
    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
    {
        pass.Apply();
    
        graphics.GraphicsDevice.DrawPrimitives(
            PrimitiveType.TriangleList,
            0,  // start vertex
            12  // number of primitives to draw
        );
    }
    ```

## See Also

### Concepts

[3D Pipeline Basics](../../whatis/graphics/WhatIs_3DRendering.md)

© 2012 Microsoft Corporation. All rights reserved.

© 2023 The MonoGame Foundation.
