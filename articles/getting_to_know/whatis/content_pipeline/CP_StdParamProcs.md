---
title: What are the Parameterized Content Processors?
description: Describes how parameterized Content Processors work in MonoGame.
---

# Parameterized Content Processors

Describes how parameterized Content Processors work in MonoGame. Many of the standard Content Pipeline Content Processors shipped with XNA Game Studio support parameter usage. Parameterization makes any standard or custom Content Processor more flexible and better able to meet the needs of your XNA Framework application. In addition to specifying values for standard parameters, you can easily implement parameter support for a new or an existing custom Content Processor. For more information, see [Developing with Parameterized Processors](CP_CustomParamProcs.md).

When you select a game asset, the Properties window displays the parameters for the related Content Processor. Use the Properties window at any time to modify these parameter values.

> **Tip**
>
> If you change the Content Processor for a game asset to a different Content Processor, all parameter values are reset to their default values. This means that if you modify the **Generate Mipmaps** parameter value for the [TextureProcessor](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.TextureProcessor) and then switch to a different Content Processor (for example, [FontTextureProcessor Class](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.FontTextureProcessor)), the parameters switch to the default values for that Content Processor. If you then switch back again, the modified values are reset to the default values of the original Content Processor. _The values do not revert to the modified values you set originally_.

## Standard Parameterized Content Processors

The following describes only standard Content Processors that accept parameters, the parameter types, and their default value. For more information about all standard Content Processors, see [Standard Content Importers and Content Processors](CP_StdImpsProcs.md).

| Friendly name| Type name | Input type | Output type | Description |
|-------------------|-----------|------------|-------------|------------------------------|
| Model | [ModelProcessor](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.ModelProcessor)| [NodeContent Class](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.NodeContent)| [ModelContent Class](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.ModelContent) |A parameterized Content Processor that outputs models as a [ModelContent Class](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.ModelContent) object.<br>Available parameters:<br>*Color Key Color–Any valid [Color](xref:Microsoft.Xna.Framework.Color). [Magenta](xref:Microsoft.Xna.Framework.Color) is the default value.<br>*   Color Key Enabled–A Boolean value indicating if color keying is enabled. The default value is **true**.<br>*Generate Mipmaps–A Boolean value indicating if mipmaps are generated. The default value is **false**.<br>*   Generate Tangent Frames–A Boolean value indicating if tangent frames are generated. The default value is **false**.<br>*Resize Textures to Power of Two–A Boolean value indicating if a texture is resized to the next largest power of 2. The default value is **false**.<br>*   Scale–Any valid [float](http://msdn.microsoft.com/en-us/library/system.single.aspx) value. The default value is 1.0.<br>*Swap Winding Order–A Boolean value indicating if the winding order is swapped. This is useful for models that appear to be drawn inside out. The default value is **false**.<br>*   Texture Format–Any valid value from [TextureProcessorOutputFormat](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.TextureProcessorOutputFormat). Textures are either unchanged, converted to the Color format, or Compressed using the specified Compression algorithm.<br>*X Axis Rotation–Number, in degrees of rotation. The default value is 0.<br>*   Y Axis Rotation–Number, in degrees of rotation. The default value is 0.<br>*   Z Axis Rotation–Number, in degrees of rotation. The default value is 0.
| Sprite Font Texture|[FontTextureProcessor](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.FontTextureProcessor)|[TextureContent Class](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.TextureContent)|[SpriteFontContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.SpriteFontContent)|A parameterized Content Processor that outputs a sprite font texture as a [SpriteFontContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.SpriteFontContent) object.<br>Available parameters:<br><br>*   First Character–Any valid character. The space character is the default value.|
| Texture|[TextureProcessor](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.TextureProcessor)|[TextureContent Class](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.TextureContent)|[TextureContent Class](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.TextureContent)|A parameterized Content Processor that outputs textures as a [TextureContent Class](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.TextureContent) object.<br>Available parameters:<br><br>*Color Key Color–Any valid [Color](xref:Microsoft.Xna.Framework.Color). [Magenta](xref:Microsoft.Xna.Framework.Color) is the default value.<br>*   Color Key Enabled–A Boolean value indicating if color keying is enabled. The default value is **true**.<br>*Generate Mipmaps–A Boolean value indicating if mipmaps are generated. The default value is **false**.<br>*   Resize to Power of Two–A Boolean value indicating if a texture is resized to the next largest power of 2. The default value is **false**.<br>*   Texture Format–Any valid value from [TextureProcessorOutputFormat](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.TextureProcessorOutputFormat). Textures are unchanged, converted to the **Color** format, or Compressed using the specified Compression algorithm.|

## See Also

- [Adding Content to a Game](../../howto/Content_Pipeline/HowTo_GameContent_Add.md)  

---

© 2012 Microsoft Corporation. All rights reserved.

© 2023 The MonoGame Foundation.
