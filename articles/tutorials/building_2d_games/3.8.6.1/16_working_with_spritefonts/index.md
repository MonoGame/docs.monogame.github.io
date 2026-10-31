---
title: "Chapter 16: Working with SpriteFonts"
description: "Learn how to create and use SpriteFonts to render text in your MonoGame project, including loading custom fonts and controlling text appearance."
---

In [Chapter 06](../06_working_with_textures/index.md), you learned how to load and render textures to display sprites in your game. While images are essential for visual elements, most games also need text for things like scores, player instructions, dialogue, and UI elements. MonoGame provides the [**SpriteFont**](xref:Microsoft.Xna.Framework.Graphics.SpriteFont) class to handle text rendering, which works together with the familiar [**SpriteBatch**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) we have been using for drawing textures.

In this chapter, you will:

- Learn how MonoGame handles text rendering with SpriteFonts.
- Create a `SpriteFont` description using the Content Builder application.
- Load custom fonts for use in your game.
- Render text using various parameters to control appearance.
- Implement text rendering in our game.

We will first start by understanding how text rendering works in MonoGame.

## Understanding SpriteFonts

MonoGame processes fonts through the content pipeline to create a texture atlas of font characters. MonoGame uses the texture atlas approach rather than directly using system fonts for several important reasons:

- **Cross-platform Compatibility**: System fonts cannot be guaranteed to exist on all platforms.
- **Consistency**: Ensures that the text appears the same across all platforms.
- **GPU Rendering**: Graphics cards do not understand font formats directly; they can only render textures.
- **Performance**: Pre-rendering the glyphs to a texture atlas allow for faster rendering at runtime with no texture swapping.

A [**SpriteFont**](xref:Microsoft.Xna.Framework.Graphics.SpriteFont) in MonoGame consists of:

1. A texture atlas containing pre-rendered glyphs (characters).
2. Data that tracks the position, size, and spacing of each character.
3. Kerning information for adjusting spacing between specific character pairs.

The texture atlas approach means fonts are rendered as sprites, using the same [**SpriteBatch**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) system you learned about for drawing textures ([Chapter 7: Optimized Texture Rendering](../07_optimizing_texture_rendering/index.md)).  When you draw text, MonoGame is actually drawing small portions of the texture atlas for each character assembled together to form complete words and sentences.

## Creating a SpriteFont Description

To use text in your game, you first need to create a SpriteFont Description (*.spritefont*) file and process it through the content pipeline. To do this, we'll use the .NET *MonoGame SpriteFont Template* (`mgsf`).

