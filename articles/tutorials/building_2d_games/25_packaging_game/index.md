---
title: "Chapter 25: Packaging Your Game for Distribution"
description: "Learn how to package your game for distribution across Windows, macOS, and Linux platforms."
---

After all of our work creating Dungeon Slime, we need to prepare the game for distribution to players.  Properly packaging your game ensure it runs correctly on different platforms without requiring players to have development tools installed.

In this chapter you will:

- Learn how to prepare your game for release.
- Package your game for Windows, macOS, and Linux platforms.
- Create platform-specific distributions with appropriate configurations.
- Understand important publishing parameters and their impact on game performance.
- Address common cross-platform distribution challenges.
- Learn about third-party tools that can automate the packaging process.

## Understanding Game Packaging

When developing with MonoGame, you are working in a .NET environment that abstracts away many platform-specific details.  However, when distributing your game to players, you need to ensure they can run it without installing the .NET runtime or other development dependencies.

### Self-Contained Deployments

The recommended approach for distributing MonoGame games is to use self-contained deployments.  This approach packages your game with all necessary .NET dependencies, resulting in a larger distribution but ensuring your game runs without requiring players install additional runtimes.

A self-contained deployment offers several advantages:

- Players can run your game without installing the .NET runtime.
- Your game will always use the exact version of the runtime it was developed with.
- Distribution is simplified with fewer external dependencies.

The main trade-off is a larger distribution size compared to framework-dependent deployments, but this is usually worth it for the improved player experience.

## Preparing Your Game for Release

Before packaging your game for distribution, you should take some preparatory steps:

1. **Set Release Configuration**: Ensure your build configuration is set to "Release" rather than "Debug" for better performance and smaller executable size.
2. **Update Game Information**: Verify your game's title, version, and other information in the project's properties file (*.csproj*).
3. **Final Testing**: Perform thorough testing in Release mode ot catch any issue that might not appear in Debug mode.
4. **Asset Optimization**: Consider optimizing larger content files to reduce the final package size.

## Platform-Specific Packaging

Now that we understand the general packaging concepts, we will explore how to create distributions for WIndows, macOS, and Linux.  Each platform has specific requirements and tooling that we will need to navigate.  Choose the instructions below based on the platform you are using.

> [!IMPORTANT]
> The packaging instructions for each platform are designed to be executed on that same platform.  This is because each operating system provides specific tools needed for proper packaging (like `lipo` on macOS or permission settings on Unix-based systems).  When building on Windows for macOS or Linux, the executable permissions cannot be set since Windows lacks this concept.
>
> While you will need access to each platform for the steps below, do not worry if you do not have all these systems available.  At the end of this chapter, third party libraries provided by MonoGame community members are included that can automate these processes for you without requiring you to own each type of machine.

### [Windows](#tab/windows)

Windows is the most straightforward platform to target since MonoGame development typically happens on Windows machines.

#### Building for Windows

To create a self-contained application for Window, open a new command prompt window in the same folder as the as the main game project (in our case the folder with the *DungeonSlime.csproj* file) and execute the following .NET CLI command:

```sh
dotnet publish -c Release -r win-x64 -p:PublishReadyToRun=false -p:TieredCompilation=false --self-contained
```

This command specifies:

- `-c Release`: Builds in Release configuration for better performance.
- `-r win-x64`: Targets 64-bit Windows platforms.
- `-p:PublishReadyToRun=false`: Disables ReadyToRun compilation (explained later).
- `-p:TieredCompilation=false`: Disables Tiered Compilation (explained later).
- `--self-contained`: Includes the .NET runtime in the package.

