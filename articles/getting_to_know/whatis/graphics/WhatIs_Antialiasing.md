---
title: What is Antialiasing?
description: The definition of Antialsing for MonoGame!
requireMSLicense: true
---

Antialiasing is a technique for softening or blurring sharp edges so they appear less jagged when rendered.

Antialiasing is accomplished by multi-sampling each pixel at multiple pixel locations and combining the samples to generate a final pixel color. Increasing the number of samples per pixel increases the amount of antialiasing which generates a smoother edge:

* 4x multisampling requires four samples per pixel.
* 2x multisampling requires two sampler per pixel.
* And so on.

Use the [MultiSampleCount](xref:Microsoft.Xna.Framework.Graphics.PresentationParameters.MultiSampleCount) property of the [PresentationParameters](xref:Microsoft.Xna.Framework.Graphics.PresentationParameters) class to set the number of samples for the back buffer.

Set the [PreferMultiSampling](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.PreferMultiSampling) property on the [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager) class to **true** to enable multi-sampling for the back buffer.

> [!IMPORTANT]
> This will be ignored if the hardware does not support multi-sampling.

## See Also

[Enabling Anti-aliasing (Multi-sampling)](../../howto/graphics/HowTo_Enable_Anti_Aliasing.md)  
