---
title: What Is Blend State?
description: The definition for the Blend State for MonoGame!
---

# What Is Blend State?

Blend state controls how color and alpha values are blended when combining rendered data with existing render target data.

The blend state class, [BlendState](xref:Microsoft.Xna.Framework.Graphics.BlendState), contains state that controls how colors are blended. Each time you render, the pixel data you generate (call it source data) is stored in a render target. The render target might be empty or it might already contain data (call it destination data). Blending occurs each time you combine source and destination data.

You have a lot of control over how you blend the source and the destination data. For example, you can choose to overwrite the destination with the source data by setting [BlendState.Opaque](xref:Microsoft.Xna.Framework.Graphics.BlendState) or combine the data by adding them together using [BlendState.Additive](xref:Microsoft.Xna.Framework.Graphics.BlendState). You can blend only the color data or the alpha data, or both, by setting up the blending functions [ColorBlendFunction](xref:Microsoft.Xna.Framework.Graphics.BlendState.ColorBlendFunction) and [AlphaBlendFunction](xref:Microsoft.Xna.Framework.Graphics.BlendState.AlphaBlendFunction). You can even limit your blending operation to one or more colors (or channels) using [ColorWriteChannels](xref:Microsoft.Xna.Framework.Graphics.BlendState.ColorWriteChannels).

For an example that creates and uses a state object, see [Creating a State Object](../../howto/graphics/HowTo_Create_a_StateObject.md).

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
