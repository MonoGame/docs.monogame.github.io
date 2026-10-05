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
> `TextureProcessor` and `TextureProcessorOutputFormat` are in the `Microsoft.Xna.Framework.Content.Pipeline.Processors` namespace.

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

When your game uses a data format that the pipeline does not know, a custom importer and processor lets the builder check the file during the build. Problems are then found when you build, not when the game starts and fails. An importer reads a file into a type, and a processor converts that type into the final content.

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

The **Build-Content** action provided by MonoGame builds the content of a project in a GitHub Actions workflow. It executes your Content Builder project, so everything in the earlier parts of this course applies unchanged.

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
| -------- | --------- |
| `build-content` | Runs a Content Builder project to build content. |
| `install-wine` | Installs Wine, for running Windows tools on Linux or macOS. |
| `install-fonts` | Installs fonts that your assets need. |
| `install-android-dependencies` | Installs what Android builds need. |
| `publish-itchio` | Publishes a build to itch.io. |

### Build-Content inputs and outputs

| Input | Required | Default | Description |
| ------- | ---------- | --------- | ------------- |
| `content-builder-path` | No | `./CBPlatformTest/Content` | Path to the Content Builder project. When `content-builder-repo` is set, this is the folder inside that repository. |
| `content-builder-repo` | No | empty | A repository that holds the builder, as `owner/repo`. |
| `assets-path` | No | `./Assets` | Path to the assets. When `assets-repo` is set, this is the folder inside that repository. |
| `assets-repo` | No | empty | A repository that holds the assets, as `owner/repo`. |
| `monogame-platform` | Yes | none | The platform to build for, such as `DesktopGL`, `Android` or `iOS`. |
| `output-folder` | Yes | none | The folder to build the content into, relative to the repository root. The builder writes a `Content` folder inside it. |
| `additional-args` | No | empty | Extra arguments for the builder, separated by spaces. |
| `upload-output` | No | `false` | Upload the built content as a workflow artifact. |
| `configuration` | No | `Release` | The build configuration for the Content Builder project. |
| `github-token` | No | empty | A token for cloning private repositories. |

The action returns these outputs:

| Output | Description |
| -------- | ------------- |
| `output-folder` | The full path of the folder that holds the built content. |
| `log-file` | The full path of the build log. |
| `success` | `true` when the content built without failures. |

The path inputs mean different things for local and remote sources. For a local builder, set only `content-builder-path`, from the repository root. For a remote builder, set `content-builder-repo` to the repository and `content-builder-path` to the folder inside it, or to an empty string for the root. The same pattern applies to the assets.

The action does not install .NET. Add a `setup-dotnet` step before it.

> [!IMPORTANT]
> The `output-folder` is the output folder of your game project, not its `Content` folder. For example, use `./MyGame/bin/Release/net10.0` and not `./MyGame/bin/Release/net10.0/Content`. The builder adds the `Content` folder itself, so a path that ends in `Content` produces `Content/Content` and the game cannot find its assets.

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
      - uses: actions/checkout@v7

      - name: Setup .NET
        uses: actions/setup-dotnet@v6
        with:
          dotnet-version: '10.0.x'

      - name: Process content
        uses: MonoGame/monogame-actions/build-content@v1
        with:
          content-builder-path: './Content/Builder'
          assets-path: './Content/Assets'
          monogame-platform: 'DesktopGL'
          output-folder: './MyGame/bin/Release/net10.0'
          configuration: Release

      - name: Build game
        run: dotnet build -c Release MyGame/MyGame.csproj -p:WorkflowMode=true
