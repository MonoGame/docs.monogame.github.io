---
title: What Is a Back Buffer?
description: The definition of a Back Buffer for MonoGame!
requireMSLicense: true
---

A back buffer is a [render target](./WhatIs_Render_Target.md) whose contents will be sent to the device when [GraphicsDevice.Present](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice.Present) is called.

The graphics pipeline renders to a render target, the particular render target that the device presents to the display is called the back buffer. Use the [BackBufferWidth](xref:Microsoft.Xna.Framework.Graphics.PresentationParameters.BackBufferWidth) and [BackBufferHeight](xref:Microsoft.Xna.Framework.Graphics.PresentationParameters.BackBufferHeight) properties to get the back buffer dimensions. Render directly to the back buffer or to a render target by configuring the device using [GraphicsDevice.SetRenderTarget](/api/Microsoft.Xna.Framework.Graphics.GraphicsDevice.html#Microsoft_Xna_Framework_Graphics_GraphicsDevice_SetRenderTarget_Microsoft_Xna_Framework_Graphics_RenderTarget2D_) and [GraphicsDevice.SetRenderTargets](/api/Microsoft.Xna.Framework.Graphics.GraphicsDevice.html#Microsoft_Xna_Framework_Graphics_GraphicsDevice_SetRenderTargets_Microsoft_Xna_Framework_Graphics_RenderTargetBinding___).

* For Windows, the back buffer is created to match the dimensions of the [ClientBounds](xref:Microsoft.Xna.Framework.GameWindow.ClientBounds) by default.
* For Consoles, the back buffer is created with the dimensions that have been specified by the user. When going into full-screen mode on Windows.
* On Mobile, it is recommended to set the BackBuffer dimensions to the expected resolution based on the desired orientation of the device (Portrait or Landscape).

It is often desirable to set the back buffer dimensions to match the [DisplayMode](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice.DisplayMode) dimensions so that the game ("display") resolution does not change when it goes into the full-screen mode.

The back buffer created for consoles is not necessarily the same size as the final resolution on a television connected for display. Consoles automatically scale output to the television resolution selected by the user in the System. If the aspect ratio of the back buffer is different from the aspect ratio of the television display mode, the console will automatically add black bars (also called letter-boxing) if the user's display is not widescreen.

In addition, if you request a back-buffer resolution that is not supported by the output device, the MonoGame framework automatically selects the highest resolution supported by the output device. For example, if you create a back-buffer with a resolution of 1920x1080 (for example, 1080p or 1080i) and display it on a device with 480i resolution, the back-buffer is resized to 480i automatically.

## See Also

[Viewport](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice.Viewport)
