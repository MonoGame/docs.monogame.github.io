---
title: What is Sprite Font XML Schema Reference?
description: Describes the valid tags and values for Sprite-Font (.spritefont) XML files used by the Content Pipeline to create SpriteFont textures.
---

# Sprite Font XML Schema Reference

Describes the valid tags and values for Sprite-Font (.spritefont) XML files used by the Content Pipeline to create [SpriteFont](xref:Microsoft.Xna.Framework.Graphics.SpriteFont) textures.

|Tag name|Content type|Content description|
|-|-|-|
|`<FontName>`|string|The name of the font to be imported. This is not the name of a font file, but rather the friendly name that identifies the font once it is installed on your computer. You can use the **Fonts** folder in Control Panel to see the names of fonts installed on your system, and to install new ones as well. The Content Pipeline supports the same fonts as the [System.Drawing.Font](http://msdn.microsoft.com/en-us/library/system.drawing.font.aspx) class, including TrueType fonts, but not bitmap (.fon) fonts.|
|`<Size>`|float|The point size of the font to be imported.|
|`<Spacing>`|float|The number of pixels to add between each character when the string is rendered.|
|`<UseKerning>`|Boolean|Specifies whether to use kerning information when rendering the font. Default value is **true**.|
|`<Style>`|Regular, Bold, Italic, or Bold Italic|The style of the font to be imported.|
|`<DefaultCharacter>`|char|The Unicode character to substitute any time an attempt is made to render characters that are not in the font. Specifying this element is optional.|
|`<CharacterRegions>`|One or more `<CharacterRegion>` tags|One or more numerical ranges indicating which subset of Unicode characters to import.|
|`<CharacterRegion>`|One `<Start>` and one `<End>` tag|The beginning and ending of a region of Unicode characters.|
|`<Start>`|char|The first Unicode character to include in a `<CharacterRegion>`.|
|`<End>`|char|The last Unicode character to include in a `<CharacterRegion>`.|

## Example

Here is a sample .spritefont file:

```xml
    <?xml version="1.0" encoding="utf-8"?>
    <XnaContent xmlns:Graphics="Microsoft.Xna.Framework.Content.Pipeline.Graphics">
      <Asset Type="Graphics:FontDescription">
        <FontName>Courier New</FontName>
        <Size>18</Size>
        <Spacing>0</Spacing>
        <UseKerning>true</UseKerning>
        <Style>Regular</Style>
        <CharacterRegions>
          <CharacterRegion>
            <Start>32</Start>
            <End>127</End>
          </CharacterRegion>
        </CharacterRegions>
      </Asset>
    </XnaContent>
```

Here is a sample localized .spritefont file:

```xml
    <?xml version="1.0" encoding="utf-8"?>
    <XnaContent xmlns:Graphics="Microsoft.Xna.Framework.Content.Pipeline.Graphics">
      <Asset Type="Graphics:LocalizedFontDescription">
        <FontName>Courier New</FontName>
        <Size>18</Size>
        <Spacing>0</Spacing>
        <UseKerning>true</UseKerning>
        <Style>Regular</Style>
        <CharacterRegions>
          <CharacterRegion>
            <Start>32</Start>
            <End>127</End>
          </CharacterRegion>
        </CharacterRegions>
        <ResourceFiles>
          <Resx>Strings.resx</Resx>
          <Resx>Strings-fr.resx</Resx>
        </ResourceFiles>
      </Asset>
    </XnaContent>
```

## See Also

- [Adding Content to a Game](../../howto/Content_Pipeline/HowTo_GameContent_Add.md)  

### Reference

- [SpriteFont](xref:Microsoft.Xna.Framework.Graphics.SpriteFont)  

---

© 2012 Microsoft Corporation. All rights reserved.

© 2023 The MonoGame Foundation.
