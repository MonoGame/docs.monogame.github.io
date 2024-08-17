---
title: Additional steps for setting up your macOS and Linux development environment
description: This section provides a step-by-step guide for setting up your development environment on macOS and Linux.
---

The only development environment that MonoGame officially supports on Mac/Linux is [Visual Studio Code](https://code.visualstudio.com/).

> [!IMPORTANT]
> This guide assumes you have already followed the steps outlined in [Setting up your development environment for VSCode](./1_setting_up_your_development_environment_vscode.md)
>
> This article only contains additional information specific to Mac/Linux hosts

## MacOS considerations

For the time being, MonoGame requires that you install the x64 version of the .NET SDK even if you are running on an Apple Silicon mac. It is also required that [Rosetta](https://support.apple.com/en-us/HT211861) is enabled.

## Setup Wine For Effect Compilation

Effect (shader) compilation requires access to DirectX.  This means it will not work natively on macOS and Linux systems, but it can be used through [Wine](https://www.winehq.org/).

### [macOS](#tab/macos)

MonoGame provides a setup script that can be executed to setup the Wine environment for Effect (shader) compilation.  However, this script has the following prerequisites that must first be setup:

- **curl** must be installed
- **p7zip** must be installed
- **wine-stable** must be installed.

These can be installed using **brew**.

1. Open a terminal window.
2. Enter the following command:

    ```sh
    brew install p7zip curl
    brew install --cask wine-stable
    ```

    > [!CAUTION]
    > It is recommended that you use `wine-stable` and not `wine-staging`.

3. Now that the prerequisites are installed, download the [mgfxc_wine_setup.sh](https://monogame.net/downloads/net6_mgfxc_wine_setup.sh) script and execute it by entering the following command in the terminal:

```sh
wget -qO- https://monogame.net/downloads/net6_mgfxc_wine_setup.sh | bash
```

This will create new directory called `.winemonogame` in your home directory.  If you ever wish to undo the setup this script performed, just simply delete that directory.

### [Linux](#tab/linux)

MonoGame provides a setup script that can be executed to setup the Wine environment for Effect (shader) compilation.  However, this script has the following prerequisites that must first be setup:

- **curl** must be installed
- **p7zip** must be installed
- **wine64** must be installed.

For Debian-based distributions like Ubuntu, you can perform the following:

1. Open a terminal window
2. Enter the following command

    ```sh
    sudo apt install curl p7zip-full wine64
    ```

    > [!TIP]
    > If you receive an error stating that either of the packages do not have an install candidate, you may need to enable the universe apt repository.  To do this, enter the following commands in the terminal
    >
    > ```sh
    > sudo add-apt-repository universe
    > sudo apt update
    > ```
    >
    > Then try installing the packages again.

    > [!CAUTION]
    > If you plan to install Wine using the `winehq-*` package instead, it is recommended that you use the `winehq-stable` package and not `-staging`.

3. Now that the prerequisites are installed, download the [mgfxc_wine_setup.sh](https://monogame.net/downloads/net6_mgfxc_wine_setup.sh) script and execute it by entering the following command in the terminal:

    ```sh
    wget -qO- https://monogame.net/downloads/net6_mgfxc_wine_setup.sh | bash
    ```

This will create new directory called `.winemonogame` in your home directory.  If you ever wish to undo the setup this script performed, just simply delete that directory.

---

## Apple Silicon Known Issues

There is currently a two known issue when building content on an Apple Silicon (M1/M2) Mac:

1. **Building Textures**: An exception occurs stating that the **freeimage** lib could not be found.
2. **Building SpriteFonts**: An exception occurs stating that the **freetype** lib could not be found.
3. **Building Models**: An exception occurs starting that the **assimp** lib could not be found.

These issue occur due to needing compiled versions of these libs for the M1/M2 architecture.  [There is currently work being done to resolve this](https://github.com/MonoGame/MonoGame/issues/8124), however in the meantime, you can use the following workaround that has been provided by community members.

1. Download and install the x64 version of [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0). This will place an x64 version of .NET 6 in a `/usr/local/share/dotnet/x64` directory.
NOTE: It MUST be the x64 version in order for this to work. This will allow the x64 native libraries that the MonoGame Content Pipeline uses to function on the Apple Silicon device.
Currently it also needs to be .NET 6 for the 3.8.1 Release of MonoGame.

2. Open your .csproj and add the following lines to the first `<PropertyGroup>` section.

    ```xml
    <DotnetCommand>/usr/local/share/dotnet/x64/dotnet</DotnetCommand>
    ```

3. (Alternative) The directory above is not in the path. But we do not want the system to be confused on which .NET is should be using. So rather thatn putting the x64 verison in the path we should instead create a symlink named `dotnet64`.

    ```sh
    sudo ln -s /usr/local/share/dotnet/x64/dotnet /usr/local/share/dotnet/dotnet64
    ```

We can then use this value as the value for `DotnetCommand`

```xml
<DotnetCommand>dotnet64</DotnetCommand>
```

> [!IMPORTANT]
> A fast follow release is planned for Mac/Linux support to improve the content management side of MonoGame.

**Next up:** [Creating a new project](2_creating_a_new_project_netcore.md)
