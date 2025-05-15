---
title: MGCB Editor
description: Learn about the MonoGame Content Builder (MGCB) Editor, the front-end GUI editor for MonoGame content builder projects.
---

![MCGB Editor](images/mgcb_editor.png)

The MGCB Editor has the following features:

* Create, open, and save MGCB projects.
* Import existing XNA .contentproj.
* Tree view showing content of project.
* Property grid for editing content settings.
* Full undo/redo support.
* Build, rebuild, and clean the project.
* Rebuild selected items.
* Create new content like fonts and xml.
* Support for custom importers/processors/writers.
* Template format for adding new custom content types.

## Installation Instructions

The MGCB Editor is automatically installed (if you are using MonoGame's templates) and accessible by double-clicking an .mgcb file from Visual Studio 2022 if you have the extension installed, or right-clicking and selecting "Open in MGCB editor" in VSCode if you have the `MonoGame Content Builder (editor)` extension installed.

Alternatively, you can open the MGCB Editor from the .NET command line. This will only work if you are using the MonoGame templates and executing the command from the root directory of your project:

```sh
dotnet mgcb-editor
```

> [!NOTE]
> You will need to buid the project at least once in order for the .NET system to download and register the tool with your project utilizing the `dotnet-tools.json` configuration file located in the `.config` folder, or use the `dotnet tool restore` command shown below.
>
> ```sh
> dotnet tool restore
> ```

See [Using MGCB Editor](../content_pipeline/using_mgcb_editor.md) for more information.
