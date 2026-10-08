---
title: "Content Builder, Part 2: Setting up the Content Builder"
description: Learn how the Content Builder project works, how to write the rules that build your assets, and how to add each type of asset to your project.
---

This is part 2 of a four-part guide on the MonoGame Content Builder. [Part 1](index.md) created a project and built a first asset. This part explains how the content builder project works and how to write the rules that decide how your content is built.

## The Content Builder project

The content builder project contains the following by default:

```text
MyGame.Content/
├── Assets/                  # Your source assets
├── Builder/
│   └── Builder.cs           # Entry point and build rules
├── BuildContent.targets     # Runs the builder when a game project builds
└── MyGame.Content.csproj    # Project file with the pipeline packages
```

The project is a console application. It references the MonoGame content pipeline package and the native tools the pipeline uses to process content, such as FreeType for fonts, FFmpeg for audio and video, Assimp for models, and the shader and texture compression tools.

> [!NOTE]
> Unlike the MGCB tooling, it is not a `dotnet` tool, so you do not need a `dotnet-tools.json` file.

> [!TIP]
> Keep the builder in its own project. It is possible to put the builder inside your game project, but a separate project stops build tooling and runtime code from interfering with each other.

### How a game project runs the builder

Each game project needs to import the `BuildContent.targets`, as follows:

```xml
<Import Project="..\MyGame.Content\BuildContent.targets" />
```

> [!IMPORTANT]
> If your content is not building, the most likely reason is that your platform's `.csproj` is missing the above import definition to instruct the Content Builder to generate the content.  This might actually be desired for automation scenarios where you build the game and content separately.
>
> It is not required to build the content and game at the same time (especially for development), but content is REQUIRED to be present in the Game's runtime folder to run the game!

The file adds a `BuildContent` target that runs before your game project compiles. The target does the following:

1. Builds the Content Builder project.
2. Runs the resulting program with the `build` command, passing the platform from the `MonoGamePlatform` property of the game project, the `Assets` folder, and the output and intermediate folders of the game project.
3. Reports any line the builder prints with an `[E]` prefix as a build error, and any line with a `[W]` prefix as a build warning.
4. Adds the built files to the Android assets or the iOS and macOS bundle resources, for the platforms that package content inside the application.

The built files are written to a `Content` folder inside the output of the game project. Your game loads them through `ContentManager`, with `Content.RootDirectory` set to `"Content"`.

> [!NOTE]
> For Android and iOS the content must be packaged inside the application bundle. `BuildContent.targets` handles this, so no extra steps are needed in the platform projects.

### Running the builder yourself

You can also run the builder directly. From the folder that contains your solution:

```bash
dotnet run --project MyGame.Content -- build -s MyGame.Content/Assets -o MyGame.DesktopGL/bin -i MyGame.DesktopGL/obj/Content -p DesktopGL
```

Everything after the `--` is passed to the builder. This is useful when you want to check your rules without building the whole game.

## The entry point

`Builder.cs` contains the entry point of the console application and the `Builder` class that holds your rules.

```csharp
using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Framework.Content.Pipeline.Builder;

var contentCollectionArgs = new ContentBuilderParams()
{
    Mode = ContentBuilderMode.Builder,
    WorkingDirectory = $"{AppContext.BaseDirectory}../../",
    SourceDirectory = "Assets",
    Platform = TargetPlatform.DesktopGL
};
var builder = new Builder();

if (args is not null && args.Length > 0)
{
    builder.Run(args);
}
else
{
    builder.Run(contentCollectionArgs);
}

return builder.FailedToBuild > 0 ? -1 : 0;

public class Builder : ContentBuilder
{
    public override IContentCollection GetContentCollection()
    {
        var contentCollection = new ContentCollection();

        // Define your content collection rules here.

        return contentCollection;
    }
}
```

The program decides where its settings come from:

- When it is started with arguments, as `BuildContent.targets` does, it reads its settings from the command line.
- When it is started without arguments, for example when you run it from your IDE, it uses the `ContentBuilderParams` object in the file. Check that the paths in that object are correct for your project.

