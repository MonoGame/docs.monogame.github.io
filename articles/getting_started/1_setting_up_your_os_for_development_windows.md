---
title: Setting up your OS for development on Windows
description: This section provides a step-by-step guide for setting up your development environment on Windows.
---

To develop with MonoGame in C#, you will need to install the .NET SDK. As of MonoGame 3.8.2 the minimum supported version is .NET 8.

> [!IMPORTANT]
> If your chosen IDE for development is [Visual Studio](https://visualstudio.microsoft.com/) then you can skip this step as .NET 8 is included with the Visual Studio Installer.

## Install .NET 8 SDK

You can follow the instructions below based your operating system to install the .NET 8.0 SDK

1. Navigate to [https://dotnet.microsoft.com/en-us/download/dotnet/8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

    ![Download .NET For Windows](./images/1_setting_up_your_development_environment/dotnet_8_download_page.png)

2. Click the **.NET SDK x64** download button to download.  This will take you to the download page where the **dotnet-sdk-8.x.yyy-win-x64.exe** will download.
3. When the download completes, run the **dotnet-sdk-8.x.yyy-win-x64.exe** installer and complete the steps to install .NET on your machine.
4. When the install completes, open a new **Command Prompt** window and run the command `dotnet` to verify the installation was successful.

    ![Verify Installation](./images/1_setting_up_your_development_environment/vscode/windows/verify-install.png)

## Next Steps

Choose from one of the three IDE options on Windows:

- [Setting up Visual Studio](./2_choosing_your_ide_visual_studio.md)
- [Setting up VSCode](./2_choosing_your_ide_vscode.md)
- [Setting up Rider](./2_choosing_your_ide_rider.md)
