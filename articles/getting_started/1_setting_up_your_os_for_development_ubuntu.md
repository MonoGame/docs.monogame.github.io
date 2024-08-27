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

## Install additional workloads

If you intend to also work with platforms such as `Android` or `iOS`, you will need to install the additional .NET workload templates for those platforms which include additional features and simulators to support those platforms.  Just run the following commands from the terminal for the platforms below you wish to install.

### [Android](#tab/android)

```cli
    dotnet workload install android
```

### [iOS](#tab/iOS)

```cli
    dotnet workload install ios
```

---

> [!NOTE]
> You can use `dotnet workload search` to detect any other available workloads you wish to use.

## Setup Wine For Effect Compilation

Effect (shader) compilation requires access to DirectX.  This means it will not work natively on macOS and Linux systems, but it can be used through [Wine](https://www.winehq.org/).

MonoGame provides a setup script that can be executed to setup the Wine environment for Effect (shader) compilation.

1. Open a terminal window
2. Enter the following command

    ```sh
    sudo apt install wget curl p7zip-full wine64
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

3. Now that the prerequisites are installed, download the [mgfxc_wine_setup.sh](https://monogame.net/downloads/net8_mgfxc_wine_setup.sh) script and execute it by entering the following command in the terminal:

    ```sh
    wget -qO- https://monogame.net/downloads/net8_mgfxc_wine_setup.sh | bash
    ```

This will create new directory called `.winemonogame` in your home directory.  If you ever wish to undo the setup this script performed, just simply delete that directory.

## Next Steps

Choose from one of the two IDE options on Ubuntu:

- [Setting up VSCode](./2_choosing_your_ide_vscode.md)
- [Setting up Rider](./2_choosing_your_ide_rider.md)
