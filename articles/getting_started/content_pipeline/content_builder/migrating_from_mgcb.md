---
title: "Content Builder, Part 4: Migrating from MGCB"
description: Learn how to move an existing MonoGame project from the MGCB Editor and .mgcb files to the Content Builder.
---

This is part 4 of a four-part guide on the MonoGame Content Builder. It explains how to move a game that uses the MGCB Editor and a `.mgcb` file over to a Content Builder project. If you are starting a new game, use the blank StartKit templates described in [part 1](index.md) instead.

> [!NOTE]
> The legacy MGCB documentation is still available in [Using MGCB Editor](../using_mgcb_editor.md) for projects that stay on the older tools.

## What changes

| MGCB | Content Builder |
|------|-----------------|
| The list of content lives in a `.mgcb` file. | The rules for content live in `Builder.cs`, as C# code. |
| You edit the `.mgcb` file with the MGCB Editor. | You edit `Builder.cs` in your normal editor. |
| Content builds through the `MonoGame.Content.Builder.Task` package and a `MonoGameContentReference` item. | Content builds through an `Import` of `BuildContent.targets` in each game project. |
| The `dotnet-mgcb` and `dotnet-mgcb-editor` tools are restored from `dotnet-tools.json`. | There are no `dotnet` tools. The builder is a project in your solution. |
| Each asset is listed in the `.mgcb` file with its importer, processor and settings. | A rule can match a whole folder. Only assets that need non-default settings need their own rule. |
| Custom importers and processors are DLLs listed under `/reference`, limited to .NET 8. | Custom importers and processors are normal project code or project references. |

Your game code does not change. The built files are still `.xnb` files in a `Content` folder, and `Content.Load` still takes the same asset names.

## Before you start

- Update your game to MonoGame 3.8.6.
- Make sure your project builds and runs. It is much easier to find problems when you start from a working project.
- Keep a copy of the content output from your current build. You will compare it with the new output later.
- Use version control, so that you can see exactly what the migration changes.

The steps below use a project from the standard `mgdesktopgl` template as the example. The same steps apply to the other platform templates.

## Step 1: Create the Content Builder project

Open a terminal in the folder that contains your solution file. Create the Content Builder project with the `mgcb` template, in a folder next to your game project:

```bash
dotnet new mgcb -o MyGame.Content
```

Add it to your solution:

```bash
dotnet sln add MyGame.Content/MyGame.Content.csproj
```

> [!NOTE]
> You can also create the project from your IDE. Search for "MonoGame Content Builder" in the new project dialog. In Visual Studio and Rider, name the project `MyGame.Content` and place it next to your game project.

