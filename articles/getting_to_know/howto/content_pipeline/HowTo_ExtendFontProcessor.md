---
title: How to Extend the Font Description Processor to Support Additional Characters?
description: Describes the process of developing a custom content processor needed to add additional characters to a FontDescription object based on the text that is required by the game.
requireMSLicense: true
---

In a font description (.spritefont) file, the `<CharacterRegions>` area can be used to add additional characters to a font description. This enables you to use a [SpriteFont](xref:Microsoft.Xna.Framework.Graphics.SpriteFont) to render an additional range of characters.

> [!IMPORTANT]
> For the MGCB system (MGCB Editor), all custom pipeline extension libraries **MUST** target `.NET 8` or lower, this is due to the Editor and Command-line project only supporting `.NET 8` currently.  If you get errors with an Extension at build time, the `.NET` version of the library is the most likely cause.
>
> This will be resolved with the introduction of the new [Content Builder Project](/articles/getting_started/content_pipeline/content_builder_project.md) solution, which removes such limitations.

For some languages, this approach is not ideal. For example, Chinese and Japanese both have many thousands of characters. Adding the full range of characters to `<CharacterRegions>` dramatically increases the size of the font asset and the time required to build the font asset. A better solution adds individual characters whenever the specific characters are needed. You can create a custom content processor to implement this solution.

In this example, a file called _messages.txt_ contains all the text rendered by the game. The custom processor adds all the characters contained in the text in this file to a [FontDescription](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.FontDescription). Then it processes the object in the standard way using the base [FontDescriptionProcessor](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.FontDescriptionProcessor) functionality. All the characters in messages.txt will then be available to the [SpriteFont](xref:Microsoft.Xna.Framework.Graphics.SpriteFont) object at run time.

> [!IMPORTANT]
> This tutorial assumes you are using Visual Studio as your IDE, for VSCode please adapt the IDE interactions appropriately.

## Using the Font Description Processor

1. create a new MonoGame project called `FontGame` using the MonoGame template of your choice (for simplicity choose a desktop template)

2. Add a new `SpriteFont` called `DefaultFont` to a game project by opening the MGCB Editor, then right-click the `Content` node, click **Add**, and then click **New Item**.

3. Add the new SpriteFont to the game by selecting the **SpriteFont Description (.spritefont)** template, name the font `DefaultFont` and then click **Add**.

    ![Adding the `DefaultFont` SpriteFont](./images/mgcg_editor_new_spritefont.png)

4. Modify this file to use an existing font and any additional characteristics you prefer.

    For more information, see [Sprite Font XML Schema Reference](../../whatis/content_pipeline/CP_SpriteFontSchema.md).

5. Add a new text file named `messages.txt` to the game project by right-clicking on the FontGame project node in Solution Explorer, click **Add**, and then click **New Item**.

6. Select the **Text File** template, enter **messages.txt** for the file name, and then click **Add** to add the text file to the game.

7. In the new text file, enter any messages that will be printed by the font described in the Sprite Font file.

    > We will use the method [File.ReadAllText](http://msdn.microsoft.com/en-us/library/ms143369.aspx) to read the text in this file. This method requires a carriage return ("\\r") or line feed ("\\n") after the last string, so be sure to follow the last line of text in the file with a carriage return or line feed.

### To create the new content processor project

The Content Pipeline is part of the build process and it is separate from your game code, therefore you need to create a new assembly that contains the code developed in this topic. Creating this new assembly project is the first step in developing a new processor.

1. To add the new processor project to the game solution, go to Solution Explorer, right-click the **Solution** node, click **Add**, and then click **New Project**.

2. In the dialog box, select the template **MonoGame Content Pipeline Extension (MonoGame Team))**, enter `MyFontProcessor` in the **Name** field, and then click **OK**. The new project automatically contains references to the MonoGame Framework run-time and design-time Content Pipeline assemblies.

### To extend the font processor

1. Open the `Processor1.cs` which is the template Content Processor example.

1. Add the following lines of code, after the last `using` statement:

    ```csharp
    using System.IO;
    using System.ComponentModel;
    ```

1. Remove the placeholder code that assigns the processor input (`TInput`) and output (`TOutput`) types, including the `Process" method. They will not be needed.

1. Change the base class of `Processor1` from [ContentProcessor<TInput, TOutput>](xref:Microsoft.Xna.Framework.Content.Pipeline) to [FontDescriptionProcessor](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.FontDescriptionProcessor), which identifies it as a SpriteFont processor.

    > [NOTE]
    > You can read all about the different types of built-in processors in the API docs [Content.Pipeline.Processors](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors)

1. Change the class name and the Processor display name to something a little more unique, so that we know which processor we are selecting, from "Processor1" to "**MyFontProcessor**".

    ```csharp
    [ContentProcessor(DisplayName = "MyFontProcessor")]
    internal class MyFontProcessor : FontDescriptionProcessor
    ```

1. Add a new processor parameter (property) to the the class declaration and adorn it with [C# Attributes](https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/reflection-and-attributes/) to state additional processing parameters for the property.

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
    private string messageFile = @"../messages.txt";
    ```

    > [!NOTE]
    > As the "messages.txt" file is not in our content project, we need to supply a path "relative" to the "Content" project where the Content Processor is running from.
    > Hence the path to the file is denoted as `@"../messages.txt"`.