The exit code is `-1` if any asset failed to build, so build servers and scripts can detect failures.

### Settings: ContentBuilderParams

`ContentBuilderParams` holds the settings for a build.

| Property | Command line | Description | Default |
| ---------- | -------------- | ------------- | --------- |
| `Mode` | `build` command | `ContentBuilderMode.Builder` builds content. `ContentBuilderMode.None` prints the help text. | `None` |
| `WorkingDirectory` | `--workingDir` | The folder that the other directories are relative to. | The current directory |
| `SourceDirectory` | `-s`, `--src` | The folder that contains your source assets. | `Content` |
| `OutputDirectory` | `-o`, `--output` | Where built content is written. | `bin/Content` |
| `IntermediateDirectory` | `-i`, `--intermediate` | Where the build cache is kept. | `obj/Content` |
| `Platform` | `-p`, `--platform` | The platform to build content for. | `DesktopGL` |
| `GraphicsProfile` | `-g`, `--graphics-profile` | `Reach` or `HiDef`. | `HiDef` |
| `CompressContent` | `--compress` | Compress the built files. | `false` |
| `LogLevel` | `-l`, `--loglevel` | `Debug`, `Info`, `Warning` or `Error`. | `Info` |
| `Rebuild` | `build --rebuild` | Ignore the cache and build everything again. | `false` |
| `SkipClean` | `build --skip-clean` | Keep old built files and cache data after the build. | `false` |

The templates pass `-s` explicitly, so the default of `Content` for `SourceDirectory` does not apply to them. Directories can be absolute or relative to `WorkingDirectory`.

> [!IMPORTANT]
> `WorkingDirectory` must resolve to the same folder however the builder is started. The template uses a path relative to the location of the program, which differs between a `Debug` and a `Release` build. If the builder cannot find your assets, try an absolute path and work back from there.

Built files are written to a `Content` folder inside `OutputDirectory`. With the default settings, an asset `Assets/Textures/hero.png` is written to `bin/Content/Content/Textures/hero.xnb`. `BuildContent.targets` points `OutputDirectory` at the output folder of the game project, so the game finds the files in `Content/Textures/hero.xnb` next to its executable.

The supported `Platform` values include `DesktopGL`, `DesktopVK`, `Windows`, `WindowsDX12`, `MacOSX`, `Android` and `iOS`. The `MonoGamePlatform` property in your game project is passed to the builder as this value.

## Writing build rules

The `ContentCollection` holds the rules that decide which files are built, copied or ignored. Nothing is built unless a rule includes it.

### Including and excluding content

| Method | What it does |
| -------- | -------------- |
| `Include` | Builds the file with the pipeline. The importer and processor are chosen from the file extension unless you supply them. |
| `IncludeCopy` | Copies the file to the output without processing it. |
| `Exclude` | Removes the file from the build. |

Each method has a version for a single file and a version that takes a rule type for a group of files:

```csharp
var contentCollection = new ContentCollection();

// One file.
contentCollection.Include("Textures/hero.png");

// A group of files, matched with a wildcard pattern.
contentCollection.Include<WildcardRule>("Textures/*.png");

// Copy a file without processing it.
contentCollection.IncludeCopy("Data/config.json");
contentCollection.IncludeCopy<WildcardRule>("Levels/*.txt");

// Leave a file out.
contentCollection.Exclude("Textures/debug_texture.png");
contentCollection.Exclude<WildcardRule>("Fonts/*.ttf");
```

Paths are relative to your `Assets` folder and use `/` as the separator.

> [!IMPORTANT]
> The last rule that matches a file wins. You can include everything and then exclude the files you do not want. If you add the rules in the opposite order, the all inclusive include that is evaluated last would bring everything back.

```csharp
// Build everything, copy the json files, and leave out fonts and debug files.
contentCollection.Include<WildcardRule>("*");
contentCollection.IncludeCopy<WildcardRule>("*.json");
contentCollection.Exclude<WildcardRule>("Fonts/*.ttf");
contentCollection.Exclude<WildcardRule>("*debug*");
```