```

> [!IMPORTANT]
> The `output-folder` must be where the game project expects to find its `Content` folder.

The `-p:WorkflowMode=true` argument tells the game build that the content is already built. It only has an effect when your project uses the `WorkflowMode` property, which is described in [project changes for automated builds](#project-changes-for-automated-builds).

> [!TIP]
> For the full list of options and more examples, see the [Build-Content documentation](https://github.com/MonoGame/monogame-actions/blob/main/build-content/README.md).

### Sample repositories

MonoGame provides sample repositories that show the different arrangements:

- [MonoGame-CBPlatform-Test](https://github.com/MonoGame/MonoGame-CBPlatform-Test) holds a complete platformer with the game, the builder and the assets together, for .NET 10. It has three workflows:
  - [build.yml](https://github.com/MonoGame/MonoGame-CBPlatform-Test/blob/main/.github/workflows/build.yml) builds the content and the game in one job for each platform, in a matrix of DesktopGL (Windows, Linux and macOS), DesktopVK, WindowsDX12, Android and iOS.
  - [build-content-separate.yml](https://github.com/MonoGame/MonoGame-CBPlatform-Test/blob/main/.github/workflows/build-content-separate.yml) builds the content for every platform first, then passes it to the game build jobs as an artifact.
  - [build-remote.yml](https://github.com/MonoGame/MonoGame-CBPlatform-Test/blob/main/.github/workflows/build-remote.yml) builds with a builder and assets taken from other repositories. The repositories are workflow inputs, so you can leave them empty to use the ones in the same repository.

  The repository also has a `test-build.ps1` script that runs the same steps on your own machine for one platform.
- [MonoGame-CBPlatform-BuilderTest](https://github.com/MonoGame/MonoGame-CBPlatform-BuilderTest) holds only a Content Builder project. It is useful when you want a builder that artists can use, and its workflow builds assets from a remote repository.
- [MonoGame-CBPlatform-TestAssets](https://github.com/MonoGame/MonoGame-CBPlatform-TestAssets) holds only assets. Its workflow builds them with a builder from another repository. The assets do not have to be at the root, so one repository can hold several sets of assets for different targets.

> [!NOTE]
> The iOS project in the sample builds but does not publish. Publishing needs signing details and a team in an Apple developer account. See [Packaging Games](../../packaging_games.md) for how to set up iOS publishing.

### A multi-platform workflow

A matrix runs the same steps for each platform. Each entry sets the platform, the runner, the project, the runtime identifier, the .NET workload and the target framework.

This is an extract from the sample, and `build.yml` has the full list:

```yaml
strategy:
  fail-fast: false
  matrix:
    include:
      - platform: DesktopGL
        os: windows-latest
        project: CBPlatformTest.DesktopGL/CBPlatformTest.DesktopGL.csproj
        runtime: win-x64
        workload: ''
        tfm: net10.0
      - platform: DesktopGL
        os: ubuntu-latest
        project: CBPlatformTest.DesktopGL/CBPlatformTest.DesktopGL.csproj
        runtime: linux-x64
        workload: ''
        tfm: net10.0
      - platform: Android
        os: windows-latest
        project: CBPlatformTest.Android/CBPlatformTest.Android.csproj
        runtime: android-arm64
        workload: android
        tfm: net10.0-android
      - platform: iOS
        os: macos-26
        project: CBPlatformTest.iOS/CBPlatformTest.iOS.csproj
        runtime: ios-arm64
        workload: ios
        tfm: net10.0-ios
```

The first steps of each job pin the .NET SDK with a `global.json` file, install it, and install the workload when the platform needs one:

```yaml
env:
  DotnetVersion: 10.0.401
  Configuration: ${{ inputs.configuration || 'Release' }}

steps:
  - uses: actions/checkout@v7

  - name: Generate global.json
    shell: bash
    run: |
      echo '{ "sdk": { "version": "${{ env.DotnetVersion }}" } }' > global.json

  - name: Setup .NET SDK
    uses: actions/setup-dotnet@v6
    with:
      dotnet-version: ${{ env.DotnetVersion }}
      global-json-file: ./global.json

  - name: Install workload
    if: ${{ matrix.workload != '' }}
    run: dotnet workload install ${{ matrix.workload }}
```

Each platform writes its output to a different folder, so the content output path is calculated for each matrix entry. It must be the folder that the game build will use as its output folder, so it has to match the configuration and runtime identifier of the build step. iOS builds do not use a runtime identifier folder:

```yaml
- name: Set output paths
  id: paths
  shell: bash
  run: |
    PROJECT_DIR=$(dirname "${{ matrix.project }}")
    if [ "${{ matrix.platform }}" == "iOS" ]; then
      OUTPUT_DIR="bin/${{ env.Configuration }}/${{ matrix.tfm }}"
    else
      OUTPUT_DIR="bin/${{ env.Configuration }}/${{ matrix.tfm }}/${{ matrix.runtime }}"
    fi
    echo "project_dir=$PROJECT_DIR" >> $GITHUB_OUTPUT
    echo "output_dir=$OUTPUT_DIR" >> $GITHUB_OUTPUT
    echo "content_output=./CBPlatformTest/$PROJECT_DIR/$OUTPUT_DIR" >> $GITHUB_OUTPUT
