---
title: "Chapter 05: Content Pipeline"
description: Learn the advantages of using the Content Pipeline to load assets and go through the processes involved in loading your first asset
---

Every game has assets: images to represent the visual graphics to players, audio to provide sound effects and background music, fonts to render text with, and much more.  These assets start out as raw files (e.g. *.png* image files or *.mp3* audio files), which you will need to load into the game to use.

## Chapter Video

|<iframe style="max-width: 50%; aspect-ratio: 16 / 9; width: 100%;" src="https://www.youtube.com/embed/7Q9xZScrrCg?si=xicd9mSs3d90YzvK" title="Live Recording" frameborder="0" allow="autoplay; encrypted-media; picture-in-picture" allowfullscreen></iframe>|
|:-:|
|**Live Recording**|

## Loading Assets

Loading assets can be done during runtime directly from file, or it can be loaded through the **Content Pipeline**.  Both of these methods are two sides of the same coin, and there are trade-offs to each approach.

For instance, to load an image file directly at runtime, you would need to:

1. Add the image file to your project.
2. Configure the project to copy the image file on build to the build output folder.
3. Load the image file as a texture at runtime using the [**Texture2D.FromFile**](xref:Microsoft.Xna.Framework.Graphics.Texture2D.FromFile(Microsoft.Xna.Framework.Graphics.GraphicsDevice,System.String)) method.

> [!IMPORTANT]
> One major drawback of loading an image file as a texture directly at runtime is that the data is in a compressed format, such as .png or .jpg. These compression formats are not understood by a Graphics Processing Unit (GPU). In order to be used by your game, this data will need to be decompressed into raw bytes, a format that the GPU understands. This operation can potentially leave a larger memory footprint for your assets. You will also need to handle how different compression formats work on the platform you are targeting, such as desktops, mobile and consoles.

On the other side of the coin, MonoGame offers the **Content Pipeline**; a workflow for managing assets. The workflow is made up of a set of tools and utilities that are automatically added by default when you create a new MonoGame project using the MonoGame project templates. To use this workflow, you need to:

1. Add the asset file to your Content project.
1. Configure the *MonoGame Content Builder* console application to compile your asset file.
1. Perform a project build. This will cause the *MonoGame Content Builder* application to compile the assets defined in the content project, optimized for the target platform, and automatically copy them to the game project build folder.
1. Load the compiled asset at runtime using the [**ContentManager**](xref:Microsoft.Xna.Framework.Content.ContentManager).

