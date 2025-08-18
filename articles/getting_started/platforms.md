---
title: Supported Platforms
description: Platforms that MonoGame supports building games for.
---

MonoGame supports building games for the following **systems**:

| **Desktop PCs**             | **Mobiles**                | **Gaming consoles***                                                           |
| --------------------------- | -------------------------- | ------------------------------------------------------------------------------ |
| Windows<br/>macOS<br/>Linux | iOS<br/>iPadOS<br/>Android | Xbox<br/>PlayStation 4<br/>PlayStation 5<br/>Nintendo Switch                   |

> [!IMPORTANT]
> **Gaming consoles are restricted to registered developers and are not publicly available nor publicly documented. To get access to those platforms, please contact your console account manager(s). MonoGame documentation for closed platforms is available in their respective repositories.*

## Understanding MonoGame's platform types

There are different implementations of MonoGame that we call **target platforms** (or just **platforms**).

The platforms mostly correspond to the systems MonoGame supports but some platforms support multiple systems. For instance, the *DesktopGL* platform can be used to build games that will run either on Windows, macOS, or Linux with the same base code and project.

Each platform comes with its own project template that you can choose when starting a project.

Below is a list of public platforms with their corresponding NuGet package, the `dotnet new` template identifier, and an explanation of the platform. 

- [WindowsDX](#windowsdx)
- [DesktopGL](#desktopgl)
- [Android](#android)
- [iOS](#ios)

Beside these target platforms, MonoGame provides additional templates for shared game logic and extensions to the MonoGame Content Pipeline that can be used across all platforms.

- [Understanding MonoGame's platform types](#understanding-monogames-platform-types)
- [Platform details](#platform-details)
  - [WindowsDX](#windowsdx)
  - [DesktopGL](#desktopgl)
  - [Android](#android)
  - [iOS](#ios)
- [Other templates](#other-templates)
  - [.NET Class Library](#net-class-library)
  - [Shared Project](#shared-project)
  - [Content Pipeline Extension](#content-pipeline-extension)

## Platform details

### WindowsDX

| **Supported Systems** | **NuGet Package**            | **Template ID** |
| --------------------- | ---------------------------- | --------------- |
| Windows               | MonoGame.Framework.WindowsDX | mgwindowsdx     |

WindowsDX uses WinForms to manage the game window, **DirectX** (9.0c or newer) is used for graphics, and XAudio is used for audio.

> [!NOTE]
> DX 12 for Windows and Xbox coming very soon.

You can target **Windows*** 8.1 and up with this platform.

WindowsDX requires the [DirectX June 2010](https://www.microsoft.com/en-us/download/details.aspx?id=8109) runtime to both build and run games. Make sure that your players have it installed (otherwise you might be missing sound and gamepad rumble support).

### DesktopGL

| **Supported Systems** | **NuGet Package**            | **Template ID** |
| --------------------- | ---------------------------- | --------------- |
| Windows, macOS, Linux | MonoGame.Framework.DesktopGL | mgdesktopgl     |

DesktopGL uses SDL for windowing, **OpenGL** for graphics, and OpenAL-Soft for audio. 

DesktopGL supports **Windows** (8.1 and up), **macOS** (Catalina 10.15 and up) and **Linux** (64bit-only).

DesktopGL requires at least OpenGL 2.0 with the ARB_framebuffer_object extension (or alternatively at least OpenGL 3.0).

DesktopGL is a convenient way to publish builds for Windows, macOS, and Linux from a single project and source code. It also allows to cross-compile any build from any of these operating systems (e.g. you can build a Linux game from Windows).

You can target Windows 8.1 (and up), macOS Catalina 10.15 (and up), and Linux with this platform.

DesktopGL currently does not have a `VideoPlayer` implementation.

> [!NOTE]
> New native platform with upgraded libraries and Metal/Vulkan support coming very soon.

### Android

| **Supported Systems** | **NuGet Package**          | **Template ID** |
| --------------------- | -------------------------- | --------------- |
| Android               | MonoGame.Framework.Android | mgandroid       |

The Android platform uses [Xamarin.Android](https://docs.microsoft.com/en-us/xamarin/android/). **OpenGL** is used for graphics, and OpenAL for audio.

Building for Android requires the .NET Xamarin component to be installed. You can install it with the Visual Studio installer (if you are using Visual Studio) or with the CLI command ```dotnet workload install android``` (if you are working with Rider, VS Code, or the CLI).

Building for Android also requires the Java 11 JDK (we recommend that you use [the Microsoft's distribution](https://docs.microsoft.com/en-us/java/openjdk/download#openjdk-11)) as well as the Android SDK 31.

> [!NOTE]
> New native platform with upgraded libraries and Metal/Vulkan support coming very soon.

### iOS

| **Supported Systems** | **NuGet Package**      | **Template ID** |
| --------------------- | ---------------------- | --------------- |
| iOS, iPadOS           | MonoGame.Framework.iOS | mgios           |

The iOS platform uses [Xamarin.iOS](https://docs.microsoft.com/en-us/xamarin/ios/). **OpenGL** is used for graphics, and OpenAL for audio.

Building for iOS requires the .NET Xamarin component to be installed. You can install it with the Visual Studio installer (if you are using Visual Studio) or with the CLI command `dotnet workload install ios` (if you are working with Rider, VS Code, or the CLI).

The latest version of Xcode will also be required.

You can test and deploy an iOS game on Windows by [pairing your Visual Studio 2022 with a mac on your local network](https://docs.microsoft.com/en-us/xamarin/ios/get-started/installation/windows/connecting-to-mac/). This feature is not available for Rider, Visual Studio Code, or the CLI.

> [!NOTE]
> New native platform with upgraded libraries and Metal/Vulkan support coming very soon.

## Other templates

### .NET Class Library

**Template ID**: mglib

A project template to create [.NET](https://learn.microsoft.com/en-us/dotnet/standard/class-library-overview) libraries to distribute code through a DLL. This can be used to redistribute libraries or to share code between multiple projects (like different platforms).

> [!NOTE]
> See the [MonoGame Samples](https://github.com/MonoGame/MonoGame.Samples) for examples of how a .NET class library is used to share code between platforms.

### Shared Project

**Template ID**: mgshared

A project template to create a [shared project](https://docs.microsoft.com/en-us/xamarin/cross-platform/app-fundamentals/shared-projects) which can be used to share code between multiple other projects. The difference with .NET Standard libraries is that shared projects do not produce an intermediate DLL and the code is directly shared and built into the other projects it reference.

### Content Pipeline Extension

**Template ID**: mgpipeline

A project template for writing custom logic for handling content and building it into XNB files.

> [!NOTE]
> See [What Is the Content Pipeline?](../getting_to_know/whatis/content_pipeline/CP_Overview.md) for more infomation and read about [What is a Custom Importer](../getting_to_know/whatis/content_pipeline/CP_AddCustomProcImp.md) here.
