---
title: Drawing 3D Primitives using Lists or Strips
description: Demonstrates how to draw 3D primitives using lines and triangles arranged as strips or lists.
---

# Drawing 3D Primitives using Lists or Strips

Demonstrates how to draw 3D primitives using lines and triangles arranged as strips or lists.

A 3D primitive describes how vertex data is ordered. This example demonstrates line and triangle primitive types that are the basis for all drawing calls in the MonoGame Framework. To render primitives, you need to create a basic effect and transformation matrices. This topic follows the steps described in [Creating a Basic Effect](HowTo_Create_a_BasicEffect.md) to create an instance of [BasicEffect](xref:Microsoft.Xna.Framework.Graphics.BasicEffect). This sample uses orthographic projection, but you can also use perspective projection to render primitives. The vertices used in the sample are of type [VertexPositionColor Structure](xref:Microsoft.Xna.Framework.Graphics.VertexPositionColor) which contains per-vertex position and color.

## Creating Vertices

The major steps for this example are:

- Creating Vertices
- Drawing a Line List
- Drawing a Line Strip
- Drawing A Triangle List
- Drawing a Triangle Strip

### To create vertices

1. Create a list of vertices in 3D space that represent the points to draw.

    The following code creates eight vertices and stores them in an array of type **VertexPositionColor**.

    ``` csharp
    primitiveList = new VertexPositionColor[points];
    
    for (int x = 0; x < points / 2; x++)
    {
        for (int y = 0; y < 2; y++)
        {
            primitiveList[(x * 2) + y] = new VertexPositionColor(
                new Vector3(x * 100, y * 100, 0), Color.White);
        }
    }
    ```

    These eight points form a triangle strip consisting of six triangles drawn along the plane z = 0, with the first point at (0, 0, 0). The camera is positioned at (0, 0, 1) looking at (0, 0, 0). An orthogonal projection matrix is created with the upper-left point at (0, 0) and the lower-right point at (800, 600). In addition, a translation matrix shifts the point set to the center of the screen. The code below positions the points, the camera and the required transformations.

    ``` csharp
    viewMatrix = Matrix.CreateLookAt(
        new Vector3(0.0f, 0.0f, 1.0f),
        Vector3.Zero,
        Vector3.Up
        );
    
    projectionMatrix = Matrix.CreateOrthographicOffCenter(
        0,
        (float)GraphicsDevice.Viewport.Width,
        (float)GraphicsDevice.Viewport.Height,
        0,
        1.0f, 1000.0f);
    ```

## Drawing a Line List

The example in this section uses the sample vertex list created by following step 1 in the Creating Vertices procedure.

### To draw a line list

1. Create an index array that indexes into the vertex buffer.

    This identifies a series of lines.

    ``` csharp
    // Initialize an array of indices of type short.
    lineListIndices = new short[(points * 2) - 2];
    
    // Populate the array with references to indices in the vertex buffer
    for (int i = 0; i < points - 1; i++)
    {
        lineListIndices[i * 2] = (short)(i);
        lineListIndices[(i * 2) + 1] = (short)(i + 1);
    }
    ```

2. Render the lines by calling [DrawUserIndexedPrimitives](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice), which specifies [PrimitiveType.LineList](xref:Microsoft.Xna.Framework.Graphics.PrimitiveType) to determine how to interpret the data in the vertex array.

    ``` csharp
    GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
        PrimitiveType.LineList,
        primitiveList,
        0,  // vertex buffer offset to add to each element of the index buffer
        8,  // number of vertices in pointList
        lineListIndices,  // the index buffer
        0,  // first index element to read
        7   // number of primitives to draw
    );
    ```

## Drawing a Line Strip

The example in this section uses the same point list and renders the same output as the Drawing a Line List procedure. However, it uses a line strip primitive type when it identifies the indices of the vertex array to draw. Fewer indices are stored when you use a line strip.

### To draw a line strip

1. Create a list of indices to identify the order in which to draw the points in the specified point list.

    Only half the number of indices used for the line list are needed here because the data consist of a series of connected lines.

    ``` csharp
    // Initialize an array of indices of type short.
    lineStripIndices = new short[points];
    
    // Populate the array with references to indices in the vertex buffer.
    for (int i = 0; i < points; i++)
    {
        lineStripIndices[i] = (short)(i);
    }
    ```

    This is equivalent to setting `lineStripIndices` to the following array, which consists of a series of *connected* lines between pointList\[0\], pointList\[1\], and pointList\[2\], and so forth.

    ``` csharp
    lineStripIndices = new short[8]{ 0, 1, 2, 3, 4, 5, 6, 7 };
    ```

