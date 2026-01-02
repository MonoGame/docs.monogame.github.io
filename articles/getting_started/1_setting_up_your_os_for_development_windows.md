---
title: Setting up your OS for development on Windows
description: This section provides a step-by-step guide for setting up your development environment on Windows.
---

To develop with MonoGame in C#, you must install the .NET SDK. As of MonoGame 3.8.4.1 the minimum supported version is .NET 9, .NET 10 is also supported.

> [!IMPORTANT]
> If your chosen IDE for development is [Visual Studio](https://visualstudio.microsoft.com/) then you can skip this step as .NET 8 is included with the Visual Studio Installer.

## Install .NET 9 SDK

You can follow the instructions below based on your operating system to install the .NET 9.0 SDK

1. Navigate to [https://dotnet.microsoft.com/en-us/download/dotnet/9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

    ![Download .NET For Windows](./images/1_setting_up_your_development_environment/dotnet_9_download_page.png)

2. Click the **.NET SDK x64** download button to download.  This will take you to the download page where the **dotnet-sdk-9.x.yyy-win-x64.exe** will download.
3. When the download completes, run the **dotnet-sdk-9.x.yyy-win-x64.exe** installer and complete the steps to install .NET on your machine.
4. When the installation completes, open a new **Command Prompt** window and run the command `dotnet` to verify the installation was successful.

    ![Verify Installation](./images/1_setting_up_your_development_environment/vscode/windows/verify-install.png)

## Install additional workloads

If you intend to also work with platforms such as `Android` or `iOS`, you will need to install the additional .NET workload templates for those platforms which include additional features and simulators to support those platforms.  Just run the command-line command for the platforms below you wish to install.

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

## Next Steps

Choose from one of the three IDE options on Windows:

- [Setting up Visual Studio](./2_choosing_your_ide_visual_studio.md)
- [Setting up VSCode](./2_choosing_your_ide_vscode.md)
- [Setting up Rider](./2_choosing_your_ide_rider.md)
