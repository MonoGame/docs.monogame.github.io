---
title: Setting up your OS for development on Arch Linux
description: This section provides a step-by-step guide for setting up your development environment on Arch Linux.
---

> [!TIP]
> Arch Linux is a rolling release distribution. This guide was tested in October 2025 with kernel `version 6.15.9-arch1-1`.

To develop with MonoGame in C#, you will need to install the .NET 9 SDK. As of MonoGame 3.8.4.1 the recommended version is .NET 9, .NET 10 is also supported.

> [!TIP]
> You can still continue to use .NET 8 if you wish, you will just need to downgrade your client project .NET version in the `csproj` setup for your project (if using the default templates)

## Install .NET 9 SDK

1. Open a new **Terminal** window.
2. Enter the following command in the terminal to install the latest .NET 9 SDK:

    ```sh
    sudo pacman -Syu
    sudo pacman -S dotnet-sdk-9.0
    ```

## Install additional workloads

If you intend to also work with platforms such as `Android` or `iOS`, you will need to install the additional .NET workload templates for those platforms which include additional features and simulators to support those platforms.  Just run the following commands from the terminal for the platforms below you wish to install.

> [!IMPORTANT]
> For mobile development with iOS and Android, you must also install the MAUI workload even though MonoGame does not use MAUI. The MAUI workload contains the debugging tools required to run and debug mobile .NET applications. Without it, you will not be able to properly debug your MonoGame mobile projects.

### [Android](#tab/android)

```cli
dotnet workload install android
```

### [iOS](#tab/iOS)

```cli
dotnet workload install ios
```

### [Maui](#tab/maui)

```cli
dotnet workload install maui
```

### [Android, iOS, and Maui](#tab/all)

```cli
dotnet workload install android ios maui
```

---

> [!NOTE]
> You can use `dotnet workload search` to detect any other available workloads you wish to use.

## Setup Wine For Effect Compilation

Effect (shader) compilation requires access to DirectX.  This means it will not work natively on macOS and Linux systems, but it can be used through [Wine](https://www.winehq.org/).

MonoGame provides a setup script that can be executed to setup the Wine environment for Effect (shader) compilation.

1. Open a terminal window
2. Install Wine and required dependencies

    ```sh
    sudo pacman -S wget curl 7zip wine
    ```

    > [!IMPORTANT]
    > Arch Linux recently transitioned the Wine package to a pure wow64 build to align with upstream Wine development.  Because of this, when you install Wine using pacman, you will not get a separate `wine64` executable that the MonoGame tools will expect.
    >
    > Reference: <https://archlinux.org/news/transition-to-the-new-wow64-wine-and-wine-staging/>
    >
    > This means you will need to create a symlink for MonoGame to reference with the following command
    >
    > ```sh
    > sudo ln -s /usr/bin/wine /usr/local/bin/wine64
    > ```

3. Now that the prerequisites are installed, download the [mgfxc_wine_setup.sh](https://monogame.net/downloads/net9_mgfxc_wine_setup.sh) script and execute it by entering the following command in the terminal:

    ```sh
    wget -qO- https://monogame.net/downloads/net9_mgfxc_wine_setup.sh | bash
    ```

    > [!TIP]
    > When running the Wine setup script, you may see a popup asking to install **Wine Mono** for .NET support.  You can cancel/close this popup.  It's not needed because the MonoGame script installs the actual Windows .NET SDK instead.

    > [!NOTE]
    > This script will create a new directory called `.winemonogame` in your home directory.  If you ever wish to undo the setup this script performed, just simply delete that directory.

4. After the script completes, you must add the `MGFXC_WINE_PATH` environment variable to your **system environment** for it to be accessible to all applications:

    ```sh
    echo 'MGFXC_WINE_PATH="'$HOME'/.winemonogame"' | sudo tee -a /etc/environment
    ```

    > [!IMPORTANT]
    > Unlike other Linux distributions, such as Ubuntu, Arch Linux may not automatically load environment variables from your user profile at `~/.profile` for all applications.  Adding `MGFXC_WINE_PATH` to `/etc/environment` ensures it is available system-wide, including for GUI applications (such as the MGCB Editor) and IDEs

5. For the environment variable to take effect, you can either log out and back in (recommended) or reboot your system.

6. After logging back in, verify the environment variable is set:

    ```sh
    echo $MGFXC_WINE_PATH
    ```

    > [!NOTE]
    > When running the above command you should see an output similar to the following
    >
    > ```sh
    > /home/yourusername/.winemonogame
    > ```

## Next Steps

Choose from one of the two IDE options on Arch Linux:

- [Setting up VSCode](./2_choosing_your_ide_vscode.md)
- [Setting up Rider](./2_choosing_your_ide_rider.md)

## Troubleshooting

### Issue: "wine64 not found error"

If the setup script fails with `wine64 not found`, ensure you created the symlink from step 2 above

```sh
sudo ln -s /usr/bin/wine /usr/local/bin/wine64
```

### Issue: Shader compilation fails with "MGFXC effect compiler requires a valid wine installation"

This means the `MGFXC_WINE_PATH` environment variable is not set.  Ensure you:

1. Added it to `/etc/environment` as shown above in step 4
2. Logged out and logged back in
3. Can see it with `echo $MGFXC_WINE_PATH`

### Issue: Wine creates a `.wine-mgfxc` directory instead of using `.winemonogame`

This is another symptom of the environment variable not being set.  Follow the steps in the previous issue to resolve it.

### Issue: Wine version too old

The MonoGame setup script requires Wine 8.0 or later.  Check your version:

```sh
wine --version
```

If it's too old, update your system:

```sh
sudo pacman -Syu
```
