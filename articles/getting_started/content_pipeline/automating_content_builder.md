---
title: Automating Content Building with GitHub Actions
description: Learn how to automate MonoGame content building using the Build-Content GitHub Action in your CI/CD workflows.
---

> [!NOTE]
> These instructions **REQUIRE** you to use the latest `MonoGame Develop` release (currently `3.8.5-develop.13`)
>
> See the [instructions here](https://docs.monogame.net/articles/getting_to_know/howto/HowTo_Install_Preview_Release.html) for how to install the preview project templates and update your project to use `3.8.5-develop` (preview) releases.
>
> [How to install MonoGame Preview packages](https://docs.monogame.net/articles/getting_to_know/howto/HowTo_Install_Preview_Release.html)

## Overview

The **Build-Content** composite GitHub Action provides a powerful way to automate the building of MonoGame content as part of your continuous integration and deployment (CI/CD) workflows. This action streamlines the content pipeline process, enabling you to build game assets for multiple platforms automatically whenever you push changes to your repository.  Alternatively, it can be useful for offloading the building of content to "agents" without having to alter your environment (such as having a Windows automated host to build shaders without Wine for Linux/Mac).

## Why Use GitHub Actions for Content Building?

Automating your content building process offers several key advantages:

- **Consistency**: Ensures content is built the same way every time, eliminating "works on my machine" issues.
- **Multi-platform Support**: Build content for iOS, Android, DesktopGL, Windows, and other platforms in parallel.
- **Version Control Integration**: Automatically rebuild content when assets change.
- **Artifact Management**: Store and distribute pre-built content as GitHub artifacts.
- **Team Collaboration**: Enable team members to trigger builds without needing local MonoGame setup.
- **Repository Separation**: You can optionally maintain content builders, assets and even the Game project in separate repositories for better organization.
- **Time Savings**: Reduce manual build steps and eliminate repetitive tasks.

## Automation Architecture

There are several paths open when using automation to manage content building, each with their own benefits:

- A standard game project, with the builder and assets all in the same repository - easy to maintain and self-contained.  The project simply needs to be built and the content will be generated automatically.
- A Game project, with the builder and assets all in the same repository, however the content is built first before the game project.  Preventing accidental failures and identifying asset issues in advance.  Although requires the build/runtime project to have content injected manually.
- Separate repositories for the Game and Builder/Assets.  Allows content to be worked on independently and repeatedly built without impact to the main game.  The Asset output and game output are then combined with an automated process and shipped.
- Separate repositories for all elements, Game, Builder, and assets.  Allows for greater separation of concerns, in some instances, the builder generation pushes the compiled EXE to a package for consumption by the asset build so that the builder can be worked on independently while assets continue to build with the previous generation.

There is no one single path, only that which fits the development cycle of the project and the timelines needed to support different pathways.

## MonoGame Custom Actions

MonoGame hosts several GitHub composite actions for use in automated workflows, including:

- Automated Wine installation - "install-wine"
- Font installation automation - "install-font"
- Install Android dependencies - "install-android-dependencies"
- Itchio publishing helper - "publish-itchio"
- Content Builder integration for running Content Builds - "build-content"

For the new Content Builder solution, the "**Build-Content**" action provides a comprehensive set of functionality to aid in MonoGame content automation, specifically:

- Build MonoGame content for any platform (iOS, Android, DesktopGL, Windows, MacOSX, Linux, etc.)
- Support for local or external content builder repositories. (Have the Content Builder bundled with the Game project or in a separate repository)
- Support for local or external asset repositories. (Have the source Game Assets bundled with the Game project or in a separate repository)
- Comprehensive logging with automatic artifact uploads.
- Optional content output uploads to the Source host for distribution.
- Subfolder targeting for complex repository structures.

## Basic Usage Example

Here's a simple example of using the Build-Content action in a workflow where all components (game, content builder, and assets) are in the same repository:

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
        uses: ./.github/actions/build-content
        with:
          content-builder-path: './Content/Builder'
          assets-path: './Content/Assets'
          monogame-platform: 'DesktopGL'
          output-folder: './MyGame/bin/Release/Content'
          configuration: Release
      
      - name: Build game
        run: dotnet build -c Release MyGame/MyGame.csproj -r win-x64
```

For detailed examples including external repositories, multi-platform builds, and advanced configurations, see the [Build-Content Action Documentation](https://github.com/MonoGame/monogame-actions/blob/main/build-content/README.md).

## Example setup demonstrations

MonoGame has provided several public demonstration repositories to showcase how the "Build-Content" action can be used to automate content production:

### Complete game sample in a single repository:

- [MonoGame-CBPlatform-Test](https://github.com/MonoGame/MonoGame-CBPlatform-Test)

Contains a multi-platformer project (2D Startkit) updated and outfitted with two automated paths:

  - [.github/workflows/build.yml](https://github.com/MonoGame/MonoGame-CBPlatform-Test/blob/main/.github/workflows/build.yml)&nbsp;
    Demonstrates a matrix solution that builds a project for DesktopGL, Android and iOS.  Once built, it is available as an Artifact from the build output.
  - [.github/workflows/build-remote](https://github.com/MonoGame/MonoGame-CBPlatform-Test/blob/main/.github/workflows/build-remote.yml)&nbsp;
    Demonstrates a single project build solution that uses an external "Builder" from a separate repository and "Assets" from another repository.

> [!NOTE]
> At this time, the iOS project only builds and does not "Publish", the reason for this is that you need signing details or an associated "Team" in an iOS development environment to do so.
>
> See the [Packaging Games](../packaging_games.md) guide for more details on how to setup iOS publishing.

### Dedicated builder repository - no assets:

- [MonoGame-CBPlatform-BuilderTest](https://github.com/MonoGame/MonoGame-CBPlatform-BuilderTest)

A dedicated project for the sole purpose of maintaining the Content Builder project separate from the main Game and Assets.  Useful in scenarios where you want to generate a dedicated builder deliverable for use by artists.  Also includes a workflow to automatically build Assets from a remote repository that can be downloaded from the last run.

### Dedicated Assets only repository:

- [MonoGame-CBPlatform-TestAssets](https://github.com/MonoGame/MonoGame-CBPlatform-TestAssets)

A dedicated repository containing the source assets for a game project, not affected by changes in the main game project.  This can be used remotely by the projects automation, or consumed by a Builder project.  Also contains a workflow that utilizes the above Builder project to demonstrate that the "Build-Content" automation can be run from either the builder or from the assets themselves.

Note, that while the assets are in the root of this repository, they do not have to be, and you can even maintain variations of the assets in different folders for different targets and use the automation to select the correct source folder at build time.

## Complete MonoGame Build Automation

The Build-Content action can be integrated into a complete automated build pipeline for your MonoGame projects. Here's how to set up end-to-end automation for building your game across multiple platforms.

### Multi-Platform Build Strategy

A complete workflow should handle:

1. **Environment Setup**: Configure .NET SDK and platform-specific workloads
2. **Content Building**: Process game assets for each target platform
3. **Project Building**: Compile the game for each platform
4. **Artifact Creation**: Package and upload build outputs
5. **Publishing**: Create distribution-ready packages

### Key Components of a Complete Workflow

#### 1. Platform Matrix Configuration

Use GitHub Actions matrix strategy to build for multiple platforms in parallel:

```yaml
strategy:
  matrix:
    include:
      - platform: iOS
        os: macos-latest
        runtime: ios-arm64
        workload: ios
        tfm: net9.0-ios
      - platform: Android
        os: windows-latest
        runtime: android-arm64
        workload: android
        tfm: net9.0-android
      - platform: DesktopGL
        os: windows-latest
        runtime: win-x64
        workload: ''
        tfm: net9.0
```

#### 2. Dynamic Path Resolution

Different platforms require different output paths. Use dynamic path calculation:

```yaml
- name: Set paths
  id: paths
  shell: bash
  run: |
    PROJECT_DIR="${{ matrix.project }}"
    PROJECT_DIR="${PROJECT_DIR%/*}"
    
    if [[ "${{ matrix.platform }}" == "iOS" ]]; then
      RID_PATH="bin/${{ env.Configuration }}/${{ matrix.tfm }}/iossimulator-x64"
    else
      RID_PATH="bin/${{ env.Configuration }}/${{ matrix.tfm }}/${{ matrix.runtime }}"
    fi
    
    echo "content_output=./MyGame/$PROJECT_DIR/$RID_PATH/Content" >> $GITHUB_OUTPUT
```

#### 3. Platform-Specific Workload Installation

Some platforms require additional .NET workloads:

```yaml
- name: Install workload
  if: ${{ matrix.workload != '' }}
  run: dotnet workload install ${{ matrix.workload }}
```

#### 4. Content Processing Integration

Integrate the Build-Content action to process assets:

```yaml
- name: Process content
  uses: MonoGame/monogame-actions/build-content@v1
  with:
    content-builder-path: "./Content/Builder"
    assets-path: "./Content/Assets"
    monogame-platform: ${{ matrix.platform }}
    output-folder: ${{ steps.paths.outputs.content_output }}
    configuration: ${{ env.Configuration }}
```

#### 5. Build and Publish

Execute the build and publish steps:

```yaml
- name: Restore
  run: dotnet restore ${{ matrix.project }} -r ${{ matrix.runtime }}

- name: Build
  run: dotnet build -c ${{ env.Configuration }} ${{ matrix.project }} -r ${{ matrix.runtime }}

- name: Publish
  if: ${{ matrix.platform != 'iOS' }}
  run: dotnet publish ${{ matrix.project }} -c ${{ env.Configuration }} -r ${{ matrix.runtime }} --self-contained
```

## Important Considerations

### .NET SDK Versions

> [!IMPORTANT]
> Ensure consistency between your local development environment and GitHub Actions:
>
> - Use a `global.json` file to pin the .NET SDK version
> - Specify the exact version in `actions/setup-dotnet@v5`
> - MonoGame 3.8.5+ requires .NET 9.0 or later

Example `global.json`:

```json
{
  "sdk": {
    "version": "9.0.307"
  }
}
```

### Platform-Specific Requirements

#### iOS Builds

- **Runner**: Must use `macos-26` or specific macOS version
- **Workload**: Requires `dotnet workload install ios`
- **Runtime**: Use `ios-arm64` for devices, `iossimulator-x64` for simulator
- **Special Handling**: iOS builds have different output paths and require additional parameters
- **Publishing**: iOS builds typically skip the publish step in CI

#### Android Builds

- **Runner**: Works on `windows-latest` or `ubuntu-latest`
- **Workload**: Requires `dotnet workload install android`
- **Runtime**: Common runtimes include `android-arm64`, `android-x64`
- **Signing**: Consider adding keystore signing for release builds

#### Desktop Platforms

- **Windows**: Use `windows-latest` runner, `win-x64` or `win-x86` runtime
- **Linux**: Use `ubuntu-latest` runner, `linux-x64` runtime
- **DesktopGL**: Cross-platform, can build on any runner

### CSProj updates

If you are using automation alongside normal project building, then the project templates need to be updated to work both locally (run through Visual Studio/VSCode) and through automation with a switch.  This is critical in terms of bundled platforms such as iOS and Android, which need their built content available in the right place even before the build begins.

> [!NOTE]
> See the [Build workflow](https://github.com/MonoGame/MonoGame-CBPlatform-Test/blob/main/.github/workflows/build.yml) in the main sample repository which demonstrates the complete flow from Content Building, copying, project building and packaging.

An example update for android is as follows:

```xml
  <ItemGroup>
    <!-- For CI/CD with RuntimeIdentifier -->
    <AndroidAsset Include="$(ProjectDir)bin\$(Configuration)\$(TargetFramework)\$(RuntimeIdentifier)\Content\**\*" Condition="'$(RuntimeIdentifier)' != '' AND Exists('$(ProjectDir)bin\$(Configuration)\$(TargetFramework)\$(RuntimeIdentifier)\Content')">
      <Link>Content\%(RecursiveDir)%(Filename)%(Extension)</Link>
    </AndroidAsset>
    <!-- For local builds without RuntimeIdentifier -->
    <AndroidAsset Include="$(ProjectDir)$(OutputPath)Content\**\*" Condition="'$(RuntimeIdentifier)' == '' AND Exists('$(ProjectDir)$(OutputPath)Content')">
      <Link>Content\%(RecursiveDir)%(Filename)%(Extension)</Link>
    </AndroidAsset>
  </ItemGroup>
```

The extension based on the default path for Android and iOS breaks down two fold:

- One AndroidAsset/BundleResource entry for content in the location the final build will output, populated by the CI/CD pipeline.
- One entry set using the local build output when executed by the normal Game project build.

Additionally, the Local project build is prevented from running with the following amendment:

```xml
  <Target Name="BuildContent" BeforeTargets="Build" Condition="'$(WorkflowMode)' != 'true'">
```

Which states that if the project build is run with a `-p:WorkflowMode=true` argument, the default Content build is skipped as this has already been pre-processed by the automation.

> [!NOTE]
> The Full example of this flow can be observed in the [MonoGame-CBPlatform-Test](https://github.com/MonoGame/MonoGame-CBPlatform-Test) project sample, notably the [build.yml](https://github.com/MonoGame/MonoGame-CBPlatform-Test/blob/main/.github/workflows/build.yml), [Android csproj](https://github.com/MonoGame/MonoGame-CBPlatform-Test/blob/main/CBPlatformTest/CBPlatformTest.Android/CBPlatformTest.Android.csproj) and [iOS csproj](https://github.com/MonoGame/MonoGame-CBPlatform-Test/blob/main/CBPlatformTest/CBPlatformTest.iOS/CBPlatformTest.iOS.csproj) files.

### Configuration Management

Use workflow inputs or environment variables for flexible configuration:

```yaml
on:
  workflow_dispatch:
    inputs:
      configuration:
        description: 'Build configuration'
        required: false
        default: 'Release'
        type: choice
        options:
          - Debug
          - Release

env:
  Configuration: ${{ github.event.inputs.configuration || 'Release' }}
```

### Artifact Management

Upload build outputs for distribution or debugging:

```yaml
- name: Upload build artifact
  uses: actions/upload-artifact@v5
  with:
    name: ${{ matrix.platform }}-build
    path: MyGame/${{ steps.paths.outputs.project_dir }}/bin/${{ env.Configuration }}/

- name: Upload publish artifact
  uses: actions/upload-artifact@v5
  with:
    name: ${{ matrix.platform }}-publish
    path: MyGame/${{ steps.paths.outputs.project_dir }}/bin/${{ env.Configuration }}/${{ matrix.tfm }}/${{ matrix.runtime }}/publish/
```

### Content Build Logs

The Build-Content action automatically uploads comprehensive logs:

- **Name**: `content-build-logs-<run_id>`
- **Contents**: restore.log, build.log, content-pipeline.log
- **Retention**: 30 days
- **Always Available**: Uploaded even on failure for debugging

### Release Patterns

Consider different triggers for your workflows:

```yaml
on:
  # Manual trigger
  workflow_dispatch:
  
  # On push to main branch
  push:
    branches:
      - main
  
  # On pull request
  pull_request:
    branches:
      - main
  
  # When content changes
  push:
    paths:
      - 'Content/Assets/**'
      - '**/*.csproj'
```

## Common Workflow Patterns

### Pattern 1: Separate Content Building

Build content once and reuse across multiple platform builds:

```yaml
jobs:
  build-content:
    runs-on: windows-latest
    strategy:
      matrix:
        platform: [DesktopGL, iOS, Android]
    steps:
      - name: Process content
        uses: ./.github/actions/build-content
        with:
          monogame-platform: ${{ matrix.platform }}
          upload-output: 'true'
  
  build-game:
    needs: build-content
    runs-on: ${{ matrix.os }}
    steps:
      - name: Download content
        uses: actions/download-artifact@v4
        with:
          name: content-output-${{ matrix.platform }}-${{ github.run_id }}
```

### Pattern 2: Integrated Build

Build content as part of each platform build:

```yaml
jobs:
  build:
    runs-on: ${{ matrix.os }}
    steps:
      - name: Process content
        uses: ./.github/actions/build-content
      
      - name: Build game
        run: dotnet build
```

## Troubleshooting

### Path Issues

- **Problem**: Content builder or assets not found
- **Solution**: Ensure paths are relative to repository root and use correct forward slashes
- **Tip**: Use `./` prefix for clarity (e.g., `./Content` not just `Content`)

### Platform-Specific Failures

- **Problem**: Build fails on specific platform
- **Solution**: Check workload installation and runtime compatibility
- **Tip**: Review platform-specific build logs in artifacts

### SDK Version Mismatches

- **Problem**: Build works locally but fails in Actions
- **Solution**: Add `global.json` to lock SDK version across environments
- **Tip**: Use `dotnet --list-sdks` locally to verify your version

### Content Pipeline Errors

- **Problem**: Assets fail to process
- **Solution**: Download `content-build-logs` artifact and review `content-pipeline.log`
- **Tip**: Test content building locally with the same configuration

## Additional Resources

### Official Documentation

- [Build-Content Action Repository](https://github.com/MonoGame/monogame-actions/tree/main/build-content)
- [MonoGame Content Pipeline Documentation](https://docs.monogame.net/articles/getting_started/content_pipeline/index.html)
- [GitHub Actions Documentation](https://docs.github.com/en/actions)

### .NET Publishing and Packaging

#### Cross-Platform Publishing

- [.NET Application Publishing Overview](https://learn.microsoft.com/en-us/dotnet/core/deploying/)
- [Self-Contained Deployment](https://learn.microsoft.com/en-us/dotnet/core/deploying/deploy-with-cli)
- [Runtime Identifier (RID) Catalog](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog)

#### Windows Packaging

- [Create MSIX Packages in GitHub Actions](https://learn.microsoft.com/en-us/windows/apps/package-and-deploy/ci-for-winui3)
- [Windows Application Packaging](https://learn.microsoft.com/en-us/windows/msix/desktop/desktop-to-uwp-packaging-dot-net)
- [Code Signing for Windows](https://learn.microsoft.com/en-us/windows/msix/package/sign-app-package-using-signtool)

#### Android Packaging

- [Publishing .NET Android Apps](https://learn.microsoft.com/en-us/dotnet/maui/android/deployment/)
- [Android App Signing](https://learn.microsoft.com/en-us/dotnet/maui/android/deployment/overview#sign-the-app)
- [Publishing to Google Play](https://learn.microsoft.com/en-us/dotnet/maui/android/deployment/publish-google-play)
- [GitHub Actions for Android Publishing](https://github.com/marketplace/actions/upload-android-release-to-play-store)

#### iOS Packaging

- [Publishing .NET iOS Apps](https://learn.microsoft.com/en-us/dotnet/maui/ios/deployment/)
- [iOS Code Signing](https://learn.microsoft.com/en-us/dotnet/maui/ios/deployment/overview#provision-the-app)
- [TestFlight Distribution](https://developer.apple.com/testflight/)
- [App Store Submission](https://learn.microsoft.com/en-us/dotnet/maui/ios/deployment/publish-app-store)
- [iOS CI/CD with GitHub Actions](https://docs.github.com/en/actions/deployment/deploying-xcode-applications)

#### macOS Packaging

- [Publishing .NET macOS Apps](https://learn.microsoft.com/en-us/dotnet/maui/mac-catalyst/deployment/)
- [macOS App Notarization](https://developer.apple.com/documentation/security/notarizing_macos_software_before_distribution)

#### Linux Packaging

- [.NET Linux Distribution](https://learn.microsoft.com/en-us/dotnet/core/deploying/deploy-with-cli#framework-dependent-deployment)
- [Creating AppImage for Linux](https://docs.appimage.org/packaging-guide/index.html)
- [Snap Packages](https://snapcraft.io/docs/dotnet-apps)
- [Flatpak Applications](https://docs.flatpak.org/en/latest/)

### GitHub Actions Resources

- [GitHub Actions Marketplace](https://github.com/marketplace?type=actions)
- [Matrix Strategy Documentation](https://docs.github.com/en/actions/using-jobs/using-a-matrix-for-your-jobs)
- [Workflow Syntax Reference](https://docs.github.com/en/actions/using-workflows/workflow-syntax-for-github-actions)
- [Artifact Upload/Download Actions](https://github.com/actions/upload-artifact)

### MonoGame Community Resources

- [MonoGame Packaging guide](../packaging_games.md)
- [MonoGame Discord Community](https://discord.gg/monogame)
- [MonoGame Forums](https://community.monogame.net/)
- [MonoGame GitHub Repository](https://github.com/MonoGame/MonoGame)

## Summary

Automating MonoGame content building with GitHub Actions provides a robust foundation for continuous integration and deployment. By leveraging the Build-Content composite action, you can:

- Streamline content processing across multiple platforms
- Ensure consistent builds in your team environment
- Reduce manual intervention in the build process
- Create reproducible, distributable game packages

Start with a simple single-platform workflow and gradually expand to multi-platform builds as your project grows. The flexibility of the Build-Content action supports everything from straightforward local builds to complex multi-repository architectures.
