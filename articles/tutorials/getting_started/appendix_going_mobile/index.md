---
title: "Appendix: Going Mobile"
description: Tips to take the bat and ball game from this guide to Android and iOS devices.
---

This appendix gives short tips to play the bat and ball game from [Step 6](../06_a_basic_game/index.md) on Android and iOS devices. It is not a full guide to mobile development.

## Install the mobile workloads

A *workload* adds support for a platform to the .NET SDK. To build for Android or iOS, install the workload for each platform:

```sh
dotnet workload install android
dotnet workload install ios
```

> [!IMPORTANT]
> Install the workload for each platform onto your developer machine. If a workload is not installed, the full solution will not build.  These are available for all projects, the workload simply enables your machine to build for that platform with .NET.

You will also need some additional tools from the vendors for each platform:

- **Android**: You also need the Android SDK. Visual Studio and Rider can install it for you. For other methods, refer to [Install the .NET for Android dependencies](https://learn.microsoft.com/dotnet/android/getting-started/installation/dependencies).
- **iOS**: To build and run an iOS game, you must have a Mac with Xcode installed. To install a game on an iPhone or iPad, you also need an Apple developer account. (You can build an iOS project on Windows, but you cannot build or run it).

## Add the mobile platforms to your project

The template adds platform projects only when you create a project. To add Android and iOS to the game from this guide:

1. Create a new project that includes the mobile platforms to a **separate folder** (**DO NOT** try this command targeting the existing project folder, as it will either not work or overwrite files):

    ```sh
    dotnet new monogame -n BatAndBall --Platforms vk dx12 android ios
    ```

2. Copy these files from your old project to the new project:
    - `Source/Game1.cs`.
    - All the files in `Content/Assets`.
    - `Content/Builder/Builder.cs`.
3. Build the full solution once.

> [!NOTE]
> Alternatively, you can copy the new "Platforms" folders, and the platform "resources" folders to your existing project and then update the `.sln` (or `.slnx` if you have migrated it) to include the new platforms.  Everything else should just work.

The game code and the content do not change for each platform. Only the projects in the `Platforms` folder are different.

> [!TIP]
> When you start a new game, select all the platforms that you think you will need. It is easier to delete a platform than to add one. To delete a platform, delete its folder in `Platforms` and remove the project from the solution.

## Add touch input

Phones and tablets do not have a keyboard. To move the bats with touch, you need to read the [**TouchPanel**](xref:Microsoft.Xna.Framework.Input.Touch.TouchPanel) state in `Update`.

1. At the top of `Game1.cs`, add this line:

    ```cs
    using Microsoft.Xna.Framework.Input.Touch;
    ```

2. In `Update`, add this code below the two lines that move the bats with the keyboard, and above the two `MathHelper.Clamp` lines:

    ```cs
    // Move each bat to a finger on its side of the screen.
    foreach (TouchLocation touch in TouchPanel.GetState())
    {
        float touchY = touch.Position.Y - _batTexture.Height / 2f;
        if (touch.Position.X < screen.Width / 2)
        {
            _leftBat.Y = touchY;
        }
        else
        {
            _rightBat.Y = touchY;
        }
    }
    ```

[**TouchPanel.GetState**](xref:Microsoft.Xna.Framework.Input.Touch.TouchPanel.GetState) gives one [**TouchLocation**](xref:Microsoft.Xna.Framework.Input.Touch.TouchLocation) for each finger on the screen. A finger on the left half of the screen moves the left bat. A finger on the right half moves the right bat. Thus, two players can play on one device. The `MathHelper.Clamp` lines below this code keep the bats on the screen.

The keyboard code stays in the game, so the desktop versions still operate with the keyboard.

## Screen size and orientation

- The game uses `GraphicsDevice.Viewport.Bounds` for all positions. Thus, it adapts to the screen size of each device.
- The Android project shows the game in landscape orientation. This is set by `ScreenOrientation.SensorLandscape` in `Platforms/Android/MainActivity.cs`.
- The iOS project also shows the game in landscape orientation. This is set by `UISupportedInterfaceOrientations` in `Resources/iOS/Info.plist`.

## Stop the game

- iOS does not let an app close itself. The `#if !__IOS__` lines in `Update` remove the exit code from iOS builds.
- On Android, the *Back* button of the device is the same as the *Back* button of a gamepad. Thus, the template exit code closes the game.

## Run the game on a device

- **Visual Studio**: Install the *.NET Multi-platform App UI development* workload. Then, select the Android or iOS project, select a device or an emulator in the toolbar, and press `F5`.
- **Visual Studio Code**: Install the *.NET MAUI* extension. Then, select a device in the status bar, and run the Android or iOS launch configuration.
- **Rider**: Select the run configuration for the Android or iOS project, then select a device.

On a physical Android device, enable *Developer options* and *USB debugging* before you connect it to your computer.

## Before you publish

Before you publish your game to a store, change the identity of your game in `Resources/Identity.props`:

- `ApplicationId`: a unique name for your game, for example `com.yourname.batandball`. This becomes the Android package name and the iOS bundle identifier.
- `ApplicationVersion` and `ApplicationDisplayVersion`: the version of your game. Increase `ApplicationVersion` for each store submission.

The Android name under the launcher icon comes from `Resources/Android/Values/strings.xml`.

For more information, refer to these pages:

- [Supported Platforms](../../../getting_started/platforms.md)
- [Packaging Games for Distribution](../../../getting_started/packaging_games.md)
