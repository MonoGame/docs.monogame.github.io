---
title: Updating Versions
description: A guide on updating MonoGame when a new release is pushed.
---


- [Update Existing Projects](#update-existing-projects)
   - [Update Target Framework](#update-target-framework)
   - [Update NuGet References](#update-nuget-references)
      - [Visual Studio 2022](#visual-studio-2022)
      - [Manually Editing .csproj File (VSCode/Rider)](#manually-editing-csproj-file-vscoderider)
   - [Update MGCB Editor](#update-mgcb-editor)
- [Updating Environment for New Projects](#updating-environment-for-new-projects)
   - [Update dotnet Templates](#update-dotnet-templates)
      - [Visual Studio 2022](#visual-studio-2022-1)
      - [dotnet CLI (VSCode/Rider)](#dotnet-cli-vscoderider)

When a new release of MonoGame is pushed, developers will need to manually update their development environment and/or existing project to make use of the new version.  This guide is intended to walk you through the steps of performing these updates.

## Update Existing Projects
Developers wishing to update their current project to use a new release version only need to update the *target framework*, *NuGet package versions*, and *dotnet tools* version. Doing this depends on if you are using Visual Studio 2022 or another development environment.

### Update Target Framework
Developers will need to ensure the project is updated to target the .NET version used by MonoGame at minimum (currently `net8.0`). To do this, open your project's *.csproj* file and find the `<TargetFramework>` element and change the `net-X.Y` version to `net-8.0`, then save the file.

> [!NOTE]
> Only change the .NET version number. some project types have platform specifiers such as `net6.0-windows`. The only thing that should change here is the version number.

### Update NuGet References
The following sections cover updating your NuGet packages for existing projects based on your development environment.

#### Visual Studio 2022
Open your existing project in Visual Studio 2022 and perform the following
1. Right-click on the project in the *Solution Explorer* panel and select *Manage NuGet Packages...".
2. In the NuGet Packages Manager window, click the *Updates* tab.
3. Select the MonoGame Framework packages in the list on the left, then click the *Update* button for each one on the right.

#### Manually Editing .csproj File (VSCode/Rider)
Alternatively, regardless of the development environment, developers can manually edit the *.csproj* file for their project to update to the newest MonoGame release.  To do this
1. Open your project's *.csproj* file
2. Locate the `<PacakgeReference>` elements for any MonoGame referenced packages.
3. Change the `Version=` attribute for each one to the current version of MonoGame (currently this is `3.8.2.1105`)
4. Save the *.csproj* file.

Once these changes are made, open a command prompt or terminal at the root of the project directory and enter the following commands

```sh
dotnet clean
dotnet restore
```

### Update MGCB Editor
Regardless of the development environment, users will need to update the dotnet tools manifest file manually to use the newest version of the *MonoGame Content Builder*. To do this:
1. Open the *.config/dotnet-tools.json* manifest file located in the project root directory
2. Update the version specified for each tool to the current version of MonoGame (currently this is `3.8.2.1105`).
3. Save the changes

Once these changes are made, open a command prompt or terminal at the root of the project directory and enter the following commands

```sh
dotnet tool restore
```

## Updating Environment for New Projects
The following sections will cover updating your development environment for new projects.

### Update dotnet Templates
Developers will need to update the MonoGame C# Templates used to create new projects.  Doing this depends on if you are using Visual Studio 2022 or through the dotnet cli.

> [!NOTE]
> Updating the templates will not affect existing projects.  if developers wish to update existing project, see the info in the [Update Existing Projects](#update-existing-projects) section.

#### Visual Studio 2022
Developers using Visual Studio 2022 should be using the [MonoGame C# Project Templates](https://marketplace.visualstudio.com/items?itemName=MonoGame.MonoGame-Templates-VSExtension) extension.  This provides not only the templates but also the functionality to open the *MonoGame Content Builder Editor* (MGCB Editor) within Visual Studio.  You can update the extension by performing the following

1. Open Visual Studio 2022
2. In the launch window, choose the *Continue without code* option at the bottom on the right.
3. From the top menu choose, *Extensions > Manage Extensions*  to open the *Extension Manager* panel.
4. Click the *Updates* tab and choose *MonoGame Framework C# project templates* and update it to the latest version.

If prompted to close Visual Studio to finish the update, do so now to continue installing the update.

#### dotnet CLI (VSCode/Rider)
Developers using the dotnet CLI with environments such as Visual Studio Code, JetBrains Rider, or other editors, can execute the following command in a command prompt/terminal to update the templates

```sh
dotnet new install MonoGame.Templates.CSharp
```

> [!TIP]
> If you receive a an error or warning stating there are naming conflicts in the templates, you may need to uninstall the templates first with the following command and then install them:
> 
> ```sh
> dotnet new uninstall MonoGame.Templates.CSharp
> ```

Alternatively, you can perform `dotnet new update` which will update all templates installed to their most current version available.  However, this may affect other templates you have installed that you may not wish to update, it's an all or nothing command.
