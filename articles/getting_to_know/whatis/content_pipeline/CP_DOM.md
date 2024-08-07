---
title: What is the Content Pipeline Document Object Model?
description: The MonoGame Content Document Object Model (DOM) represents the set of built-in classes that can be consumed by standard content processors.
requireMSLicense: true
---

The MonoGame Content Document Object Model (DOM) represents the set of built-in classes that can be consumed by standard content processors.

Currently, the DOM provides support for meshes, materials, textures, sprite-fonts, and animations. Outside of these, a custom importer can return a [ContentItem](xref:Microsoft.Xna.Framework.Content.Pipeline.ContentItem) with custom information in its opaque data, or a custom type you have developed.

The following diagram lists the complete Content DOM.

![Content DOM](../images/ContentPipelineTypes_small.png)

## See Also

- [What Is Content?](CP_Overview.md)  
- [What is the Content Pipeline?](CP_Architecture.md)  
- [Extending a Standard Content Processor](../../howto/content_pipeline/HowTo_Extend_Processor.md)  
- [Adding New Content Types](CP_Content_Advanced.md)  
