---
title: What are the Standard Content Importers and Content Processors?
description: Describes the standard Content Pipeline Content Importers and Content Processors of MonoGame that support various common art asset file formats.
---

# Standard Content Importers and Content Processors

Describes the standard Content Pipeline Content Importers and Content Processors of MonoGame that support various common art asset file formats.

Content Importers and Content Processors are implemented as assemblies. In addition to the standard ones provided by MonoGame and listed below, you can also use custom Content Importers and Content Processors developed by you or third parties. Use the Properties window to assign an appropriate Content Importer and Content Processor to each game asset added to your game project.

## Standard Content Importers

The following is a description of the standard Content Importers shipped with MonoGame and the type of game asset each supports.

All standard Content Importers are declared as part of the Microsoft.Xna.Framework.Content.Pipeline namespace.

| Name | Type name | Output type | Description
| ------------- |:-------------:| :----- | :---- |
| Autodesk FBX|FbxImporter|[NodeContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.NodeContent)|Imports game assets specified with the Autodesk FBX file format (.fbx). This Content Importer is designed to work with assets exported with the 2013 version of the FBX exporter. |
| Effect|EffectImporter|[EffectContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.EffectContent)|Imports a game asset specified with the DirectX Effect file format (.fx). |
| Sprite Font Description|FontDescriptionImporter|[FontDescription](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.FontDescription)|Imports a font description specified in a .spritefont file.|
| Texture|TextureImporter|[TextureContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.TextureContent)|Imports a texture. These file types are supported: .bmp, .dds, .dib, .hdr, .jpg, .pfm, .png, .ppm, and .tga.|
| X File|XImporter|[NodeContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.NodeContent)|Imports game assets specified with the DirectX X file format (.x). This Content Importer expects the coordinate system to be left-sided.|
| XML Content|XmlImporter|object|Imports XML content used for editing the values of a custom object at run time. For instance, you could pass XML code to this Content Importer that looks for the specified property of a custom type and changes it to the specified value. You could then process the custom object with a Content Processor or pass it to your game untouched using the No Processing Required Content Processor.This Content Importer is designed for scenarios like importing an XML file that describes game data at run time (similar to the Sprite Font Description Content Importer) or importing terrain data in an XML file that then is passed to a Content Processor that generates a random terrain grid using that data.|
| Other 3D Content|OpenAssetImporter|[NodeContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.NodeContent)|Imports game assets specified with one of the formats supported by assimp. A sample of supported files types are: .dae, .3ds, .blend, .obj, .fbx (v2013). More are available see [Assimp Supported File Formats](https://github.com/assimp/assimp#supported-file-formats) for more details. Note some formats might not behave correctly with the standard [ModelProcessor](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.ModelProcessor).|

# Standard Content Processors

MonoGame ships with a variety of Content Processors that support several common game asset types. Many of the standard Content Processors, such as the [TextureProcessor](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.TextureProcessor), support parameters for modifying the default behavior of the Content Processor. For more information, see [Parameterized Content Processors](CP_StdParamProcs.md).

The following describes the standard Content Processors and the type of game asset each supports.

| Name| Type name| Input type| Output type| Description|
| ----------------- |:-------------:| :----- | :---- | :---- |
| Model|[ModelProcessor](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.ModelProcessor)|[NodeContent Class](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.NodeContent)|[ModelContent Class](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.ModelContent)|A parameterized Content Processor that outputs models as a [ModelContent Class](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.ModelContent) object.<br>Available parameters:<br>*   Color Key Color–Any valid [Color](xref:Microsoft.Xna.Framework.Color). [Magenta](xref:Microsoft.Xna.Framework.Color) is the default value.<br>*   Color Key Enabled–A Boolean value indicating if color keying is enabled. The default value is **true**.<br>*   Generate Mipmaps–A Boolean value indicating if mipmaps are generated. The default value is **false**.<br>*   Generate Tangent Frames–A Boolean value indicating if tangent frames are generated. The default value is **false**.<br>*   Resize Textures to Power of Two–A Boolean value indicating if a texture is resized to the next largest power of 2. The default value is **false**.<br>*   Scale–Any valid [float](http://msdn.microsoft.com/en-us/library/system.single.aspx) value. The default value is 1.0.<br>*   Swap Winding Order–A Boolean value indicating if the winding order is swapped. This is useful for models that appear to be drawn inside out. The default value is **false**.<br>*   Texture Format–Any valid [SurfaceFormat](xref:Microsoft.Xna.Framework.Graphics.SurfaceFormat) value. Textures are either unchanged, converted to the Color format, or DXT Compressed. For more information, see [TextureProcessorOutputFormat](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.TextureProcessorOutputFormat).<br>*   X Axis Rotation–Number, in degrees of rotation. The default value is 0.<br>*   Y Axis Rotation–Number, in degrees of rotation. The default value is 0.<br>*   Z Axis Rotation–Number, in degrees of rotation. The default value is 0.|
|No Processing Required|[PassThroughProcessor](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.PassThroughProcessor)|Object|Object|Performs no processing on the file. Select this Content Processor if your content is already in a game-ready format (for example, an externally prepared DDS file) or a specialized XML format (.xml) designed for use with XNA Game Studio.|
|Sprite Font Description|[FontDescriptionProcessor](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.FontDescriptionProcessor)|[FontDescription](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.FontDescription)|[SpriteFontContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.SpriteFontContent)|Converts a .spritefont file specifying a font description into a font.|
|Sprite Font Texture|[FontTextureProcessor](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.FontTextureProcessor)|[TextureContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.TextureContent)|[SpriteFontContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.SpriteFontContent)|A parameterized Content Processor that outputs a sprite font texture as a [SpriteFontContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.SpriteFontContent) object.<br>Available parameters:<br>*   First Character–Any valid character. The space character is the default value.|
| Sprite Font Texture|[FontTextureProcessor](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.FontTextureProcessor)|[Texture2DContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.Texture2DContent)|[SpriteFontContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.SpriteFontContent)|Converts a specially marked 2D bitmap file (.bmp) into a font. Pixels of **Color.Magenta** are converted to **Color.Transparent**.|
| Texture|[TextureProcessor](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.TextureProcessor)|[TextureContent Class](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.TextureContent)|[TextureContent Class](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.TextureContent)|A parameterized Content Processor that outputs textures as a [TextureContent Class](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.TextureContent) object.<br>Available parameters:<br>*   Color Key Color–Any valid [Color](xref:Microsoft.Xna.Framework.Color). [Magenta](xref:Microsoft.Xna.Framework.Color) is the default value.<br>*   Color Key Enabled–A Boolean value indicating if color keying is enabled. The default value is **true**.<br>*   Generate Mipmaps–A Boolean value indicating if mipmaps are generated. The default value is **false**.<br>*   Resize to Power of Two–A Boolean value indicating if a texture is resized to the next largest power of 2. The default value is **false**.<br>*   Texture Format–Any valid [SurfaceFormat](xref:Microsoft.Xna.Framework.Graphics.SurfaceFormat) value. Textures are either unchanged, converted to the Color format, or DXT Compressed. For more information, see [TextureProcessorOutputFormat](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.TextureProcessorOutputFormat).|
|Localized Sprite Font Texture|[LocalizedFontProcessor](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.LocalizedFontProcessor)|[FontDescription](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.FontDescription)|[SpriteFontContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.SpriteFontContent)|Converts a .spritefont file specifying a font description into a font.|

## See Also

- [Adding Content to a Game](../../howto/Content_Pipeline/HowTo_GameContent_Add.md)  
- [What Is Content?](CP_Overview.md)  
- [Adding a Custom Importer](CP_AddCustomProcImp.md)  

---

© 2012 Microsoft Corporation. All rights reserved.

© 2023 The MonoGame Foundation.
