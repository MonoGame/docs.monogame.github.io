---
title: Setting up your development environment for VSCode
description: A step-by-step guide for setting up your development IDE using VSCode
---

This is a guide on setting up your development IDE for creating games with MonoGame using Visual Studio Code. By following this guide, you will learn how to install the necessary tools for developing C# applications and configure Visual Studio Code with recommended extensions for C# development.

By the end, you will be fully equipped to start creating games with MonoGame using Visual Studio Code.

## Install Visual Studio Code

### [Windows](#tab/windows)

1. Open your web browser and navigate to [https://code.visualstudio.com/](https://code.visualstudio.com/).

    ![Download VSCode](./images/1_setting_up_your_development_environment/vscode/windows/download-vscode.png)

2. Click the **Download for Windows** button.  This will redirect you to the download page where the installer will automatically download.
3. When the download completes, run the installer and complete the steps to install Visual Studio Code.

### [macOS](#tab/macos)

1. Open your web browser and navigate to [https://code.visualstudio.com/](https://code.visualstudio.com/).

    ![Download VSCode](./images/1_setting_up_your_development_environment/vscode/mac/download-vscode.png)

2. Click the **Download Mac Universal** button.  This will redirect you to the page where the application archive (.zip) file will begin downloading.
3. Extract the contents of the VSCode archive that downloaded by double-clicking it inside a Finder window.  This will extract the **Visual Studio Code.app** file.
4. Drag the **Visual Studio Code.app** file into the **Applications** folder, making it available in the macOS Launchpad.

### [Ubuntu](#tab/ubuntu)

1. Open your web browser and navigate to [https://code.visualstudio.com/](https://code.visualstudio.com/).
2, Click the Download .deb button.

## Setting up VS Code for development with MonoGame

1. Open Visual Studio Code
1. Open up its terminal window (`Ctrl/Cmd` + `J`)
3. Run the following command to install MonoGame Templates

    ```sh
    dotnet new install MonoGame.Templates.CSharp
    ```
4. Run the following command to install C# Dev Kit extension

    ```sh
    code --install-extension ms-dotnettools.csdevkit
    ```

## Creating an empty project

1. Open up an empty folder in Visual Studio Code
2. Open up its terminal window (`Ctrl/Cmd` + `J`)
3. Run the following command to create an empty project for desktop platforms:
    ```sh
    dotnet new mgdesktopgl
    ```
4. When VS Code asks you about automatic creation of launch / task files, press yes
5. You can now press F5 to compile and debug you game, happy coding  :)

## Next Steps

Next, get to know MonoGame by creating your first game project:

- [Creating a new project](3_creating_a_new_project_netcore.md)
