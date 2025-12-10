---
title: Working with new Content Builder Projects
description: Learn how to use the newest approach to building content in MonoGame without the legacy MGCB-Editor.
---

> [!NOTE]
> These instructions **REQUIRE** you to use the latest `MonoGame Develop` release (currently `3.8.5-develop.13`)
>
> See the [instructions here](https://docs.monogame.net/articles/getting_to_know/howto/HowTo_Install_Preview_Release.html) for how to install the preview project templates and update your project to use `3.8.5-develop` (preview) releases.
>
> [How to install MonoGame Preview packages](https://docs.monogame.net/articles/getting_to_know/howto/HowTo_Install_Preview_Release.html)

The [MonoGame Content Pipeline](/articles/getting_started/content_pipeline/why_content_pipeline) is a powerful tool for packaging and managing all the complex needs for working with and distributing assets in your MonoGame projects, solving all the complexities surrounding the use of assets on all the platforms that MonoGame supports.

From MonoGame `3.8.5` we are making a new `Console Project Style` solution to more effectively manage your content in source to reduce complexity (and installation demands) and put control of your source assets fully within your control.

> [!IMPORTANT]
> **This documentation is a work in progress and subject to change.**
>
> While every effort will be made during the `3.8.5` preview to keep this documentation up to date, check back often for reference and log issues for corrections to assist the team in its development.

## Table of Contents

1. [What is the ContentBuilder?](#what-is-the-contentbuilder)
2. [Getting Started](#getting-started)
3. [Adding the Content Builder Project to your Game](#adding-the-content-builder-project-to-your-game)
4. [Understanding ContentBuilderParams](#understanding-contentbuilderparams)
5. [Creating Your ContentCollection](#creating-your-contentcollection)
6. [Including and Excluding Content](#including-and-excluding-content)
7. [Working with Importers and Processors](#working-with-importers-and-processors)
8. [Advanced Scenarios](#advanced-scenarios)

---

If you prefer, the MonoGame Foundation recorded a short video demonstrating how to get setup with the new Content Builder Project.

<div class="embeddedvideo">
<iframe width="560" height="315" src="https://www.youtube.com/embed/QB43LgRmdNM/" title="New Content Builder Project - Getting Started" frameborder="0" allowfullscreen></iframe>
</div>

---

## What is the ContentBuilder?

The `ContentBuilder` is a powerful tool that allows you to build game content (like images, sounds, fonts, and models) programmatically using C# code instead of manually configuring content files. Think of it as your own personal content pipeline that you control entirely through code!

Instead of an installed dotnet tool (which historically generated issues), the new approach uses a simple `Console` application you manage, which is simply executed to build and generate your content.  By default, the project template pulls all the content from the designated "**Assets**" folder and compiles it using the default importers used by MonoGame.  To customize this, you only need to specify additional commands to override this default behaviour with your customizations.

> [!TIP]
> When designing the builder for your content, try to focus on building patterns for your content to reduce the need to rebuild the `Builder` itself.
>
> In an ideal scenario, you build the `Builder` once, then just keep adding assets and running the builder to produce the runtime content, identifying "early on" any issues with your assets and simplifying production.

In addition, due to content now being built through your own console application, it is easily distributable and can even be used in automation pipelines in source control.

### Features provided with the new Content Builder project

- **Automation**: Build content as part of your build process.
- **Flexibility**: Apply complex rules to determine which files get processed.
- **Control**: Fine-tune how each asset is imported and processed.
- **Integration**: Perfect for tools, editors, and custom-built pipelines.
- **Customization**: Inject additional capabilities into your asset management as required.

> [!TIP]
> Yes, it is completely possible, through automation, to have your Content and Game EXE/deliverables built separately, and then simply combined later when packaging the solution.
>
> We will look to provide more guides in the future to showcase this capability.

---

## Getting Started

To start using the ContentBuilder, you will need to create a console project using the MonoGame ContentBuilder template. Here are instructions to create your own Content Builder Project:

### [Visual Studio Code](#tab/vscode)

To add the class library using the `MonoGame Content Builder` project template in Visual Studio Code, perform the following:

1. In the [*Solution Explorer*](/articles/tutorials/building_2d_games/02_getting_started/index.md#install-the-c-dev-kit-extension) panel, right-click the solution.
2. Choose *New Project* from the context menu.
3. Enter "MonoGame Content Builder" and select it as the template to use.
4. Name the project "MyContentBuilder".
5. When prompted for a location, use the default option, which will put the new project in a folder next to your game project.
6. Select "Create Project".

### [Visual Studio 2022](#tab/vs2022)

To add the class library using the `MonoGame Content Builder` project template in Visual Studio 2022, perform the following:

1. Right-click the solution in the Solution Explorer panel.
2. Choose Add > New Project from the context menu.
3. Enter "MonoGame Content Builder" in the search box, select that template, then click Next.
4. Name the project "MyContentBuilder".
5. The default location will place the new project in a folder next to your game project; you do not need to adjust this setting.
6. Click "Create".

### [dotnet CLI](#tab/dotnetcli)

To create a Content Builder project using the `MonoGame Content Builder` project template with the dotnet CLI, perform the following:

1. Open a new Command Prompt or Terminal window in the same folder as your Game solution file. (or in an empty folder if it is separate from your Game)
2. Enter the command `dotnet new mgcb -o MyContentBuilder` to create the project, placing it in a folder next to your game project.
3. (optional) Enter the command `dotnet sln <your solution>.sln add ./MyContentBuilder/MyContentBuilder.csproj` to add the newly created Content Builder project to your game's solution file.

---

This creates a project with the following structure:

```text
MyContentBuilder/
├── Builder/
│   └── Builder.cs          # Your custom builder class
├── Assets/
│   └── readme.txt          # Placeholder text file to be replaced with your content
└── MyContentBuilder.csproj  # Project file with required packages
```

This is the default (recommended) layout for a Content Builder project, but you can customize its use how you wish:

- There is a single "entry point" (command line launch point) contained in the `builder.cs` which contains all the instructions to build content.
- A default `Assets` folder to host your content, but your content can be in any location you need it to be, simply update the parameters used to run the console application to point to the source directory and folder for your content.

> [!TIP]
> While it is possible to include the builder as part of your game code, rather than a separate project, at this time we do not recommend this.  Based on experience, it is always better to build your content separate from your runtime game to avoid collisions or issues arising in game production/deployment.

## Adding the Content Builder Project to your Game

Once you have created your `Builder` project, you have two options:

1. After building your game, you need to copy the build output from the Builder to the output of the Game, so the `exe` and the `content` folder are in the same output folder.
2. Add a `Target` definition to your `csproj` to automatically copy the Builder output to the game output when the project is built (like how the current MGCB solution works)

This section will focus on Option 2 to replicate what happens in `3.8.4` projects.

### 1. Clean up redundant files

As the Content Builder project is **NOT** a DotNet tool, you no longer need the `dotnet-tools.json` configuration.

1. Remove any `.config` folders from your solution that contain `dotnet-tools.json` files.

### 2. Remove legacy MGCB references from your `csproj`

As you would expect, we no longer need any old `Content` references or tools for building that content, aka, the `MonoGame.Content.Builder.Task` reference.

Per platform/game project:

1. Remove any `MonoGameContentReference` elements from your game's `csproj`.
2. Remove references to `MonoGame.Content.Builder.Task`.
3. Update the MonoGame version to match the Builder (not essential, but recommended)

### 3. Add the Builder task

To complete the process, add the following `Target` section to your `csproj` (before the final ```</Project>``` entry):

```xml
  <Target Name="BuildContent" BeforeTargets="Build">
    <PropertyGroup>
      <ContentOutput>$(ProjectDir)$(OutputPath)</ContentOutput>
      <ContentTemp>$(ProjectDir)$(IntermediateOutputPath)</ContentTemp>
      <ContentArgs>build -p $(MonoGamePlatform) -s Assets -o $(ContentOutput) -i $(ContentTemp)</ContentArgs>
    </PropertyGroup>
    <MSBuild Projects="..\Content\Content.csproj" Targets="Build;Run"
      Properties="RunArguments=$(ContentArgs);" />
  </Target>
```

Of note, you should take care with the following elements:

- `-s Assets` - this assumes you are using the default "Assets" source folder in the builder project.  If your source content is elsewhere you will need to provide the path to it.
- `-i $(ContentTemp)` - this uses the default `obj` folder for building the content, if you are short on space, you can provide an alternate path for temp storage.
- `Projects="..\Content\Content.csproj"` - This should **MATCH** the folder and project name of your Builder.  If you called your Builder something else or renamed the folder/csproj, make sure it matches.

You should not need to touch anything else.

> [!NOTE]
> Additionally, if you want to control HOW and WHEN your content is built/rebuilt, then you can change the `Targets="Build;Run"` options to only execute the last on Build OR Run, or a different action.
>
> See the [MS Documentation on Built targets](https://learn.microsoft.com/en-us/visualstudio/msbuild/msbuild-targets) for reference.

## Platform specific additions - Android / iOS

For platforms where the content is required to be packaged inside the output bundle, there are some additional steps required to ensure the built content is including during the Build/Publish step.

Each platform has specific requirements and require the built content to be available prior to processing the project itself.

> [!NOTE]
> Also check out the dedicated [Packaging Games advice](../packaging_games.md) published by the MonoGame team.

### Android

For Android, a specific `Include` section is needed in the project's `csproj` project definition, as follows:

> [!NOTE]
> Android uses a special `AndroidAsset` tag to identify content to include.

```xml
<ItemGroup>
    <AndroidAsset Include="$(OutputPath)Content\**\*">
        <Link>Content\%(RecursiveDir)%(Filename)%(Extension)</Link>
    </AndroidAsset>
</ItemGroup>
```

> [!NOTE]
> If you change the location where the Content Builder project outputs content to, the paths above will need to be updated to the new location to find the content to include.

At build time, the "BuildContent" task will run and execute the Content Builder, then with the output from the builder, the Android solution will automatically identify and package the content within the `APK` as per Google's packaging requirements.

> [!NOTE]
> If your content is too large to fit within the maximum size of an `APK`, you will need to alternatively use Asset Bundles to package your content, as demonstrated here:
>
> ```xml
> <ItemGroup>
>    <AndroidAsset Include="$(OutputPath)Content\**\*">
>        <Link>Content\%(RecursiveDir)%(Filename)%(Extension)</Link>
>    </AndroidAsset>
>    <AndroidAsset Update="Content\Path\SomeLargeFile.xnb" AssetPack="foo" />
> </ItemGroup>
>
> However your game project will also need to be aware and update its strategy for loading the content to take the asset packs into account.
>
> For more details, see the [Microsoft Documentation](https://devblogs.microsoft.com/dotnet/android-asset-packs-in-dotnet-android/).
>
> Additionally Dean has provided [some helper scripts to assist with the implementation here](https://gist.github.com/dellis1972/8a16abceb3b4b2222b38f8bee221534b).

### iOS

For iOS, a specific `Include` section is needed in the project's `csproj` project definition, as follows:

> [!NOTE]
> iOS uses a special `BundleResource` tag to identify content to include.

```xml
<ItemGroup>
    <BundleResource  Include="$(OutputPath)Content\**\*">
        <Link>Content\%(RecursiveDir)%(Filename)%(Extension)</Link>
    </BundleResource >
</ItemGroup>
```

> [!NOTE]
> If you change the location where the Content Builder project outputs content to, the paths above will need to be updated to the new location to find the content to include.

At build time, the "BuildContent" task will run and execute the Content Builder, then with the output from the builder, the Android solution will automatically identify and package the content within the App as per Apple's packaging standards.

## Basic Builder Project Structure

The default `Builder.cs` file looks like this, which contains everything needed to build all content in the designated Assets/Content folder using the default [Importers and Processors](/articles/getting_to_know/whatis/content_pipeline/CP_StdImpsProcs) (how MonoGame compiles content) into processed `.XNB` files for consumption by your runtime project:

```csharp
using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Framework.Content.Pipeline.Builder;

// Console program entry point, make a new builder and run using commandline arguments
var builder = new Builder();
builder.Run(args);
return builder.FailedToBuild > 0 ? -1 : 0;


public class Builder : ContentBuilder
{
    public override IContentCollection GetContentCollection()
    {
        var contentCollection = new ContentCollection();
        
        contentCollection.Include<WildcardRule>("*");
        
        // Add your content rules here

        return contentCollection;
    }
}
```

> [!NOTE]
> The example above has been simplified for reference.  The default template includes options to run the Content Builder project without parameters using default configuration to aid testing.

The default `Include` rule simply builds a list of content to process from the target folder when the program is executed.

> [!IMPORTANT]
> We recommend keeping this default structure of getting all files and then specifying overrides.  You can alter this if you so wish to only use specific includes, but that it would require more maintenance as your content grows.  Try to be generic, design rules based on the structure of your content rather than being too specific.
>
> Although check the details later about providing parameters, while samples are currently "hard coding" importer/processor values, this would be improved through your own configuration.

---

## Understanding ContentBuilderParams

`ContentBuilderParams` defines the operational configuration that tells the ContentBuilder where to find your content, where to output it, and how to process it. Think of it as the "settings panel" for your content building operation.

### Core Properties

Here are the main properties available to work with:

| Property | Description | Default Value |
|----------|-------------|---------------|
| `WorkingDirectory` | The root directory for all operations | Current directory |
| `SourceDirectory` | Where your source assets are located | `"Content"` |
| `OutputDirectory` | Where built content will be saved | `"bin/Content"` |
| `IntermediateDirectory` | Where temporary build files go | `"obj/Content"` |
| `Platform` | Target platform for the content | `TargetPlatform.DesktopGL` |
| `GraphicsProfile` | Graphics capability level | `GraphicsProfile.HiDef` |
| `CompressContent` | Whether to compress output files | `false` |
| `LogLevel` | How much information to display | `LogLevel.Info` |
| `Rebuild` | Force rebuild all content (ignore cache) | `false` |

### Creating ContentBuilderParams

#### Option 1: Direct Initialization

```csharp
var contentParams = new ContentBuilderParams()
{
    Mode = ContentBuilderMode.Builder,
    WorkingDirectory = $"{AppContext.BaseDirectory}../../../",
    SourceDirectory = "Assets",
    OutputDirectory = "bin/DesktopGL/Content",
    Platform = TargetPlatform.DesktopGL,
    GraphicsProfile = GraphicsProfile.HiDef,
    CompressContent = false,
    LogLevel = LogLevel.Info
};
```

#### Option 2: From Command Line Arguments

```csharp
var contentParams = ContentBuilderParams.Parse(args);
```

This parses command line arguments like:

```bash
mybuilder.exe --src "MyAssets" --output "bin/Content" --platform DesktopGL --compress
```

### Understanding Directory Paths

The directory paths work together to organize your content:

> [!IMPORTANT]
> The `WorkingDirectory` should always be fixed path or directly resolvable from the application's execution path, which CAN be different based on whether the console application is run in `release` or `debug` mode.  The default path provided uses a `relative` path from the point of execution.  If in doubt, test with an `absolute` path and work back from there.

```text
WorkingDirectory/           # Your project root
├── SourceDirectory/        # Input: Your raw assets (images, sounds, etc.)
│   ├── Textures/
│   └── Audio/
├── OutputDirectory/        # Output: Built .xnb files ready for your game
│   ├── Textures/
│   └── Audio/
└── IntermediateDirectory/  # Temporary: Cache and build information
    └── cache/
```

**Important Note:** Paths can be absolute or relative to the `WorkingDirectory`.

---

## Creating Your ContentCollection

The `ContentCollection` is where you define the rules for how your content should be handled. It is like creating a recipe book that tells the ContentBuilder what to do with each file.

### Basic ContentCollection Setup

```csharp
public override IContentCollection GetContentCollection()
{
    var contentCollection = new ContentCollection();
    
    contentCollection.Include<WildcardRule>("*");

    // Your rules go here
    
    return contentCollection;
}
```

The default process queries your target assets folder and builds a list of all content, this ensures that **EVERYTHING** contained within that folder is available for processing, making it easy to simply add new content files to the structure without having to rebuild the `ContentBuilder`.  Only when you need to change the rules or specify different processing logic for various asset types or folders should you need to change this.

---

## Including and Excluding Content

The ContentCollection provides flexible ways to specify which files should be processed and how, by default in the template, "All content" is included and processed using the defaults.  Any files that cannot be compiled will be reported in the console output.

Any additional `ContentCollection` commands provided after the global include simply update the "processing" list, as all content is only built ONCE (per folder).

> [!TIP]
> The recommended approach is to leave the default "include" rule to gather all assets, and then provide overrides to the assets collected below.  You "Can" remove the default include and just write specific inclusion commands if you wish instead, the choice is completely up to you and how you want to manage your content.
>
> Just make sure to use a path that reduces the need to rebuild the "Builder" application using policies or patterns for your content to minimise friction when building content!

### Understanding Rule Order

**Important:** Rules are evaluated in order, and the **last matching rule wins**!

> [!NOTE]
> Rules simply update the processing order for each file. Whatever the last rule applied to affect the file/folder definition is, is the final state in which a file will be processed.  If you are not getting the output you expect, you can output the collection to file and examine it for reference, then check WHY a file was in its final state.

For example, the following rule order shows that all `png` files are included for processing initially, but then a later rule updates this and specifically excludes `png` files from processing. The result, no `png` files are processed and they are ignored:

```csharp
// Include all PNG files
contentCollection.Include<WildcardRule>("*.png");

// But exclude debug files
contentCollection.Exclude<WildcardRule>("*debug*.png");

// Result: All PNG files EXCEPT those with "debug" in the name
```

### Simple File Inclusion

If you want to do it the hard way, you can specify individual rules for including / updating content to be built, specifying specifically how the file should be processed, to which folder/file and how it should be processed.

> [!NOTE]
> Note the "Copy" rule, there are situations in content where you simply DO NOT want the file to be processed, only copied to the output folder, normally where there is additional processing required at runtime.
>
> MonoGame does recommend using custom [Importers](/articles/getting_to_know/whatis/content_pipeline/CP_AddCustomProcImp.md) and [Processors](/articles/getting_to_know/whatis/content_pipeline/CP_StdParamProcs.md) to handle custom content, especially `XML`, `TXT`, or `JSON` files, as issues with content can be identified earlier when content is built (and not when your game runs, and then invariably crashes)

**Include a Single File:**

```csharp
// Build a specific texture
contentCollection.Include("Textures/hero.png");

// Copy a file without processing
contentCollection.IncludeCopy("Data/config.json", "config.json");
```

**With Custom Output Path:**

```csharp
// Save to a different location
contentCollection.Include("Sprites/player.png", "Characters/Player.xnb");
```

### Pattern-Based Inclusion

The recommended pattern to use is to group your content either by folder or type and then use Wildcards to collectively refer to how content should be processed, e.g. all images in this folder should be processed in this way, rather than file by file.

#### Using Wildcards

Wildcards are perfect for matching multiple files with simple patterns:

```csharp
// Include ALL files
contentCollection.Include<WildcardRule>("*");

// Include all PNG files
contentCollection.Include<WildcardRule>("*.png");

// Include all files in Textures folder
contentCollection.Include<WildcardRule>("Textures/*");

// Include all PNG files in any subfolder
contentCollection.Include<WildcardRule>("*/*.png");

// Include specific file types
contentCollection.Include<WildcardRule>("*.{png,jpg,bmp}");
```

**Wildcard Pattern Reference:**

| Pattern | Matches |
|---------|---------|
| `*` | Any sequence of characters |
| `?` | Any single character |
| `[abc]` | Any character in the set |
| `[!abc]` | Any character NOT in the set |
| `{a,b,c}` | Any of the alternatives |

#### Using Regular Expressions

For more complex patterns, use [regular expressions](https://learn.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference):

> [!NOTE]
> Be wary, RexEx expressions are a dark art and may behave differently depending on the language used.  Use tools like [regex101](https://regex101.com/) (an online regex tester tool) to verify and check your search results, but note they have been known to operate differently based on OS/Language at runtime.

```csharp
// Match files ending with _diffuse.png
contentCollection.Include<RegexRule>(@".*_diffuse\.png$");

// Match numbered files
contentCollection.Include<RegexRule>(@"level_\d+\.tmx");

// Match files in specific folders
contentCollection.Include<RegexRule>(@"^(Textures|Sprites)/.*\.png$");
```

### Excluding Content

Sometimes you need to exclude certain files from processing:

> [!NOTE]
> This prevents the Content Builder from doing anything with a file and effectively ignores it, useful for associated files (like `.ttf` font files usually included alongside a `.spriteFont` descriptor) that do not need processing as they are consumed by another content process.

**Exclude Specific Files:**

```csharp
// Exclude a specific file
contentCollection.Exclude("Textures/debug_texture.png");

// Exclude multiple files
contentCollection.Exclude("Textures/test_01.png");
contentCollection.Exclude("Textures/test_02.png");
```

**Exclude Using Patterns:**

```csharp
// Exclude all files starting with "test_"
contentCollection.Exclude<WildcardRule>("test_*");

// Exclude all files in Debug folder
contentCollection.Exclude<WildcardRule>("Debug/*");

// Exclude backup files
contentCollection.Exclude<RegexRule>(@".*\.bak$");

// Exclude temporary files
contentCollection.Exclude<RegexRule>(@".*~$");
```

### Advanced Pattern Examples

As stated, it is recommended to define patterns for your content, defining rules for processing your content (and preventing changing/rebuilding the Content Builder exe), the following are examples of such patterns, grouping specific content into specific folders.

> [!NOTE]
> There are no predefined rules for how you organize your content, it is completely up to you and the needs of your project to determine how it is best organized.  Just make sure to include decisions on how to ease the burden of managing the importing content in those decisions so that you do not need to frequently keep updating your Builder.
>
> Set it once, and then you have strict rules for how content is processed and errors should be easily identifiable.

**Organize by File Type:**

```csharp
var contentCollection = new ContentCollection();

// Images
contentCollection.SetContentRoot("Graphics");
contentCollection.Include<WildcardRule>("*.{png,jpg,bmp}");

// Audio
contentCollection.SetContentRoot("Audio");
contentCollection.Include<WildcardRule>("*.{wav,mp3,ogg}");

// Fonts
contentCollection.SetContentRoot("Fonts");
contentCollection.Include<WildcardRule>("*.spritefont");

// Everything else (copy as-is)
contentCollection.SetContentRoot("Data");
contentCollection.IncludeCopy<WildcardRule>("*.{xml,json,txt}");
```

### Custom Output Paths with Functions

If you wish, you can also transform file paths dynamically:

```csharp
// Flatten directory structure
contentCollection.Include<WildcardRule>(
    "Textures/**/*.png",
    outputPath: filePath => Path.GetFileName(filePath)
);

// Add prefixes
contentCollection.Include<WildcardRule>(
    "*.png",
    outputPath: filePath => $"processed_{filePath}"
);

// Reorganize structure
contentCollection.Include<WildcardRule>(
    "Source/*/icon.png",
    outputPath: filePath => {
        var folderName = Path.GetDirectoryName(filePath);
        return $"Icons/{folderName}_icon.xnb";
    }
);
```

---

## Working with Importers and Processors

Importers and processors are the heart of the [content pipeline](/articles/getting_started/content_pipeline/why_content_pipeline). They determine how your raw assets are transformed into game-ready content and built to meet the needs for each target platform.

> [!IMPORTANT]
> Different platforms have different requirements for content and while you can manage it yourself by packaging and loading it manually, this usually leads to complicated scenarios, especially when targeting consoles.

### What are Importers and Processors?

- **Importer**: Reads your source file (e.g., PNG, WAV) and converts it into an intermediate format
- **Processor**: Takes the imported data and processes it into the final game format, sometimes including additional content or textures identified by the Importer. (Such as the skinned model importer)

### Default Behavior

By default, MonoGame automatically selects the appropriate importer and processor based on file extension:

```csharp
// MonoGame will automatically use:
// - TextureImporter for .png files
// - TextureProcessor to create the final texture
contentCollection.Include("hero.png");
```

### Specifying Custom Importers and Processors

Sometimes you need more control. Here is how to specify which importer and processor to use and override the default behaviour:

**Using Built-in Processors:**

```csharp
// Texture with specific processor
var textureImporter = new TextureImporter();
var textureProcessor = new TextureProcessor();
contentCollection.Include("hero.png", textureImporter, textureProcessor);

// Audio with specific processor  
var audioImporter = new WavImporter();
var audioProcessor = new SoundEffectProcessor();
contentCollection.Include("explosion.wav", audioImporter, audioProcessor);
```

> [!NOTE]
> As can be seen in the updated examples using the new Content Builder Project system, this can also be simplified to just:
>
> `contentCollection.Include("hero.png", new TextureImporter(), new TextureProcessor());`
>
> It just depends on which pattern you are more comfortable with.

**Pattern-Based with Custom Processors:**

This can also be used with pattern-based logic, applying the specified Importer/Processor to a group of assets:

```csharp
// Apply texture processor to all PNG files
var textureImporter = new TextureImporter();
var textureProcessor = new TextureProcessor();
contentCollection.Include<WildcardRule>(
    "*.png",
    contentImporter: textureImporter,
    contentProcessor: textureProcessor
);

// Apply font processor to all sprite fonts
var fontImporter = new FontDescriptionImporter();
var fontProcessor = new FontDescriptionProcessor();
contentCollection.Include<WildcardRule>(
    "*.spritefont",
    contentImporter: fontImporter,
    contentProcessor: fontProcessor
);
```

### Handling Custom Importer / Processor caching

The caching validation process that determines whether the Content Builder re-processes content by default is decided by:

- The comparison between the last state of the built asset and the source
- It will also take into account the "Version" of the Content Importer and Processor that was used at the time the content was built.

The cache is only invalidated when either of these conditions are met, else it will skip over processing of the asset for performance as it has not detected a change.

> [!NOTE]
> MonoGame manages the versions of the [built-in Importers and Processors](../../getting_to_know/whatis/content_pipeline/CP_StdImpsProcs.md), so unless MonoGame makes changes to these elements, the cache will not be invalidated unless you [Force a rebuild](#understanding-contentbuilderparams).

If you want content to be rebuilt when a Custom Importer or Processor has changed (content that uses the custom entities in its builder configuration), then the version of the Custom Content Pipeline Extension has to change, to do this, simply update the "Version" property on the respective Importer or Processor as follows:

```csharp
public override string Version { get; set; } = "new Version";
```

> [!NOTE]
> The version can be anything you like, so long as it is a recognisable and readable string.  Preferably something you can increment.

### Configuring Processor Parameters

Many processors have configurable parameters that control how they process content. If you need to override these parameters with specific values, here is how to customize them:

#### Example: Texture Processing Options

```csharp
var textureImporter = new TextureImporter();
var textureProcessor = new TextureProcessor
{
    ColorKeyColor = Color.Magenta,           // Transparent color
    ColorKeyEnabled = true,                   // Enable color keying
    GenerateMipmaps = true,                   // Create mipmaps
    PremultiplyAlpha = true,                  // Premultiply alpha
    ResizeToPowerOfTwo = false,               // Keep original size
    TextureFormat = TextureProcessorOutputFormat.Color
};

contentCollection.Include<WildcardRule>(
    "Sprites/*.png",
    contentImporter: textureImporter,
    contentProcessor: textureProcessor
);
```

#### Example: Audio Processing Options

```csharp
var audioImporter = new WavImporter();
var soundProcessor = new SoundEffectProcessor
{
    Quality = ConversionQuality.Best
};

contentCollection.Include<WildcardRule>(
    "SoundEffects/*.wav",
    contentImporter: audioImporter,
    contentProcessor: soundProcessor
);
```

#### Example: Font Processing Options

```csharp
var fontImporter = new FontDescriptionImporter();
var fontProcessor = new FontDescriptionProcessor
{
    PremultiplyAlpha = true,
    TextureFormat = TextureProcessorOutputFormat.Color
};

contentCollection.Include<WildcardRule>(
    "Fonts/*.spritefont",
    contentImporter: fontImporter,
    contentProcessor: fontProcessor
);
```

---

## Advanced Scenarios

The following scenarios are of a more advanced nature for scenarios where you need specific customisation to suit your delivery needs.

### Scenario 1: Platform-Specific Content

Build different content for different platforms:

> [!NOTE]
> By default, content will be formatted specifically for the platform it is built for.  This example is only where you might swap content based on the platform, e.g. for different quality sizes, store requirements or controller references.  All to make the runtime project simpler to manage.

```csharp
public override IContentCollection GetContentCollection()
{
    var contentCollection = new ContentCollection();
    
    if (Parameters.Platform == TargetPlatform.DesktopGL)
    {
        // High-quality textures for desktop
        var processor = new TextureProcessor
        {
            GenerateMipmaps = true,
            TextureFormat = TextureProcessorOutputFormat.Color
        };
        contentCollection.Include<WildcardRule>("*.png", 
            contentProcessor: processor);
    }
    else if (Parameters.Platform == TargetPlatform.Android)
    {
        // Compressed textures for mobile
        var processor = new TextureProcessor
        {
            GenerateMipmaps = true,
            TextureFormat = TextureProcessorOutputFormat.DxtCompressed
        };
        contentCollection.Include<WildcardRule>("*.png",
            contentProcessor: processor);
    }
    
    return contentCollection;
}
```

### Scenario 2: Multi-Language Content

Organize content by language:

> [!NOTE]
> MonoGame recommends using a custom localized importer for language-specific content, this example serves only as an alternative strategy for handling localization.

```csharp
public override IContentCollection GetContentCollection()
{
    var contentCollection = new ContentCollection();
    string language = "en"; // Get from configuration
    
    // Default content
    contentCollection.Include<WildcardRule>("Common/*");
    
    // Language-specific content
    contentCollection.SetContentRoot("Localized");
    contentCollection.Include<WildcardRule>($"Languages/{language}/*");
    
    return contentCollection;
}
```

### Scenario 3: Dynamic Content from External Sources

Process content from multiple sources:

```csharp
public override IContentCollection GetContentCollection()
{
    var contentCollection = new ContentCollection();
    
    // Base game content
    contentCollection.Include<WildcardRule>("BaseGame/*");
    
    // DLC content (if available)
    var dlcPath = Path.Combine(Parameters.RootedSourceDirectory, "DLC");
    if (Directory.Exists(dlcPath))
    {
        contentCollection.SetContentRoot("DLC");
        contentCollection.Include<WildcardRule>("DLC/*");
    }
    
    // Mod content (if available)
    var modPath = Path.Combine(Parameters.RootedSourceDirectory, "Mods");
    if (Directory.Exists(modPath))
    {
        contentCollection.SetContentRoot("Mods");
        contentCollection.Include<WildcardRule>("Mods/*");
    }
    
    return contentCollection;
}
```

---

## Debugging

At times you might need to debug your Content Builder, and while you can just run it manually in debug mode and attach your IDE to it, sometimes it is also valuable to FORCE the application to pause and let you step through importing and processing, especially when working with Content Pipeline Extensions.

Alternatively, you may simply want to inject additional assets into your project for debugging purposes (such as overlays, etc) which are only consumed in a `Debug` build of your project (using '#if DEBUG` in your runtime too).

> [!NOTE]
> This is a rather advanced area which for most situations you do not need to go to these lengths.

The following scenarios outline these options as they pertain to the Content Builder solution:

### Scenario 1: Debugging the Builder

If you wish to inject a `Debugger` breakpoint to determine the cause of any issues when building content, you can add the following before the `Builder` runs in your `ContentBuilder` project, this will cause DotNet to request to launch the default debugger and automatically attach it to the project when it is run:

```csharp
#if DEBUG
using System.Diagnostics;
#endif

// If you need to debug the content build process you can enable
// this, build the game, then attach the debugger when prompted.
#if DEBUG
Debugger.Launch();
#endif

// Launch point
builder.Run(args);
```

### Scenario 2: Conditional Debug Content

Useful when you need to include extra content in debug only builds for testing, e.g. texture overlays or additional fonts:

```csharp
public override IContentCollection GetContentCollection()
{
    var contentCollection = new ContentCollection();
    
    // Always include game content
    contentCollection.Include<WildcardRule>("GameAssets/*");
    
    #if DEBUG
    // Include debug visualizations only in debug builds
    contentCollection.SetContentRoot("Debug");
    contentCollection.Include<WildcardRule>("DebugAssets/*");
    #endif
    
    return contentCollection;
}
```

---

## Summary

The MonoGame ContentBuilder provides a powerful, flexible way to build your game content programmatically. Here are the key takeaways:

1. **ContentBuilderParams** configures where and how content is built.
2. **ContentCollection** defines rules for which files to process.
3. **Include/Exclude** methods with patterns give you precise control.
4. **Importers and Processors** can be customized for your needs.
5. **Rule order matters** - the last matching rule wins!

With these tools, you can create sophisticated content build pipelines that adapt to different platforms, languages, build configurations, and project structures.

Happy content building! 🎮✨

---

## Additional Resources

- [MonoGame Documentation](https://docs.monogame.net)
- [Content Pipeline Documentation](https://docs.monogame.net/articles/getting_to_know/whatis/content_pipeline/)
- [Custom Content Processors](https://docs.monogame.net/articles/content_pipeline/custom_effects.html)
