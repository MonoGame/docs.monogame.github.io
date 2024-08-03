---
title: What Is Depth Stencil State?
description: The definition for the Depth Stencil State for MonoGame!
---

# What Is Depth Stencil State?

Depth stencil state controls how the depth buffer and the stencil buffer are used.

The depth buffer stores floating-point depth or z data for each pixel while the stencil buffer stores integer data for each pixel. The depth-stencil state class, [DepthStencilState](xref:Microsoft.Xna.Framework.Graphics.DepthStencilState), contains state that controls how depth and stencil data impacts rendering.

During rendering, the z position (or depth) of each pixel is stored in the depth buffer. When rendering pixels more than once -- such as when objects overlap -- depth data is compared between the current pixel and the previous pixel to determine which pixel is closer to the camera. When a pixel passes the depth test, the pixel color is written to a render target and the pixel depth is written to the depth buffer. For more information about a depth buffer, see [What Is a Depth Buffer?](WhatIs_DepthBuffer.md).

A depth buffer may also contain stencil data, which is why a depth buffer is often called a depth-stencil buffer. Use a stencil function to compare a reference stencil value -- a global value you set -- to the per-pixel value in the stencil buffer to mask which pixels get saved and which are discarded. For more information about a stencil buffer, see [What Is a Stencil Buffer?](WhatIs_StencilBuffer.md)

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
