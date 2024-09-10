---
title: What are Graphics?
description: The basics of the graphics architecture for MonoGame!
requireMSLicense: true
---

## Graphical basics 101

The topics in this section describe the graphical conventions used by MonoGame in rendering and managing content on a screen.

### 2D Concepts

* [What Is Sprite?](WhatIs_Sprite.md)

  Sprites are 2D bitmaps that are drawn directly to a render target without using the pipeline for transformations, lighting or effects. Sprites are commonly used to display information such as health bars, number of lives, or text such as scores. Some games, especially older games, are composed entirely of sprites.

### 3D concepts

* [What Is 3D Rendering?](WhatIs_3DRendering.md)

  The 3D graphics pipeline uses a graphics device to load resources and render a 3D scene using an effect.

* [What Is a Configurable Effect?](WhatIs_ConfigurableEffect.md)

  MonoGame ships with a number of built in effects. These effects have properties so you can use to change behaviour or switch features on or off.

* [What Is a Depth Buffer?](WhatIs_DepthBuffer.md)

  A depth buffer contains per-pixel floating-point data for the z depth of each pixel rendered. A depth buffer may also contain stencil data which can be used to do more complex rendering such as simple shadows or outlines.

* [What Is Blend State?](WhatIs_BlendState.md)

  Blend state controls how color and alpha values are blended when combining rendered data with existing render target data.

* [What Is Depth Stencil State?](WhatIs_DepthStencilState.md)

  Depth stencil state controls how the depth buffer and the stencil buffer are used.

* [What Is Rasterizer State?](WhatIs_Rasterizer.md)

  Rasterizer state determines how to render 3D data such as position, color, and texture onto a 2D surface.

* [What Is Sampler State?](WhatIs_Sampler.md)

  Sampler state determines how texture data is sampled using texture addressing modes, filtering, and level of detail.

* [What Is Color Blending?](WhatIs_ColorBlending.md)

  Color blending mixes two colors together to produce a third color.

* [What Is a Stencil Buffer?](WhatIs_StencilBuffer.md)

  A stencil buffer contains per-pixel integer data which is used to add more control over which pixels are rendered. A stencil buffer can also be used in combination with a depth buffer to do more complex rendering such as simple shadows or outlines.

* [What Is a Model Bone?](WhatIs_ModelBone.md)

  A model bone is a matrix that represents the position of a mesh as it relates to other meshes in a 3D model.

### Other Graphical concepts

* [What Is a Graphics Profile?](WhatIs_GraphicsProfile.md)

  Provides conceptual information about the MonoGame Framework concept of graphics profiles, including explanations of the Reach and HiDef profiles.

* [What Is a Back Buffer?](WhatIs_BackBuffer.md)

  A back buffer is a render target whose contents will be sent to the device when [GraphicsDevice.Present](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice.Present) is called.

* [What Is a Render Target?](WhatIs_Render_Target.md)

  A render target is a memory buffer for rendering pixels. One common use for a render target is offscreen rendering.

* [What Is a View Frustum?](WhatIs_ViewFrustum.md)

  A view frustum is a 3D volume that defines how models are projected from camera space to projection space. Objects must be positioned within the 3D volume to be visible.

* [What Is a Viewport?](WhatIs_Viewport.md)

  A viewport is a 2D rectangle that defines the size of the rendering surface onto which a 3D scene is projected.

* [What Is Antialiasing?](WhatIs_Antialiasing.md)

  Antialiasing is a technique for softening or blurring sharp edges so they appear less jagged when rendered.
