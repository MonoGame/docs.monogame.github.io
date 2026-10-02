---
title: "Content Builder, Part 3: Advanced Scenarios and Automation"
description: Learn how to build different content for each platform, add custom importers and processors, and automate content builds with GitHub Actions.
---

This is part 3 of a four-part guide on the MonoGame Content Builder. [Part 2](setup.md) covered the rules for building content. This part covers the situations that need more than the basic rules, and how to run content builds in an automated workflow.

## Platform specific content

The `Parameters` property of the `Builder` class holds the settings of the current build, including the target platform. Use it to change what is built for each platform.

By default, the builder already produces content in the format that suits the target platform. You can use platform-specific rules only when you want different results per platform, for example, to use a different texture compression format for mobile devices.

```csharp
public override IContentCollection GetContentCollection()
{
    var contentCollection = new ContentCollection();

    if (Parameters.Platform == TargetPlatform.Android)
    {
        contentCollection.Include<WildcardRule>("Textures/*.png",
            contentProcessor: new TextureProcessor
            {
                GenerateMipmaps = true,
                TextureFormat = TextureProcessorOutputFormat.EtcCompressed
            });
    }
    else
    {
        contentCollection.Include<WildcardRule>("Textures/*.png",
            contentProcessor: new TextureProcessor
            {
                GenerateMipmaps = true,
                TextureFormat = TextureProcessorOutputFormat.Color
            });
    }

    return contentCollection;
}
```

> [!NOTE]
>`TextureProcessor` and `TextureProcessorOutputFormat` are in the `Microsoft.Xna.Framework.Content.Pipeline.Processors` namespace.

## Optional content folders

Rules depend on what is on disk at the time of a content run. This is useful for content that only some builds have, such as extra content packs. `Parameters.RootedSourceDirectory` gives the full path of the assets folder.

```csharp
public override IContentCollection GetContentCollection()
{
    var contentCollection = new ContentCollection();

    // Base game content.
    contentCollection.Include<WildcardRule>("BaseGame/*");

    // An extra content pack, built only when the folder exists.
    var packPath = Path.Combine(Parameters.RootedSourceDirectory, "Packs");
    if (Directory.Exists(packPath))
    {
        contentCollection.SetContentRoot("Content/Packs");
        contentCollection.Include<WildcardRule>("Packs/*");
    }

    return contentCollection;
}
```

The content root keeps the extra files inside the `Content` folder, in a folder of their own. Remember that a root applies to every rule you add after it.

## Custom importers and processors

When your game uses a data format that the pipeline does not know, a custom importer and processor lets the builder check the file during the build. Problems are then found when you build, not when the game starts and fails.  An importer reads a file into a type, and a processor converts that type into the final content.

This example reads a text file and converts it to upper case:

```csharp
using Microsoft.Xna.Framework.Content.Pipeline;

[ContentImporter(".note", DisplayName = "Note Importer", DefaultProcessor = "NoteProcessor")]
public class NoteImporter : ContentImporter<string>
{
    public override string Version { get; } = "1";

    public override string Import(string filename, ContentImporterContext context)
        => File.ReadAllText(filename);
}

[ContentProcessor(DisplayName = "NoteProcessor")]
public class NoteProcessor : ContentProcessor<string, string>
{
    public override string Process(string input, ContentProcessorContext context)
        => input.ToUpperInvariant();
}
```

Because the builder is an ordinary console project, you can put these classes in the Content Builder project itself and use them in a rule directly:

```csharp
contentCollection.Include("Data/welcome.note", new NoteImporter(), new NoteProcessor());
```

An importer in the Content Builder project is also found from the file extension in its `ContentImporter` attribute, so this rule has the same result, using the default processor named in the attribute:

```csharp
contentCollection.Include("Data/welcome.note");
```

You can also keep them in a separate library, which is useful when other tools share the same extensions. The `mgpipeline` template creates a pipeline extension library, and the `mgpipelineitem` item template adds an importer and processor to a project:

```bash
dotnet new mgpipelineitem -n Note -o Note
```

