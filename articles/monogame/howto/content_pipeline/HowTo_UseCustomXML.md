---
title: How to use an XML File to Specify Content?
description: Game assets managed through the Content Pipeline include graphic items such as textures, models and meshes; sound files such as dialogue or music; and custom data that governs the behavior of the game.
requireMSLicense: true
---

Data tables, for example, are custom data that might describe different characters’ attributes or the features of each level in the game. The content and format of this data is specific to the requirements of the game. Custom game data in the form of an XML file also can be loaded into your game through the standard features of the Content Pipeline.

When the Content Pipeline is used, the game does not have to parse the XML format in which the game data is originally stored. Data loaded by the game through [ContentManager](xref:Microsoft.Xna.Framework.Content.ContentManager) is read in deserialized form directly into a managed code object.

## In This Section

- [How to Add custom game data as an XML file](HowTo_Add_XML.md)

    Describes how to add custom game data as an XML file through the Content Pipeline.

- [Generating a custom XML File](HowTo_GenerateCustomXML.md)

    Describes how to use [IntermediateSerializer](xref:Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate.IntermediateSerializer) from a Windows application to generate XML content to add to a MonoGame application.

- [Adding an XML Content File to a MonoGame Project](HowTo_GameContent_Add.md)

    Describes how to add custom game data as an XML file through the Content Pipeline.

- [Loading XML Content at Runtime](HowTo_Load_XML.md)

    Describes how to load custom game data at game runtime through the Content Pipeline.

- [XML Elements for XMLImporter](../../whatis/Content_Pipeline/CP_XML_Elements.md)

    Describes the elements of an XML file that can be processed by the [XmlImporter Class](xref:Microsoft.Xna.Framework.Content.Pipeline.XmlImporter).

- [Sprite Font XML Schema Reference](../../whatis/Content_Pipeline/CP_SpriteFontSchema.md)

    Describes the valid tags and values for Sprite-Font (.spritefont) XML files used by the Content Pipeline to create [SpriteFont](xref:Microsoft.Xna.Framework.Graphics.SpriteFont) textures.

## See Also

### Concepts

[Adding Content to a Game](./HowTo_GameContent_Add.md)
