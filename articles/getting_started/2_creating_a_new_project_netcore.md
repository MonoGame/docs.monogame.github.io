---
title: .NET CLI (JetBrains Rider or Visual Studio Code)
description: A step-by-step guide for creating a new project using JetBrains Rider or Visual Studio Code.
---

This guide will walk you through building a starter game with MonoGame using only the command line/terminal on your operating system and a lightweight coding tool of your choice (such as [Visual Studio Code](https://code.visualstudio.com/) or [JetBrains Rider](https://www.jetbrains.com/rider/)).

> [!NOTE]
> It is assumed that you have already properly installed the .NET 8 SDK and MonoGame.

> [!IMPORTANT]
> Be aware that for iOS/iPadOS and Android, development might be limited when using the .NET CLI. Many of the development features for those targets are exclusive to Visual Studio 2022.

## Create a MonoGame Project

You can now create new MonoGame projects. To do that:

- Create a new directory for your project.

- Open a new terminal window or command prompt and navigate to your project directory.

- Run `dotnet new <TemplateID> -o <ProjectName>` to create your project, where `<TemplateID>` is a platform identifier, and `<ProjectName>` the name of your project.

For example:

```cli
dotnet new mgdesktopgl -o MyGame
```

> To know which platform identifier (short name) to use for your project, please refer to [Target Platforms](./platforms.md), or type the following command into the command prompt to list the installed templates and their corresponding short names:
>
> ```cli
> dotnet new -l
> ```

Once created, you can open your code editor of choice in the new folder and begin editing.

> To run your project, check the instructions for [packaging your game](./packaging_games.md) to build the executable using the .NET tooling.

**Next up:** [Understanding the code](3_understanding_the_code.md)
