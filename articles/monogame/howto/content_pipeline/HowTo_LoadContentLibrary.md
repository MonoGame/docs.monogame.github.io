---
title: How to load content within a Game Library?
description: It may be desirable in some designs to load and draw content within the methods of a Game Library. For example, you may wish to distribute code that displays textures, models, or fonts (such as a DrawableGameComponent) in a .DLL.
requireMSLicense: true
---

There are two advanced techniques that accommodate this, one in which the binary content is separate from the .DLL, and one in which the content is embedded within the .DLL.

- [Loading Content in a custom Game Library](#loading-content-in-a-custom-game-library)
- [Embedding Content in a Game Library](#embedding-content-in-a-game-library)

## Loading Content in a custom Game Library

In this method, the compiled content used by the game library is distributed in its own .xnb files, separate from the .DLL.

This is the preferred method in most cases, as it makes the most efficient use of memory, especially when binary content is large.

### To create compiled content

1. Create a new solution that contains a game library project.
2. [Add a game content project](HowTo_GameContent_Add.md) to the solution.
3. In a library project, choose **Add, New Item**, and select "Resources File."
4. Select the game library project, then right-click and choose **Add Content Reference...**, and select the game content project you just created.

When the solution is built, the resources in the content project will be compiled into binary .xnb files. These files will reside in the "bin\\x86\\Debug\\Content" directory of the game library project (if built as an x86 debug project).

### To load compiled content within the Game Library

1. Create a new class that is the child of [DrawableGameComponent](xref:Microsoft.Xna.Framework.DrawableGameComponent).
2. Define a new [ContentManager](xref:Microsoft.Xna.Framework.Content.ContentManager) and, within the constructor of the new class, set its **RootDirectory** to the path where the compiled content is to be stored.

    For flexibility, the path string may be a parameter passed to the constructor by the game client.

    ```csharp
    public static ContentManager LibContent;
    
    public GameLibComponent(Game game, string contentdirectory) : base(game)
    {
        LibContent = new ContentManager(game.Services);
        LibContent.RootDirectory = contentdirectory;
    }
    ```

3. In the [Game.LoadContent](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_LoadContent) method, load your content normally using your [ContentManager](xref:Microsoft.Xna.Framework.Content.ContentManager).

    ```csharp
    SpriteBatch spriteBatch;
    Texture2D LibTexture;
    
    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        LibTexture = LibContent.Load<Texture2D>("mytexture");
    }
    ```

## Embedding Content in a Game Library

You may also embed binary content resources directly in the Game Library and load them from within. This technique requires the declaration of those resources in a reference file. It allows you to distribute code that displays textures, models, or fonts (such as a [DrawableGameComponent](xref:Microsoft.Xna.Framework.DrawableGameComponent)) in a .DLL without distributing the .xnb files separately.

> **Note**
>
> Be aware that all embedded resources are loaded into memory with the .DLL, and cannot be unloaded from main memory. For this reason, this technique is not recommended for most applications, and should only be used when the content to embed is very small.

### To add content to a Game Library as references

1. Build an existing project containing the content you wish to add.
2. In a library project, choose **Add**, **New Item**, and select "Resources File".
3. If the **Resource Designer** is not opened automatically, double-click the .resx file in the **Solution Explorer**.
4. From the **Resource Designer**, choose **Add Resource**, **Add Existing File**.
5. Navigate to the "bin\\x86\\Debug\\Content" directory of the project that built the content you wish to add.

    > This assumes it was built as an x86 Debug project.

6. Select the .xnb files for the content you wish to add to the library.

    > Ensure the dialog box is displaying "All Files".

Once content has been added to the **Resource Designer**, any code running from within the Library can load the content with a special [ContentManager](xref:Microsoft.Xna.Framework.Content.ContentManager).

### To load embedded content within a Game Library

1. Create a new class that is the child of [DrawableGameComponent](xref:Microsoft.Xna.Framework.DrawableGameComponent).
2. Define a new [ContentManager](xref:Microsoft.Xna.Framework.Content.ContentManager).
3. Within the constructor of the new class, create a new instance of the [ResourceContentManager](xref:Microsoft.Xna.Framework.Content.ResourceContentManager) class and assign it to your [ContentManager](xref:Microsoft.Xna.Framework.Content.ContentManager).

    The second parameter to the [ResourceContentManager](xref:Microsoft.Xna.Framework.Content.ResourceContentManager) constructor identifies the resource project that contains your embedded resources.

    ```csharp
    public static ContentManager LibContent;
    
    public GameLibComponent(Game game)
        : base(game)
    {
        ResourceContentManager resxContent;
        resxContent = new ResourceContentManager(game.Services, ResourceFile.ResourceManager);
        LibContent = resxContent;
    }
    ```

4. In the [Game.LoadContent](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_LoadContent) method, load your content normally using your [ContentManager](xref:Microsoft.Xna.Framework.Content.ContentManager).

    ```csharp
    SpriteBatch spriteBatch;
    Texture2D LibTexture;
    
    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        LibTexture = LibContent.Load<Texture2D>("mytexture");
    }
    ```

## See Also

### Reference

- [Game Class](xref:Microsoft.Xna.Framework.Game)  
- [Game.LoadContent](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_LoadContent)  
- [UnloadContent](xref:Microsoft.Xna.Framework.Game.UnloadContent)  
- [Game Members](xref:Microsoft.Xna.Framework.Game)  
- [Microsoft.Xna.Framework Namespace](xref:Microsoft.Xna.Framework)  