You also get the benefit of the assets being pre-processed and compiled to an optimized format for the target platform. For instance, image files can be compiled using [DXT compression](https://en.wikipedia.org/wiki/S3\_Texture\_Compression), which is a format that is understood by GPUs without needing to be decompressed first, reducing the memory footprint.

> [!NOTE]
> For more information on the benefits of compiling assets and what optimizations it can offer, see the [Content Pipeline](../../../../getting_started/content_pipeline/index.md) documentation.

For this tutorial series, we are going to focus on using the content pipeline workflow to load assets.  Doing this will allow you, as the developer, to get accustomed to using the content pipeline tools, and also give the benefits of having assets pre-compiled to optimized formats.

## The MonoGame Content Builder

At the center of the content pipeline workflow is the *MonoGame Content Builder*: a simple console application for importing, decompressing, and managing your game's assets.

This tutorial gives you step-by-step instructions for using the Content Builder to manage the assets for Dungeon Slime. For more information on the *MonoGame Content Builder* in general, see the [Content Builder](../../../../getting_started/content_pipeline/content_builder/index.md) documentation.

### Creating Folders to Organize Content

Organizing your game assets into folders helps keep your content project manageable as it grows. For this tutorial, we'll create all of our folders inside the `Content\Assets` folder that comes with the *MonoGame Empty Project* template. We'll start by creating an `images` folder to hold the image assets we will add to the game throughout this tutorial series.

1. In the VSCode *Explorer* pane, navigate to the `Content\Assets` folder inside the `DungeonSlime` directory.
1. Right-click the `Assets` folder and select *New folder...*.
1. Name the new folder `images` and press **Enter**.
1. (Optional) Delete the `readme.txt` file inside the `Assets` folder.

When you're done, your directory structure should look like the following image:

| ![**Figure 5-1: Content Builder Assets Directory**](./images/asset-directory-structure.png) |
| :-------------------------------------------------: |
| **Figure 5-1: Content Builder Assets Directory** |


### Adding Your First Asset

Now that we have a folder structure, we can add our first image asset to the project.  For this example, we will use the MonoGame logo.  Perform the following:

1. Download the MonoGame logo by right-clicking the following image and saving it as `logo.png` somewhere on your computer:

    | ![Figure 5-2: MonoGame Horizontal Logo](./images/logo.png) |
    | :--------------------------------------------------------: |
    |          **Figure 5-2: MonoGame Horizontal Logo**          |

2. Copy `logo.png` from where you saved it.
3. Paste `logo.png` into the `Assets\images` directory we created in the previous section.

### Configuring the Content Builder

Next, we need to configure the Content Builder application to build the MonoGame logo into your game. To do this, we'll edit the `Builder.cs` file that comes with the *MonoGame Empty Project* template. `Builder.cs` is our main mechanism for telling the Content Builder what content to build and how to build it.

1. Navigate to the `Content\Builder\` directory.
2. Open `Builder.cs`.
3. In `Builder.cs`, find the `GetContentCollection()` method.
4. Add the following code to `GetContentCollection()`:

[!code-csharp[](./snippets/builder.cs?highlight=7)]

This code tells the Content Builder to import all PNG images from the `images` folder into your game project. When you build your game, any PNG images in the `images` folder are converted to the `.xnb` format and built into your game's build directory, where they can be ingested by the [ContentManager](xref:Microsoft.Xna.Framework.Content.ContentManager).

## Understanding the Content Pipeline Workflow

Now that we have added our first asset, we can take a moment to understand what happens to this asset in the Content Pipeline workflow:

1. You create source files for your game assets, including images, audio, fonts, effects, and 3D models.
1. You add your game asset source files to the `Assets` directory in the Content Builder project.
1. You configure `Builder.cs` to include the compiled game assets in your game build.
1. When you perform a build of your project, the Content Builder:
   1. Compiles the assets defined in `Builder.cs` into `.xnb` files.
   2. Copies the compiled `.xnb` files from the Content project's build folder to your game project's build folder.
4. At runtime, you load the compiled assets using the [ContentManager](xref:Microsoft.Xna.Framework.Content.ContentManager).

<!-- TODO: Fix the diagram to show the Content Builder instead of the MGCB Editor stuff -->
<!-- The following diagram demonstrates this workflow: -->
<!---->
<!-- | ![Figure 5-7: MonoGame Content Pipeline Workflow diagram showing the process flow from source files (Images, Audio, Fonts, Effects, Models) through the MGCB Editor to generate the Content.mgcb file, which is then processed by MonoGame.Content.Builder.Tasks to create compiled .xnb assets (Xnb formats for each type), which are finally loaded by the ContentManager at runtime](./images/content-pipeline-workflow-full.png) | -->
<!-- | :----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | -->
<!-- |                       **Figure 5-7: MonoGame Content Pipeline Workflow diagram showing the process flow from source files (Images, Audio, Fonts, Effects, Models) through the MGCB Editor to generate the Content.mgcb file, which is then processed by MonoGame.Content.Builder.Tasks to create compiled .xnb assets (Xnb formats for each type), which are finally loaded by the ContentManager at runtime**                       | -->
<!---->
The Content Pipeline offers significant advantages:

- Assets are pre-processed and optimized for your target platform.
- Image files can be compiled using formats like DXT compression, which GPU's understand natively.
- Asset loading is simplified and consistent across platforms.

## The ContentManager Class

To load assets in your game code, MonoGame provides the [**ContentManager**](xref:Microsoft.Xna.Framework.Content.ContentManager) class. The [**Game**](xref:Microsoft.Xna.Framework.Game) already has a [**Content**](xref:Microsoft.Xna.Framework.Game.Content) property which is a ready-to-use instance of the [**ContentManager**](xref:Microsoft.Xna.Framework.Content.ContentManager)

### ContentManager Methods

The key methods for asset loading are:

| Method                                                                                                    | Returns | Description                                                                  |
| --------------------------------------------------------------------------------------------------------- | ------- | ---------------------------------------------------------------------------- |
| [**Load&lt;T&gt;(string)**](xref:Microsoft.Xna.Framework.Content.ContentManager.Load``1(System.String))   | `T`     | Loads the asset of type `T` that has been processed by the content pipeline. |
| [**Unload**](xref:Microsoft.Xna.Framework.Content.ContentManager.Unload)                                  | `void`  | Unloads all assets that have been loaded by that content manager instance.   |

> [!TIP]
> When an asset is loaded for the first time, the [**ContentManager**](xref:Microsoft.Xna.Framework.Content.ContentManager) internally caches it.  Loading the same asset again will return the cached version, avoiding extra disk reads.

## Understanding Content Paths

When loading content, you need to specify the path to the asset, minus the extension. This path is relative to the ContentManager's [**RootDirectory**](xref:Microsoft.Xna.Framework.Content.ContentManager.RootDirectory) property, which is set to **"Content"** by default in the `Game1` constructor.

For example, with our newly added logo in the "images" folder, the path would be "images/logo" (without the file extension). The reason for this relates to the build process. When you build your project, the Content Builder project [compiles your assets and copies them to the game's output folder](#understanding-the-content-pipeline-workflow).

This creates a folder structure in your output directory similar to:

```sh
DungeonSlime/
    └── bin/
        └── Debug/
            └── net8.0/
                ├── DungeonSlime.exe
                └── Content/
                    └── images/
                        └── logo.xnb
```

> [!NOTE]
> Notice that the compiled asset has an .xnb extension, but when loading the asset in code, you refer to it without any extension.

## Loading and Displaying Your First Asset

Now that we have the MonoGame logo added as an asset in the content project, we can modify the game to display the logo. In the *DungeonSlime* project, open the `Game1.cs` file (`Source\Game1.cs`) and perform the following:

1. Add a field to store the logo texture by inserting this line after the class declaration:

    ```cs
    // The MonoGame logo texture
    private Texture2D _logo;
    ```

2. In the [**LoadContent**](xref:Microsoft.Xna.Framework.Game.LoadContent) method, add this line to load the logo texture:

    ```cs
    _logo = Content.Load<Texture2D>("images/logo");
    ```

3. Finally, in the [**Draw**](xref:Microsoft.Xna.Framework.Game.Draw(Microsoft.Xna.Framework.GameTime)) method, add these lines before the `base.Draw(gameTime);` call:

    ```cs
    // Begin the sprite batch to prepare for rendering.
    SpriteBatch.Begin();

    // Draw the logo texture
    SpriteBatch.Draw(_logo, Vector2.Zero, Color.White);

    // Always end the sprite batch when finished.
    SpriteBatch.End();
    ```

    > [!NOTE]
    > We will go into more details about the [**SpriteBatch**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) in the next chapter.

The complete updated `Game1.cs` file should now look like this:

[!code-csharp[](./snippets/game1.cs?highlight=10-11,27,48-55)]

Running the game now will show the MonoGame logo displayed in the upper-left corner of the game window.

| ![Figure 5-3: The MonoGame logo drawn to the game window](./images/logo-drawn.png) |
| :--------------------------------------------------------------------------------: |
|             **Figure 5-3: The MonoGame logo drawn to the game window**             |

<!-- ## Adding Built-In Asset Types -->
<!---->
<!-- The MGCB Editor can also create certain built-in asset types.  In this section we will explore these types and this functionality.  If not already open, [open the MGCB Editor](#the-mgcb-editor) and perform the following: -->
<!---->
<!-- 1. Select the `Content` node. -->
<!-- 2. Right-click it and choose `Add > New Item...` from the context menu. -->
<!-- 3. In the dialog that appears, you will see the available built-in types. -->
<!---->
<!--     | ![Figure 5-9: New file pop-up](./images/new-file-popup.png) | -->
<!--     | :---------------------------------------------------------: | -->
<!--     |               **Figure 5-9: New file pop-up**               | -->
<!---->
<!-- The available built-in types include: -->
<!---->
<!-- - **Effect (.fx)**: A shader file that creates custom visual effects by controlling how graphics are rendered on the GPU. -->
<!-- - **SpriteFont Description (.spritefont)**: A configuration file that defines how text will be displayed in your game, including character set and font properties. -->
<!-- - **Sprite Effect (.fx)**: A shader specifically designed for use with 2D sprites to create special visual effects. -->
<!-- - **Xml Content (.xml)**: A structured data file for storing game information like levels, dialogues, or configuration settings. -->
<!-- - **LocalizedSpriteFont Description (.spritefont)**: A configuration file for creating fonts with support for multiple languages. -->
<!---->
<!-- > [!NOTE] -->
<!-- > Each built-in asset type comes with a template that includes the minimum required structure and settings. -->
<!---->
<!-- For now, click the `Cancel` button on the new file dialog.  We will explore these built-in types further in later chapters when we need them. -->

## Conclusion

In this chapter, you accomplished the following:

- You learned about the advantages of loading assets using the **Content Pipeline**.
- You created a folder structure to organize your game assets.
- You added an image file asset to the content project.
- You configured the Content Builder to compile and copy the image asset to your project output folder.
- You understood the Content Pipeline workflow and how MonoGame automates the process.
- You loaded and displayed your first asset using the [**ContentManager**](xref:Microsoft.Xna.Framework.Content.ContentManager).

In the next chapter, we will explore working with textures in more detail and learn about different rendering options.

## Test Your Knowledge

1. What are the two main ways of loading a texture, and what are the pros and cons of each approach?

    :::question-answer
    The two main ways to load a texture in MonoGame are:

    1. Directly from file using [**Texture2D.FromFile**](xref:Microsoft.Xna.Framework.Graphics.Texture2D.FromFile(Microsoft.Xna.Framework.Graphics.GraphicsDevice,System.String)).   This method requires manually setting up file copying, offers no pre-processing benefits, and can have a higher memory footprint.

    2. Using the content pipeline with [**Content.Load\<Texture2D>**](xref:Microsoft.Xna.Framework.Content.ContentManager.Load``1(System.String)).  Using the content pipeline optimizes textures into formats for the target platform(s), automatically handles compiling and copying assets during build, and reduces memory footprint, but requires additional setup using the MGCB Editor.
    :::

2. During the MonoGame content pipeline workflow, assets are compiled and then copied to the project output folder. What is responsible for performing this task?

    :::question-answer
    The Content Builder application.
    :::
