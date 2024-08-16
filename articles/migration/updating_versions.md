---
title: Updating Versions
description: A guide on updating MonoGame when a new release is pushed.
---

When a new release of MonoGame is pushed, developers will need to manually update their development environment and/or existing project to make use of the new version.  This guide is intended to walk you through the steps of performing these updates.

## Update dotnet Templates
Developers will need to update the MonoGame C# Templates used to create new projects.  Doing this depends on if you are using Visual Studio 2022 or through the dotnet cli.

> [!NOTE]
> Updating the templates will not affect existing projects.  if developers wish to update existing project, see the info in the [Update Existing Projects](#update-existing-projects) section.

### Visual Studio 2022
Developers using Visual Studio 2022 should be using the [MonoGame C# Project Templates](https://marketplace.visualstudio.com/items?itemName=MonoGame.MonoGame-Templates-VSExtension) extension.  This provides not only the tempaltes but also the functinality to open the *MonoGame Content Builder Editor* (MGCB Editor) within Visual Studio.  You can update the extension by performing the following

1. Open Visual Studio 2022
2. In the lanuch window, choose the *Continue without code* option at the bottom on the right.
3. From the top menu choose, *Extensions > Manage Extensions*  to open the *Extension Manager* panel.
4. Click the *Updates* tab and choose *MonoGame Framework C# project templates* and update it to the latest version.

If prompted to close Visual Studio to finish the update, do so now to continue installing the update.

### dotnet CLI (VSCode/Rider)
Develoeprs using the dotnet CLI with environments such as Visual Studio Code, JetBrains Rider, or other editors, can exceute the following command in a command prompt/terminal to update the templates

```sh
dotnet new install MonoGame.Templates.CSharp
```

> [!TIP]
> If you receive a an error or warning stating there are naming conflicts in the templates, you may need to uninstall the templates first with the following command and then install them:
> 
> ```sh
> dotnet new uninstall MonoGame.Templates.CSharp
> ```

Alternatively, you can perform `dotnet new update` which will update all templates installed to their most current version avaialble.  However, this may affect other templates you have installed that you may not wish to update, it's an all or nothing command.

## Update Existing Projects
Developers wishing to update their current project to use a new release version only need to update the *target framework*, *NuGet package verions*, and *dotnet tools* version. Doing this dpeends on if you are using Visual Studio 2022 or another development environment

### Visual Studio 2022
Open your existing project in Visual Studio 2022 and perform the following
1. Right-click on the project in the *Solution Explorer* panel and select *Manage NuGet Packages...".
2. In the NuGet Packages Manager window, click the *Updates* tab.
3. Select the MonoGame Framework packages in the list on the left, then click the *Update* button for each one on the right.

### Manually Editing .csproj File
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