If a rule for a file that already has a rule uses the same path, the new rule replaces the old one, including its importer, processor and output path.

> [!TIP]
> Group assets that need the same treatment in the same folder, for example all sound effects in `Sounds`. A rule such as `Include<WildcardRule>("Sounds/*")` then covers every sound you add later, and you do not need to change or rebuild the builder when your assets change.

### Wildcard patterns

`WildcardRule` uses the same pattern syntax as the Visual Basic `Like` operator, matched against the path relative to the `Assets` folder.

| Pattern | Matches |
| --------- | --------- |
| `*` | Any sequence of characters, including `/`. |
| `?` | Any single character. |
| `[abc]` | Any one of the characters in the set. |
| `[!abc]` | Any one character that is not in the set. |

Because `*` also matches the `/` separator, `Textures/*.png` includes PNG files in `Textures` and in every folder below it. A pattern that needs more than one extension must use one rule for each:

```csharp
contentCollection.Include<WildcardRule>("Textures/*.png");
contentCollection.Include<WildcardRule>("Textures/*.jpg");
```

> [!WARNING]
> A pattern that matches nothing does not produce a warning. Brace alternatives such as `*.{png,jpg}` are not part of the syntax, so that pattern silently matches no files. If a file is missing from your output, check your patterns first.

### Regular expressions

For patterns that wildcards cannot express, you can use a `RegexRule`. The expression is matched against the relative path with `/` separators and is not anchored, so add `^` and `$` when you want to match the whole path.

```csharp
// Textures and sounds only.
contentCollection.Include<RegexRule>(@"^(Textures|Sounds)/.*\.(png|wav)$");

// Files that end in _diffuse.png.
contentCollection.Include<RegexRule>(@".*_diffuse\.png$");

// Leave out backup files.
contentCollection.Exclude<RegexRule>(@".*\.bak$");
```