The output will be placed in a directory like *bin/Release/net8.0/win-x64/publish/*, relative to the game's *.csproj* file.  This directory will contain the executable and all necessary files to run your game.

#### Creating a Windows Distribution

Once you have created a build for Windows, to create a Windows distribution, you can simply:

1. Zip the entire contents of the publish folder.
2. Distribute the ZIP file to your players.
3. Players can extract the ZIP and run the executable directly.

> [!NOTE]
> If you are using the WindowsDX platform target, players may need to install the [DirectX June 2010 Runtime](https://www.microsoft.com/en-us/download/details.aspx?id=8109) for audio and gamepad support.  If you are targeting this platform, consider including this information in your game's documentation.

### [macOS](#tab/macOS)

Packaging for macOS requires creating an **Application Bundle** (*.app*), which is a directory structure that macOS recognizes as an application.

#### Building for macOS

For macOS, you will need to build for both the Intel (x64) and Apple Silicon (arm64) to support all modern mac computers.  Open a new terminal window in the same folder as the *DungeonSlime.csproj* file (the main game project).

> [!TIP]
> The following sections will guide you through several terminal commands that build on each other.  It is best to use a single terminal window located in your projects root directory (where the *DungeonSlime.csproj* file is) for all of these steps to ensure paths remain consistent.

First, to create the Intel (x64) self contained application, execute the following .NET CLI command in the terminal:

```sh
dotnet publish -c Release -r osx-x64 -p:PublishReadyToRun=false -p:TieredCompilation=false --self-contained
```

This command specifies:

- `-c Release`: Builds in Release configuration for better performance.
- `-r osx-x64`: Targets Intel (x64) macOS platforms.
- `-p:PublishReadyToRun=false`: Disables ReadyToRun compilation (explained later).
- `-p:TieredCompilation=false`: Disables Tiered Compilation (explained later).
- `--self-contained`: Includes the .NET runtime in the package.

The output from this command will be placed in a directory like *bin/Release/net8.0/osx-x64/publish/*, relative to the game's *.csproj* file.

Next, to create the Apple Silicon (arm64) self contained application for macOS, in the same terminal window, execute the following .NET CLI command:

```sh
dotnet publish -c Release -r osx-arm64 -p:PublishReadyToRun=false -p:TieredCompilation=false --self-contained
```

The only difference in this command is the use of `-r osx-arm64` which specifies to target the Apple Silicon (arm64) macOS platform.

The output from this command will be placed in a directory like *bin/Release/net8.0/osx-arm64/publish/*, relative to the game's *.csproj* file.

#### Creating a macOS Application Bundle

With the Intel (x64) and Apple Silicon (arm64) builds completed, we can now create the macOS **Application Bundle**.  macOS applications follow a very specific directory structure:

```sh
YourGame.app/
├── Contents/
│   ├── Info.plist
│   ├── MacOS/
│   │   └── YourGame
│   └── Resources/
│       ├── Content/
│       └── YourGame.icns
```

To create this structure, from the same terminal window:

1. First, create the folder structure by executing the following commands:

    ```sh
    mkdir -p bin/Release/DungeonSlime.app/Contents/MacOS/
    mkdir -p bin/Release/DungeonSlime.app/Contents/Resources/Content
    ```

    > [!NOTE]
    > The `mkdir -p` command creates directories including any necessary parent directories.  The `-p` flag ensures all intermediate directories are created without error if they do not exist yet.

2. Combine the Intel (x64) and Apple Silicon (arm64) builds into a *universal binary*.  To do this, execute the following commands:

    ```sh
    lipo -create bin/Release/net8.0/osx-arm64/publish/DungeonSlime bin/Release/net8.0/osx-x64/publish/DungeonSlime -output bin/Release/DungeonSlime.app/Contents/MacOS/DungeonSlime
    ```

    > [!NOTE]
    > The `lipo` command is a macOS utility that works with multi-architecture binaries.  Here, it combines the Intel (x64) and Apple Silicon (arm64) executables into a single "universal binary" that can run natively on both Apple Silicon and Intel processor based Macs.

3. Next, copy the your content files to the expected location within the application bundle structure.  To do this, execute the following commands:

    ```sh
    cp -R bin/Release/net8.0/Content bin/Release/DungeonSlime.app/Contents/Resources/Content
    ```

    > [!NOTE]
    > The `cp -R` command copies files recursively, meaning it will copy the entire directory structure.  The `-R` flag ensures all subdirectories and their contents are copied.

4. Create a new file called *Info.plist* in the *Contents* directory of the application bundle with the following command:

    ```sh
    touch bin/Release/DungeonSlime.app/Contents/Info.plist
    ```

    > [!NOTE]
    > The `touch` command creates an empty file if it does not exist or updates the modification time if it does exist.  We are using it here to create a blank file that we will populate with content in the next step.

5. Open the *Info.plist* file you just created in a text editor and add the following content to the file and save it.

    [!code-xml[](./snippets/info.plist?highlight=8,10,12,16,30)]

    > [!NOTE]
    > The *Info.plist* file is a critical component of any macOS application bundle.  It contains essential metadata that macOS uses to:
    >
    > - Identify the application (bundle identifier, name, version).
    > - Display the application correctly in Finder and the Dock.
    > - Associate the correct icon with the application.
    > - Define the application's capabilities and requirements.
    > - Provide copyright and developer information.
    >
    > Without a properly formatted *Info.plist* file, macOS would not recognize your game as a valid application, and users would be unable to launch it through normal means.

    > [!TIP]
    > The highlighted sections in the *Info.plist* file need to be customized for your game:
    > - Replace all instances of "DungeonSlime" with your game's name.
    > - The `CFBundleIconFile` value (line 10) must exactly match the name of your .icns file that we will create in the next step.
    > - Update the bundle identifier on line 12 with your domain.
    > - Modify the copyright information on line 30 as needed.
    >
    > Getting these values right, especially the icon filename, ensures your game appears correctly on macOS.
    >
    > For more information on the *Info.plist* manifest file, refer to the [About Info.plist Keys and Values](https://developer.apple.com/library/archive/documentation/General/Reference/InfoPlistKeyReference/Introduction/Introduction.html) Apple Developer documentation.

6. Next, create the application bundle *.icns* icon file.  To do this, execute the following commands:

    ```sh
    mkdir -p bin/Release/DungeonSlime.iconset
    sips -z 16 16 Icon.png --out bin/Release/DungeonSlime.iconset/icon_16x16.png
    sips -z 32 32 Icon.png --out bin/Release/DungeonSlime.iconset/icon_16x16@2x.png
    sips -z 32 32 Icon.png --out bin/Release/DungeonSlime.iconset/icon_32x32.png
    sips -z 64 64 Icon.png --out bin/Release/DungeonSlime.iconset/icon_32x32@2x.png
    sips -z 128 128 Icon.png --out bin/Release/DungeonSlime.iconset/icon_128x128.png
    sips -z 256 256 Icon.png --out bin/Release/DungeonSlime.iconset/icon_128x128@2x.png
    sips -z 256 256 Icon.png --out bin/Release/DungeonSlime.iconset/icon_256x256.png
    sips -z 512 512 Icon.png --out bin/Release/DungeonSlime.iconset/icon_256x256@2x.png
    sips -z 512 512 Icon.png --out bin/Release/DungeonSlime.iconset/icon_512x512.png
    sips -z 1024 1024 Icon.png --out bin/Release/DungeonSlime.iconset/icon_512x512@2x.png
    iconutil -c icns bin/Release/DungeonSlime.iconset --output bin/Release/DungeonSlime.app/Contents/Resources/DungeonSlime.icns
    ```

    > [!NOTE]
    > These commands create a macOS icon file (*.icns*) with multiple resolutions:
    > - `sips` (Scriptable Image Processing System) resizes the source image to create different icon sizes.
    > - Each size is necessary for different display scenarios in macOS (Dock, Finder, etc.).
    > - The `iconutil` command then compiles these images into a single *.icns* file that macOS recognizes as an application icon.

7. Set executable permissions for the game executable.  To do this, execute the following command:

    ```sh
    chmod +x bin/Release/DungeonSlime.app/Contents/MacOS/DungeonSlime
    ```

    > [!NOTE]
    > the `chmod +x` command changes the file permissions to make it executable. WIthout this step, macOS would not be able to run the application.

#### Distributing for macOS

For macOS distribution:

1. Archive the application bundle using the tar.gz archive format to preserve the executable permissions that were set.  To do this, execute the following command in the same terminal window:

    ```sh
    tar -czf DungeonSlime-osx.tar.gz -C bin/Release/DungeonSlime.app
    ```

    > [!NOTE]
    > The `tar` command creates an archive file:
    > - `-c` creates a new archive.
    > - `-z` compresses the archive using gzip.
    > - `-f` specifies the filename to create
    > - `-C` changes to the specified directory before adding files.
    >
    > Unlike ZIP files, the tar.gz format preserves Unix file permissions, which is crucial for maintaining the executable permission we set in the previous steps.

2. Distribute the tar.gz archive file to players.
3. Players can extract the tar.gz archive file and run the application bundle to play the game.

### [Linux](#tab/linux)

Linux packaging is relatively straightforward, but requires attention to ensure executable permission are set.

#### Building for Linux

To create a self-contained application for LInux, open a new Terminal window in the same folder as the *DungeonSlime.csproj* file (your main game project folder) and execute the following .NET CLI command:

```sh
dotnet publish -c Release -r linux-x64 -p:PublishReadyToRun=false -p:TieredCompilation=false --self-contained
```

- `-c Release`: Builds in Release configuration for better performance.
- `-r linux-x64`: Targets 64-bit Linux platforms.
- `-p:PublishReadyToRun=false`: Disables ReadyToRun compilation (explained later).
- `-p:TieredCompilation=false`: Disables Tiered Compilation (explained later).
- `--self-contained`: Includes the .NET runtime in the package.

The output will be placed in a directory like *bin/Release/net8.0/linux-x64/publish*, relative to the *DungeonSlime.csproj* file.  This folder will contain the executable and all necessary files to run the game.

#### Creating a Linux Distribution

Once you have created a build for Linux, to create a distributable archive:

1. Ensure the main executable has proper execute permissions by executing the following command in the same terminal window:

    ```sh
    chmod +x bin/Release/net8.0/linux-x64/publish/DungeonSlime
    ```

    > [!NOTE]
    > the `chmod +x` command changes the file permissions to make it executable. WIthout this step, Linux would not be able to run the application.

2. Package the game using the tar.gz archive format to preserve executable permissions by executing the following command:

    ```sh
    tar -czf DungeonSlime-linux-x64.tar.gz -C bin/Release/net8.0/linux-x64/publish/
    ```

    > [!NOTE]
    > The `tar` command creates an archive file:
    > - `-c` creates a new archive.
    > - `-z` compresses the archive using gzip.
    > - `-f` specifies the filename to create
    > - `-C` changes to the specified directory before adding files.
    >
    > Unlike ZIP files, the tar.gz format preserves Unix file permissions, which is crucial for maintaining the executable permission we set in the previous step.

---

## Important .NET Publishing Parameters

When publishing your game, there are several .NET parameters that can significantly impact the performance of the game.  In the above sections, these are all set to the recommended values, however, we will examine them in detail below.

### ReadyToRun (R2R)

ReadyToRun is a feature in .NET that pre-compiles code to improve startup time.  This sounds like a good thing on paper, however, for games, it can lead to micro-stutters during gameplay.

This happens because ReadyToRun-compiled code is initially of lower quality, and the Just-In-Time (JIT) compiler will trigger periodically to optimize the code further.  These optimization passes can cause visible stutters in the game.

For games, it is recommended to disable ReadyToRun by setting `-p:PublishReadyToRun=false` in your publish command, which we have already included in our examples.

For more information on ReadyToRun, refer to the [ReadyToRun deployment overview](https://learn.microsoft.com/en-us/dotnet/core/deploying/ready-to-run) documentation on Microsoft Learn

### Tiered Compilation

Tiered compilation is another .NET feature that works similarly to ReadyToRun.  It initially compiles code quickly at a lower optimization level, then recompiles frequently-used methods with higher optimization.

While this improves application startup time, it can also cause stutters during gameplay as methods are recompiled.  It is recommended to disable tiered compilation by setting `-p:TieredCompilation=false` in your publish command, which we have already included in our examples.

For more information on Tiered Compilation, refer to the [Tiered compilation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-core-3-0#tiered-compilation) section on Microsoft Learn.

### Native AOT (Ahead-of-Time) Compilation

Native AOT compilation (specified with `-p:PublishAot=tru`) compiles your entire application to native code at build time, eliminating the need for JIT compilation during runtime.  This can provide better performance and a smaller distribution size.

However, AOT has limitations:

1. No support for runtime reflection.
2. No runtime code generation.
3. Some third-party libraries your game uses may not be compatible.

For MonoGame game, AOT can work well if you avoid these limitations.

For more information on Native AOT, refer to the [Native AOT deployment overview](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/?tabs=windows%2Cnet8) documentation on Microsoft Learn.

### Trimming

Trimming (specified with `-p:Trimming:true`) removes unused code from your distribution to reduce size.  It is automatically enabled when using AOT.

While trimming can significantly reduce your game's size, it may remove types that appear unused bot are accessed indirectly through reflection or generics causing runtime errors.

For more information on Trimming, refer to the [Trim self-contained applications](https://learn.microsoft.com/en-us/dotnet/core/deploying/trimming/trim-self-contained) documentation on Microsoft Learn.

### Single File Publishing

Single file publishing packages your entire application into a single executable.  While this sounds convenient, it is essentially a self-extracting archive that extracts to a temporary directory at runtime.

This can significantly increase startup time for larger games and may fail on system with restricted permissions of limited storage.  For this reason, it is not recommended to use this option for games.

For more information on Single File Publishing, refer to the [Create a single file for application deployment](https://learn.microsoft.com/en-us/dotnet/core/deploying/single-file/overview?tabs=cli) documentation on Microsoft Learn.

## Cross-Platform Considerations

When distributing your games across multiple platforms, be aware of these additional considerations:

### File Paths

Different operating systems use different path separators (Windows uses backslashes, macOS and Linux use forward slashes).  Always use `Path.Combine` in your code rather than hardcoding path separators.

### Case Sensitivity

Windows is case-insensitive for filenames, but macOS and Linux are case-sensitive.  Ensure your asset references use the exact case that matches your files for maximum compatibility.

### External Dependencies

Try to minimize external dependencies.  If your game requires additional libraries or runtimes, document these requirements clearly for players.

## Mobile Platforms

While this tutorial series has focused on creating a 2D game for desktop platforms, MonoGame also offers support for mobile development on Android and iOS.  The game we have built throughout this series could be adapted for touch controls and distributed through mobile app stores with additional work.

Mobile deployment involves several considerations beyond those of desktop platforms:

- App store submission process and platform-specific requirements.
- Platform-specific signing and certification procedures.
- Extensive device compatibility testing across various screen sizes and hardware.
- Optimization of touch input controls (replacing our keyboard and gamepad input).
- Power consumption management and performance optimization for mobile hardware.

For the Dungeon Slime game, adapting to mobile would require:

- Implementing touch controls to replace the keyboard/gamepad movement.
- Potentially rethinking game mechanics to suit mobile play patterns.

> [!NOTE]
> Mobile deployment for MonoGame games is significantly more complex than desktop deployment and typically requires platform-specific development environs (Android Studio for Android and Xcode for iOS).  A comprehensive guide to mobile deployment would require its own dedicated tutorial series.

If you are interested in extending the Dungeon Slime game, or future games, to mobile paltforms after completing this tutorial series, these resources provide a good starting point:

- [Android Deployment Guide](https://learn.microsoft.com/en-us/previous-versions/xamarin/android/deploy-test/publishing/)
- [iOS App Store Distribution](https://learn.microsoft.com/en-us/previous-versions/xamarin/ios/deploy-test/app-distribution/app-store-distribution/publishing-to-the-app-store?tabs=windows)

## Third-Party Packaging Tools

While the platform-specific packaging steps outlined in this chapter give you complete control over the distribution process, they require multiple commands and potentially access to different operating system.  Fortunately, the MonoGame community has developed several tools that can automate these packaging steps across platforms.

### GameBundle

[GameBundle](https://github.com/Ellpeck/GameBundle) is a .NET command-line tool created by [Ellpeck](https://github.com/Ellpeck) that simplifies packaging MonoGame and other .NET applications into several distributable formats.  As described in its documentation, "GameBundle is a tool to package MonoGame and other .NET applications into several distributable formats."

This tool can automatically bundle your game for Windows, Linux, and macOS platforms, create applications bundles for macOS, and handle various packaging configurations with a single command.

For more information about GameBundle, including installation and usage instructions, visit the [official repository on GitHub](https://github.com/Ellpeck/GameBundle)

### MonoPack

[MonoPack](https://github.com/shyfox-studio/MonoPack) is a .NET command-line tool created by [ShyFox Studio](https://github.com/shyfox-studio) designed specifically for MonoGame projects.  According to its documentation, "MonoPack is a dotnet tool used for MonoGame projects to package the game for Windows, Linux, and/or macOS".

Key features include:

- Cross-platform packaging capabilities (build for any OS from any OS).
- Automatic creation of macOS application bundles.
- Appropriate compression formats for each target platform for distribution.
  
For more information about MonoPack, including installation and usage instructions, visit the [official repository on GitHub](https://github.com/shyfox-studio/MonoPack)

## Conclusion

In this chapter, you learned how to package your MonoGame project for distribution across Windows, macOS, and Linux platforms. You now understand how to create self-contained deployments for each target platform, the impact of various .NET publishing options on game performance, and important cross-platform considerations.

Whether you choose to use the manual platform-specific packaging steps or automate the process with tools like [GameBundle](#gamebundle) or [MonoPack](#monopack), you now have the knowledge to ensure your game runs smoothly for players across different platforms without requiring them to install additional dependencies.

## Test Your Knowledge

1. Why is it recommended to use self-contained deployments for distributing MonoGame games?

    :::question-answer
    Self-contained deployments package your game with all necessary .NET dependencies, ensuring players can run the game without installing the .NET runtime. This simplifies distribution, guarantees your game uses the exact runtime version it was developed with, and provides a better player experience despite the larger package size.
    :::

2. Why should ReadyToRun and Tiered Compilation be disabled when publishing games?

    :::question-answer
    ReadyToRun and Tiered Compilation both initially produce lower-quality code that gets optimized during runtime. This dynamic optimization process causes micro-stutters during gameplay as the Just-In-Time compiler triggers to improve code quality. Disabling these features results in slightly longer startup times but provides smoother gameplay without performance hitches.
    :::

3. What is the purpose of the *Info.plist* file when creating a macOS application bundle?

    :::question-answer
    The *Info.plist* file contains essential metadata about the macOS application, including the bundle identifier, application name, version, copyright information, minimum system requirements, and other configuration details. macOS requires this file to properly recognize and display the application in the Finder and Dock, and to associate the correct icon and file types with the application.
    :::

4. What is the advantage of using a tar.gz archive over zip file when distributing for macOS and Linux?

    :::question-answer
    A tar.gz archive preserves Unix fil permissions, which is crucial for maintaining the executable permissions set on game files.  Without these permissions, users would need to manually set execute permissions before running the game.  ZIP files do not reliably preserve these Unix-specific permissions, which could prevent the game from running directly after extraction on macOS and Linux platforms.
    :::

5. What is the purpose of creating a universal binary for macOS distributions?

    :::question-answer
    A universal binary combines executables for multiple CPU architectures (Intel x64 and Apple Silicon arm64) into a single file.  This allows the game to run natively on both older Intel-based Macs and newer Apple Silicon Macs without requiring separate distributions.
    :::
