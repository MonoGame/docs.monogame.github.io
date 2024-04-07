---
title: Drawing Text with a Sprite
description: Demonstrates how to import a SpriteFont into a project and to draw text using DrawString
---

# Drawing Text with a Sprite

Demonstrates how to import a [SpriteFont](xref:Microsoft.Xna.Framework.Graphics.SpriteFont) into a project and to draw text using [SpriteBatch.DrawString](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_DrawString_Microsoft_Xna_Framework_Graphics_SpriteFont_System_String_Microsoft_Xna_Framework_Vector2_Microsoft_Xna_Framework_Color_).

## Adding a Sprite Font and Drawing Text

1. Double click on your Content.mgcb file in Solution Explorer, click **New Item** Button.

2. In the **Add New Item** dialog box, select **Sprite Font Description** and add the filename in the edit box at the top of the dialog.

   You may find it convenient at this point to change the name of the new file from "SpriteFont1" to the friendly name of the font you intend to load (keeping the .spritefont file extension). The friendly name identifies the font once it is installed on your computer, for example, "Courier New" or "Times New Roman." When you reference the font in your code, you must use the friendly name you have assigned it.
   Pipeline tool creates a new .spritefont file for your font.

3. Right click on your new font file in the Pipeline project explorer and select Open (or Open With to choose your preferred editor).

4. If you did not name the new file with the font's friendly name, type the friendly name of the font to load into the FontName element.
   Again, this is not the name of a font file, but rather the name that identifies the font once it is installed on your computer. You can use the Fonts folder in the **Control Panel** to see the names of fonts installed on your system, and to install new ones. The content pipeline supports the same fonts as the [System.Drawing.Font](http://msdn.microsoft.com/en-us/library/system.drawing.font.aspx) class, including TrueType fonts, but not bitmap (.fon) fonts. You may find it convenient to save the new .spritefont file using this friendly name. When you reference the font in your code, you must use the friendly name you have assigned it.

   If you want to use a custom font, you are put the .ttf or .oft in the same directory as the .spritefont file and the build system will pick it up. There is no need to install the font system wide.

5. If necessary, change the **Size** entry to the point size you desire for your font.

6. If necessary, change the **Style** entry to the style of font to import.
   You can specify **Regular**, **Bold**, **Italic**, or **Bold, Italic**. The **Style** entry is case sensitive.

7. Specify the character regions to import for this font.

   Character regions specify which characters in the font are rendered by the [SpriteFont](xref:Microsoft.Xna.Framework.Graphics.SpriteFont). You can specify the start and end of the region by using the characters themselves, or by using their decimal values with an &# prefix. The default character region includes all the characters between the space and tilde characters, inclusive.

### To draw text on the screen

1. Add a Sprite Font to your project as described above.

2. Create a [SpriteFont](xref:Microsoft.Xna.Framework.Graphics.SpriteFont) object to encapsulate the imported font.

3. Create a [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) object for drawing the font on the screen.

4. In your [Game.LoadContent](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_LoadContent) method, call [ContentManager.Load](xref:Microsoft.Xna.Framework.Content.ContentManager#Microsoft_Xna_Framework_Content_ContentManager_Load__1_System_String_), specifying the [SpriteFont](xref:Microsoft.Xna.Framework.Graphics.SpriteFont) class and the asset name of the imported font.

5. Create your [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) object, passing the current [GraphicsDevice](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice).

    ```csharp
    SpriteFont Font1;
    Vector2 FontPos;
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);
        Font1 = Content.Load<SpriteFont>("Courier New");
    
        // TODO: Load your game content here            
        FontPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2,
            graphics.GraphicsDevice.Viewport.Height / 2);
    }
    ```

6. In your [Game.Draw](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Draw_Microsoft_Xna_Framework_GameTime_) method, call [SpriteBatch.Begin](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_Begin_Microsoft_Xna_Framework_Graphics_SpriteSortMode_Microsoft_Xna_Framework_Graphics_BlendState_Microsoft_Xna_Framework_Graphics_SamplerState_Microsoft_Xna_Framework_Graphics_DepthStencilState_Microsoft_Xna_Framework_Graphics_RasterizerState_Microsoft_Xna_Framework_Graphics_Effect_System_Nullable_Microsoft_Xna_Framework_Matrix__) on the [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) object.

7. If necessary, determine the origin of your text.

   If you want to draw your text centered on a point, you can find the center of the text by calling [SpriteFont.MeasureString](xref:Microsoft.Xna.Framework.Graphics.SpriteFont#Microsoft_Xna_Framework_Graphics_SpriteFont_MeasureString_System_String_) and dividing the returned vector by 2.

8. Call [SpriteBatch.DrawString](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_DrawString_Microsoft_Xna_Framework_Graphics_SpriteFont_System_String_Microsoft_Xna_Framework_Vector2_Microsoft_Xna_Framework_Color_) to draw your output text, specifying the [SpriteFont](xref:Microsoft.Xna.Framework.Graphics.SpriteFont) object for the font you want to use.
  
   All other parameters of [SpriteBatch.DrawString](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_DrawString_Microsoft_Xna_Framework_Graphics_SpriteFont_System_String_Microsoft_Xna_Framework_Vector2_Microsoft_Xna_Framework_Color_) produce the same effects as a call to [SpriteBatch.Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_Draw_Microsoft_Xna_Framework_Graphics_Texture2D_Microsoft_Xna_Framework_Vector2_Microsoft_Xna_Framework_Color_).

9. Call [SpriteBatch.End](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_End) after all text is drawn.

    ```csharp
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
    
        spriteBatch.Begin();
    
        // Draw Hello World
        string output = "Hello World";
    
        // Find the center of the string
        Vector2 FontOrigin = Font1.MeasureString(output) / 2;
        // Draw the string
        spriteBatch.DrawString(Font1, output, FontPos, Color.LightGreen,
            0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
    
        spriteBatch.End();
        base.Draw(gameTime);
    }
    ```

## See Also

### Tasks

[Drawing a Sprite](HowTo_Draw_A_Sprite.md)  

#### Concepts

[What Is a Sprite?](../../whatis/graphics/WhatIs_Sprite.md)

#### Reference

[SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch)  
[SpriteBatch.DrawString](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch#Microsoft_Xna_Framework_Graphics_SpriteBatch_DrawString_Microsoft_Xna_Framework_Graphics_SpriteFont_System_String_Microsoft_Xna_Framework_Vector2_Microsoft_Xna_Framework_Color_)  
[SpriteFont](xref:Microsoft.Xna.Framework.Graphics.SpriteFont)  
[ContentManager.Load](xref:Microsoft.Xna.Framework.Content.ContentManager#Microsoft_Xna_Framework_Content_ContentManager_Load__1_System_String_)  

---

© 2012 Microsoft Corporation. All rights reserved.

© 2023 The MonoGame Foundation.