> [!NOTE]
> Use a tool such as [regex101](https://regex101.com/) to check a complicated expression before you rely on it.

### Choosing where the output goes

By default, the built file retains the same path for output and gets the `.xnb` extension. To change the path, pass an output path. Do not add an extension to the path. For built content the builder adds `.xnb`.

```csharp
// A single file.
contentCollection.Include("Sprites/player.png", "Characters/Player");

// A group of files, using a function that receives the relative path of each file.
contentCollection.Include<WildcardRule>("Textures/*.png", outputPath: path => Path.GetFileName(path));
contentCollection.Include<WildcardRule>("Sounds/*.wav", outputPath: path => $"Audio/{Path.GetFileName(path)}");
```

The first group rule above writes every texture to the root of the output, so two textures with the same file name in different folders conflict.

`SetContentRoot` changes the folder that the rules added after it write into. The default content root is `Content`, and setting a new root replaces it. Include `Content` in the root so that the files stay inside the folder your game loads from:

```csharp
contentCollection.SetContentRoot("Content/Graphics");
contentCollection.Include<WildcardRule>("Textures/*.png");

contentCollection.SetContentRoot("Content/Audio");
contentCollection.Include<WildcardRule>("Sounds/*.wav");
```

This writes `Content/Graphics/Textures/hero.xnb` and `Content/Audio/Sounds/jump.xnb`. A root of `"Graphics"` alone would write the files next to the `Content` folder, where `ContentManager` does not look.

## Using artifacts output

By default, each project writes its build output to `bin` and `obj` folders inside its own project folder. The built content goes with the game, so it ends up next to the game executable.

For a DesktopGL project this is:

```text
MyGame.DesktopGL/bin/Debug/net10.0/Content/ball.xnb
```

> [!NOTE]
> The above path assumes your project targets .NET 10, if you are using a different version, make sure to update the path appropriately.

The .NET SDK has an alternative layout called `artifacts` output. It collects the output of every project in the solution into one `artifacts` folder, which keeps the project folders clean and gives you a single folder to ignore in source control or to upload from a build server.

To turn it on, add a `Directory.Build.props` file next to your solution file:

```xml
<Project>
  <PropertyGroup>
    <UseArtifactsOutput>true</UseArtifactsOutput>
  </PropertyGroup>
</Project>
```

The setting needs .NET 8 SDK or later. The output of every project, including the Content Builder project, then moves to the `artifacts` folder, grouped by project name and build configuration:

```text
artifacts/
├── bin/
│   ├── MyGame.Content/debug/                  # The Content Builder program
│   └── MyGame.DesktopGL/debug/                # The game, with its built content
│       ├── MyGame.dll
│       └── Content/ball.xnb
└── obj/
    ├── MyGame.Content/debug/
    └── MyGame.DesktopGL/debug/                # Includes the content build cache
```

The folder name uses the lower case configuration name, and it adds the runtime identifier when you build with one. A build with `-r win-x64` writes to `artifacts/bin/MyGame.DesktopGL/debug_win-x64/Content`. To place the folder somewhere else, set `ArtifactsPath` in the same file.

### What this means for the content build

The Content Builder follows the output of the game project. `BuildContent.targets` passes the output and intermediate folders of the game project to the builder, so the built content moves with the game and the build cache moves to `artifacts/obj`. The content is still written to a `Content` folder next to the game executable, so `Content.RootDirectory` stays `"Content"` and you do not need to change your game code.

What can need changing is anything outside the game code that refers to the old `bin` path, such as:

- Scripts, installers and packaging steps that copy or zip `bin/Debug/net10.0`.
- The `output-folder` input of the Build-Content action, if your workflow builds the content before the game. See [part 3](advanced.md#automating-content-builds-with-github-actions).
- Any project file with a fixed `bin\$(Configuration)` path for content.

> [!NOTE]
> The sample projects in [part 3](advanced.md#project-changes-for-automated-builds) are not affected, because `BuildContent.targets` reads the content from `$(OutputPath)`, which follows the artifacts layout.

### If the build fails after enabling it

If the build stops with error `MSB3073` when it runs the Content Builder, your copy of `BuildContent.targets` looks for the builder program at a fixed `bin\Debug` path inside the Content Builder project. With artifacts output the program is in `artifacts/bin` instead.

Change the target to take the location from the build of the Content Builder project, as in these lines:

```xml
<MSBuild Projects="$(MSBuildThisFileDirectory)MyGame.Content.csproj" Targets="Build" RemoveProperties="Configuration;TargetFramework;RuntimeIdentifier;RuntimeIdentifiers">
  <Output TaskParameter="TargetOutputs" ItemName="_ContentBuilderAssembly" />
</MSBuild>
<PropertyGroup>
  <ContentCommand>@(_ContentBuilderAssembly->'%(RootDir)%(Directory)%(Filename)')</ContentCommand>
</PropertyGroup>
```

> [!IMPORTANT]
> The above is the default in the `3.8.6` templates, if you are upgrading from `3.8.5` we recommend overwriting the `BuildContent.targets` from a fresh copy of the Content Builder project template.

Also check that the output and intermediate paths are built with `$([MSBuild]::NormalizePath('$(ProjectDir)', '$(OutputPath)'))`. With artifacts output, `OutputPath` is a full path, so joining it to `$(ProjectDir)` as plain text gives a path that does not exist.

## Importers and processors

The builder turns a source file into a built asset in two steps:

- An **importer** reads the source file, for example a PNG or a WAV, into an intermediate form.
- A **processor** converts the intermediate form into the final runtime format for the platform. A processor can also pull in other assets that the file refers to, such as the textures of a model.

When you do not provide either one, the builder selects them from the file extension. The standard importers and processors are listed in [Standard Content Importers and Content Processors](../../../getting_to_know/whatis/content_pipeline/CP_StdImpsProcs.md).

To override the choice, or to change the settings of a processor, pass instances to `Include`:

```csharp
using Microsoft.Xna.Framework.Content.Pipeline.Audio;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

// One file.
contentCollection.Include("Textures/hero.png", new TextureImporter(), new TextureProcessor
{
    PremultiplyAlpha = false,
    GenerateMipmaps = true
});

// A group of files.
contentCollection.Include<WildcardRule>(
    "Sounds/*.wav",
    contentImporter: new WavImporter(),
    contentProcessor: new SoundEffectProcessor { Quality = ConversionQuality.Best });
```

Each processor has its own settings. [Part 4](./migrating_from_mgcb.md) lists the settings that were available in the MGCB Editor and the property that replaces each one.

> [!NOTE]
> You can also define your own [Custom content Importers and Processors](../../../getting_to_know/whatis/content_pipeline/CP_AddCustomProcImp.md) if you want to do additional processing or conversion for custom assets such as `level` files, or build collision data to bundle with a loaded model.

### Caching

The builder keeps a cache in the intermediate folder so that it only rebuilds assets that need it. An asset is only built again when any of these change:

- The source file or a file it depends on.
- The settings of the importer or processor, including their `Version`.
- The graphics profile or compression setting of the build.
- The content root or the build or copy state of the file.

> [!NOTE]
> The `Version` property on the base importer and processor classes exists so that you can invalidate the cache for a custom importer or processor when its code changes. Change it to any string you can recognise, preferably one you can increment:
>
> ```csharp
> public override string Version { get; set; } = "2";
> ```
>
> MonoGame updates the versions of the built-in importers and processors, so you only need to force a rebuild with `build --rebuild` when you want to ignore the cache entirely.

When the builder finishes, it deletes built files and cache data for assets that are no longer part of the collection. Use `build --skip-clean` to keep them.

## Adding assets by type

Using the Content Builder, to add a new asset you put the file in the `Assets` folder and make sure a rule includes it. If the folder rule already covers the file, there is nothing else to do.

> [!NOTE]
> Previously, using the MGCB Editor, you had to add each asset through the editor using the **Edit** menu and set its processor in the property grid.  This was very tedious and often put people off using MonoGame due to the issue that arose with either building the MGCB itself (dotnet tool version mismatches) or forgetting to save changes in the editor prior to a build.
>
> Now with the Content Builder, things are much simpler, and more like how XNA handled content.

The sections below cover each type of asset.

### Images

Copy the image into the `Assets` folder. By default, the `TextureImporter` and `TextureProcessor` are used to import the image. Load it as a `Texture2D` at runtime.

### Audio

Copy the audio file into the `Assets` folder.

- `.wav` files build with the `SoundEffectProcessor`. Load them as a `SoundEffect`.
- `.mp3`, `.ogg` and `.wma` files build with the `SongProcessor`. Load them as a `Song`.

To build a `.wav` as a `Song`, pass the processor explicitly:

```csharp
contentCollection.Include("Music/theme.wav", new WavImporter(), new SongProcessor());
```

### SpriteFonts

A SpriteFont is a `.spritefont` description file that names a font and its size. To create a SpriteFont, you use the `mgsf` dotnet item template.

To add a new SpriteFont to your project simply run the following from the command-line/terminal in your project's `Assets` folder:

```bash
cd MyGame.Content/Assets
dotnet new mgsf -n Hud -o Fonts --IncludeFont
```

This creates `Fonts/Hud.spritefont`, and with `--IncludeFont` it also copies `Roboto-Bold.ttf` into the same folder. Open the `.spritefont` file to set the font name, size, spacing and the range of characters that the font should include.

> [!NOTE]
> The builder treats a `.ttf` file as a font of its own, so exclude the font file when the `.spritefont` file refers to it. Otherwise the builder tries to build both:
>
> ```csharp
> contentCollection.Include<WildcardRule>("*");
> contentCollection.Exclude<WildcardRule>("Fonts/*.ttf");
> ```

> [!IMPORTANT]
> If the `.spritefont` file names a font that is installed on your machine instead of a file, every developer and build server needs that font installed.
>
> Storing the `.ttf` in the `Assets` folder alongside the font avoids this.
> See [TrueType fonts](../adding_ttf_fonts.md) for more detail.

Load the font as a `SpriteFont`:

```csharp
var font = Content.Load<SpriteFont>("Fonts/Hud");
```

### Localized fonts

A localized font is a `.spritefont` file of type `LocalizedFontDescription` that lists the `.resx` files whose text the font must be able to draw. Build it with the `LocalizedFontProcessor` instead of the default font processor:

```csharp
contentCollection.Include("Fonts/Hud.spritefont", new FontDescriptionImporter(), new LocalizedFontProcessor());
```

See [Localization](../localization.md) for how the `.resx` files are listed in the font description.

### Effects

Create or copy a `.fx` file into the `Assets` folder.  By default, the `EffectImporter` and `EffectProcessor` are used to import the effect/shader file, and the processor compiles the effect for the target platform. It is loaded as an `Effect` at runtime.

To set compiler symbols or the debug mode, pass a processor with the settings:

```csharp
contentCollection.Include("Effects/Dissolve.fx", new EffectImporter(), new EffectProcessor
{
    Defines = "QUALITY=2",
    DebugMode = EffectProcessorDebugMode.Optimize
});
```

See [Custom Effects](../custom_effects.md) for more about writing effects.

### XML and other data files

Copy the `.xml` file into the `Assets` folder. The `XmlImporter` reads the file using the [`IntermediateSerializer`](https://github.com/SimonDarksideJ/XNAGameStudio/wiki/Everything-you-ever-wanted-to-know-about-IntermediateSerializer), so the file must follow the XNA XML content format and its types must be available to the builder. See the [XML content how-to](../../../getting_to_know/howto/content_pipeline/HowTo_Add_XML.md) for the format.

Files that are not in a pipeline format, such as `.json` or plain text, have no importer. To import these you have two choices:

- Copy the file to the output with `IncludeCopy`, and read it at runtime yourself.
- Write a custom [importer](https://github.com/MonoGame/Starter-Kit-3D-Platformer/blob/main/Content/Builder/JsonImporter.cs) and [processor](https://github.com/MonoGame/Starter-Kit-3D-Platformer/blob/main/Content/Builder/JsonSceneProcessor.cs), so that problems with the file are found when you build and not when the game runs. See [part 3](advanced.md#custom-importers-and-processors).

> [!NOTE]
> The Importer and Processor links provided are from the [Starter-Kit-3D-Platformer](https://github.com/MonoGame/Starter-Kit-3D-Platformer) sample provided by the MonoGame Foundation, which uses a custom importer/processor to handle the json configuration from a blender scene.

### Models and video

Copy the model or video into the `Assets` folder. Models use the `FbxImporter` importer and the `ModelProcessor` processor by default. `.mp4` and `.wmv` files use the `VideoProcessor`. Models that refer to textures have those textures built too.

> [!NOTE]
> Fun fact, the `FbxImporter` is actually a front for the [Open Asset Importer Library (`Assimp`)](https://assimp.org/) that MonoGame uses to import model files, which supports 40+ formats, not just FBX.

## Content Pipeline extensions

As stated previously, the content pipeline enables you to build your own custom content importers and processors.

To create a new extension, you can use the `mgpipelineitem` item template to create a new importer and a processor in the current folder.

Alternatively, you can use the `mgpipeline` project template to create a separate library for them.

> [!IMPORTANT]
> For the MGCB editor, if you created a separate library for use with the Content Pipeline it **MUST** target **.NET 8**.
>
> With the Content Builder, this is no longer an issue and you can use .NET 9 or 10 for all projects.

See [part 3](advanced.md#custom-importers-and-processors) for how to use them with the builder.

## Debugging the builder

To see more detail in the console, run the builder with `-l Debug`.

If you need to step through an importer or processor, ask the builder to start a debugger. Add this before `builder.Run`, build, then attach your IDE when prompted:

```csharp
#if DEBUG
System.Diagnostics.Debugger.Launch();
#endif
```

You can also run the builder as a normal console application from your IDE, with the arguments you would pass on the command line, and place breakpoints in your own importers and processors.

> [!NOTE]
> If a Content Builder project does not run when you build in Rider, see the build settings described in [Setting up Rider](../../2_choosing_your_ide_rider.md).

## Next steps

[Part 3](advanced.md) covers platform specific content, custom importers and automating content builds.
