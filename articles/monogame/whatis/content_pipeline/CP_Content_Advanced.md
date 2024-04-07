---
title: What are custom content types?
description: Content Builders and Processors can be customized to handle almost any content type.
---

# Adding New Content Types

The MonoGame Content Pipeline builds game assets included in your project into a form your game can load at run time.

This game asset build process is controlled by Content Pipeline importers and content processors. When you press F5 to build a game created with MonoGame, the appropriate Content Pipeline importer and processor for each asset is invoked. Each asset then is automatically built into your game.

The flexibility of this process enables you to create and update game assets using a variety of digital content creation (DCC) tools. MonoGame supplies importers for several popular export formats supported by DCC tools, and also lets you develop custom importers for other formats.

## In This Section

- [What is the Content Pipeline Architecture?](CP_Architecture.md)

    Describes the components and execution flow of the MonoGame Content Pipeline.

- [Content Pipeline Document Object Model](CP_DOM.md)

    Describes the built-in object support features for assets in the Content Pipeline.

- [Loading Additional Content Types](CP_Customizing.md)

    Describes the options for customizing the Content Pipeline to support nonstandard game assets or special-purpose needs.

- [Tips for Developing Custom Importers and Processors](CP_Tips_For_Developing.md)

    Provides useful information about how to design and debug a custom Content Pipeline importer or processor.

- [Adding a Custom Importer](CP_AddCustomProcImp.md)

    Describes how to add a custom processor or importer to an existing game solution.

- [Extending a Standard Content Processor](../../howto/Content_Pipeline/HowTo_Extend_Processor.md)

    Describes how MonoGame lets you modify or extend the behavior of any standard Content Pipeline processor that ships with the product.

- [Developing with Parameterized Processors](CP_CustomParamProcs.md)

    Describes how to develop with parameterized processors, both standard and custom.

- [How to: Extend the Font Description Processor to Support Additional Characters](../../howto/Content_Pipeline/HowTo_ExtendFontProcessor.md)

    Describes the process of developing a custom content processor needed to add additional characters to a [FontDescription](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.FontDescription) object based on the text that is required by the game.

---

© 2012 Microsoft Corporation. All rights reserved.

© 2023 The MonoGame Foundation.
