---
title: "Step 1: Prepare Your Operating System"
description: Install the .NET SDK and the MonoGame templates on Windows, macOS or Linux.
---

MonoGame is a *framework*, not a game engine, it does not have an editor that you install. Instead, MonoGame is a set of .NET libraries and tools that you use from your usual code editor, thus, the first step is to install `.NET` and the `MonoGame project templates`.

In this step, you will:

- Install the .NET *SDK* (Software Development Kit).
- Make sure that the .NET SDK operates correctly.
- Install the MonoGame project templates.
- Learn about the optional tools for mobile and shader development.

## Install the .NET SDK

The .NET SDK contains the compiler and the command line tools that build your game.

> [!IMPORTANT]
> MonoGame 3.8.6 recommends the .NET 10 SDK or later.  .NET 9 will still work, but is being phased out by Microsoft in favour of the latest LTS (Long Term Support) release.

### [Windows](#tab/windows)

1. Open a web browser and go to [https://dotnet.microsoft.com/download/dotnet/10.0](https://dotnet.microsoft.com/download/dotnet/10.0).
2. In the *SDK* column, find the *Windows* row.
3. Click *x64* to download the installer. If your computer has an Arm processor, click *Arm64*.
4. When the download is complete, run the installer.
5. Follow the instructions in the installer.

### [macOS](#tab/macos)

1. Open a web browser and go to [https://dotnet.microsoft.com/download/dotnet/10.0](https://dotnet.microsoft.com/download/dotnet/10.0).
2. In the *SDK* column, find the *macOS* row.
3. Click the installer for your Mac:
    - For a Mac with Apple silicon (M1 or later), click *Arm64*.
    - For a Mac with an Intel processor, click *x64*.
4. When the download is complete, open the `.pkg` file.
5. Follow the instructions in the installer.

> [!IMPORTANT]
> Only install ONE architecture for .NET 10 on macOS, DO NOT install both x64 and ARM, as they are incompatible on the same machine.

### [Linux](#tab/linux)

Use the package manager for your Linux distribution.

**Ubuntu** (20.04 or later):

1. Open a terminal.
2. Run this command:

    ```sh
    sudo apt-get update && sudo apt-get install -y dotnet-sdk-10.0
    ```

    > [!NOTE]
    > If `apt-get` cannot find the `dotnet-sdk-10.0` package, add the .NET backports repository with `sudo add-apt-repository ppa:dotnet/backports`. Then, run the command again.

**Arch Linux**:

1. Open a terminal.
2. Run these commands:

    ```sh
    sudo pacman -Syu
    sudo pacman -S dotnet-sdk
    ```

For other distributions, refer to [Install .NET on Linux](https://learn.microsoft.com/dotnet/core/install/linux).

---

## Make sure that the .NET SDK is installed

1. Open a new terminal window:
    - On Windows, open *Command Prompt* or *PowerShell*.
    - On macOS, open *Terminal*.
    - On Linux, open your terminal application.
2. Run this command:

    ```sh
    dotnet --list-sdks
    ```

3. Make sure that the list shows version `10.0` or later.

> [!NOTE]
> Open a *new* terminal window after you install the SDK. A terminal that was open before the installation cannot find the `dotnet` command.  If in doubt, you can reboot your machine, but in most cases, a new command prompt/terminal is sufficient.

## Install the MonoGame templates

The MonoGame .NET templates create new MonoGame projects for you. This guide uses the new single project *MonoGame Empty Game* template for creating new solutions with your choice of platforms.

> [!TIP]
> Previously, MonoGame had single projects for single platforms and you had to manually combine them by hand, or use one of the "StartKits" and strip out the platforms you did not want.
>
> The new single project template resolves this, allowing you to select which platforms you want from the start, complete with the new [Content Builder](../../../getting_started/content_pipeline/content_builder/index.md) solution.

1. In the terminal, run this command:

    ```sh
    dotnet new install MonoGame.Templates.CSharp
    ```

2. To make sure that the templates are installed, run this command:

    ```sh
    dotnet new list monogame
    ```

3. Make sure that the list shows *MonoGame Empty Game*.

> [!TIP]
> If you installed the MonoGame templates before, the same command updates them to the latest version.

## Optional tools

You do not need the tools in this section for this guide. Install them only if you need them later.

### Workloads for mobile platforms

To make games for Android or iOS, install the .NET workloads for these platforms:

```sh
dotnet workload install android
dotnet workload install ios
```

> [!NOTE]
> To build a game for iOS, you must have a Mac with Xcode installed.

### WINE for shader compilation on macOS and Linux

The shader compiler uses DirectX tools that operate only on Windows. To compile shaders (effects) on macOS or Linux, you must install [WINE](https://www.winehq.org/). This guide does not use shaders.

For the WINE instructions, refer to [Setting up Wine for effect compilation](../../../getting_started/tools/wine_setup.md). That page has steps for macOS, Ubuntu and Arch Linux.

## Conclusion

In this step, you accomplished the following:

- You installed the .NET SDK.
- You made sure that the `dotnet` command operates.
- You installed the MonoGame project templates.

In the next step, you will install an IDE to write and run your code.

## Test Your Knowledge

1. Which version of the .NET SDK does MonoGame 3.8.6 need?

    :::question-answer
    MonoGame 3.8.6 recommends the .NET 10 SDK or later.
    :::

2. Which command installs the MonoGame project templates?

    :::question-answer
    `dotnet new install MonoGame.Templates.CSharp`
    :::

3. Why must you open a new terminal window after you install the .NET SDK?

    :::question-answer
    A terminal that was open before the installation will not find the `dotnet` command, as the terminal only gets informed of paths when it opens. A new terminal window loads the updated settings.
    :::
