---
title: "Appendix 01: MonoGame Project Templates"
description: This appendix contains information on the C# project templates offered by MonoGame and what each is used for.
---

MonoGame offers several templates to create a new project based on the platform(s) you are targeting and ancillary project templates for extensions and libraries.

In VSCode and Visual Studio 2022, when you create a new .NET project, you can see the MonoGame templates available to create the project with. You can also see a list of the MonoGame templates by opening a _Command Prompt_ or _Terminal_ window and executing the command

```sh
dotnet new list MonoGame
```

The following table lists the different templates available and what they are used for

| Template Name                                 | Short Name    | Description                                                                                                                  |
| --------------------------------------------- | ------------- | ---------------------------------------------------------------------------------------------------------------------------- |
| _MonoGame Android Application_                | `mgandroid`   | A MonoGame game project template targeting the Android platform using OpenGL                                                 |
| _MonoGame Content Pipeline Extension_         | `mgpipeline`  | A MonoGame project template to create a content pipeline extension library.                                                  |
| _MonoGame Cross-Platform Desktop Application_ | `mgdesktopgl` | A MonoGame project template targeting Windows, macOS, and Linux using OpenGL.                                                |
| _MonoGame Game Library_                       | `mglib`       | A MonoGame project template to create a game library that can be referenced by other projects.                               |
| _MonoGame iOS Application_                    | `mgios`       | A MonoGame project template targeting iPhone and iPad.                                                                       |
| _MonoGame Shared Library Project_             | `mgshared`    | A MonoGame project template that creates a shared C# project that can be used to share code and content with other projects. |
| _MonoGame Windows Desktop Application_        | `mgwindowsdx` | A MonoGame project template targeting Windows only using DirectX.                                                            |

> The _Short Name_ of a templates is the name you would use when creating a new MonoGame project using the dotnet CLI command. For example, if creating a new _MonoGame Cross-Platform Desktop Application_ using the dotnet CLI, you would execute the following command using the `mgdesktopgl` short name:
>
> ```sh
> dotnet new mgdesktopgl
> ```