2. Render the line strip by calling [DrawUserIndexedPrimitives](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice), which specifies [PrimitiveType.LineStrip](xref:Microsoft.Xna.Framework.Graphics.PrimitiveType) to determine how to interpret the data in the vertex array.

    Fewer vertices are used to render the same number of primitives rendered earlier by the line list.

    > [!NOTE]
    > In the example code, the line strip is rendered by a series of red lines instead of the white lines used for the previous line list. This is accomplished by changing the vertex color before you draw the line strip. After you complete the drawing, it reverts to the original color. In addition, vertex coloring was enabled for the basic effect (using [BasicEffect.VertexColorEnabled]](xref:Microsoft.Xna.Framework.Graphics.BasicEffect#Microsoft_Xna_Framework_Graphics_BasicEffect_VertexColorEnabled) Property</A>).
    > The color change indicates a different primitive type was used to achieve the same result.

    ``` csharp
    for (int i = 0; i < primitiveList.Length; i++)
        primitiveList[i].Color = Color.Red;
    
    GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
        PrimitiveType.LineStrip,
        primitiveList,
        0,   // vertex buffer offset to add to each element of the index buffer
        8,   // number of vertices to draw
        lineStripIndices,
        0,   // first index element to read
        7    // number of primitives to draw
    );
    for (int i = 0; i < primitiveList.Length; i++)
        primitiveList[i].Color = Color.White;
    ```

## Drawing A Triangle List

A triangle list, like a line list, is a primitive type that indicates you need to interpret the vertices in the vertex buffer as a series of separately drawn triangles.

### To draw a triangle list

1. Create an array to hold the list of indices that identify a series of triangles to draw from the specified point list.

    ``` csharp
    triangleListIndices = new short[(width - 1) * (height - 1) * 6];
    
    for (int x = 0; x < width - 1; x++)
    {
        for (int y = 0; y < height - 1; y++)
        {
            triangleListIndices[(x + y * (width - 1)) * 6] = (short)(2 * x);
            triangleListIndices[(x + y * (width - 1)) * 6 + 1] = (short)(2 * x + 1);
            triangleListIndices[(x + y * (width - 1)) * 6 + 2] = (short)(2 * x + 2);
    
            triangleListIndices[(x + y * (width - 1)) * 6 + 3] = (short)(2 * x + 2);
            triangleListIndices[(x + y * (width - 1)) * 6 + 4] = (short)(2 * x + 1);
            triangleListIndices[(x + y * (width - 1)) * 6 + 5] = (short)(2 * x + 3);
        }
    }
    ```

    This is equivalent to setting `triangleListIndices` to the following array, which consists of a series of triangles between pointList\[0\], pointList\[1\], and pointList\[2\], and so forth.

    ``` csharp
    triangleListIndices = new short[18]{ 0, 1, 2, 2, 1, 3, 2, 3, 4, 4, 3, 5, 4, 5, 6, 6, 5, 7 };
    ```

2. Render the lines by calling [DrawUserIndexedPrimitives](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice)

    This specifies [PrimitiveType.TriangleList](xref:Microsoft.Xna.Framework.Graphics.PrimitiveType), which determines how the data in the vertex array is interpreted.

    ``` csharp
    GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
        PrimitiveType.TriangleList,
        primitiveList,
        0,   // vertex buffer offset to add to each element of the index buffer
        8,   // number of vertices to draw
        triangleListIndices,
        0,   // first index element to read
        6    // number of primitives to draw
    );
    ```

## Drawing a Triangle Strip

A triangle strip is a set of triangles that share multiple vertices. This example shows you how to render an object that looks the same as the object rendered with a triangle list. However, fewer vertices are needed because the triangles share multiple vertices.

### To draw a triangle strip

1. Create an array to hold the list of indices that identify a strip of triangles.

    ``` csharp
    // Initialize an array of indices of type short.
    triangleStripIndices = new short[points];
    
    // Populate the array with references to indices in the vertex buffer.
    for (int i = 0; i < points; i++)
    {
        triangleStripIndices[i] = (short)i;
    }
    ```

    This is equivalent to setting `triangleStripIndices` to the following array, which consists of a series of *connected* triangles between pointList\[0\], pointList\[1\], and pointList\[2\], and so forth.

    ``` csharp
    triangleStripIndices = new short[8]{ 0, 1, 2, 3, 4, 5, 6, 7 };
    ```

2. Render the lines by calling [DrawUserIndexedPrimitives](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice).

    This specifies [PrimitiveType.TriangleStrip](xref:Microsoft.Xna.Framework.Graphics.PrimitiveType) to determine how to interpret the data in the vertex array. Fewer vertices are used to render the same number of primitives rendered earlier by the triangle list.

    > [!NOTE]
    > In the example code, the triangle strip is rendered by a series of red lines instead of the white lines used for the previous triangle list. The color change indicates a different primitive type was used to achieve the same result.

    ``` csharp
    for (int i = 0; i < primitiveList.Length; i++)
        primitiveList[i].Color = Color.Red;
    
    GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
        PrimitiveType.TriangleStrip,
        primitiveList,
        0,  // vertex buffer offset to add to each element of the index buffer
        8,  // number of vertices to draw
        triangleStripIndices,
        0,  // first index element to read
        6   // number of primitives to draw
    );
    for (int i = 0; i < primitiveList.Length; i++)
        primitiveList[i].Color = Color.White;
    ```

© 2012 Microsoft Corporation. All rights reserved.

© 2023 The MonoGame Foundation.
