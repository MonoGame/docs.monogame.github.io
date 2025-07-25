---
title: "Chapter 25: Packaging Your Game for Distribution"
description: "Learn how to package your game for distribution across Windows, macOS, and Linux platforms."
---

After all of our work creating Dungeon Slime, we need to prepare the game for distribution to players.  Properly packaging your game ensures it runs correctly on different platforms without requiring players to have development tools installed.

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
2. **Update Game Information**: Verify your game's title, version, and other information in the project's properties file (`.csproj`).
3. **Final Testing**: Perform thorough testing in Release mode to catch any issues that might not appear in Debug mode.
4. **Asset Optimization**: Consider optimizing larger content files to reduce the final package size.

## Platform-Specific Packaging

Now that we understand the general packaging concepts, we will explore how to create distributions for Windows, macOS, and Linux.  Each platform has specific requirements and tooling that we will need to navigate.  Choose the instructions below based on the platform you are using.

> [!IMPORTANT]
> The packaging instructions for each platform are designed to be executed on that same platform.  This is because each operating system provides specific tools needed for proper packaging (like `lipo` on macOS or `permission settings` on Unix-based systems).  When building on Windows for macOS or Linux, the executable permissions cannot be set since Windows lacks these concepts.
>
> While you will need access to each platform for the steps below, do not worry if you do not have all these systems available.  At the end of this chapter, third party libraries provided by MonoGame community members are included that can automate these processes for you without requiring you to own each type of machine.

### [Windows](#tab/windows)

Windows is the most straightforward platform to target since MonoGame development typically happens on Windows machines.

#### Building for Windows

To create a self-contained application for Windows, open a new command prompt window in the same folder as the main game project (in our case the folder with the `DungeonSlime.csproj` file) and execute the following .NET CLI command:

```sh
dotnet publish -c Release -r win-x64 -p:PublishReadyToRun=false -p:TieredCompilation=false --self-contained
```

This command specifies:

- `-c Release`: Builds in Release configuration for better performance.
- `-r win-x64`: Targets 64-bit Windows platforms.
- `-p:PublishReadyToRun=false`: Disables ReadyToRun compilation (explained later).
- `-p:TieredCompilation=false`: Disables Tiered Compilation (explained later).
- `--self-contained`: Includes the .NET runtime in the package.

The output will be placed in a directory like `bin/Release/net8.0/win-x64/publish/`, relative to the game's `.csproj` file.  This directory will contain the executable and all necessary files to run your game.

> [!NOTE]
> If your base game project is created with **dotnet 9.0** (which at the time of writing is the default), the above folder will be more like `bin/Release/net9.0/win-x64/publish/`, just so you are aware.
> Noting the change in folder from `net8.0` to `net9.0`.

#### Creating a Windows Distribution

Once you have created a build for Windows, to create a Windows distribution, you can simply:

1. Zip the entire contents of the publish folder.
2. Distribute the ZIP file to your players.
3. Players can extract the ZIP and run the executable directly.

