---
title: Drawing 3D Primitives using Lists or Strips
description: Demonstrates how to draw 3D primitives using lines and triangles arranged as strips or lists.
requireMSLicense: true
---

## Overview

A 3D primitive describes how vertex data is ordered. This example demonstrates line and triangle primitive types that are the basis for all drawing calls in the MonoGame Framework. To render primitives, you need to create a basic effect and transformation matrices. This topic follows the steps described in [Creating a Basic Effect](HowTo_Create_a_BasicEffect.md) to create an instance of [BasicEffect](xref:Microsoft.Xna.Framework.Graphics.BasicEffect). This sample uses orthographic projection, but you can also use perspective projection to render primitives. The vertices used in the sample are of type [VertexPositionColor Structure](xref:Microsoft.Xna.Framework.Graphics.VertexPositionColor) which contains per-vertex position and color.

### Prerequisites

This tutorial assumes you are following on from the project created in [How to create a Basic Effect](HowTo_Create_a_BasicEffect.md)

## Creating Vertices

The major steps for this example are:

- Creating Vertices
- Drawing a Line List
- Drawing a Line Strip
- Drawing A Triangle List
- Drawing a Triangle Strip

## To create vertices

First we need to setup some data to draw our primitives, essentially a list of points that we will connect together and draw in different ways.

1. Position the camera and create the required transformations.

    ``` csharp
    // Matrix to translate the drawn primitives to the center of the screen.
    private Matrix translationMatrix;
    
    // Number of vertex points to draw the primitive with.
    private int points = 8;
    
    // The length of the primitive lines to draw.
    private int lineLength = 100;

    protected override void Initialize()
    {
        worldMatrix = Matrix.Identity;

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

        Vector2 screenCenter = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
        var primitiveCenter = new Vector2((points / 2 - 1) * lineLength / 2, lineLength / 2);
        translationMatrix = Matrix.CreateTranslation(screenCenter.X - primitiveCenter.X, screenCenter.Y - primitiveCenter.Y, 0);
 
       base.Initialize();
    }
    ```

1. Create a list of vertices in 3D space that represent the points to draw.

    The following code creates eight vertices (determined from the `points` variable) and stores them in an array of type **VertexPositionColor**.

    ``` csharp
    // The vertex sata array.
    private VertexPositionColor[] primitiveList;

    protected override void LoadContent()
    {
    ... // <-Existing Load Content from Creating a Basic Effect

        primitiveList = new VertexPositionColor[points];
        
        for (int x = 0; x < points / 2; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                primitiveList[(x * 2) + y] = new VertexPositionColor(
                    new Vector3(x * lineLength, y * lineLength, 0), Color.White);
            }
        }
    ```

    - These eight points form a `triangle strip` consisting of six triangles drawn along the plane `z = 0`, with the first point at `(0, 0, 0)`.
    - The camera is positioned at `(0, 0, 1)` looking at `(0, 0, 0)`.
    - An orthogonal projection matrix is created with the upper-left point at `(0, 0)` and the lower-right point at the current `GraphicsDevice` screen dimensions.
    - In addition, a translation matrix shifts the `primitiveList` point set to the center of the screen.

1. To make the drawing of lines clearer, also change the `Clear` color in the `Draw` method to `black`.  Just makes it easier to see drawn lines.

    ```csharp
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
    ```

## Drawing a Line List

The example in this section uses the sample vertex list created by following step 1 in the `Creating Vertices` steps.

![Drawing a Line List](HowTo_Draw_3D_Primitives_Line_List.png)

### To draw a line list

1. Create an index array that indexes into the vertex buffer.

    This identifies a series of lines.

    ``` csharp
    private short[] lineListIndices;

    protected override void LoadContent()
    {
    ... // <-Existing Load Content

        // Initialize an array of indices of type short.
        lineListIndices = new short[(points * 2) - 2];
        
        // Populate the array with references to indices in the vertex buffer
        for (int i = 0; i < points - 1; i++)
        {
            lineListIndices[i * 2] = (short)(i);
            lineListIndices[(i * 2) + 1] = (short)(i + 1);
        }
    ```

