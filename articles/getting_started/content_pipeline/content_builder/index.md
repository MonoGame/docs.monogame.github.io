---
title: "Content Builder, Part 1: Getting Started"
description: Learn what the MonoGame Content Builder is, create a project with the Blank StartKit template, and build and load your first asset.
---

This is part 1 of a four-part guide on the MonoGame Content Builder. It covers what the Content Builder is, how to create a project that uses it, and how to build and load your first asset.

| Part | What it covers |
|------|----------------|
| **1. Getting Started** (this page) | What the Content Builder is, creating a project, building and loading your first asset. |
| [2. Setting up the Content Builder](setup.md) | How the project is put together, build rules, importers and processors, and adding each type of asset. |
| [3. Advanced Scenarios and Automation](advanced.md) | Platform specific content, custom importers and building content using automation with GitHub Actions. |
| [4. Migrating from MGCB](migrating_from_mgcb.md) | Moving an existing project from the MGCB Editor and `.mgcb` files to the Content Builder. |

## What is the Content Builder?

Games load content such as textures, sounds and fonts through the [Content Pipeline](../why_content_pipeline.md). The pipeline converts your source files into optimized `.xnb` files for the platform you are targeting, and your game loads those files at runtime with the `ContentManager`.

> [!NOTE]
> You can still load content directly from files if you wish, avoiding the Content Pipeline. Check the API guides for each asset type for more information.  But it is recommended to use the Content Pipeline for efficiency, especially if you are considering delivering to console.

The Content Builder is how the pipeline builds content from MonoGame 3.8.6. It is a small console application that lives in your solution. When it runs, it reads your source assets from a folder (by default, the `Assets` folder in your Content project) and applies the rules you write in C#, generating the built `.xnb` files to the output of your game ready to be consumed.

> [!NOTE]
> Unlike the legacy MGCB, the Content Builder is a standalone project, there is no `dotnet-tools.json`, no editor to install, and no `.mgcb` file to keep in sync. The rules that decide how each asset is built are code, so they can be reviewed, debugged and automated like the rest of your game.

If you are working on an existing game that uses the MGCB Editor, the [legacy MGCB documentation](../using_mgcb_editor.md) stays available, and [part 4](migrating_from_mgcb.md) explains how to move over.

<div class="embeddedvideo">
<iframe width="560" height="315" src="https://www.youtube.com/embed/QB43LgRmdNM/" title="New Content Builder Project - Getting Started" frameborder="0" allowfullscreen></iframe>
</div>

## Before you start

You need the following:

- The .NET 10 SDK or later.
- The MonoGame 3.8.6 project templates, or the MonoGame extension for your IDE. See [Getting Started](../../index.md) for how to set up your environment.
- To install or update the templates from a terminal, run `dotnet new install MonoGame.Templates.CSharp`.

## Create a project

The quickest way to start is the **MonoGame Content Builder Platform Blank StartKit** template. It creates a solution with a shared game library, projects for each supported platform, and a Content Builder project that is already wired into every game project.

> [!NOTE]
> A project template built specifically around the Content Builder is being developed. Until it is released, the StartKit templates are the recommended way to start a new game that uses the Content Builder.

### [Visual Studio](#tab/vs)

1. Choose **Create a new project**.
2. Search for "MonoGame Content Builder Platform Blank StartKit" and select it, then click **Next**.
3. Give the project a name, for example `MyGame`, and click **Create**.

### [Visual Studio Code](#tab/vscode)

1. Open the Command Palette and run **.NET: New Project**.
2. Choose "MonoGame Content Builder Platform Blank StartKit".
3. Give the project a name, for example `MyGame`, and choose a location.

### [dotnet CLI](#tab/dotnetcli)

Run the following command in an empty folder:

```bash
dotnet new mgblankmgcbstartkit -n MyGame
```

---

> [!NOTE]
> Follow the standard .NET naming rules for the project name. Do not start the name with a number or a special character, and do not use spaces.

### What you get

The template creates the following structure:

