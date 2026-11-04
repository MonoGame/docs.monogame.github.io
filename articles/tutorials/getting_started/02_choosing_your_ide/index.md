---
title: "Step 2: Choose Your IDE"
description: Install and configure Visual Studio, Visual Studio Code or JetBrains Rider for MonoGame development.
---

This is a guide for setting up your development environment for creating games with MonoGame using the IDE or Editor of your choice. By following this guide, you will learn how to install the necessary tools for developing C# applications.

This guide gives instructions for three IDEs, but any IDE supporting .NET SDK development will do:

| IDE | Operating systems | Cost |
| --- | --- | --- |
| Visual Studio | Windows only | The Community edition is free for individuals and small teams. |
| Visual Studio Code | Windows, macOS and Linux | Free. |
| JetBrains Rider | Windows, macOS and Linux | Free for non-commercial use. |

Select one IDE. If you do not know which IDE to use, use Visual Studio Code, because .NET runs on three platforms, Windows, macOS, and Linux.

In this step, you will:

- Install an IDE.
- Install the extensions or workloads that the IDE needs for .NET development.

## Install your IDE

### [Visual Studio](#tab/vs)

1. Open a web browser and go to [https://visualstudio.microsoft.com/](https://visualstudio.microsoft.com/).
2. Download the *Community* edition of Visual Studio.
3. When the download is complete, run the installer.
4. In the *Workloads* tab, select *.NET desktop development*.
5. Optional: To make games for Android or iOS, also select *.NET Multi-platform App UI development*.
6. Click *Install*.

> [!NOTE]
> Visual Studio shows the MonoGame templates that you installed in [Step 1](../01_setting_up_your_os/index.md#install-the-monogame-templates). You do not need to install a MonoGame extension for this guide.
>
> The `MonoGame Extension` on the Visual Studio marketplace is only needed when working with the legacy MGCB Editor.

### [Visual Studio Code](#tab/vscode)

1. Open a web browser and go to [https://code.visualstudio.com/](https://code.visualstudio.com/).
2. Download the installer for your operating system:
    - On Windows, run the installer.
    - On macOS, open the `.zip` file. Then, move *Visual Studio Code.app* to your *Applications* folder.
    - On Ubuntu, download the `.deb` package and open it to install.
3. Start Visual Studio Code.
4. Open the *Extensions* panel. Click the *Extensions* icon in the *Activity Bar*, or select `View > Extensions`.
5. In the search box, type `C# Dev Kit`.
6. Click *Install* for the *C# Dev Kit* extension from Microsoft.

> [!NOTE]
> The *C# Dev Kit* extension also installs the *C#* extension and the *.NET Install Tool* extension. These extensions give C# code completion, debugging and project tools.

> [!IMPORTANT]
> On Arch Linux, the `code` package from `pacman` is *Code - OSS*. *Code - OSS* cannot use the *C# Dev Kit* extension. Install the `visual-studio-code-bin` package from the Arch User Repository (AUR) instead.

**Optional:** To debug games on Android or iOS devices, also install the *.NET MAUI* extension from Microsoft.

### [Rider](#tab/rider)

1. Open a web browser and go to [https://www.jetbrains.com/rider/](https://www.jetbrains.com/rider/).
2. Download Rider for your operating system. You can also install Rider with the [JetBrains Toolbox App](https://www.jetbrains.com/toolbox-app/).
3. Run the installer and follow the instructions.
4. Start Rider. When Rider asks for a license, select the non-commercial option or enter your license.

The MonoGame projects in this guide use the *Content Builder* to build game content. To make sure that Rider runs the Content Builder, change one build setting:

1. Open `File > Settings` (on macOS, `Rider > Settings`).
2. Go to `Build, Execution, Deployment > Toolset and Build > Build`. (menu names may vary by Rider version)
3. Select *Invoke BeforeBuild and AfterBuild targets for skipped projects*.
4. Click *Save*.

> [!NOTE]
> If content does not build after this change, clear the *Use ReSharper Build* option in the same settings page.

---

## Conclusion

In this step, you accomplished the following:

- You installed an IDE.
- You installed the extensions or workloads that your IDE needs for .NET development.

In the next step, you will create your first MonoGame project and run it.

## Test Your Knowledge

1. What is an IDE?

    :::question-answer
    An *Integrated Development Environment* (IDE) is an application that you use to write, build and debug code.
    :::

2. Which IDE in this guide runs only on Windows?

    :::question-answer
    Visual Studio.
    :::

3. Which extension must you install in Visual Studio Code for C# development?

    :::question-answer
    The *C# Dev Kit* extension from Microsoft.
    :::