> [!NOTE]
> If you are using the WindowsDX platform target, players may need to install the [DirectX June 2010 Runtime](https://www.microsoft.com/en-us/download/details.aspx?id=8109) for audio and gamepad support.  If you are targeting this platform, consider including this information in your game's documentation.

### [macOS](#tab/macOS)

Packaging for macOS requires creating an **Application Bundle** (`.app`), which is a directory structure that macOS recognizes as an application.

#### Building for macOS

For macOS, you will need to build for both the Intel (x64) and Apple Silicon (arm64) to support all modern Mac computers.  Open a new terminal window in the same folder as the `DungeonSlime.csproj` file (the main game project).

> [!TIP]
> The following sections will guide you through several terminal commands that build on each other.  It is best to use a single terminal window located in your projects root directory (where the `DungeonSlime.csproj` file is) for all of these steps to ensure paths remain consistent.

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

The output from this command will be placed in a directory like `bin/Release/net8.0/osx-x64/publish/`, relative to the game's `.csproj` file.

> [!NOTE]
> If your base game project is created with **dotnet 9.0** (which at the time of writing is the default), the above folder will be more like `bin/Release/net9.0/osx-x64/publish/`, just so you are aware.
> Noting the change in folder from `net8.0` to `net9.0`.

Next, to create the Apple Silicon (arm64) self contained application for macOS, in the same terminal window, execute the following .NET CLI command:

```sh
dotnet publish -c Release -r osx-arm64 -p:PublishReadyToRun=false -p:TieredCompilation=false --self-contained
```

The only difference in this command is the use of `-r osx-arm64` which specifies to target the Apple Silicon (arm64) macOS platform.

The output from this command will be placed in a directory like `bin/Release/net8.0/osx-arm64/publish/`, relative to the game's `.csproj` file.

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

2. Copy all files from the Intel (x64) build to the MacOS directory. This ensures all the required dependencies are included. To do this, execute the following command:

    ```sh
    cp -R bin/Release/net8.0/osx-x64/publish/* bin/Release/DungeonSlime.app/Contents/MacOS/
    ```

    > [!NOTE]
    > This copies all files from the publish directory, including the executable, all dependent `.dll` files, and the `Content` directory that contains your game assets.

3. Replace the executable with a universal binary that works on both Intel and Apple Silicon Macs. To do this, execute the following command:

    ```sh
    lipo -create bin/Release/net8.0/osx-arm64/publish/DungeonSlime bin/Release/net8.0/osx-x64/publish/DungeonSlime -output bin/Release/DungeonSlime.app/Contents/MacOS/DungeonSlime
    ```

    > [!NOTE]
    > The `lipo` command is a macOS utility that works with multi-architecture binaries.  Here, it combines the Intel (x64) and Apple Silicon (arm64) executables into a single "universal binary" that can run natively on both Apple Silicon and Intel processor based Macs.

4. Move the Content directory from the MacOS directory to the Resources directory, following macOS application bundle conventions. To do this, execute the following command:

    ```sh
    mv bin/Release/DungeonSlime.app/Contents/MacOS/Content bin/Release/DungeonSlime.app/Contents/Resources/
    ```

    > [!NOTE]
    > This moves the `Content` directory to the expected location for resources in a macOS application bundles.

5. Create a new file called `Info.plist` in the `Contents` directory of the application bundle with the following command:

    ```sh
    touch bin/Release/DungeonSlime.app/Contents/Info.plist
    ```

    > [!NOTE]
    > The `touch` command creates an empty file if it does not exist or updates the modification time if it does exist.  We are using it here to create a blank file that we will populate with content in the next step.

6. Open the `Info.plist` file you just created in a text editor and add the following content to the file and save it.

    [!code-xml[](./snippets/info.plist?highlight=8,10,12,16,30)]

    > [!NOTE]
    > The `Info.plist` file is a critical component of any macOS application bundle.  It contains essential metadata that macOS uses to:
    >
    > - Identify the application (bundle identifier, name, version).
    > - Display the application correctly in Finder and the Dock.
    > - Associate the correct icon with the application.
    > - Define the application's capabilities and requirements.
    > - Provide copyright and developer information.
    >
    > Without a properly formatted `Info.plist` file, macOS would not recognize your game as a valid application, and users would be unable to launch it through normal means.

    > [!TIP]
    > The highlighted sections in the `Info.plist` file need to be customized for your game:
    > - Replace all instances of "DungeonSlime" with your game's name.
    > - The `CFBundleIconFile` value (line 10) must exactly match the name of your `.icns` file that we will create in the next step.
    > - Update the bundle identifier on line 12 with your domain.
    > - Modify the copyright information on line 30 as needed.
    >
    > Getting these values right, especially the icon filename, ensures your game appears correctly on macOS.
    >
    > For more information on the `Info.plist` manifest file, refer to the [About Info.plist Keys and Values](https://developer.apple.com/library/archive/documentation/General/Reference/InfoPlistKeyReference/Introduction/Introduction.html) Apple Developer documentation.


7. Next, create the application bundle `.icns` icon file. To do this, perform the following:

    1. First, you will need a `.png` file that can be used to create the icon set for the final `.icns` output.  If you already have a `.png` icon for your game, ensure it is in the root of the main project directory and is named `Icon.png`.  If you do not have one already prepared, you can use the `Icon.bmp` that was generated in the root of the main project directory when you initially created the project.  However, it will need to be converted to a `.png` first. To do this, execute the following command:

        ```sh
        sips -s format png Icon.bmp --out Icon.png        
        ```

        > [!NOTE]
        > `sips` (Scriptable Image Processing System) is a command line tool in macOS for image manipulation.  Here we are using it to convert a `.bmp` to a `.png`.  In a moment, we will also use it to resize the `.png` into different icon sizes required for the application bundle.

    2. Next, create a directory that we can output each of the generated `.png` icon files to for the icon set.  Execute the following command:

        ```sh
        mkdir -p bin/Release/DungeonSlime.iconset
        ```

    3. Now we use the `sips` command to generate the icon for each size required for a mac app bundle.  Each size generated is necessary for different display scenarios in macOS (Dock, Finder, etc.).  To do this, execute the following commands:

       ```sh
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
        ```

    4. Finally, combine all of the generated icons for the icon set into a `.icns` file.  To do this, execute the following:

        ```sh
        iconutil -c icns bin/Release/DungeonSlime.iconset --output bin/Release/DungeonSlime.app/Contents/Resources/DungeonSlime.icns
        ```

        > [!NOTE]
        > `iconutil` is a command line tool in macOS used to convert icon sets into a single high-resolution `.icns` file.

        > [!TIP]
        > After creating the `.icns` file using the above command, if you open the folder in Finder with `DungeonSlime.app` and it shows a blank square as the icon instead of the one you just created, right-click on `DungeonSlime.app` and choose `Get Info` from the context menu.  This will force it to do a refresh and show the icon properly.  After doing this, if the icon still does not show, then you need to double check that the `CFBundleIconFile` value in the `Info.plist` is named **exactly** the same as the `.icns` file that was created (minus the extension).

8. Set executable permissions for the game executable.  To do this, execute the following command:

    ```sh
    chmod +x bin/Release/DungeonSlime.app/Contents/MacOS/DungeonSlime
    ```

    > [!NOTE]
    > the `chmod +x` command changes the file permissions to make it executable. Without this step, macOS would not be able to run the application.

#### Distributing for macOS

For macOS distribution:

1. Archive the application bundle using the `tar.gz` archive format to preserve the executable permissions that were set.  To do this, execute the following command in the same terminal window:

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
    > Unlike **ZIP** files, the `tar.gz` format preserves Unix file permissions, which is crucial for maintaining the executable permission we set in the previous steps.

2. Distribute the `tar.gz` archive file to players.
3. Players can extract the `tar.gz` archive file and run the application bundle to play the game.

### [Linux](#tab/linux)

Linux packaging is relatively straightforward, but requires attention to ensure executable permission are set.

#### Building for Linux

To create a self-contained application for Linux, open a new Terminal window in the same folder as the `DungeonSlime.csproj` file (your main game project folder) and execute the following .NET CLI command:

```sh
dotnet publish -c Release -r linux-x64 -p:PublishReadyToRun=false -p:TieredCompilation=false --self-contained
```

- `-c Release`: Builds in Release configuration for better performance.
- `-r linux-x64`: Targets 64-bit Linux platforms.
- `-p:PublishReadyToRun=false`: Disables ReadyToRun compilation (explained later).
- `-p:TieredCompilation=false`: Disables Tiered Compilation (explained later).
- `--self-contained`: Includes the .NET runtime in the package.

The output will be placed in a directory like `bin/Release/net8.0/linux-x64/publish`, relative to the `DungeonSlime.csproj` file.  This folder will contain the executable and all necessary files to run the game.

> [!NOTE]
> If your base game project is created with **dotnet 9.0** (which at the time of writing is the default), the above folder will be more like `bin/Release/net9.0/linux-x64/publish/`, just so you are aware.
> Noting the change in folder from `net8.0` to `net9.0`.

#### Creating a Linux Distribution

Once you have created a build for Linux, to create a distributable archive:

1. Ensure the main executable has proper execution permissions by executing the following command in the same terminal window:

    ```sh
    chmod +x bin/Release/net8.0/linux-x64/publish/DungeonSlime
    ```

    > [!NOTE]
    > the `chmod +x` command changes the file permissions to make it executable. Without this step, Linux would not be able to run the application.

2. Package the game using the `tar.gz` archive format to preserve executable permissions by executing the following command:

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
    > Unlike **ZIP** files, the `tar.gz` format preserves Unix file permissions, which is crucial for maintaining the executable permission we set in the previous step.

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

Native AOT compilation (specified with `-p:PublishAot=true`) compiles your entire application to native code at build time, eliminating the need for JIT compilation during runtime.  This can provide better performance and a smaller distribution size.

However, AOT has limitations:

1. No support for runtime reflection.
2. No runtime code generation.
3. Some third-party libraries your game uses may not be compatible.

For MonoGame game, AOT can work well if you avoid these limitations.

> [!NOTE]
> Native AoT is recommended for mobile platforms due to its performance benefits and smaller binary size, which are important for mobile devices with limited resources.  Additionally, it is mandatory when targeting console platforms (Xbox, PlayStation, Switch) as these platforms typically do not support JIT compilation for security and performance reasons.

For more information on Native AOT, refer to the [Native AOT deployment overview](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/?tabs=windows%2Cnet8) documentation on Microsoft Learn.

### Trimming

Trimming (specified with `-p:Trimming:true`) removes unused code from your distribution to reduce size.  It is automatically enabled when using AOT.

While trimming can significantly reduce your game's size, it may remove types that appear unused but are accessed indirectly through reflection or generics causing runtime errors.

> [!IMPORTANT]
> Trimming can cause issues with content pipeline extensions that are used at runtime.  When the compiler cannot detect that certain types are used (especially with reflection or generic collections), they might be trimmed away, resulting in "type not found" exceptions when loading content.
>
> If you encounter runtime exceptions about missing types when loading content with trimming enabled, you can resolve this by ensuring the compiler recognizes the types being used at runtime by making the following call:
>
> ```cs
> ContentTypeReaderManager.AddTypeCreator(typeof(ReflectiveReader<ReaderType>).FullName, () => new ReflectiveReader<ReaderType>())
> ```
>
> Where `ReaderType` is the `ContentTypeReader` of the content pipeline extension to be preserved.  This call should be made somewhere in your code before loading content that uses these types.  

For more information on Trimming, refer to the [Trim self-contained applications](https://learn.microsoft.com/en-us/dotnet/core/deploying/trimming/trim-self-contained) documentation on Microsoft Learn.

### Single File Publishing

Single file publishing packages your entire application into a single executable.  While this sounds convenient, it is essentially a self-extracting archive that extracts to a temporary directory at runtime.

This can significantly increase startup time for larger games and may fail on system with restricted permissions or limited storage.  For this reason, it is not recommended to use this option for games.

For more information on Single File Publishing, refer to the [Create a single file for application deployment](https://learn.microsoft.com/en-us/dotnet/core/deploying/single-file/overview?tabs=cli) documentation on Microsoft Learn.

## Cross-Platform Considerations

When distributing your games across multiple platforms, be aware of these additional considerations:

### File Paths

Different operating systems use different path separators (Windows uses backslashes, macOS and Linux use forward slashes).  Always use `Path.Combine` in your code rather than hardcoding path separators.

```cs
// Incorrect approach - will fail on some platforms
string path = "Content\\images\\atlas-definition.xml"; 

// Correct approach, works on all platforms
string path = Path.Combine("Content", "images", "atlas-definition.xml");
```

### Case Sensitivity

Windows is case-insensitive for filenames, but macOS and Linux are case-sensitive.  Ensure your asset references use the exact case that matches your files for maximum compatibility.

```cs
// If the content path on disk is:
// images/Atlas.xnb

// On Windows, this would work fine since windows is case-insensitive.
// On macOS and Linux, this would fail since they are case-sensitive.
Texture2D text = Content.Load<Texture2D>("images/atlas");
```

### External Dependencies

Try to minimize external dependencies.  If your game requires additional libraries or runtimes, document these requirements clearly for players.

> [!NOTE]
> When publishing to distribution platforms and app stores (such as Steam, Epic Game Store, App Store, or Google Play), you are typically required to disclose all external dependencies in your privacy policy or a dedicated dependencies section.  This includes third-party libraries, analytics tools, and any software components that your game depends on.
>
> Check specific requirements for each distribution platform you plant to target, as well as requirements by third-party libraries for using them, as disclosure requirements may vary.

## Asset Security and Protection

When distributing your game, you may have concerns about protecting your assets and code from unauthorized access or reverse engineering.  It is important to understand the practical limitations and trade-offs involved in various security approaches.

### XNB Asset Protection

MonoGame's content pipeline compiles assets into compressed XNB format, which provides a basic level of protection for your game content.  For the majority of indie game projects, this compression is sufficient protection for several reasons:

- **XNB files are not standard formats**: Unlike raw images or audio files, XNB files require specific knowledge and tools to extract.  While the XNB format is documented and part of the MonoGame open source code, this still creates a barrier for the casual user.
- **Practical protection**: While not cryptographically secure, XNB compression deters casual attempts at asset extraction.
- **Performance benefits**: The primary purpose of XNB compilation is optimization, with content protection being a secondary benefit.
- **Cross-platform consistency**: The same XNB format works across all MonoGame platforms without additional configuration.

> [!TIP]
> Unless you are working with highly valuable or sensitive assets (such as unreleased music from major artists or proprietary artwork), the standard XNB compression provides adequate protection for most games.

### Code Obfuscation

For protecting your game's source code logic, obfuscation tools can make reverse engineering more difficult by renaming variables, restructuring code flow, and adding dummy logic paths. However, code obfuscation comes with significant trade-offs:

- **Performance impact**: Obfuscated code often runs slower than clean, optimized code due to additional indirection and complexity.
- **Debugging complexity**: Stack traces become unreadable, making it nearly impossible to diagnose issues reported by players.
- **Build process overhead**: Additional build steps, tools, and integration are required in your development workflow.
- **Platform limitations**: Some obfuscation techniques may not work correctly across all target platforms or may interfere with .NET features that MonoGame uses internally.
- **Compatibility issues**: Obfuscation can break reflection-based code or third-party libraries.

> [!IMPORTANT]
> Consider whether the performance cost of obfuscation is worth the potential security benefits for your specific project.  For most indie game, the impact on player experience may outweigh the security advantages.

### The Reality of Modern Society

It is crucial to understand that in the modern digital landscape, no security measure is truly impenetrable:

- **Corporate security investments**: Major corporations invest millions of dollars annually in security research and implementation, yet breaches still occur regularly. This demonstrates the fundamental challenge of client-side protection.
- **Advanced tools**: Sophisticated reverse engineering tools are readily available and constantly improving, making traditional protection less effective.
- **AI-assisted analysis**: Artificial intelligence can now assist in code analysis and pattern recognition, making traditional obfuscation techniques less reliable.
- **Determined attackers**: If someone is sufficiently motived to extract your assets or reverse engineer your code, they will likely succeed regardless of protection measures.
- **Diminished returns**: For indie developers, time spent on extensive security measures often exceeds the value of the content being protected and could be better invested in core development.

### Practical Security Recommendations

For most MonoGame projects, consider these practical approaches to content protection:

1. **Accept standard protection**: The built-in XNB compression is sufficient for typical use cases and provides the good balance of protection and performance.
2. **Focus on gameplay**: Invest development time in creating compelling gameplay rather than extensive security measures.
3. **Legal protection**: Consider proper licensing, terms of service, and copyright notices as your primary protection—intellectual property law provides stronger protection than technical measures.
4. **Contractual compliance**: If using licensed assets with specific protection requirements, work with the licensor to understand what constitutes "reasonable protection."
5. **Threat assessment**: Realistically evaluate whether your game is likely to be a target for asset extraction or reverse engineering.

> [!NOTE]
> Remember that the goal of asset protection should be to deter the casual extraction, not to create an impenetrable fortress.  The time and resources spent on extensive security measures are often better invested in improving the game itself.

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
> Mobile deployment for MonoGame games is significantly more complex than desktop deployment and typically requires platform-specific development environments (Android Studio for Android and Xcode for iOS).  A comprehensive guide to mobile deployment will be covered in a future tutorial.

If you are interested in extending the Dungeon Slime game, or future games, to mobile platforms after completing this tutorial series, these resources provide a good starting point:

- [Android Deployment Guide](https://learn.microsoft.com/en-us/previous-versions/xamarin/android/deploy-test/publishing/)
- [iOS App Store Distribution](https://learn.microsoft.com/en-us/previous-versions/xamarin/ios/deploy-test/app-distribution/app-store-distribution/publishing-to-the-app-store?tabs=windows)

## Third-Party Packaging Tools

While the platform-specific packaging steps outlined in this chapter give you complete control over the distribution process, they require multiple commands and potentially access to different operating systems.  Fortunately, the MonoGame community has developed several tools that can automate these packaging steps across platforms.

### GameBundle

[GameBundle](https://github.com/Ellpeck/GameBundle) is a .NET command-line tool created by [Ellpeck](https://github.com/Ellpeck) that simplifies packaging MonoGame and other .NET applications into several distributable formats.  This tool can automatically bundle your game for Windows, Linux, and macOS platforms, create applications bundles for macOS, and handle various packaging configurations with a single command.

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

3. What is the purpose of the `Info.plist` file when creating a macOS application bundle?

    :::question-answer
    The `Info.plist` file contains essential metadata about the macOS application, including the bundle identifier, application name, version, copyright information, minimum system requirements, and other configuration details. macOS requires this file to properly recognize and display the application in the Finder and Dock, and to associate the correct icon and file types with the application.
    :::

4. What is the advantage of using a tar.gz archive over zip file when distributing for macOS and Linux?

    :::question-answer
    A tar.gz archive preserves Unix file permissions, which is crucial for maintaining the executable permissions set on game files.  Without these permissions, users would need to manually set execute permissions before running the game.  ZIP files do not reliably preserve these Unix-specific permissions, which could prevent the game from running directly after extraction on macOS and Linux platforms.
    :::

5. What is the purpose of creating a universal binary for macOS distributions?

    :::question-answer
    A universal binary combines executables for multiple CPU architectures (Intel x64 and Apple Silicon arm64) into a single file.  This allows the game to run natively on both older Intel-based Macs and newer Apple Silicon Macs without requiring separate distributions.
    :::