```text
MyGame/
├── MyGame.slnx
├── MyGame.Core/                 # Shared game code
├── MyGame.Content/              # The Content Builder project
│   ├── Assets/                  # Your source assets
│   │   └── readme.txt
│   ├── Builder/
│   │   └── Builder.cs           # The rules that decide how assets are built
│   ├── BuildContent.targets     # Runs the builder when a game project builds
│   └── MyGame.Content.csproj
├── MyGame.DesktopGL/            # Platform projects
├── MyGame.DesktopVK/
├── MyGame.WindowsDX12/
├── MyGame.Android/
└── MyGame.iOS/
```

> [!TIP]
> Any platforms you are not using can be safely deleted, just make sure to remove both the folder and the entry in the `.slnx` file.

Each platform project imports `BuildContent.targets` and sets a `MonoGamePlatform` property, so the builder knows which platform to build content for. When you build a platform project, the builder runs first and the built content is placed in the output folder next to your game.

> [!TIP]
> If you want to see a complete game rather than an empty one, the **MonoGame Content Builder Platform 2D StartKit** template creates the same solution with a finished platformer and its assets. Create it with `dotnet new mg2dmgcbstartkit -n MyGame`. The [MonoGame-CBPlatform-Test](https://github.com/MonoGame/MonoGame-CBPlatform-Test) repository also contains a sample that you can browse.

## Add your first asset

The blank template builds nothing until you tell it to. This is deliberate, so that you decide which files are part of your game.

1. Copy an image, for example `ball.png`, into `MyGame.Content/Assets`.
2. Open `MyGame.Content/Builder/Builder.cs` and find the `GetContentCollection` method.
3. Add a rule that includes everything in the `Assets` folder:

    ```csharp
    public override IContentCollection GetContentCollection()
    {
        var contentCollection = new ContentCollection();

        // Build every file in the Assets folder with its default importer and processor.
        contentCollection.Include<WildcardRule>("*");

        return contentCollection;
    }
    ```

    > [!NOTE]
    > This is a very simple example to get started, it is recommended that you design how content is structured in your final game project and organise them with folders, then using rules to load specific content from each folder.  For more details see [Writing build rules](setup.md#writing-build-rules)

4. Build your game project, or press **Run**. The builder runs as part of the build.

The built `.xnb` file is written to the `Content` folder in the output of your game project, for example `MyGame.DesktopGL/bin/Debug/net10.0/Content/ball.xnb`.

> [!NOTE]
> The builder prints a warning, such as `Assets/readme.txt: Importer: Not found`, for any file that it has no [content importer](../../../getting_to_know/whatis/content_pipeline/CP_StdImpsProcs.md) for. A warning does not fail the build. To silence it, remove the file or add an `Exclude` rule for it, as described in [part 2](setup.md#including-and-excluding-content).

## Load the asset in your game

The game loads built content from the `Content` folder with the `ContentManager`, exactly as in earlier versions of MonoGame. The template already sets `Content.RootDirectory = "Content"` in `MyGame.Core`.

> [!NOTE]
> It is important to note, that the Content Pipeline and any code written to load content in any MonoGame project remains the same.  The Content Builder is simply a more advanced way of packaging your content for the game at runtime.

Load the texture in `LoadContent`, using the path of the file relative to `Assets` and without the extension:

```csharp
private Texture2D _ball;

protected override void LoadContent()
{
    base.LoadContent();

    _ball = Content.Load<Texture2D>("ball");
}
```

> [!TIP]
> If the image was in `Assets/Textures/ball.png`, the asset name is `"Textures/ball"`.

## Troubleshooting

If the game cannot find an asset at runtime, check these points first:

- The file is inside the `Assets` folder, and a rule in `Builder.cs` includes it.
- The asset name in `Content.Load` matches the path relative to `Assets`, without the extension.
- The builder output reports `0 failed`. Errors are printed with an `[E]` prefix in the build output.

## Next steps

In [part 2](setup.md) you will look at how the builder project works and how to write rules for the types of asset your game uses.
