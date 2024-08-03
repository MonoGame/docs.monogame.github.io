---
title: How to Extend the Font Description Processor to Support Additional Characters?
description: Describes the process of developing a custom content processor needed to add additional characters to a FontDescription object based on the text that is required by the game.
requireMSLicense: true
---

In a font description (.spritefont) file, the `<CharacterRegions>` area can be used to add additional characters to a font description. This enables you to use a [SpriteFont](xref:Microsoft.Xna.Framework.Graphics.SpriteFont) to render an additional range of characters.

For some languages, this approach is not ideal. For example, Chinese and Japanese both have many thousands of characters. Adding the full range of characters to `<CharacterRegions>` dramatically increases the size of the font asset and the time required to build the font asset. A better solution adds individual characters whenever the specific characters are needed. You can create a custom content processor to implement this solution.

In this example, a file called _messages.txt_ contains all the text rendered by the game. The custom processor adds all the characters contained in the text in this file to a [FontDescription](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.FontDescription). Then it processes the object in the standard way using the base [FontDescriptionProcessor](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.FontDescriptionProcessor) functionality. All the characters in messages.txt will then be available to the [SpriteFont](xref:Microsoft.Xna.Framework.Graphics.SpriteFont) object at run time.

## Using the Font Description Processor

### To specify the character regions and messages to process

1. To add a new Sprite Font called DefaultFont to a game project, go to Solution Explorer, right-click the nested Content node, click **Add**, and then click **New Item**.

2. To add the new sprite font to the game, select the **Sprite Font** template, and then click **Add**.

3. Modify this file to use an existing font and any additional characteristics you prefer.

    For more information, see [Sprite Font XML Schema Reference](../../whatis/Content_Pipeline/CP_SpriteFontSchema.md).

4. Add a file named messages.txt to the game project.

5. Right-click on the game project node in Solution Explorer, click **Add**, and then click **New Item**.

6. Select the **Text File** template, enter **messages.txt** for the file name, and then click **Add** to add the text file to the game.

7. In the new text file, enter any messages that will be printed by the font described in the Sprite Font file.

    > We will use the method [File.ReadAllText](http://msdn.microsoft.com/en-us/library/ms143369.aspx) to read the text in this file. This method requires a carriage return ("\\r") or line feed ("\\n") after the last string, so be sure to follow the last line of text in the file with a carriage return or line feed.

### To create the new content processor project

The Content Pipeline is part of the build process, and it is separate from your game code. Therefore, you need to create a new assembly that contains the code developed in this topic. Creating this new assembly project is the first step in developing a new processor.

> **Tip**
>
> It is assumed that you have an existing game project that you will modify. For the purposes of this example, the game project is called "FontGame."

1. To add the new processor project to the game solution, go to Solution Explorer, right-click the **Solution** node, click **Add**, and then click **New Project**.

2. In the dialog box, select the template **Content Pipeline Extension Library (4.0)**, enter "FontProcessor" in the **Name** field, and then click **OK**. The new project automatically contains references to the XNA Framework run-time and design-time Content Pipeline assemblies.

### To extend the font processor

1. Add the following lines of code, after the last `using` statement:

    ```csharp
    using System.IO;
    using System.ComponentModel;
    ```

2. In ContentProcessor1.cs, remove the placeholder code that assigns the processor input (`TInput`) and output (`TOutput`) types. It will not be needed.

3. Using attributes, add a processor parameter to the beginning of the class declaration.

    This parameter stores the name of the text file that stores the messages displayed by the game.

    ```csharp
    [DefaultValue("messages.txt")]
    [DisplayName("Message File")]
    [Description("The characters in this file will be automatically added to the font.")]
    public string MessageFile
    {
        get { return messageFile; }
        set { messageFile = value; }
    }
    private string messageFile = "messages.txt";
    ```

4. Change the derivation of ContentProcessor1 from [ContentProcessor](xref:Microsoft.Xna.Framework.Content.Pipeline) to [FontDescriptionProcessor](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.FontDescriptionProcessor).

5. Modify the [Process](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.FontTextureProcessor) method override to match the following code:

    ```csharp
    public override SpriteFontContent Process(FontDescription input, ContentProcessorContext context)
    ```

    This modification replaces the template parameter and return types with the proper types needed for the extended font processor.

6. Register a Content Pipeline dependency on messages.txt.

    This dependency tells the Content Pipeline that if messages.txt changes, the font must be rebuilt.

    ```csharp
    string fullPath = Path.GetFullPath(MessageFile);
    
    context.AddDependency(fullPath);
    ```

7. Read the contents of the file, and add each letter to the input font one by one. Note that the [Characters](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.FontDescription) collection keeps track of duplicates automatically. It is not necessary for the user to make sure that each letter is added only once. The **Characters** collection will contain only one instance of each character, no matter how many times **Add** has been called.

    ```csharp
    string letters = File.ReadAllText(fullPath, System.Text.Encoding.UTF8);
    
    foreach (char c in letters)
    {
        input.Characters.Add(c);
    }
    ```

    In this example, messages.txt has been saved with Unicode UTF-8 encoding, which is why this encoding format is specified in the call to [File.ReadAllText](http://msdn.microsoft.com/en-us/library/ms143369.aspx). The default file encoding format for text files that have been added to a Visual Studio project is Western European (Windows) encoding, corresponding to code page 1252. If your text file uses the default encoding, specify the character encoding as follows:

    string letters = File.ReadAllText( fullPath, System.Text.Encoding.GetEncoding( 1252
                ) );

8. Call the existing **Process** method of the base [FontDescriptionProcessor](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.FontDescriptionProcessor) to build the font with the newly requested characters.

    ```csharp
    return base.Process(input, context);
    ```

### To associate the custom font processor with the sprite font

1. Compile the solution to build **MyFontProcessor**.

    Now you need to add your custom font processor as an available content processor for the game.

2. From **Solution Explorer**, right-click the **Content** node, and then click **Add Reference**.

3. From the **Projects** tab, select your content extension project (**FontProcessor**) node, and click **OK**.

    To ensure that the processor project is always up to date when the main game is built, you need to create a project dependency.

4. In Solution Explorer, right-click the game project (**FontGame**) node, and then click **Project Dependencies**.

5. Select the check box next to **FontProcessor**, and then click **OK** to add a new dependency so that **FontGame** depends on **FontProcessor**.

6. Change the content processor for the .spritefont file from **Sprite Font Description - XNA Framework** to the newly created processor.

7. Select the .spritefont file, and then in the **Properties** window, choose your custom processor from the drop-down list associated with the **ContentProcessor** field.

When you build the solution, the new processor adds the characters in the messages.txt file to the list of characters available to the [SpriteFont](xref:Microsoft.Xna.Framework.Graphics.SpriteFont).

> **Note**
>
> To debug a Content Pipeline importer or processor, add the following line to the processor code to launch the debugger.
>
> ```csharp
> System.Diagnostics.Debugger.Launch();
> ```

## See Also

[Extending a Standard Content Processor](./HowTo_Extend_Processor.md)
[Adding New Content Types](../../whatis/Content_Pipeline/CP_Content_Advanced.md)