```

The later steps build the content, restore and build the game, upload the build, and publish. Every game build must pass `-p:WorkflowMode=true`, which is explained in the next section:

```yaml
- name: Build content
  uses: MonoGame/monogame-actions/build-content@v1
  with:
    content-builder-path: ./CBPlatformTest/Content
    assets-path: ./CBPlatformTest/Content/Assets
    monogame-platform: ${{ matrix.platform }}
    output-folder: ${{ steps.paths.outputs.content_output }}
    configuration: ${{ env.Configuration }}

- name: Restore
  working-directory: ./CBPlatformTest
  run: dotnet restore ${{ matrix.project }} -r ${{ matrix.runtime }}

- name: Build (iOS)
  if: ${{ matrix.platform == 'iOS' }}
  working-directory: ./CBPlatformTest
  run: dotnet build -c ${{ env.Configuration }} ${{ matrix.project }} -p:WorkflowMode=true

- name: Build
  if: ${{ matrix.platform != 'iOS' }}
  working-directory: ./CBPlatformTest
  run: dotnet build -c ${{ env.Configuration }} ${{ matrix.project }} -r ${{ matrix.runtime }} -p:WorkflowMode=true

- name: Upload build artifact
  uses: actions/upload-artifact@v7
  with:
    name: ${{ matrix.platform }}-${{ matrix.runtime }}-build
    path: CBPlatformTest/${{ steps.paths.outputs.project_dir }}/${{ steps.paths.outputs.output_dir }}/

- name: Publish
  if: ${{ matrix.platform != 'iOS' }}
  working-directory: ./CBPlatformTest
  run: dotnet publish ${{ matrix.project }} -c ${{ env.Configuration }} -r ${{ matrix.runtime }} -p:WorkflowMode=true --self-contained
```

### Project changes for automated builds

The platform projects and `BuildContent.targets` in the sample are written to work both from an IDE and from a workflow. The `WorkflowMode` property is used to distinguish the mode of operation. The property is not part of MonoGame. It is a property that the sample defines. Each game project sets it to `false`, and the workflows turn it on with `-p:WorkflowMode=true` on the `dotnet build` and `dotnet publish` commands:

```xml
<PropertyGroup>
  <MonoGamePlatform>Android</MonoGamePlatform>
  <!-- Set to true by the workflows after the Build-Content action has already built the content. -->
  <WorkflowMode>false</WorkflowMode>
</PropertyGroup>

<ItemGroup>
  <UpToDateCheckInput Include="..\Content\Assets\**\*" />
</ItemGroup>

<Import Project="..\Content\BuildContent.targets" />
```

> [!NOTE]
> The `UpToDateCheckInput` item makes Visual Studio treat any change to an asset as a change that needs a content rebuild.

In `BuildContent.targets`, the `BuildContent` target has a condition so that it is skipped when the workflow has already built the content. The target also restores the Content Builder project first, so that a game project builds on a fresh clone:

```xml
<Target Name="BuildContent" BeforeTargets="BeforeCompile" Condition="'$(WorkflowMode)' != 'true'">
  <!-- Restore first, so a platform project builds on a fresh clone. Restore needs its own evaluation, hence the unique session property. -->
  <MSBuild Projects="$(MSBuildThisFileDirectory)Content.csproj" Targets="Restore" Properties="MSBuildRestoreSessionId=$([System.Guid]::NewGuid())" RemoveProperties="Configuration;TargetFramework;RuntimeIdentifier;RuntimeIdentifiers" />
  <MSBuild Projects="$(MSBuildThisFileDirectory)Content.csproj" Targets="Build" RemoveProperties="Configuration;TargetFramework;RuntimeIdentifier;RuntimeIdentifiers">
    <Output TaskParameter="TargetOutputs" ItemName="_ContentBuilderAssembly" />
  </MSBuild>
  <!-- ... the properties and the Exec task that run the builder ... -->
