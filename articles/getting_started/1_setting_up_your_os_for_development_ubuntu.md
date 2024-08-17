---
title: Setting up your OS for development on Ubuntu
description: This section provides a step-by-step guide for setting up your development environment on Windows.
---

To develop with MonoGame in C#, you will need to install the .NET SDK. As of MonoGame 3.8.2 the minimum supported version is .NET 8.

## Install .NET 8 SDK

1. Open a new **Terminal** window.
2. Enter the following command in the terminal to download the **dotnet-install.sh**

    ```sh
    wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
    ```

3. Grant permission for the script to execute by entering the following command in the terminal:

    ```sh
    chmod +x ./dotnet-install.sh
    ```

4. Run the script to install the .NET 8 SDK by entering the following command in the terminal:

    ```sh
    ./dotnet-install.sh
    ```

    ![.NET Install Script](./images/1_setting_up_your_development_environment/vscode/linux/dotnet-install-script.png)

5. You will now need to setup your environment variables so that the `dotnet` command is recognized.  To do this, open the file `~/.bashrc` in a text editor and add the following lines to the end of the file.

    ```sh
    export DOTNET_ROOT=$HOME/.dotnet
    export PATH=$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools
    ```

    ![Add Environment Variables](./images/1_setting_up_your_development_environment/vscode/linux/add-environment-variables.png)

6. Save and close the file, close any open terminal windows, then open a new terminal window so the new environment variables are registered.
7. Enter the `dotnet` command to validate that the .NET 8 SDK is now installed.

    ![Verify Installation](./images/1_setting_up_your_development_environment/vscode/linux/verify-install.png)

## Setup Wine For Effect Compilation

Effect (shader) compilation requires access to DirectX.  This means it will not work natively on macOS and Linux systems, but it can be used through [Wine](https://www.winehq.org/).

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

## Next Steps

Choose from one of the two IDE options on Ubuntu:

- [Setting up VSCode](./2_choosing_your_ide_vscode.md)
- [Setting up Rider](./2_choosing_your_ide_rider.md)