The project has the same layout as the one described in [part 2](setup.md#the-content-builder-project). The `Assets` folder holds a placeholder file that you can delete.

## Step 2: Move your assets

Copy or move everything from inside your old `Content` folder to `MyGame.Content/Assets`, except for the `.mgcb` file, and the `bin` and `obj` folders. Keep the folder structure the same as the structure of the folders decides the asset names. Keeping it the same means your `Content.Load` calls keep working.

Do not delete the old `.mgcb` file, as we will use it in the next step.

## Step 3: Turn the `.mgcb` file into rules

Open the `.mgcb` file in a text editor. The top of the file holds the settings for the whole build, and the rest lists each asset. This is an extract from a StartKit project:

```text
/outputDir:bin/$(Platform)
/intermediateDir:obj/$(Platform)
/platform:DesktopGL
/config:
/profile:Reach
/compress:False

#begin Fonts/Hud.spritefont
/importer:FontDescriptionImporter
/processor:FontDescriptionProcessor
/processorParam:PremultiplyAlpha=True
/processorParam:TextureFormat=Compressed
/build:Fonts/Hud.spritefont

#begin Levels/00.txt
/copy:Levels/00.txt
```

Do not translate the file one asset at a time. Look for the folders in your project that share the same treatment, and write one rule for each. The default importer and processor work for most images, sounds, effects and fonts, so a single rule can cover them:

```csharp
public override IContentCollection GetContentCollection()
{
    var contentCollection = new ContentCollection();

    // Build everything with the default importer and processor.
    contentCollection.Include<WildcardRule>("*");

    // Copy the level files instead of building them.
    contentCollection.IncludeCopy<WildcardRule>("Levels/*.txt");

    // The font file is used by the .spritefont file, so it is not built on its own.
    contentCollection.Exclude<WildcardRule>("Fonts/*.ttf");

    return contentCollection;
}
```

Use this table to find the equivalent of each part of a `.mgcb` file.

| In the `.mgcb` file | In the Content Builder |
|---------------------|------------------------|
| `/importer` and `/processor` | Pass an importer and a processor to `Include`. Leave them out to use the defaults. |
| `/processorParam:Name=Value` | Set the property on the processor, for example `new TextureProcessor { PremultiplyAlpha = false }`. A parameter that has its default value does not need to be copied. |
| `/build:Path` | `Include`. If the output path is different from the source path, use the overload that takes an output path. |
| `/copy:Path` | `IncludeCopy`. |
| `/platform` | The `MonoGamePlatform` property of the game project, or the `-p` option of the builder. |
| `/profile` | The `-g` option of the builder. The builder default is `HiDef`. |
| `/compress` | The `--compress` option of the builder. |
| `/outputDir` and `/intermediateDir` | The `-o` and `-i` options. `BuildContent.targets` sets both for you. |
| `/reference` | A project reference to your custom importer and processor library, or the classes themselves in the Content Builder project. See [custom importers and processors](#custom-importers-and-processors). |

The settings of the built-in processors are properties of the processor classes. The next section lists them.

> [!IMPORTANT]
> The `mgdesktopgl` and other standard templates write `/profile:Reach` in the `.mgcb` file, but the builder builds for `HiDef` unless you tell it otherwise. If your game relies on the `Reach` profile, add `-g Reach` to the `ContentArgs` property in `BuildContent.targets`, next to the `-p` option:
>
> ```xml
> <ContentArgs>build -p $(MonoGamePlatform) -g Reach ...</ContentArgs>
> ```

### Processor settings

In the MGCB Editor you changed these settings in the property grid. In the Content Builder you set the property with the same name on the processor object. All of the processors are in the [`Microsoft.Xna.Framework.Content.Pipeline.Processors`](xref:Microsoft.Xna.Framework.Content.Pipeline.Processors) namespace.

| Asset type | Processor | Properties |
|------------|-----------|------------|
| Image | `TextureProcessor` | `ColorKeyColor`, `ColorKeyEnabled`, `GenerateMipmaps`, `MakeSquare`, `PremultiplyAlpha`, `ResizeToPowerOfTwo`, `TextureFormat` |
| Sound effect | `SoundEffectProcessor` | `Quality` |
| Song | `SongProcessor` | `Quality` |
| Model | `ModelProcessor` | `ColorKeyColor`, `ColorKeyEnabled`, `DefaultEffect`, `GenerateMipmaps`, `GenerateTangentFrames`, `PremultiplyTextureAlpha`, `PremultiplyVertexColors`, `ResizeTexturesToPowerOfTwo`, `RotationX`, `RotationY`, `RotationZ`, `Scale`, `SwapWindingOrder`, `TextureFormat` |
| Font | `FontDescriptionProcessor` | `PremultiplyAlpha`, `TextureFormat` |
| Localized font | `LocalizedFontProcessor` | `PremultiplyAlpha`, `TextureFormat` |
| Effect | `EffectProcessor` | `DebugMode`, `Defines` |
| Video | `VideoProcessor` | None |
| XML and other data | `PassThroughProcessor` | None |

An `.mgcb` entry for an image with non-default settings:

```text
#begin Textures/hero.png
/importer:TextureImporter
/processor:TextureProcessor
/processorParam:ColorKeyColor=255,0,255,255
/processorParam:ColorKeyEnabled=True
/processorParam:GenerateMipmaps=True
/processorParam:TextureFormat=DxtCompressed
/build:Textures/hero.png
```

becomes this rule:

```csharp
contentCollection.Include("Textures/hero.png", new TextureImporter(), new TextureProcessor
{
    ColorKeyColor = Color.Magenta,
    ColorKeyEnabled = true,
    GenerateMipmaps = true,
    TextureFormat = TextureProcessorOutputFormat.DxtCompressed
});
```

> [!TIP]
> `Color` is `Microsoft.Xna.Framework.Color`. Add `using Microsoft.Xna.Framework;` to the top of `Builder.cs` to use it. The builder already references the MonoGame framework.

### Custom importers and processors

If your `.mgcb` file has `/reference` lines, your project has a pipeline extension library. You have two options:

- Move the importer and processor classes into the Content Builder project. They are found by the extensions in their `ContentImporter` attribute, or you can pass them to `Include` yourself.
- Keep the library, and add a project reference to it from the Content Builder project. Retarget the library to .NET 10, the same version as the builder (or .NET 9 if you prefer). The older limit to .NET 8 for extensions in the Editor no longer applies.

See [part 3](advanced.md#custom-importers-and-processors) for an example. If a custom importer or processor uses the same name as before, the asset names of your built content do not change.

## Step 4: Update your game project

Open the `.csproj` file of each game project. For a standard template, the file contains the following:

```xml
<ItemGroup>
  <MonoGameContentReference Include="Content\Content.mgcb" />
</ItemGroup>

<ItemGroup>
  <PackageReference Include="MonoGame.Framework.DesktopGL" Version="3.8.*" />
  <PackageReference Include="MonoGame.Content.Builder.Task" Version="3.8.*" />
</ItemGroup>
```

Make these changes:

1. Remove the `MonoGameContentReference` item, and any `Content` items that include `.xnb` files or link to the old `Content` folder.
2. Remove the `MonoGame.Content.Builder.Task` package reference.
3. Add a `MonoGamePlatform` property for the platform of the project.
4. Import `BuildContent.targets` from the Content Builder project, at the end of the file before `</Project>`.

```xml
<PropertyGroup>
  <MonoGamePlatform>DesktopGL</MonoGamePlatform>
</PropertyGroup>

<ItemGroup>
  <PackageReference Include="MonoGame.Framework.DesktopGL" Version="3.8.*" />
</ItemGroup>

<Import Project="..\MyGame.Content\BuildContent.targets" />
```

Use the value that matches the platform of the project:

| Game project | `MonoGamePlatform` |
|--------------|--------------------|
| Cross-platform desktop (OpenGL) | `DesktopGL` |
| Windows DirectX | `Windows` |
| Android | `Android` |
| iOS | `iOS` |
| Cross-platform desktop (Vulkan) | `DesktopVK` |
| Windows DirectX 12 | `WindowsDX12` |

If your solution has a project for each platform, make these changes in every project. They can all share the same Content Builder project.

> [!NOTE]
> Content for Android and iOS is packaged into the application by `BuildContent.targets`. You do not need to keep the old steps for this.

## Step 5: Remove the old tools

Once the new build works, remove what you no longer need:

- The `.mgcb` file and the old `Content` folder, once the assets have been moved.
- The `.config/dotnet-tools.json` file, or the `dotnet-mgcb` and `dotnet-mgcb-editor` entries in it if it holds other tools.
- Any `dotnet tool restore` step in your scripts or build servers that existed only for MGCB.

> [!NOTE]
> The MGCB Editor edits `.mgcb` files and cannot open a Content Builder project. If you want to keep a project on the editor for a while longer, leave it as it is and use the [legacy documentation](../using_mgcb_editor.md).

## Step 6: Build and compare

Build your game. The builder runs first and writes its messages to the build output.

- A line that starts with `[E]` is an error and fails the build.
- A line that starts with `[W]` is a warning. A message such as `Importer: Not found` means that a rule includes a file that has no importer. Exclude the file, or copy it with `IncludeCopy`.
- The last lines report how many assets succeeded and failed.

Compare the new `Content` folder in your output with the copy you kept before the migration. The set of `.xnb` and copied files should be the same. If a file is missing, check these things:

- The file is in `Assets`, and a rule includes it.
- The pattern matches the file. Patterns that use braces, such as `*.{png,jpg}`, match nothing. See [wildcard patterns](setup.md#wildcard-patterns).
- A later `Exclude` rule is not removing it. The last matching rule wins.

Finally, run the game and check that it loads and uses its content.

## Mapping the MGCB Editor to the Content Builder

If you used the MGCB Editor every day, this table shows what each action becomes.

| In the MGCB Editor | In the Content Builder |
|--------------------|------------------------|
| Open the `.mgcb` file | Open `Builder/Builder.cs` |
| Edit > Add > Existing Item | Copy the file into `Assets`. A rule must include it. |
| Edit > Add > New Item | Create the file in `Assets`. See [adding assets by type](setup.md#adding-assets-by-type). |
| Change the importer or processor of an item | Pass an importer and processor to `Include`. |
| Change a processor setting in the property grid | Set the property on the processor object. |
| Add a reference to a custom pipeline library | Add a project reference, or put the classes in the Content Builder project. |
| Build | Build your game project, or run the builder with the `build` command. |
| Rebuild | Run the builder with `build --rebuild`. |
| Clean | Not needed. The builder removes the output and cache of assets that are no longer in the collection. |
| Change the target platform | Set `MonoGamePlatform` in each game project. |

## Common problems

| Problem | Solution |
|---------|----------|
| The game cannot find `Content/...` at runtime | Check that the project imports `BuildContent.targets` and that the output has a `Content` folder. Check that `Content.RootDirectory` is `"Content"`. |
| The builder cannot find the assets | The working directory is wrong. See the note about `WorkingDirectory` in [part 2](setup.md#settings-contentbuilderparams). |
| A font fails to build | The `.spritefont` file names a font that is not installed, or the `.ttf` file is built on its own. Keep the `.ttf` file in the same folder and exclude it with `Exclude`. |
| Built content differs in format from before | Check the settings that the `.mgcb` file listed for the asset, including the graphics profile. |
| A custom importer is not used | Check that the library is referenced from the Content Builder project and targets the same .NET version, then pass the importer and processor to `Include`. |
| The builder does not run in Rider | See the build settings in [Setting up Rider](../../2_choosing_your_ide_rider.md). |

## Next steps

Your project now builds its content with the Content Builder. For platform-specific content, custom importers and automated builds, see [part 3](advanced.md).
