---
title: What is 3D Rendering?
description: The basics of the 3D rendering pipeline for MonoGame!
requireMSLicense: true
---

The 3D graphics pipeline uses a `graphics device` (the current display device rendering to the screen) to load resources and render a 3D scene using an [Effect](xref:Microsoft.Xna.Framework.Graphics.Effect).

In general, the 3D pipeline requires the following state information for initialization:

* World, view, and projection matrices to transform 3D vertices into a 2D space.
* A vertex buffer which contains the geometry to render.
* An effect that sets the render state necessary for drawing the geometry.

> [!NOTE]
> For a more detailed explanation of World, View and Projection matrices , check out this [GameFromScratch - beginning 3D](https://gamefromscratch.com/monogame-tutorial-beginning-3d-programming/) article.

As you become comfortable with these ideas, you may want to learn more about the following:

* Manipulating vertices
* Creating your own effects
* Applying textures
* Improving performance by using index buffers.

The MonoGame Framework uses a shader-driven programmable pipeline and requires a graphics card capable of at least Shader Model 2.0. A class called [BasicEffect](xref:Microsoft.Xna.Framework.Graphics.BasicEffect) encapsulates most of these common operations.

> ![IMPORTANT]
> Shader requirements depend on the platform being targeted, the majority of MonoGame titles these days require Shader Model 3.0 as a minimum.  You can use [this guide](https://www.lifewire.com/determine-directx-version-and-shader-model-812997) as a reference.
> MonoGame automatically converts all shaders to their appropriate platform based on the "shader level" defined.

## The Graphics Device

When you create a game with MonoGame, the framework initializes a graphics device for you.

The [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager) initializes the [GraphicsDevice](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice).

Before [Initialize](xref:Microsoft.Xna.Framework.Game.Initialize) is called, there are three ways to change the [GraphicsDevice](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice) settings:

1. Set the appropriate properties such as the [PreferredBackBufferHeight](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.PreferredBackBufferHeight) and [PreferredBackBufferWidth](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.PreferredBackBufferWidth) on the [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager) in your game's constructor.

    ```csharp
        _graphics.PreferredBackBufferHeight = 768;
        _graphics.PreferredBackBufferWidth = 1024;
        _graphics.ApplyChanges();
    ```

1. Handle the `PreparingDeviceSettings` event on the [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager), and change the [PreparingDeviceSettingsEventArgs.GraphicsDeviceInformation.PresentationParameters](xref:Microsoft.Xna.Framework.Graphics.PresentationParameters) member properties.

    ```csharp
    _graphics.PreparingDeviceSettings += OnPreparingDeviceSettings;
    private void OnPreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
    {
        e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth = 1024;
        e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight = 768;
    }
    ```

    > [!WARNING]
    > Any changes made to the [PreparingDeviceSettingsEventArgs](xref:Microsoft.Xna.Framework.PreparingDeviceSettingsEventArgs) will override the [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager) preferred settings.

1. Handle the `DeviceCreated` event on the [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager), and change the [PresentationParameters](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice.PresentationParameters) of the [GraphicsDevice](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice) directly.

    ```csharp
    IGraphicsDeviceService graphicsDeviceService = (IGraphicsDeviceService)
        Game.Services.GetService(typeof(IGraphicsDeviceService));

    if (graphicsDeviceService != null)
    {
        graphicsDeviceService.DeviceCreated += OnDeviceCreated;
    }

    private void OnDeviceCreated(object sender, EventArgs e)
    {
        // Handle updating Graphics Device Presentation Parameters
    }
    ```

When you call [Game.Initialize](xref:Microsoft.Xna.Framework.Game.Initialize) the [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager) creates and configures [GraphicsDevice](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice). You can then safely access [GraphicsDevice](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice) settings such as the backbuffer, depth/stencil buffer, viewport, and render states in the [Initialize](xref:Microsoft.Xna.Framework.Game.Initialize) method.

> [!IMPORTANT]
> After you call [Game.Initialize](xref:Microsoft.Xna.Framework.Game.Initialize), changes to the [PresentationParameters](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice.PresentationParameters) of the [GraphicsDevice](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice) will not take effect until you call [GraphicsDeviceManager.ApplyChanges](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.ApplyChanges). Other changes, such as render states, will take effect immediately.

## Resources

A graphics resource is a collection of data stored in memory that can be accessed by either the CPU or GPU. The types of resources that an application might use include:

* [Render targets](./WhatIs_Render_Target.md)
* [Vertex buffers](xref:Microsoft.Xna.Framework.Graphics.VertexBuffer)
* [Index buffers](xref:Microsoft.Xna.Framework.Graphics.IndexBuffer)
* [Textures](xref:Microsoft.Xna.Framework.Graphics.Texture)

> ![NOTE]
> FOr a more detailed understanding in the use of these terms, check out [Riemers 3D series](https://github.com/simondarksidej/XNAGameStudio/wiki/Riemers3DXNA1Terrainoverview) on the XNAGameStudio Archive.

Based on the resource management mode that was used when a resource is created, it should be reloaded when the device is reset. For more information, see [Loading Resources](../../howto/content_pipeline/HowTo_LoadContentLibrary.md).

### Vertex and Index Buffers

A vertex buffer contains a list of 3D vertices to be streamed to the graphics device. Each vertex in a vertex buffer may contain data about not only the 3D coordinate of the vertex but also other information describing the vertex, such as the vertex normal, color, or texture coordinate.  Which you should use will depend on your usage and the needs of the vertex information you are drawing with, [for example](https://stackoverflow.com/questions/4702150/difference-between-using-vertexpositionnormaltexture-and-vertexpositiontexture.)

The MonoGame Framework contains several classes to describe common vertex declaration types, such as:

* [VertexPositionColor](xref:Microsoft.Xna.Framework.Graphics.VertexPositionColor)

  Vertex Declaration containing Position and Color of a vertex.

* [VertexPositionColorTexture](xref:Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture)

  Vertex Declaration containing Position, Color and Texture of a vertex.

* [VertexPositionNormalTexture](xref:Microsoft.Xna.Framework.Graphics.VertexPositionNormalTexture)

  Vertex Declaration containing Position, Normal and Texture of a vertex.

* [VertexPositionTexture](xref:Microsoft.Xna.Framework.Graphics.VertexPositionTexture).

  Vertex Declaration containing Position and Texture of a vertex.

> [!NOTE]
> Use the [VertexElement](xref:Microsoft.Xna.Framework.Graphics.VertexElement) class to compose custom vertex types.

Vertex buffers can contain indexed or non-indexed vertex data.

> [!NOTE]
> If a **Vertex Buffer** is not indexed, all of the vertices are placed in the vertex buffer in the order they are to be rendered. Because 3D line lists or triangle lists often reference the same vertices multiple times, this can result in a large amount of redundant data.
>
> **Index buffers** allow you to list each vertex only once in the vertex buffer. An index buffer is a list of indices into the vertex buffer, given in the order that you want the vertices to render.

### Render a non-indexed vertex buffer

To render a non-indexed vertex buffer, call the [GraphicsDevice.DrawPrimitives](/api/Microsoft.Xna.Framework.Graphics.GraphicsDevice.html#Microsoft_Xna_Framework_Graphics_GraphicsDevice_DrawPrimitives_Microsoft_Xna_Framework_Graphics_PrimitiveType_System_Int32_System_Int32_) or [GraphicsDevice.DrawUserPrimitives](/api/Microsoft.Xna.Framework.Graphics.GraphicsDevice.html#Microsoft_Xna_Framework_Graphics_GraphicsDevice_DrawUserPrimitives__1_Microsoft_Xna_Framework_Graphics_PrimitiveType___0___System_Int32_System_Int32_) Methods.

### Render an indexed vertex buffer

To render an indexed buffer, call the [GraphicsDevice.DrawIndexedPrimitives](/api/Microsoft.Xna.Framework.Graphics.GraphicsDevice.html#Microsoft_Xna_Framework_Graphics_GraphicsDevice_DrawIndexedPrimitives_Microsoft_Xna_Framework_Graphics_PrimitiveType_System_Int32_System_Int32_System_Int32_) or [GraphicsDevice.DrawUserIndexedPrimitives](/api/Microsoft.Xna.Framework.Graphics.GraphicsDevice.html#Microsoft_Xna_Framework_Graphics_GraphicsDevice_DrawUserIndexedPrimitives__1_Microsoft_Xna_Framework_Graphics_PrimitiveType___0___System_Int32_System_Int32_System_Int16___System_Int32_System_Int32_) Methods.

### Textures

A texture resource is a structured collection of texture data. The data in a texture resource is made up of one or more sub-resources that are organized into arrays and mipmap chains. Textures are filtered by a texture sampler as they are read. The type of texture influences how the texture is filtered.

You can apply textures by using the [Texture](xref:Microsoft.Xna.Framework.Graphics.BasicEffect.Texture) property of the [BasicEffect](xref:Microsoft.Xna.Framework.Graphics.BasicEffect) class, or choose to write your own effect methods to apply textures.