> [!IMPORTANT]
> If you create a separate library for use with the Content Pipeline, be it a custom pipeline project or even a class library, it **MUST** target **.NET 8**.  This is a known limitation with the content pipeline currently and will be addressed in a future release.  Although in practice, for the work needed for most custom Content Importers, this has not caused an issue to date.

The item template creates placeholder types that you replace with your own. To use a library, add a project reference from the Content Builder project to the library. The library should target .NET 8 only.

The built-in importers read the data your game needs from standard formats. A custom type that you write to a built file also needs a `ContentTypeWriter` in the pipeline and a `ContentTypeReader` in your game. See [Adding a Custom Importer/Processor](../../../getting_to_know/whatis/content_pipeline/CP_AddCustomProcImp.md) for the full process.

Update the `Version` of an importer or processor whenever you change its behavior, so that the builder knows to rebuild the assets that use it. See [caching](setup.md#caching) in part 2.

## Automating content builds with GitHub Actions

The **Build-Content** action provided by MonoGame, builds the content of a project in a GitHub Actions workflow. It executes your Content Builder project, so everything in the earlier parts of this course applies unchanged.

Automating content builds has several advantages:

- Content is built the same way every time, which removes "works on my machine" problems.
- Content for several platforms can be built in parallel.
- Content builds can run on a machine that suits them. For example, a Windows machine can compile shaders without Wine.
- The builder, the assets and the game can live in separate repositories.
- Built content can be stored as a workflow artifact for others to download.

### Ways to organize a project

The action supports several arrangements. Choose the one that fits how your team works:

- **One repository.** The game, the builder and the assets are together. This is the simplest to maintain, and building the game builds the content.
- **One repository, content first.** Everything is together, but the content is built before the game, so problems with assets are found early. The built content is then copied into the game build.
- **Separate repositories for the game and for the builder and assets.** Content can change and be built without touching the game. The outputs are combined when you package the game.
- **Separate repositories for everything.** The builder, the assets and the game each have their own repository. The builder can be released on its own, and the assets keep building with the previous version of it.

### The MonoGame actions

MonoGame hosts several actions in the [monogame-actions](https://github.com/MonoGame/monogame-actions) repository:

| Action | Purpose |
|--------|---------|
| `build-content` | Runs a Content Builder project to build content. |
| `install-wine` | Installs Wine, for running Windows tools on Linux or macOS. |
| `install-fonts` | Installs fonts that your assets need. |
| `install-android-dependencies` | Installs what Android builds need. |
| `publish-itchio` | Publishes a build to itch.io. |

### Build-Content inputs and outputs

| Input | Required | Default | Description |
|-------|----------|---------|-------------|
| `content-builder-path` | No | `./CBPlatformTest/Content` | Path to the Content Builder project. When `content-builder-repo` is set, this is the folder inside that repository. |
| `content-builder-repo` | No | empty | A repository that holds the builder, as `owner/repo`. |
| `assets-path` | No | `./Assets` | Path to the assets. When `assets-repo` is set, this is the folder inside that repository. |
| `assets-repo` | No | empty | A repository that holds the assets, as `owner/repo`. |
| `monogame-platform` | Yes | none | The platform to build for, such as `DesktopGL`, `Android` or `iOS`. |
| `output-folder` | Yes | none | Where the built content is written, relative to the repository root. |
| `additional-args` | No | empty | Extra arguments for the builder, separated by spaces. |
| `upload-output` | No | `false` | Upload the built content as a workflow artifact. |
| `configuration` | No | `Release` | The build configuration for the Content Builder project. |
| `github-token` | No | empty | A token for cloning private repositories. |

The action returns these outputs:

| Output | Description |
|--------|-------------|
| `output-folder` | The full path of the folder that holds the built content. |
| `log-file` | The full path of the build log. |
| `success` | `true` when the content built without failures. |

The path inputs mean different things for local and remote sources. For a local builder, set only `content-builder-path`, from the repository root. For a remote builder, set `content-builder-repo` to the repository and `content-builder-path` to the folder inside it, or to an empty string for the root. The same pattern applies to the assets.

The action does not install .NET. Add a `setup-dotnet` step before it.

### A basic workflow

The following workflow builds the content of a project in which the game, the builder and the assets share a repository, then builds the game:

```yaml
name: Build Game with Content

on:
  workflow_dispatch:

jobs:
  build:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v5

      - name: Setup .NET
        uses: actions/setup-dotnet@v5
        with:
          dotnet-version: '9.0.x'

      - name: Process content
        uses: MonoGame/monogame-actions/build-content@v1
        with:
          content-builder-path: './Content/Builder'
          assets-path: './Content/Assets'
          monogame-platform: 'DesktopGL'
          output-folder: './MyGame/bin/Release/net9.0/Content'
          configuration: Release

      - name: Build game
        run: dotnet build -c Release MyGame/MyGame.csproj
```

> [!IMPORTANT]
> The `output-folder` must be where the game project expects to find its `Content` folder. For the full list of options and more examples, see the [Build-Content documentation](https://github.com/MonoGame/monogame-actions/blob/main/build-content/README.md).

### Sample repositories

MonoGame provides sample repositories that show the different arrangements:

- [MonoGame-CBPlatform-Test](https://github.com/MonoGame/MonoGame-CBPlatform-Test) holds a complete platformer with the game, the builder and the assets together. Its [build.yml](https://github.com/MonoGame/MonoGame-CBPlatform-Test/blob/main/.github/workflows/build.yml) builds the game for DesktopGL, Android and iOS with a matrix. Its [build-remote.yml](https://github.com/MonoGame/MonoGame-CBPlatform-Test/blob/main/.github/workflows/build-remote.yml) builds with a builder and assets taken from other repositories.
- [MonoGame-CBPlatform-BuilderTest](https://github.com/MonoGame/MonoGame-CBPlatform-BuilderTest) holds only a Content Builder project. It is useful when you want a builder that artists can use, and its workflow builds assets from a remote repository.
- [MonoGame-CBPlatform-TestAssets](https://github.com/MonoGame/MonoGame-CBPlatform-TestAssets) holds only assets. Its workflow builds them with a builder from another repository. The assets do not have to be at the root, so one repository can hold several sets of assets for different targets.

> [!NOTE]
> The iOS project in the sample builds but does not publish. Publishing needs signing details and a team in an Apple developer account. See [Packaging Games](../../packaging_games.md) for how to set up iOS publishing.

### A multi-platform workflow

A matrix runs the same steps for each platform. Each entry sets the runner, the runtime identifier, the .NET workload and the target framework:

```yaml
strategy:
  matrix:
    include:
      - platform: iOS
        os: macos-26
        runtime: ios-arm64
        workload: ios
        tfm: net10.0-ios
      - platform: Android
        os: windows-latest
        runtime: android-arm64
        workload: android
        tfm: net10.0-android
      - platform: DesktopGL
        os: windows-latest
        runtime: win-x64
        workload: ''
        tfm: net10.0
```

Each platform writes its output to a different folder, so the content output path needs to be calculated for each entry. The sample repository does this in a step before the content build:

```yaml
- name: Set output paths
  id: paths
  shell: bash
  run: |
    PROJECT_DIR=$(dirname "${{ matrix.project }}")
    if [ "${{ matrix.platform }}" == "iOS" ]; then
      RID_PATH="bin/${{ env.Configuration }}/${{ matrix.tfm }}"
    else
      RID_PATH="bin/${{ env.Configuration }}/${{ matrix.tfm }}/${{ matrix.runtime }}"
    fi
    echo "content_output=./MyGame/$PROJECT_DIR/$RID_PATH/Content" >> $GITHUB_OUTPUT
```

The later steps install the workload when there is one, build the content, then build and publish the game. The `WorkflowMode` property is explained in the next section.

```yaml
- name: Install workload
  if: ${{ matrix.workload != '' }}
  run: dotnet workload install ${{ matrix.workload }}

- name: Process content
  uses: MonoGame/monogame-actions/build-content@v1
  with:
    content-builder-path: './Content'
    assets-path: './Content/Assets'
    monogame-platform: ${{ matrix.platform }}
    output-folder: ${{ steps.paths.outputs.content_output }}
    configuration: ${{ env.Configuration }}

- name: Build (iOS)
  if: ${{ matrix.platform == 'iOS' }}
  run: dotnet build -c ${{ env.Configuration }} ${{ matrix.project }} -p:WorkflowMode=true

- name: Build (other platforms)
  if: ${{ matrix.platform != 'iOS' }}
  run: dotnet build -c ${{ env.Configuration }} ${{ matrix.project }} -r ${{ matrix.runtime }} -p:WorkflowMode=true

- name: Publish
  if: ${{ matrix.platform != 'iOS' }}
  run: dotnet publish ${{ matrix.project }} -c ${{ env.Configuration }} -r ${{ matrix.runtime }} --self-contained
```

### Project changes for automated builds

Android and iOS bundle the content inside the application, so the content must be in the right place before the project builds. The platform projects from the templates look for content in the output folder of a local build. A workflow puts the content somewhere else, so the project needs to look in both places.

For Android, the sample project includes content from the runtime identifier folder when a runtime identifier is set, and from the normal output folder otherwise:

```xml
<ItemGroup>
  <!-- CI/CD builds with a RuntimeIdentifier -->
  <AndroidAsset Include="$(ProjectDir)bin\$(Configuration)\$(TargetFramework)\$(RuntimeIdentifier)\Content\**\*" Condition="'$(RuntimeIdentifier)' != '' AND Exists('$(ProjectDir)bin\$(Configuration)\$(TargetFramework)\$(RuntimeIdentifier)\Content')">
    <Link>Content\%(RecursiveDir)%(Filename)%(Extension)</Link>
  </AndroidAsset>
  <!-- Local builds without a RuntimeIdentifier -->
  <AndroidAsset Include="$(ProjectDir)$(OutputPath)Content\**\*" Condition="'$(RuntimeIdentifier)' == '' AND Exists('$(ProjectDir)$(OutputPath)Content')">
    <Link>Content\%(RecursiveDir)%(Filename)%(Extension)</Link>
  </AndroidAsset>
</ItemGroup>
```

For iOS, the sample project uses a `WorkflowMode` property. The property is not part of MonoGame. It is a property that the sample project defines to tell a workflow build from a local build. The project sets it to `false`, and the workflow turns it on with `-p:WorkflowMode=true` on the `dotnet build` command:

```xml
<PropertyGroup>
  <WorkflowMode>false</WorkflowMode>
</PropertyGroup>

<ItemGroup>
  <!-- Workflow builds -->
  <BundleResource Include="$(ProjectDir)bin\$(Configuration)\$(TargetFramework)\Content\**\*" Condition="'$(WorkflowMode)' == 'true' AND Exists('$(ProjectDir)bin\$(Configuration)\$(TargetFramework)\Content')">
    <Link>Content\%(RecursiveDir)%(Filename)%(Extension)</Link>
  </BundleResource>
  <!-- Local builds -->
  <BundleResource Include="$(ProjectDir)$(OutputPath)Content\**\*" Condition="'$(WorkflowMode)' != 'true' AND Exists('$(ProjectDir)$(OutputPath)Content')">
    <Link>Content\%(RecursiveDir)%(Filename)%(Extension)</Link>
  </BundleResource>
</ItemGroup>
```

If your workflow has already built the content, you can stop `BuildContent.targets` from building it a second time when the game project builds. Add a condition to the target, and pass the property from the workflow:

```xml
<Target Name="BuildContent" BeforeTargets="BeforeCompile" Condition="'$(WorkflowMode)' != 'true'">
```

See the [Android](https://github.com/MonoGame/MonoGame-CBPlatform-Test/blob/main/CBPlatformTest/CBPlatformTest.Android/CBPlatformTest.Android.csproj) and [iOS](https://github.com/MonoGame/MonoGame-CBPlatform-Test/blob/main/CBPlatformTest/CBPlatformTest.iOS/CBPlatformTest.iOS.csproj) project files in the sample repository for the complete files.

### Platform requirements

> [!IMPORTANT]
> Use the same .NET SDK version in your workflow and on your development machine. Add a `global.json` file to pin the version, and set the same version in `actions/setup-dotnet`. MonoGame `3.8.6` projects use .NET 9 or later. (although .NET 10 is recommended)

```json
{
  "sdk": {
    "version": "9.0.307"
  }
}
```

| Platform | Runner | Notes |
|----------|--------|-------|
| iOS | A macOS runner such as `macos-26` | Needs `dotnet workload install ios`. Use `ios-arm64` for devices. The sample skips publishing in CI. |
| Android | `windows-latest` or `ubuntu-latest` | Needs `dotnet workload install android`. Add keystore signing for release builds. |
| Windows | `windows-latest` | Use a runtime such as `win-x64`. |
| Linux | `ubuntu-latest` | Use a runtime such as `linux-x64`. |
| DesktopGL | Any runner | The content builds on any operating system. |

### Logs and artifacts

The action uploads the build logs as an artifact named `content-build-logs-<platform>-<run_id>`, even when the build fails. The logs are `restore.log`, `build.log` and `content-pipeline.log`. The artifact is kept for 30 days. Set `upload-output` to `true` to upload the built content too, in an artifact named `content-output-<platform>-<run_id>`.

### Workflow patterns

**Build content separately.** Build the content once for each platform, upload it, and download it in the game build jobs:

```yaml
jobs:
  build-content:
    runs-on: windows-latest
    strategy:
      matrix:
        platform: [DesktopGL, iOS, Android]
    steps:
      - uses: actions/checkout@v5
      - uses: actions/setup-dotnet@v5
        with:
          dotnet-version: '9.0.x'
      - name: Process content
        uses: MonoGame/monogame-actions/build-content@v1
        with:
          content-builder-path: './Content'
          assets-path: './Content/Assets'
          monogame-platform: ${{ matrix.platform }}
          output-folder: './Output/${{ matrix.platform }}'
          upload-output: 'true'

  build-game:
    needs: build-content
    runs-on: windows-latest
    steps:
      - name: Download content
        uses: actions/download-artifact@v5
        with:
          name: content-output-DesktopGL-${{ github.run_id }}
```

**Build content with the game.** Run the Build-Content step in each platform job, before the game build, as in the multi-platform workflow above.

You can also choose when a workflow runs. For example, rebuild when the assets or project files change:

```yaml
on:
  push:
    paths:
      - 'Content/Assets/**'
      - '**/*.csproj'
```

### Troubleshooting

| Problem | What to check |
|---------|---------------|
| The builder or assets are not found | Paths are relative to the repository root. Use a `./` prefix and forward slashes. Check the `content-builder-path` and `assets-path` rules for remote repositories. |
| A build fails for one platform only | Check that the workload is installed and that the runtime identifier matches the platform. Read the logs for that platform. |
| The build works locally but fails in the workflow | The .NET SDK versions differ. Add a `global.json`. Run `dotnet --list-sdks` locally to check your version. |
| Assets fail to build | Download the `content-build-logs` artifact and read `content-pipeline.log`. Run the same build locally with the same platform. |
| Cloning a repository fails | Use the `owner/repo` format, with no `https://` and no `.git`. For a private repository, set `github-token`. |
| The platform is not recognized | The `monogame-platform` value must match a platform name exactly, including case. |

### More information

- [Build-Content action](https://github.com/MonoGame/monogame-actions/tree/main/build-content)
- [Packaging Games](../../packaging_games.md)
- [GitHub Actions documentation](https://docs.github.com/en/actions)
- [.NET runtime identifier catalog](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog)

## Next steps

[Part 4](migrating_from_mgcb.md) explains how to move an existing project from the MGCB Editor to the Content Builder.
