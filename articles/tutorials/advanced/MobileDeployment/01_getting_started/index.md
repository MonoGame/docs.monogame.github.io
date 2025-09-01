---
title: "01: Setting Up MonoGame for Android and iOS Development"
description: "Get started with MonoGame mobile development by setting up your development environment, tools, and SDKs for Android and iOS platforms."
---

# Getting Started and Overview

This tutorial extends the **MonoGame 2D Dungeon Slime tutorial** to mobile platforms. If you have not completed the desktop tutorial yet, we recommend finishing it first as we will be building directly on that foundation.

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

**Platform Support:** Android development can be accomplished on either Windows PC or Mac. However, ARM64 variants of Windows do not run the Android simulator well.

**Required Tools:**
- **Android Device Manager** - Used to set up simulators (accessible through Visual Studio)
- **Android SDK Manager** - Used to install the SDK platforms

## iOS Development

**Platform Requirement:** iOS development requires a Mac, whether you develop entirely on it or use the **Pair to Mac** option with Windows Visual Studio.

Since **Visual Studio for Mac** has been deprecated, [JetBrains Rider](https://www.jetbrains.com/rider/) or [VS Code](https://code.visualstudio.com/) can be used to develop for iOS. There is a non-commercial licence available of Rider.

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