1. Add a new [Process](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.FontTextureProcessor) method override to match the following code:

    ```csharp
    public override SpriteFontContent Process(FontDescription input, ContentProcessorContext context)
    {}
    ```

    This modification replaces the template parameter and return types with the proper types needed for the extended font processor.

1. Inside the `Process` method, register a Content Pipeline dependency on `messages.txt`, which will read specifically from a file by that name from the Game Project.

    This dependency tells the Content Pipeline that if messages.txt changes, the font must be rebuilt.

    ```csharp
    string fullPath = Path.GetFullPath(MessageFile);
    
    context.AddDependency(fullPath);
    ```

1. Next, we add functionality to read the contents of the file and add each letter to the input font one by one. Note that the [Characters](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.FontDescription) collection keeps track of duplicates automatically, it is not necessary for the user to make sure that each letter is added only once. The **Characters** collection will contain only one instance of each character, no matter how many times **Add** has been called.

    ```csharp
    string letters = File.ReadAllText(fullPath, System.Text.Encoding.UTF8);
    
    foreach (char c in letters)
    {
        input.Characters.Add(c);
    }
    ```

    > [!NOTE]
    > In this example, messages.txt has been saved with Unicode UTF-8 encoding, which is the default encoding format that is specified in the call to [File.ReadAllText](http://msdn.microsoft.com/en-us/library/ms143369.aspx). However, the default file encoding format for text files that have been added to a Visual Studio project is `Western European (Windows) encoding`, corresponding to code page `1252`. If your text file uses a different encoding, specify the character encoding as follows:

    ```csharp
    string letters = File.ReadAllText( fullPath, System.Text.Encoding.GetEncoding( 1252 ) );
    ```

1. Finally, call the `Base` **Process** method of the [FontDescriptionProcessor](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors.FontDescriptionProcessor) to build the font with the newly requested characters.

    ```csharp
    return base.Process(input, context);
    ```

Done.  If your txt file is located elsewhere, make sure to update the path to the file appropriately, FROM the content project folder in the Processor project.

Final code:

```csharp
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System.ComponentModel;
using System.IO;

namespace FontProcessor
{
    [ContentProcessor(DisplayName = "MyFontProcessor")]
    internal class MyFontProcessor : FontDescriptionProcessor
    {
        public override SpriteFontContent Process(FontDescription input, ContentProcessorContext context)
        {
            string fullPath = Path.GetFullPath(MessageFile);

            context.AddDependency(fullPath);
            string letters = File.ReadAllText(fullPath, System.Text.Encoding.UTF8);

            foreach (char c in letters)
            {
                input.Characters.Add(c);
            }
            return base.Process(input, context);
        }

        [DefaultValue("messages.txt")]
        [DisplayName("Message File")]
        [Description("The characters in this file will be automatically added to the font.")]
        public string MessageFile
        {
            get { return messageFile; }
            set { messageFile = value; }
        }
        private string messageFile = @"../messages.txt";
    }
}
```

### Associate the custom font processor with the sprite font in the MGCB tool

1. Compile the solution to build **MyFontProcessor**, as you need to add your custom font processor as an available content processor for the content pipeline.

1. Open the MGCB tool for your `Content` project, click (select) the **Content** node, navigate down to the properties section and then click in the **References** property.

    [MGCB Editor window properties](./images/mgcb_editor_content_properties_referencesSelected.png)

1. Navigate to the `dll` for the built `MyFontProcessor` project (which by default is in `\FontGame\MyFontProcessor\bin\debug\net6.0`), select it and click "open".

    > [IMPORTANT]
    > Ensure that the processor project is always up to date when the main game is built, you need to create a project dependency.  Also use either "debug" or "Release" build types for the processor project, the MGCB tool will NOT dynamically select it when you change the Projects build type.

1. Build the Content Project to ensure everything is connected as it should be.

1. Select the `.spritefont` file, and then in the **Properties** window change the `processor` to`MyFontProcessor` in the drop-down list associated with the **ContentProcessor** field.

When you build the solution, the new processor adds the characters in the messages.txt file to the list of characters available to the [SpriteFont](xref:Microsoft.Xna.Framework.Graphics.SpriteFont).

## See Also

[Extending a Standard Content Processor](./HowTo_Extend_Processor.md)
[Adding New Content Types](../../whatis/content_pipeline/CP_Content_Advanced.md)
