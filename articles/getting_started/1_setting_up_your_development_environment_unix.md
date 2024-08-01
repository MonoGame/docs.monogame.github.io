---
title: Setting up your development environment for macOS and Linux
description: A step-by-step guide for setting up your development environment on macOS and Linux.
---

# Setting up your development environment for macOS and Linux

This section provides a step-by-step guide for setting up your development environment on macOS and Linux.

The only development environment that MonoGame officially supports on Linux is [Visual Studio Code](https://code.visualstudio.com/).

## Install .NET 6 SDK

- .NET SDK at: [https://dotnet.microsoft.com/en-us/download](https://dotnet.microsoft.com/en-us/download)
- If you are intending to target mobile platforms, make sure that you have the corresponding workload installed

```sh
dotnet workload install ios
dotnet workload install android
```

## macOS considerations

For the time being, MonoGame requires that you install the x64 version of the .NET SDK even if you are running on an Apple Silicon mac. It is also required that [Rosetta](https://support.apple.com/en-us/HT211861) is enabled.

## [Optional] Install Visual Studio Code

You can install Visual Studio Code at: [https://code.visualstudio.com/Download](https://code.visualstudio.com/Download)

In order to code and build C# projects, you will also need to install a Visual Studio Code C# extension. You can do this with the following commands:

```sh
code --install-extension ms-dotnettools.csharp
```

If you want, you can also install the C# Dev Kit extensions, which are not open source:
```sh
code --install-extension ms-dotnettools.csdevkit
code --install-extension ms-dotnettools.dotnet-maui
```

## Install MonoGame templates

```sh
dotnet new --install MonoGame.Templates.CSharp
```

## [Optional] Setup Wine for effect compilation

Effect (shader) compilation requires access to DirectX, so it will not work natively on Linux systems, but it can be used through Wine. Here are instructions to get this working (providing that your distribution is using apt).

Install wine64:

```sh
sudo apt install wine64 p7zip-full curl
```

Create wine prefix:

```sh
wget -qO- https://monogame.net/downloads/net6_mgfxc_wine_setup.sh | bash
```

> [!NOTE]
> For Linux Mint 22 Wilma and potentially other Linux Distributions it could be necessary to make the `wine64` binary available first before running the script if it fails to find the wine64 binary either by adding the correct installation directory to the `$PATH` variable or by adding a symlink to a folder which is already part of the `$PATH` variable.

Option 1: Add the installation directory to the `$PATH` variable:

```sh
echo 'export PATH=$PATH:/usr/lib/wine' >> ~/.bashrc && source ~/.bashrc
wine64 --version
```

Option 2: Add a symlink to an already existing folder of the `$PATH` variable:

```sh
sudo ln -s /usr/lib/wine/wine64 /usr/bin/wine64
wine64 --version
```

If you ever need to undo the script, simply delete the `.winemonogame` folder in your home directory.

**Next up:** [Creating a new project](2_creating_a_new_project_netcore.md)
