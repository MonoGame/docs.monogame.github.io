---
title: Whats New
description: What is new with the release of MonoGame 3.8.*
---

## MonoGame 3.8.4

> [!NOTE]
> **Coming soon**
> Refer to the [CHANGELOG](https://github.com/MonoGame/MonoGame/blob/develop/CHANGELOG.md) for a more complete list of the changes.

Notes for the final 3.8.4 release are still in progress.  Preview builds are available.

> [!NOTE]
> More details to follow

## MonoGame 3.8.3

> [!NOTE]
> Refer to the [CHANGELOG](https://github.com/MonoGame/MonoGame/blob/develop/CHANGELOG.md) for a more complete list of the changes.

* SDL bumped to 2.32.2
* VS2022 Extension with new 2D cross-platform project templates
* Content pipeline working on all supported platforms
* Content Pipeline updated to use the new FreeType and FreeImage
* Improved / updated content pipeline dependencies (Textures, Audio, Input)
* Several SoundEffect fixes, including OpenAL update
* GamePad button mappings on Android and iOS, Plus Joystick input fixes
* MonoGame updated to be more AOT/trimming compliant
* Fix Android MediaPlayer Song
* Preliminary scaffolding for new Native platform
* API Documentation updates
* Vulkan in preview

> [!NOTE]
> More details to follow

## MonoGame 3.8.2

> [!NOTE]
> Refer to the [CHANGELOG](https://github.com/MonoGame/MonoGame/blob/develop/CHANGELOG.md) for a more complete list of the changes.

* Update to .NET 8, including the MGCB tool (now a local tool rather than global, allowing different projects to use different versions)
* Removal of Windows UWP due to Microsoft shutting support for it (you can still use UWP if you remain on 3.8.1)
* Updated build dependencies to improve support.
* New documentation, including the continuing migration of the older XNA documentation.
* A MASSIVE clean-up of the MonoGame XML documentation, big shout out to (@AristurtleDev and his team).
* Build system improvements (mainly a MonoGame building backend thing).
* Various bug fixes across many areas of the framework.

> [!NOTE]
> More details to follow

## MonoGame 3.8.1 (including Hotfix)

The MonoGame 3.8.1 release marks some big changes since 3.8.0 in how we build and distribute.

> [!NOTE]
> Refer to the [CHANGELOG](https://github.com/MonoGame/MonoGame/blob/develop/CHANGELOG.md) for a more complete list of the changes.

## .NET 6 Support

We now support [.NET 6](https://docs.microsoft.com/en-us/dotnet/core/introduction) exclusively.  This brings us up to date with the latest improvements in the .NET ecosystem and allows for exciting new features like [.NET NativeAOT Runtime](https://github.com/dotnet/runtimelab/tree/feature/NativeAOT) and much easier distribution of your games for Windows, macOS and Linux.

## Visual Studio 2022 extension

MonoGame 3.8.1 now comes with an optional Visual Studio extension which will install all the MonoGame project templates and will allow for quick access to the [MGCB Editor](./getting_started/tools/mgcb_editor.md).

This extension is available for Visual Studio 2022, and Visual Studio 2022 for Mac.

If you are migrating from 3.8.0 it is recommended to uninstall the existing global MGCB .NET tools. We recommend reviewing the [Migrating from 3.8.0](./migration/migrate_38.md) documentation, as there are changes to how 3.8.1 works with the .NET tools and the new extension.

## Visual Studio 2019 and prior are no longer supported

Since .NET 6 is not supported by Visual Studio 2019, starting with MonoGame 3.8.1 it will no longer be possible to build games with it.

Moving forward, we will only support Visual Studio 2022, and Visual Studio 2022 for Mac.

If you need to use Visual Studio 2019, we encourage you to stick to MonoGame 3.8.0.

JetBrains Rider and Visual Studio Code can be used regardless of the version of MonoGame.

## Last version to support 32bit Windows

MonoGame 3.8.1 will be the last version to support building and running 32bit games on Windows.

This is motivated by the fact that 32-bit players are nearly extinct (less than 0.24% of the Steam user base). It will also help developers with less complex distribution and less confusing debug/build experiences.

## Apple silicon (M1+) support

Games built using the ```DesktopGL``` [platform](./getting_started/platforms.md) and targeting ```osx-arm64``` will now run natively on Apple silicon without Rosetta emulation.

**However**, it is not yet possible to use the [MGCB](./getting_started/tools/mgcb.md) or the [MGCB Editor](./getting_started/tools/mgcb_editor.md) on Apple silicon, unless you are running the ```osx-x64``` variant of the .NET SDK (and therefore using Rosetta emulation). We are working toward resolving this inconvenience.
