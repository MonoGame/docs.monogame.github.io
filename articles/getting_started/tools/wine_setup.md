---
title: Setting up Wine for effect compilation
description: Learn how to set up Wine on macOS and Linux so that MonoGame can compile effects (shaders).
---

Effect (shader) compilation requires access to DirectX. This means it will not work natively on macOS and Linux systems, but it can be used through [Wine](https://www.winehq.org/).

MonoGame provides a setup script that sets up the Wine environment for effect compilation. The [MonoGame Effects Compiler](mgfxc.md) (MGFXC) then uses that environment to compile your effects.

> [!NOTE]
> You do not need Wine on Windows. On macOS and Linux, you only need it if your game uses its own effects.

## Set up Wine

Follow the steps for your operating system.

### [macOS](#tab/macos)

1. Install [Homebrew](https://brew.sh)
2. Open a terminal window and enter the following commands:

    ```sh
    brew install wget p7zip curl
    brew install --cask xquartz
    curl -L -o wine.tar.xz "https://github.com/Gcenx/macOS_Wine_builds/releases/download/11.0_1/wine-stable-11.0_1-osx64.tar.xz"
    tar -xJf wine.tar.xz
    sudo mv "Wine Stable.app" /Applications/
    sudo xattr -dr com.apple.quarantine "/Applications/Wine Stable.app"
    echo 'export PATH="/Applications/Wine Stable.app/Contents/Resources/wine/bin:$PATH"' >> ~/.zprofile
    source ~/.zprofile
    wine --version
    ```

3. Now that the prerequisites are installed, download the [mgfxc_wine_setup.sh](https://monogame.net/downloads/net9_mgfxc_wine_setup.sh) script and execute it by entering the following command in the terminal:

    ```sh
    wget -qO- https://monogame.net/downloads/net9_mgfxc_wine_setup.sh | bash
    ```

This will create a new directory called `.winemonogame` in your home directory. If you ever wish to undo the setup this script performed, delete that directory.

> [!NOTE]  
> For the time being, MonoGame requires that you install the **Intel** version even if you are using an Apple Silicon (M1/M2) Mac. For Apple Silicon Macs, it also requires that [Rosetta](https://support.apple.com/en-us/HT211861) be enabled.

### [Ubuntu](#tab/ubuntu)

1. Download the latest Wine for your version of Ubuntu from the [WineHQ Debian and Ubuntu page](https://gitlab.winehq.org/wine/wine/-/wikis/Debian-Ubuntu).
2. Open a terminal window and enter the following command:

    ```sh
    sudo apt install wget curl p7zip-full
    ```

    > [!TIP]
    > If you receive an error stating that any of the packages do not have an install candidate, you may need to enable the universe apt repository. To do this, enter the following commands in the terminal:
    >
    > ```sh
    > sudo add-apt-repository universe
    > sudo apt update
    > ```
    >
    > Then try installing the packages again.

3. Now that the prerequisites are installed, download the [mgfxc_wine_setup.sh](https://monogame.net/downloads/net9_mgfxc_wine_setup.sh) script and execute it by entering the following command in the terminal:

    ```sh
    wget -qO- https://monogame.net/downloads/net9_mgfxc_wine_setup.sh | bash
    ```

This will create a new directory called `.winemonogame` in your home directory. If you ever wish to undo the setup this script performed, delete that directory.

### [Arch Linux](#tab/arch)

1. Open a terminal window.
2. Install Wine and the required dependencies:

    ```sh
    sudo pacman -S wget curl 7zip wine
    ```

    > [!IMPORTANT]
    > Arch Linux recently transitioned the Wine package to a pure wow64 build to align with upstream Wine development. Because of this, when you install Wine using pacman, you will not get a separate `wine64` executable that the MonoGame tools expect.
    >
    > Reference: <https://archlinux.org/news/transition-to-the-new-wow64-wine-and-wine-staging/>
    >
    > This means you will need to create a symlink for MonoGame to reference with the following command:
    >
    > ```sh
    > sudo ln -s /usr/bin/wine /usr/local/bin/wine64
    > ```

3. Now that the prerequisites are installed, download the [mgfxc_wine_setup.sh](https://monogame.net/downloads/net9_mgfxc_wine_setup.sh) script and execute it by entering the following command in the terminal:

    ```sh
    wget -qO- https://monogame.net/downloads/net9_mgfxc_wine_setup.sh | bash
    ```

    > [!TIP]
    > When running the Wine setup script, you may see a popup asking to install **Wine Mono** for .NET support. You can close this popup. It is not needed because the MonoGame script installs the actual Windows .NET SDK instead.

    > [!NOTE]
    > This script will create a new directory called `.winemonogame` in your home directory. If you ever wish to undo the setup this script performed, delete that directory.

4. After the script completes, you must add the `MGFXC_WINE_PATH` environment variable to your **system environment** for it to be accessible to all applications:

    ```sh
    echo 'MGFXC_WINE_PATH="'$HOME'/.winemonogame"' | sudo tee -a /etc/environment
    ```

    > [!IMPORTANT]
    > Unlike other Linux distributions, such as Ubuntu, Arch Linux may not automatically load environment variables from your user profile at `~/.profile` for all applications. Adding `MGFXC_WINE_PATH` to `/etc/environment` ensures it is available system-wide, including for GUI applications (such as the MGCB Editor) and IDEs.

5. For the environment variable to take effect, you can either log out and back in (recommended) or reboot your system.

6. After logging back in, verify the environment variable is set:

    ```sh
    echo $MGFXC_WINE_PATH
    ```

    > [!NOTE]
    > When running the above command you should see an output similar to the following:
    >
    > ```sh
    > /home/yourusername/.winemonogame
    > ```

---

## Troubleshooting

### Issue: "wine64 not found" error on Arch Linux

If the setup script fails with `wine64 not found`, ensure you created the symlink from step 2 of the Arch Linux instructions:

```sh
sudo ln -s /usr/bin/wine /usr/local/bin/wine64
```

### Issue: Shader compilation fails with "MGFXC effect compiler requires a valid Wine installation"

This means the `MGFXC_WINE_PATH` environment variable is not set. On Arch Linux, ensure you:

1. Added it to `/etc/environment` as shown in step 4 of the Arch Linux instructions.
2. Logged out and logged back in.
3. Can see it with `echo $MGFXC_WINE_PATH`.

### Issue: Wine creates a `.wine-mgfxc` directory instead of using `.winemonogame`

This is another symptom of the environment variable not being set. Follow the steps in the previous issue to resolve it.

### Issue: Wine version too old

The MonoGame setup script requires Wine 8.0 or later. Check your version:

```sh
wine --version
```

If it is too old on Arch Linux, update your system:

```sh
sudo pacman -Syu
```

### Issue: Rider cannot compile effects on Linux

You might need to add the `MGFXC_WINE_PATH` environment variable to `/etc/environment` for Rider to pick it up. See [Rider cannot see all Environmental Variables](https://rider-support.jetbrains.com/hc/en-us/community/posts/5777151094034-Rider-cannot-see-all-Environmental-Variables) for details.
