---
title: What Is Sampler State?
description: The definition for a Sampler State for MonoGame!
---

# What Is Sampler State?

Sampler state determines how texture data is sampled using texture addressing modes, filtering, and level of detail.

Sampling is done each time a texture pixel, or texel, is read from a texture. A texture contains an array of texels, or texture pixels. The position of each texel is denoted by (u,v), where _u_ is the width and _v_ is the height, and is mapped between 0 and 1 based on the texture width and height. The resulting texture coordinates are used to address a texel when sampling a texture.

When texture coordinates are below 0 or above 1, the texture address mode defines how the texture coordinate addresses a texel location. For example, when using [TextureAddressMode.Clamp](https://monogame.net/api/Microsoft.Xna.Framework.Graphics.TextureAddressMode.html), any coordinate outside the 0-1 range is clamped to a maximum value of 1, and minimum value of 0 before sampling.

If the texture is too large or too small for the polygon, the texture is filtered to fit the space. A magnification filter enlarges a texture, a minification filter reduces the texture to fit into a smaller area. Texture magnification repeats the sample texel for one or more addresses which yields a blurrier image. Texture minification is more complicated because it requires combining more than one texel value into a single value. This can cause aliasing or jagged edges depending on the texture data. The most popular approach for minification is to use a mipmap. A mipmap is a multi-level texture. The size of each level is a power-of-two smaller than the previous level down to a 1x1 texture. When minification is used, a game chooses the mipmap level closest to the size that is needed at render time.

Use the [SamplerState](xref:Microsoft.Xna.Framework.Graphics.SamplerState) class to create a sampler state object. Set the sampler state to the graphics device using the [GraphicsDevice.SamplerStates Property](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice.SamplerStates) property.

This is the default state for sampling:

* Use linear filtering.
* Wrap texture addresses on boundaries.
* Set the maximum anisotropy value to 4.
* Do not use mip maps or LOD bias.

These are the corresponding API states:

* Set [Filter](xref:Microsoft.Xna.Framework.Graphics.SamplerState.Filter) to **TextureFilter.Linear**.
* Set [AddressU](xref:Microsoft.Xna.Framework.Graphics.SamplerState.AddressU), [AddressV](xref:Microsoft.Xna.Framework.Graphics.SamplerState.AddressV), and [AddressW](xref:Microsoft.Xna.Framework.Graphics.SamplerState.AddressW) to **TextureAddressMode.Wrap**.
* Set [MaxAnisotropy](xref:Microsoft.Xna.Framework.Graphics.SamplerState.MaxAnisotropy) to 4.
* Set [MaxMipLevel](xref:Microsoft.Xna.Framework.Graphics.SamplerState.MaxMipLevel) and [MipMapLevelOfDetailBias](xref:Microsoft.Xna.Framework.Graphics.SamplerState.MipMapLevelOfDetailBias) to 0.

Built-in state objects make it easy to create objects with the most common sampler state settings. The most common settings are **LinearClamp**, **LinearWrap**, **PointClamp**, **PointWrap**, **AnisotropicClamp**, and **AnisotropicWrap**. For an example of creating a state object, see [Creating a State Object](../../howto/graphics/HowTo_Create_a_StateObject.md).

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