2. Render the lines by calling [DrawUserIndexedPrimitives](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice#Microsoft_Xna_Framework_Graphics_GraphicsDevice_DrawUserIndexedPrimitives__1_Microsoft_Xna_Framework_Graphics_PrimitiveType___0___System_Int32_System_Int32_System_Int32___System_Int32_System_Int32_), which specifies [PrimitiveType.LineList](xref:Microsoft.Xna.Framework.Graphics.PrimitiveType) to determine how to interpret the data in the vertex array.

    ``` csharp
    GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
        PrimitiveType.LineList,
        primitiveList,
        0,  // vertex buffer offset to add to each element of the index buffer
        points,  // number of vertices in pointList
        lineListIndices,  // the index buffer
        0,  // first index element to read
        points - 1   // number of primitives to draw
    );
    ```

## Drawing a Line Strip

The example in this section uses the same point list and renders the same output as the Drawing a Line List procedure. However, it uses a `line strip` primitive type when it identifies the indices of the vertex array to draw. **Fewer indices are stored when you use a line strip.**

![Drawing a Line List](HowTo_Draw_3D_Primitives_Line_List.png)

### To draw a line strip

1. Create a list of indices to identify the order in which to draw the points in the specified point list.

    Only half the number of indices used for the line list are needed here because the data consist of a series of connected lines.

    ``` csharp
    private short[] lineStripIndices;

    protected override void LoadContent()
    {
        ... // <-Existing Load Content

        // Initialize an array of indices of type short.
        lineStripIndices = new short[points];
        
        // Populate the array with references to indices in the vertex buffer.
        for (int i = 0; i < points; i++)
        {
            lineStripIndices[i] = (short)(i);
        }
    ```

    > [!NOTE]
    > This is equivalent to setting `lineStripIndices` with the following array that consists of a series of **connected** lines between `pointList[0]`, `pointList[1]`, and `pointList[2]`, and so forth.
    >
    > ``` csharp
    > lineStripIndices = new short[8]{ 0, 1, 2, 3, 4, 5, 6, 7 };
    > ```

1. Render the line strip by calling [DrawUserIndexedPrimitives](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice#Microsoft_Xna_Framework_Graphics_GraphicsDevice_DrawUserIndexedPrimitives__1_Microsoft_Xna_Framework_Graphics_PrimitiveType___0___System_Int32_System_Int32_System_Int32___System_Int32_System_Int32_), which specifies [PrimitiveType.LineStrip](xref:Microsoft.Xna.Framework.Graphics.PrimitiveType) to determine how to interpret the data in the vertex array.

    Fewer vertices are used to render the same number of primitives rendered earlier by the line list.

    ``` csharp
    GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
        PrimitiveType.LineList,
        primitiveList,
        0,  // vertex buffer offset to add to each element of the index buffer
        points,  // number of vertices in pointList
        lineListIndices,  // the index buffer
        0,  // first index element to read
        points - 1   // number of primitives to draw
    );
    ```

## Drawing A Triangle List

A triangle list, like a line list, is a primitive type that indicates you need to interpret the vertices in the vertex buffer as a series of separately drawn triangles.

### To draw a triangle list

1. Create an array to hold the list of indices that identify a series of triangles to draw from the specified point list.

    ``` csharp
    private short[] triangleStripIndices;
    private int triangleWidth = 10;
    private int triangleHeight = 10;

    protected override void LoadContent()
    {
        ... // <-Existing Load Content
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

    > [!NOTE]
    > This is equivalent to setting `triangleListIndices` to the following array, which consists of a series of triangles between  `pointList[0]`, `pointList[1]`, and `pointList[2]`, and so forth.
    >
    > ``` csharp
    > triangleListIndices = new short[18]{ 0, 1, 2, 2, 1, 3, 2, 3, 4, 4, 3, 5, 4, 5, 6, 6, 5, 7 };
    > ```

2. Render the lines by calling [DrawUserIndexedPrimitives](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice#Microsoft_Xna_Framework_Graphics_GraphicsDevice_DrawUserIndexedPrimitives__1_Microsoft_Xna_Framework_Graphics_PrimitiveType___0___System_Int32_System_Int32_System_Int32___System_Int32_System_Int32_)

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
    private short[] triangleStripIndices;

    protected override void LoadContent()
    {
        ... // <-Existing Load Content

        // Initialize an array of indices of type short.
        triangleStripIndices = new short[points];
        
        // Populate the array with references to indices in the vertex buffer.
        for (int i = 0; i < points; i++)
        {
            triangleStripIndices[i] = (short)i;
        }
    ```

    > [!NOTE]
    > This is equivalent to setting `triangleStripIndices` to the following array, which consists of a series of **connected** triangles between `pointList[0]`, `pointList[1]`, and `pointList[2]`, and so forth.
    >
    > ``` csharp
    > triangleStripIndices = new short[8]{ 0, 1, 2, 3, 4, 5, 6, 7 };
    > ```

2. Render the lines by calling [DrawUserIndexedPrimitives](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice#Microsoft_Xna_Framework_Graphics_GraphicsDevice_DrawUserIndexedPrimitives__1_Microsoft_Xna_Framework_Graphics_PrimitiveType___0___System_Int32_System_Int32_System_Int32___System_Int32_System_Int32_).

    This specifies [PrimitiveType.TriangleStrip](xref:Microsoft.Xna.Framework.Graphics.PrimitiveType) to determine how to interpret the data in the vertex array. Fewer vertices are used to render the same number of primitives rendered earlier by the triangle list.

    > [!NOTE]
    > In the example code, the triangle strip is rendered by a series of red lines instead of the white lines used for the previous triangle list. The color change indicates a different primitive type was used to achieve the same result.

    ``` csharp
    GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
        PrimitiveType.TriangleStrip,
        primitiveList,
        0,  // vertex buffer offset to add to each element of the index buffer
        8,  // number of vertices to draw
        triangleStripIndices,
        0,  // first index element to read
        6   // number of primitives to draw
    );
    ```

## See Also

- [How to enable Anti-aliasing](HowTo_Enable_Anti_Aliasing.md)

### Concepts

- [What Is 3D Rendering?](../../whatis/grasphics/WhatIs_3DRendering.md)

### Reference

- [GraphicsDevice](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice)
- [DrawUserIndexedPrimitives](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice#Microsoft_Xna_Framework_Graphics_GraphicsDevice_DrawUserIndexedPrimitives__1_Microsoft_Xna_Framework_Graphics_PrimitiveType___0___System_Int32_System_Int32_System_Int32___System_Int32_System_Int32_)