</Target>
```

The `AddGeneratedContentAssets` target has no condition, so it runs in a workflow build too. It reads the `Content` folder from the output folder of the game project, which is where the Build-Content action wrote it, and adds those files to the Android assets, the iOS bundle resources or the publish folder, depending on the platform. This is why the Android and iOS projects do not need any content items of their own, and why the `output-folder` in the workflow must match the output folder of the game build.

> [!NOTE]
> See the complete [BuildContent.targets](https://github.com/MonoGame/MonoGame-CBPlatform-Test/blob/main/CBPlatformTest/Content/BuildContent.targets) and the [Android](https://github.com/MonoGame/MonoGame-CBPlatform-Test/blob/main/CBPlatformTest/CBPlatformTest.Android/CBPlatformTest.Android.csproj) and [iOS](https://github.com/MonoGame/MonoGame-CBPlatform-Test/blob/main/CBPlatformTest/CBPlatformTest.iOS/CBPlatformTest.iOS.csproj) project files in the sample repository.

### Platform requirements

> [!IMPORTANT]
> Use the same .NET SDK version in your workflow and on your development machine. Add a `global.json` file to pin the version, and set the same version in `actions/setup-dotnet`. MonoGame `3.8.6` projects use .NET 9 or later. (although .NET 10 is recommended)

```json
{
  "sdk": {
    "version": "10.0.401"
  }
}
```

The sample generates this file in the workflow from the `DotnetVersion` setting, so the version is set in one place.

Choose the runner for each platform by what that platform needs to build. Where a platform can build on any operating system, use the cheapest runner, which is Linux:

| Platform | Runner | Notes |
| ---------- | -------- | ------- |
| Android | Any runner. Use `ubuntu-latest`. | The cheapest option, because Android builds do not depend on the operating system. Needs `dotnet workload install android`. Add keystore signing for release builds. |
| iOS | A macOS runner such as `macos-26` | iOS builds only run on macOS. Needs `dotnet workload install ios`. Use `ios-arm64` for devices. The sample skips publishing in CI. |
| macOS | A macOS runner | Build the macOS runtimes, such as `osx-arm64`, on macOS. |
| Windows (WindowsDX12) | `windows-latest` | Windows builds only run on Windows. Use a runtime such as `win-x64`. |
| Linux | `ubuntu-latest` | Use a runtime such as `linux-x64`. |
| DesktopGL and DesktopVK | The runner for the operating system of the runtime | For example `win-x64` on `windows-latest`, `linux-x64` on `ubuntu-latest` and `osx-arm64` on a macOS runner, as the DesktopGL entries in the sample do. |

The Build-Content step itself does not depend on the target operating system. When you build the content in a separate job, use the cheapest runner that suits your builder and assets.

### Logs and artifacts

The action uploads the build logs as an artifact named `content-build-logs-<platform>-<run_id>-<run_attempt>-<job_index>`, even when the build fails. The logs are `restore.log`, `build.log` and `content-pipeline.log`. The artifact is kept for 30 days. Set `upload-output` to `true` to upload the built content too, in an artifact named `content-output-<platform>-<run_id>-<run_attempt>-<job_index>`.

The artifact holds the `Content` folder and its files. When you download it into the output folder of a game build, the files end up in `<output folder>/Content`, which is where the game build expects them.

### Workflow patterns

**Build content separately.** Build the content once for each platform and upload it. The game build jobs then download the content into their output folder before they build. This is the `build-content-separate.yml` workflow in the sample, shortened here:

```yaml
jobs:
  build-content:
    runs-on: windows-latest
    strategy:
      matrix:
        platform: [DesktopGL, DesktopVK, WindowsDX12, Android, iOS]
    steps:
      - uses: actions/checkout@v7
      - uses: actions/setup-dotnet@v6
        with:
          dotnet-version: '10.0.x'
      - name: Build content
        uses: MonoGame/monogame-actions/build-content@v1
        with:
          content-builder-path: ./CBPlatformTest/Content
          assets-path: ./CBPlatformTest/Content/Assets
          monogame-platform: ${{ matrix.platform }}
          output-folder: ./Output/${{ matrix.platform }}
          upload-output: 'true'

  build-game:
    needs: build-content
    runs-on: ${{ matrix.os }}
    # strategy and matrix as in the multi-platform workflow
    steps:
      # ... checkout, SDK, workload and "Set output paths" steps ...
      - name: Download content
        uses: actions/download-artifact@v8
        with:
          pattern: content-output-${{ matrix.platform }}-${{ github.run_id }}-*
          merge-multiple: true
          path: ${{ steps.paths.outputs.content_output }}

      # ... restore, build with -p:WorkflowMode=true, upload and publish steps ...
```

The artifact name ends with the run attempt and job number, so the download uses a `pattern` with a wildcard instead of a fixed `name`.

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
| --------- | --------------- |
| The builder or assets are not found | Paths are relative to the repository root. Use a `./` prefix and forward slashes. Check the `content-builder-path` and `assets-path` rules for remote repositories. |
| The game cannot find its content, and the output has `Content/Content` | The `output-folder` ends in `Content`. Use the output folder of the game project, and the builder adds the `Content` folder. |
| The game builds but has no content | The `output-folder` does not match the output folder of the game build. Check that the configuration, target framework and runtime identifier are the same in both. |
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
