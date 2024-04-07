---
title: What is 3D Rendering?
description: The basics of the 3D rendering pipeline for MonoGame!
---

# 3D Pipeline Basics

The 3D graphics pipeline uses a graphics device to load resources and render a 3D scene using an effect.

In general, the 3D pipeline requires the following state for initialization:

* World, view, and projection transforms to transform 3D vertices into a 2D space.
* A vertex buffer which contains the geometry to render.
* An effect that sets the render state necessary for drawing the geometry.

As you become comfortable with these ideas, you may want to learn more about the following: manipulating vertices, creating your own effects, applying textures, or improving performance by using index buffers.

The MonoGame Framework uses a shader-driven programmable pipeline. It requires a graphics card capable of at least Shader Model 2.0, but requirements depend on the platform being targeted. The MonoGame Framework provides a class called [BasicEffect](xref:Microsoft.Xna.Framework.Graphics.BasicEffect) that encapsulates most of these common operations.

## The Graphics Device

When you create a game with MonoGame, the framework initializes a graphics device for you.

The [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager) initializes the [GraphicsDevice](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice) before you call [Game.Initialize](xref:Microsoft.Xna.Framework.Game.Initialize). Before you call [Initialize](xref:Microsoft.Xna.Framework.Game.Initialize), there are three ways to change the [GraphicsDevice](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice) settings:

1. Set the appropriate properties (e.g. [PreferredBackBufferHeight](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.PreferredBackBufferHeight), [PreferredBackBufferWidth](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.PreferredBackBufferWidth)) on the [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager) in your game's constructor.

2. Handle the ```PreparingDeviceSettings``` event on the [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager), and change the [PreparingDeviceSettingsEventArgs.GraphicsDeviceInformation.PresentationParameters](xref:Microsoft.Xna.Framework.Graphics.PresentationParameters) member properties.

    Any changes made to the [PreparingDeviceSettingsEventArgs](xref:Microsoft.Xna.Framework.PreparingDeviceSettingsEventArgs) will override the [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager) preferred settings.

3. Handle the ```DeviceCreated``` event on the [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager), and change the [PresentationParameters](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice.PresentationParameters) of the [GraphicsDevice](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice) directly.

When you call [Game.Initialize](xref:Microsoft.Xna.Framework.Game.Initialize), [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager) creates and configures [GraphicsDevice](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice). You can safely access [GraphicsDevice](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice) settings such as the backbuffer, depth/stencil buffer, viewport, and render states in [Initialize](xref:Microsoft.Xna.Framework.Game.Initialize).

After you call [Game.Initialize](xref:Microsoft.Xna.Framework.Game.Initialize), changes to the [PresentationParameters](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice.PresentationParameters) of the [GraphicsDevice](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice) will not take effect until you call [GraphicsDeviceManager.ApplyChanges](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.ApplyChanges). Other changes, such as render states, will take effect immediately.

## Resources

A resource is a collection of data stored in memory that can be accessed by the CPU or GPU. Types of resources that an application might use include render targets, vertex buffers, index buffers, and textures.

Based on the resource management mode that was used when a resource is created, it should be reloaded when the device is reset. For more information, see [Loading Resources](../../howto/Content_Pipeline/HowTo_LoadContentLibrary.md).

### Vertex and Index Buffers

A vertex buffer contains a list of 3D vertices to be streamed to the graphics device. Each vertex in a vertex buffer may contain data about not only the 3D coordinate of the vertex, but also other information describing the vertex, such as the vertex normal, color, or texture coordinate. The MonoGame Framework contains several classes to describe common vertex declaration types, such as [VertexPositionColor](xref:Microsoft.Xna.Framework.Graphics.VertexPositionColor), [VertexPositionColorTexture](xref:Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture), [VertexPositionNormalTexture](xref:Microsoft.Xna.Framework.Graphics.VertexPositionNormalTexture), and [VertexPositionTexture](xref:Microsoft.Xna.Framework.Graphics.VertexPositionTexture). Use the [VertexElement](xref:Microsoft.Xna.Framework.Graphics.VertexElement) class to compose custom vertex types.

Vertex buffers contain indexed or non-indexed vertex data.

If a vertex buffer is not indexed, all of the vertices are placed in the vertex buffer in the order they are to be rendered. Because 3D line lists or triangle lists often reference the same vertices multiple times, this can result in a large amount of redundant data.

Index buffers allow you to list each vertex only once in the vertex buffer. An index buffer is a list of indices into the vertex buffer, given in the order that you want the vertices to render.

To render a non-indexed vertex buffer, call the [GraphicsDevice.DrawPrimitives](/api/Microsoft.Xna.Framework.Graphics.GraphicsDevice.html#Microsoft_Xna_Framework_Graphics_GraphicsDevice_DrawPrimitives_Microsoft_Xna_Framework_Graphics_PrimitiveType_System_Int32_System_Int32_) or [GraphicsDevice.DrawUserPrimitives](/api/Microsoft.Xna.Framework.Graphics.GraphicsDevice.html#Microsoft_Xna_Framework_Graphics_GraphicsDevice_DrawUserPrimitives__1_Microsoft_Xna_Framework_Graphics_PrimitiveType___0___System_Int32_System_Int32_) Methods. 

To render an indexed buffer, call the [GraphicsDevice.DrawIndexedPrimitives](/api/Microsoft.Xna.Framework.Graphics.GraphicsDevice.html#Microsoft_Xna_Framework_Graphics_GraphicsDevice_DrawIndexedPrimitives_Microsoft_Xna_Framework_Graphics_PrimitiveType_System_Int32_System_Int32_System_Int32_) or [GraphicsDevice.DrawUserIndexedPrimitives](/api/Microsoft.Xna.Framework.Graphics.GraphicsDevice.html#Microsoft_Xna_Framework_Graphics_GraphicsDevice_DrawUserIndexedPrimitives__1_Microsoft_Xna_Framework_Graphics_PrimitiveType___0___System_Int32_System_Int32_System_Int16___System_Int32_System_Int32_) Methods.

### Textures

A texture resource is a structured collection of texture data. The data in a texture resource is made up of one or more subresources that are organized into arrays and mipmap chains. Textures are filtered by a texture sampler as they are read. The type of texture influences how the texture is filtered.

You can apply textures by using the [Texture](xref:Microsoft.Xna.Framework.Graphics.BasicEffect.Texture) property of the [BasicEffect](xref:Microsoft.Xna.Framework.Graphics.BasicEffect) class, or choose to write your own effect methods to apply textures.

---

© 2012 Microsoft Corporation. All rights reserved.

© 2023 The MonoGame Foundation.
