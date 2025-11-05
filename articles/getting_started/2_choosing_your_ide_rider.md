---
title: Setting up your development environment for Rider
description: A step-by-step guide for setting up your development IDE for Rider
---

This is a guide on setting up your development environment for creating games with MonoGame using Rider by JetBrains. By following this guide, you will learn how to install the tools for developing C# applications and configure Rider with recommended extensions for C# development.

By the end, you will be fully equipped to start creating games with MonoGame using Rider.

## Install Rider

You can download and install Rider from: [https://www.jetbrains.com/rider/download](https://www.jetbrains.com/rider/download)

![Rider download](./images/2_chosing_your_ide_rider_download.png)

> [!NOTE]
> Rider is free for personal use, you do have to register and create an account, but otherwise all features will be unlocked.
>

## Setting up Rider for development with MonoGame

1. Open up terminal (or Powershell on Windows)
1. Run the following command to install templates:

    ```sh
    dotnet new install MonoGame.Templates.CSharp
    ```

> [!TIP]
> Alternatively, consider using the Preview Packages provided by MonoGame to get access to the latest developments.
>
> * [How to install MonoGame Preview packages](../getting_to_know/howto/HowTo_Install_Preview_Release.md)

## Creating a new MonoGame project

To get you started with Rider, here are the steps for setting up a new Rider MonoGame project.

1. Open up Rider
2. Click on the "New Solution" button

    ![Rider new project](./images/1_setting_up_your_development_environment/rider_new_solution_button.png)

3. Select "MonoGame Cross-Platform Desktop Application" on the list on the left

    ![Rider MonoGame template](./images/1_setting_up_your_development_environment/rider_new_solution_dialog.png)

4. Press "Create"
5. You can now press `F5` to compile and debug your game, happy coding  :)

> [!NOTE]
> If you are experiencing issues with compiling effects under Linux for JetBrains Rider,
> you might need to add the `MGFXC_WINE_PATH` environment variable to `/etc/environment` for it to be picked up.  See [5777151094034-Rider-cannot-see-all-Environmental-Variables](https://rider-support.jetbrains.com/hc/en-us/community/posts/5777151094034-Rider-cannot-see-all-Environmental-Variables) for details.

## Update Project Tool references

The MonoGame Content Editor (MGCB) it a tool delivered through NuGet for your project using the tools configuration held in your `dotnet-tools.json` file (located in the `.config` folder of your project).

Once you have created your project you should run the following terminal/command-line command to ensure the tool (and the pipeline) is setup and read for your project:

```dotnetcli
    dotnet tool restore
```

> [!NOTE]
> If you ever change the version of the tools or want to upgrade them by editing the `dotnet-tools.json` configuration, you MUST run this command again to update the tools.