> [!NOTE]
> For more information on the `mgsf` template and creating SpriteFont Description files, see [Setting Up the Content Builder > SpriteFonts](../../../../getting_started/content_pipeline/content_builder/setup.md#spritefonts).

For example, you can create a new SpriteFont Description file by opening a terminal and running the following command:

```sh
dotnet new mgsf -n MySpriteFont -o SpriteFonts --IncludeFont
```

This command performs the following actions:

1. Creates a new folder in the current directory called `SpriteFonts` (the `-o` option).
2. Creates a new SpriteFont Description file called `MySpriteFont.spritefont` (the `-n` option) and adds it to the `SpriteFonts` directory.
3. Creates a *.ttf* file for the default font (the `--IncludeFont` option). In the `mgsf` template, the default font is Roboto-Bold, so passing `--IncludeFont` creates `Roboto-Bold.ttf` and adds it in the `SpriteFonts` folder.

The default SpriteFont Description file created by the *MonoGame SpriteFont Template* looks something like this:

[!code-xml[](./snippets/spritefont_description.spritefont)]

When creating a SpriteFont Description for your game, you will need to make several important decisions about font selection, size, formatting, and licensing. The following sections will guide you through customizing the SpriteFont Description using these considerations.

### Customizing the SpriteFont

The SpriteFont Description file allows you to customize various aspects of how the font will be processed and appear in your game. Here are the key elements you can modify:

#### FontName

The `<FontName>` element specifies which font to use. The *MonoGame SpriteFont Template* references "Roboto-Bold". When a font name is specified just by name like this, the font must be installed on the system where the content is built.

Alternatively, for better portability across development environments, it is recommended instead to directly reference a TrueType (.ttf) or OpenType (.otf) font file.

> [!NOTE]
> If you pass the `--IncludeFont` option when using the *MonoGame SpriteFont Template*, the *.ttf* version of Roboto-Bold is automatically added next to your *.spritefont* file. However, to use the TrueType font rather than the system font, you still need to edit the `<FontName>` element of the *.spritefont* file to reference `Roboto-Bold.ttf` (with the extension).
> 
> If you want to use a font other than Roboto-Bold (as we'll show later in this tutorial), don't use the `--IncludeFont` option.

To directly reference a TrueType or OpenType font file in your *.spritefont* file:

1. Download or locate a TTF or OTF font file.
2. Place it in the **same folder** as the `.spritefont` file.
3. Update the `<FontName>` element to include the exact filename with extension. For example, to use the Roboto-Bold TrueType font, use `Roboto-Bold.ttf` as your `<FontName>`.

> [!TIP]
> Use fonts with permissive licenses (like [SIL Open Font License](https://openfontlicense.org/)) to ensure you can **legally** use them in your game.
>
> **Always check the license of any font you use!**

#### Size

The `<Size>` element controls the font size in points. While it might seem straightforward, font sizing requires consideration and can be dependent on several factors. When choosing a font size, consider:

- **Resolution impact**: Fonts that look good at 1080p may appear too small at 4K or too large at 720p.
- **Font style**: Pixel fonts look best with small sizes to preserve crispness.
- **Use case**: Different UI elements may require different sizes for proper hierarchy.

You may want to create multiple SpriteFont Description files for different use cases in your game such as:

- A larger font for headings and titles.
- A medium-sized font for standard UI elements.
- A smaller font for detailed information.

> [!TIP]
> Creating multiple SpriteFont Description files, however, can remove some of the benefits of fonts being a texture atlas since you will now have multiple atlases for each size. You will also now have multiple assets to manage both as asset files and references in code.
>
> An alternative approach is to create a single SpriteFont Description with a larger than needed size font, then scale it down during runtime in the game. This approach allows you to maintain the single SpriteFont Description file and single texture atlas, however, the size of the texture atlas will now be larger.
>
> There are tradeoffs to each approach and you should choose the one that works best for your game.

#### Spacing

The `<Spacing>` element adjusts the space between characters. The default value of 0 uses the font's built-in spacing. Positive values increase spacing, while negative values (though rarely used) can decrease it.

#### UseKerning

The `<UseKerning>` element determines whether to use kerning information from the font. Kerning adjusts the spacing between specific pairs of characters for more visually pleasing results. For most fonts, you will want to leave this as `true`.

> [!NOTE]
> While kerning typically improves text appearance, some fonts (including Arial) may not respond optimally to kerning adjustments. If you notice unusual character spacing with a particular font, try setting this value to `false`.

#### Style

The `<Style>` element sets the font style. Valid options are "Regular", "Bold", "Italic", or "Bold, Italic". Note that not all fonts have all styles available, and using a style that does not exist will fall back to Regular.

#### DefaultCharacter

The `<DefaultCharacter>` element (commented out by default) specifies what character to use as a fallback when trying to render a character that is not included in the font. This is useful for handling special characters or international text.

#### CharacterRegions

The `<CharacterRegions>` element defines which Unicode character ranges to include in the font. The default range (32-126) covers basic Latin characters, which is sufficient for English text. Including more characters increases the font texture size but allows support for more languages or special symbols.

For most games, the default range is sufficient.

> [!NOTE]
> Although for fun, TRY using the Wingdings font :D

## Loading a SpriteFont Description

To load a SpriteFont Description, we use the [**ContentManager.Load**](xref:Microsoft.Xna.Framework.Content.ContentManager.Load%60%601(System.String)) method with the [**SpriteFont**](xref:Microsoft.Xna.Framework.Graphics.SpriteFont) type:

```cs
// Loading a SpriteFont Description using the content pipeline
SpriteFont font = Content.Load<SpriteFont>("font");
```

## Drawing Text with SpriteBatch

MonoGame's [**SpriteBatch**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) class provides several overloads of the **DrawString** method to render text. The basic approach is similar to drawing textures, with a few unique parameters specific to text rendering.

The most basic DrawString overload looks like this:

[!code-csharp[](./snippets/drawstring_basic.cs)]

This overload contains the following parameters:

1. **font**: The [**SpriteFont**](xref:Microsoft.Xna.Framework.Graphics.SpriteFont) to use for rendering.
2. **text**: The text to display (as a string or `StringBuilder`).
3. **position**: A [**Vector2**](xref:Microsoft.Xna.Framework.Vector2) defining where to draw the text.
4. **color**: The [**Color**](xref:Microsoft.Xna.Framework.Color) to tint the text.

Just like with texture rendering, there are more advanced overloads that give you additional control:

[!code-csharp[](./snippets/drawstring_full.cs)]

> [!NOTE]
> Many of these parameters (`rotation`, `origin`, `scale`, `effects`, and `layerDepth`) work exactly the same way as they do for texture rendering, as explained in [Chapter 06](../06_working_with_textures/index.md). If you need a refresher on how these parameters affect rendering, refer back to that chapter.

## Calculating Text Dimensions

One common task when working with text is determining how much space it will occupy on screen. This is important for:

- Centering text
- Creating UI layouts
- Checking if text fits within a designated area
- Implementing text wrapping

MonoGame's [**SpriteFont**](xref:Microsoft.Xna.Framework.Graphics.SpriteFont) class provides the [**MeasureString**](xref:Microsoft.Xna.Framework.Graphics.SpriteFont.MeasureString(System.String)) method that returns the dimensions of rendered text:

[!code-csharp[](./snippets/measurestring.cs)]

The returned [**Vector2**](xref:Microsoft.Xna.Framework.Vector2) contains:

- `X`: The width of the text in pixels
- `Y`: The height of the text in pixels

Below is an example of centering text on the screen by drawing the text at the center of the screen and using [**MeasureString**](xref:Microsoft.Xna.Framework.Graphics.SpriteFont.MeasureString(System.String)) to calculate the center origin of the text:

[!code-csharp[](./snippets/center_example.cs)]

## Implementing Text in Our Game

To explore implementing text, we will add text to our game to display information to the player. We will add a score counter that increases when the slime eats the bat.

### Adding the SpriteFont Description

First, we will need to create a SpriteFont Description. To do this, we'll use the *MonoGame SpriteFont Template* we covered in the [Creating a SpriteFont Description](#creating-a-spritefont-description) section of this tutorial. 

1. Open a new terminal window, either in VSCode or your shell of choice.
2. Set the current directory to your `Content/Assets` directory. For example:

```sh
cd .\DungeonSlime\Content\Assets
```

3. Run the following command:

```sh
dotnet new mgsf -n fonts -o 04B_30
```

This command:

1. Creates a new `fonts` folder inside your `Assets` directory
2. Creates a new *04B_30.spritefont* file inside the `fonts` folder.

> [!NOTE]
> We have named the SpriteFont Description file with the same name as the font we will be using. This makes it easier to remember when updating the code later.

### Download the Font File

Next, right-click the following TTF font and choose "Save Link as...". Save the file in the same folder where you created the *04B_30.spritefont* file. This ensures the font is correctly linked and available for your SpriteFont configuration.

- [04B_30.ttf](./files/04B_30.ttf){download}

### Update the SpriteFont Description

Next, open the *04B_30.spritefont* file in your code editor and make the following changes:

[!code-csharp[](./snippets/04B_30.spritefont?highlight=4,5)]

The key changes here are:

1. The `<FontName>` element was updated to `04B_30.ttf`, the exact filename with extension of the TTF font we just downloaded.
2. The `<Size>` element was updated to be `17.5`.

### Update the Content Builder

Next, we need to update the Content Builder to properly import the SpriteFont Description file and include it in our build.

1. Open *Builder.cs* (`Content\Builder\Builder.cs`).
2. Edit the **GetContentCollection** method to include the following code:

[!code-csharp[](./snippets/builder.cs?highlight=17-21)]

This code tells the Content Builder to include any *.spritefont* files inside the `fonts` folder, and to exclude any *.ttf* fonts inside the `fonts` folder. When you build the game, the SpriteFont Description file is compiled and added to your output directory.

> [!IMPORTANT]
> Even though we excluded TrueType fonts from our build, you still need to keep the *04B_30.ttf* file in your `Assets/fonts` directory. The SpriteFont Description still needs this file to generate the font atlas. See [Setting Up the Content Builder > SpriteFonts](/articles/getting_started/content_pipeline/content_builder/setup.md#spritefonts) for more information.

### Update the Game

Finally, open the `Game1.cs` file and make the following changes:

[!code-csharp[](./snippets/game1.cs?highlight=48-58,93-99,128-129,244-245,386-397)]

The key changes made are:

1. The `_font` field was added to store the SpriteFont Description when loaded.
2. The `_score` field was added to track the player's score.
3. The `_scoreTextPosition` field was added to store the precalculated position to draw the score text at.
4. The `_scoreTextOrigin` field was added to store the origin to use when drawing the score text.
5. In [**Initialize**](xref:Microsoft.Xna.Framework.Game.Initialize):
   1. The position to draw the score text is precalculated to align with the left side of the room bounds horizontally and to vertically be at the center of the first tile at the top.
   2. The origin for the score text is precalculated by measuring the string `"Score"` and multiplying the `Y` component to get the vertical center of the text, then the origin is set so that it is at the left-center.
6. In [**LoadContent**](xref:Microsoft.Xna.Framework.Game.LoadContent), the font is loaded using the Content Manager.
7. In [**Update**](xref:Microsoft.Xna.Framework.Game.Update(Microsoft.Xna.Framework.GameTime)), the player's score is increased by `100` each time the slime eats the bat.
8. In [**Draw**](xref:Microsoft.Xna.Framework.Game.Draw(Microsoft.Xna.Framework.GameTime)), the score text is drawn at the precalculated position using the precalculated origin so that it aligns in the center of the tile on the blue strip using the sprite batch.

| ![Figure 16-2: The game with score displayed in the top-left corner](./videos/gameplay.webm) |
| :------------------------------------------------------------------------------------------: |
|            **Figure 16-2: The game with score displayed in the top-left corner**             |

## Conclusion

In this chapter, you accomplished the following:

- Learned how to create SpriteFont Definition files using the *MonoGame SpriteFont Template*.
- Learned how to edit the Content Builder to build SpriteFont files.
- Learned how to reference font files for better cross-platform compatibility.
- Learned how to load SpriteFonts through the content pipeline.
- Learned how to draw text with various parameters to control appearance.
- Learned how to measure text dimensions.
- Implemented a score display in our game.

In the next chapter we will discuss MonoGame's service container and how we can use it to start breaking our monolithic game file into modules for better maintainability.

## Test Your Knowledge

1. What are the key components of a SpriteFont in MonoGame?

    :::question-answer
    A SpriteFont in MonoGame consists of:
    1. A texture atlas containing pre-rendered glyphs (characters)
    2. Data that tracks the position, size, and spacing of each character
    3. Kerning information for adjusting spacing between specific character pairs
    :::

2. Why is it recommended to include the font file in your content project rather than referencing system fonts?

    :::question-answer
    Including the font file (TTF/OTF) directly in your content project and referencing it with the file extension ensures portability across different development environments. This approach does not depend on fonts being installed on the system where the content is built.
    :::

3. What method would you use to determine how much space a text string will occupy when rendered, and what does it return?

    :::question-answer
    The [**SpriteFont.MeasureString**](xref:Microsoft.Xna.Framework.Graphics.SpriteFont.MeasureString(System.String)) method is used to determine text dimensions. It returns a [**Vector2**](xref:Microsoft.Xna.Framework.Vector2) where the X component represents the width and the Y component represents the height of the rendered text in pixels.
    :::
