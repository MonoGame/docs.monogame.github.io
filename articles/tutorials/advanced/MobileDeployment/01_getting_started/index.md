---
title: "01: Setting Up MonoGame for Android and iOS Development"
description: "Get started with MonoGame mobile development by setting up your development environment, tools, and SDKs for Android and iOS platforms."
---

# Getting Started and Overview

This tutorial extends the [Dungeon Slime](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Tutorials/learn-monogame-2d) tutorial to mobile platforms. If you have not completed the desktop tutorial yet, we recommend finishing it first as we will be building directly on that foundation.

## What You Will Learn

By the end of this mobile tutorial series, you will have:
- Ported the **Dungeon Slime** game to run on Android and iOS
- Implemented touch controls to replace mouse and keyboard input
- Set up cross-platform project architecture for code sharing
- Learned debugging techniques for mobile development
- Published your game to both Google Play and the App Store
- Created automated deployment workflows

# Development Requirements

## Android Development

**Platform Support:** Android development can be accomplished on either Windows PC or Mac.

> [!NOTE]
> ARM64 variants of Windows do not run the Android simulator very well.
>

**Required Tools:**
- **Android Device Manager** - Used to set up simulators (accessible through Visual Studio)
- **Android SDK Manager** - Used to install the SDK platforms

These tools are available by installing the Android Workload using the [MonoGame Getting Started Instructions](../../../../getting_started/platforms.md).

For more information visit about getting started with Android development:

- [Getting Started with Android Development](https://learn.microsoft.com/en-us/dotnet/android/getting-started/installation/)
- [Getting Started with Maui Development](https://learn.microsoft.com/en-us/dotnet/maui/get-started/installation)

To release your game to the Google App Store, you will need to sign up for a developer account at [Google Play Signup](https://play.google.com/console/signup). You will need to accept the Developer Distribution Agreement and pay a registration fee.

More information about the registration process can be found here [Registration process](https://support.google.com/googleplay/android-developer/answer/6112435).

## iOS Development

**Platform Requirement:** For iOS development you will <ins>require</ins> a Mac, whether you develop entirely on one or use the **Pair to Mac** option with Windows Visual Studio.

Since **Visual Studio for Mac** has been deprecated, [JetBrains Rider](https://www.jetbrains.com/rider/) or [VS Code](https://code.visualstudio.com/) can be used to develop for iOS. There is a non-commercial licence available of Rider.

For information about Pairing between Visual Studio and Mac, learn more [here](https://learn.microsoft.com/en-us/dotnet/maui/ios/pair-to-mac).

**Additional Requirements:**
- **Xcode** - Required for iOS development and deployment
- **Apple Developer Account** - Required for physical device deployment and App Store publishing ([enrollment link](https://developer.apple.com/programs/enroll/))

# Modern .NET Project Features

This tutorial utilizes modern .NET project management features to streamline cross-platform development:

## Central Package Management

We will be using [Central Package Management](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management) to maintain consistent package versions across all platform projects. This ensures your shared code library and platform-specific projects use identical dependency versions.

## SLNX Solution Format

The sample projects use the new [SLNX solution format](https://devblogs.microsoft.com/dotnet/introducing-slnx-support-dotnet-cli/) for improved cross-platform development experience and better integration with modern .NET tooling.

# Next Steps

In the next chapter, we will convert the existing Windows-only Dungeon Slime project to support multiple platforms and explore the cross-platform project architecture.
