---
title: Setting up your OS for development on Ubuntu
description: This section provides a step-by-step guide for setting up your development environment on Windows.
---

To develop with MonoGame in C#, you will need to install the .NET SDK. As of MonoGame 3.8.2 the minimum supported version is .NET 8.

## Install .NET 8 SDK

1. Open a new **Terminal** window.
2. Enter the following command in the terminal to download the **dotnet-install.sh**

    ```sh
    sudo apt-get update && sudo apt-get install -y dotnet-sdk-8.0
    ```

This will ensure the latest .NET 8 SDK is installed and setup in your environment.

## Setup Wine For Effect Compilation

Effect (shader) compilation requires access to DirectX.  This means it will not work natively on macOS and Linux systems, but it can be used through [Wine](https://www.winehq.org/).

MonoGame provides a setup script that can be executed to setup the Wine environment for Effect (shader) compilation.

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

3. Now that the prerequisites are installed, download the [mgfxc_wine_setup.sh](https://monogame.net/downloads/net6_mgfxc_wine_setup.sh) script and execute it by entering the following command in the terminal:

    ```sh
    wget -qO- https://monogame.net/downloads/net6_mgfxc_wine_setup.sh | bash
    ```

This will create new directory called `.winemonogame` in your home directory.  If you ever wish to undo the setup this script performed, just simply delete that directory.

## Next Steps

Choose from one of the two IDE options on Ubuntu:

- [Setting up VSCode](./2_choosing_your_ide_vscode.md)
- [Setting up Rider](./2_choosing_your_ide_rider.md)
