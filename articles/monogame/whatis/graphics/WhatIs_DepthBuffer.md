---
title: What Is a Depth Buffer?
description: The definition for a Depth Buffer for MonoGame!
---

# What Is a Depth Buffer?

A depth buffer contains per-pixel floating-point data for the z depth of each pixel rendered. A depth buffer may also contain stencil data which can be used to do more complex rendering such as simple shadows or outlines.

When a pixel is rendered, color data as well as depth data can be stored. If a pixel is rendered a second time - such as when two objects overlap - depth testing determines which pixel is closer to the camera. The depth function determines what to do with the test result. For example, if [CompareFunction.LessEqual](/api/Microsoft.Xna.Framework.Graphics.CompareFunction.html) is the current depth function, if the current pixel depth is less than or equal to the previous pixel depth, the current pixel depth is written to the depth buffer. Values that fail the depth test are discarded.

The depth of a pixel, which ranges between 0.0 and 1.0, is determined based on the view and projection matrices. A pixel that touches the near plane has depth 0, a pixel that touches the far plane has depth 1. As each object in the scene is rendered, normally the pixels that are closest to the camera are kept, as those objects block the view of the objects behind them.

A depth buffer may also contain stencil bits - for this reason it's often called a _depth-stencil buffer_. The depth format describes the data format of the depth buffer. The depth buffer is always 32 bits, but those bits can be arranged in different ways, similar to how texture formats can vary. A common depth format is [DepthFormat.Depth24Stencil8](/api/Microsoft.Xna.Framework.Graphics.DepthFormat.html), where 24 bits are used for the depth data and 8 bits are used for the stencil data. [DepthFormat.Depth24Stencil8Single](/api/Microsoft.Xna.Framework.Graphics.DepthFormat.html) is a more unusual format where the 24 bits for the depth buffer are arranged as a floating point value. Use [DepthFormat.None](/api/Microsoft.Xna.Framework.Graphics.DepthFormat.html) if you don't want to create a depth buffer.

Use [DepthStencilState.DepthBufferEnable](xref:Microsoft.Xna.Framework.Graphics.DepthStencilState.DepthBufferEnable) to enable or disable depth buffering. Use the [DepthStencilState.DepthBufferFunction](xref:Microsoft.Xna.Framework.Graphics.DepthStencilState.DepthBufferFunction) to change the comparison function used for the depth test. Clear the depth buffer by passing [ClearOptions.DepthBuffer](/api/Microsoft.Xna.Framework.Graphics.ClearOptions.html) to [GraphicsDevice.Clear](/api/Microsoft.Xna.Framework.Graphics.GraphicsDevice.html#Microsoft_Xna_Framework_Graphics_GraphicsDevice_Clear_Microsoft_Xna_Framework_Color_).

In MonoGame there is no **DepthStencilBuffer** type. The runtime automatically creates a depth buffer when a render target is created, and you specify the format for the depth buffer in a render target's constructor along with the surface format. This prevents a render target from being created without a matching depth buffer.

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
