---
title: How to install a preview release of MonoGame from NuGet
description: This topic describes how to use a preview release of MonoGame in a new or existing project.
---

## Overview

MonoGame provides preview deployments for testing releases prior to the full release of a new version. This gives developers early access to new features before they are made officially available.

> [!WARNING]
> While preview releases still undergo the same rigorous testing used for full releases, some features may not work as expected as it is still classed as developmental code.
>
> *Preview releases should not be used for production deployments.**
>
> The preview packages are intended for testing and validating fixes/updates and new features in your projects without drastically affecting them.

## Updating NuGet package dependencies

To use the preview packages delivered via NuGet you need to update or replace your existing MonoGame references for your project:

- The Platform NuGet package reference - e.g. `MonoGame.Framework.DesktopGL`
- The Builder Task package - `MonoGame.Content.Builder.Task` (Only required if using the Content Pipeline)

The simplest way is from the command line in the folder where your `csproj` is located (if you have multiple projects, please repeat for each) using the following syntax:

```dotnetcli
dotnet add package MonoGame.Framework.DesktopGL -v 3.8.4-preview.1
dotnet add package MonoGame.Content.Builder.Task -v 3.8.4-preview.1
```

**Replacing the preview version with the specific version you wish to install.**

> [!NOTE]
> For Visual Studio, use `Tools -> NuGet Package Manager -> Manage NuGet Packages for Solution/Project`, then click on the `Updates` tab and update each package.
>
> Make sure to select **"Include Prerelease"** for the preview packages to be detected.

## Update `dotnet-tools.json` for Content Manager

To ensure symmetry between the project version and the Content Pipeline Editor (MGCB) the `dotnet-tools.json` configuration in your project `.config` directory will also need updating.

This is simply a matter of updating the tools references, again from the command line in the projects folder, as follows:

```dotnetcli
dotnet tool install dotnet-mgcb --version 3.8.4-preview.1
dotnet tool install dotnet-mgcb-editor --version 3.8.4-preview.1
dotnet tool install dotnet-mgcb-editor-linux --version 3.8.4-preview.1
dotnet tool install dotnet-mgcb-editor-windows --version 3.8.4-preview.1
dotnet tool install dotnet-mgcb-editor-mac --version 3.8.4-preview.1
```

**Replacing the preview version with the specific version you wish to install.**

> [!NOTE]
> Alternatively, simply edit the `dotnet-tools.json` file and replace the **"version"** value across the file.


> [!IMPORTANT]
> Make sure to run `dotnet tool restore` any time you make changes to the `dotnet-tools.json` to refresh the installation of the Content Builder and Editor.

## Installing the preview templates

This step is optional, if you intend to generate a new project using the latest templates, including any automated or templates setup

1. Simply run the following from the command line:

```dotnetcli
dotnet new install MonoGame.Templates.CSharp::3.8.4-preview.1
```

**Replacing the preview version of MonoGame at the end of the command with the specific version you wish to install.**

## Visual Studio Extensions with Preview releases

At this time, the Visual Studio marketplace does not support preview versions of extensions, if you wish to test a preview version of the Visual Studio Extension, you can install it manually.

The Extension can be downloaded from the latest `GitHub Actions` build used to generate the preview build.

1. Go to the [MonoGame GitHub repository](https://github.com/MonoGame/MonoGame/actions) and select the "Actions" tab.
1. Find the build with the associated tag, e.g. [`v3.8.4-preview.1`](https://github.com/MonoGame/MonoGame/actions/runs/14713318149)
1. In the `Artifacts` for the build, you should see the extensions installer `MonoGame.Templates.VSExtension.vsix`
1. Click on the "Download Icon" on the far right-hand side of the row.
1. Once downloaded, run the `vsix` which will update your installation of the extensions package.

## See Also

- [Getting Started with MonoGame](../../getting_started/index.md)
- [Using Development NuGet Packages](../../getting_started/using_development_nuget_packages.md)
