---
title: Package games for distribution
description: How to package your game for distribution.
---

## Desktop games

To publish desktop games, it is recommended that you build your project as a [self-contained](https://docs.microsoft.com/en-us/dotnet/core/deploying/#publish-self-contained) .NET application. As such, your game will require absolutely no external dependencies and should run out-of-the-box as-is.

### Building and packaging

### [Windows](#tab/windows)

From the .NET CLI:

`dotnet publish -c Release -r win-x64 -p:PublishReadyToRun=false -p:TieredCompilation=false -p:PublishAot=true --self-contained`

> [!IMPORTANT]
> Note we are making use of the `PublishAot` option. Using `Aot` has some restrictions which may require changes to your game code. Especially if you are using Reflection.

You can then zip the content of the publish folder and distribute the archive as-is.

If you are targeting WindowsDX, note that players will need [the DirectX June 2010 runtime](https://www.microsoft.com/en-us/download/details.aspx?id=8109) to be installed on their machine for audio and gamepads to work properly.

### [macOS](#tab/macos)

We recommend that you distribute your game as an [application bundle](https://developer.apple.com/library/archive/documentation/CoreFoundation/Conceptual/CFBundles/BundleTypes/BundleTypes.html). Application bundles are directories with the following file structure:

```text
YourGame.app                    (this is your root folder)
    - Contents
        - Resources
            - Content           (this is where all your content and XNB's should go)
            - YourGame.icns     (this is your app icon, in ICNS format)
        - MacOS
            - YourGame          (the main executable for your game)
        - Info.plist            (the metadata of your app, see bellow for contents)
```

So first lets create our directory structure.

```
mkdir -p bin/Release/YourGame.app/Contents/MacOS/
mkdir -p bin/Release/YourGame.app/Contents/Resources/Content
```

Next we need to publish our application for both `arm64` (M1/M2 devices) and `x64` (Intel). From the .NET CLI:

```cli
dotnet publish -c Release -r osx-x64 -p:PublishReadyToRun=false -p:TieredCompilation=false -p:PublishAot=true --self-contained
dotnet publish -c Release -r osx-arm64 -p:PublishReadyToRun=false -p:TieredCompilation=false -p:PublishAot=true --self-contained
```

> [!IMPORTANT]
> Note we are making use of the `PublishAot` option. Using `Aot` has some restrictions which may require changes to your game code. Especially if you are using Reflection.

Next we need to comibne the two binaries into one Universal Binary which will work on both arm64 and x64 machines.
We can do this using the `xcode` utility `lipo`.

```cli
lipo -create bin/Release/net8.0/osx-arm64/publish/YourGame bin/Release/net8.0/osx-x64/publish/YourGame -output bin/Release/YourGame.app/Contents/MacOS/YourGame
```

The above command will combine the two output executables into one. 

Copy over your content

```
cp -R bin/Release/net8.0/Content bin/Release/YourGame.app/Contents/Resources/Content
```

The `Info.plist` file is a standard macOS file containing metadata about your game. Here is an example file with required and recommended values set:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>CFBundleDevelopmentRegion</key>
    <string>en</string>
    <key>CFBundleExecutable</key>
    <string>YourGame</string>
    <key>CFBundleIconFile</key>
    <string>YourGame</string>
    <key>CFBundleIdentifier</key>
    <string>com.your-domain.YourGame</string>
    <key>CFBundleInfoDictionaryVersion</key>
    <string>6.0</string>
    <key>CFBundleName</key>
    <string>YourGame</string>
    <key>CFBundlePackageType</key>
    <string>APPL</string>
    <key>CFBundleShortVersionString</key>
    <string>1.0</string>
    <key>CFBundleSignature</key>
    <string>FONV</string>
    <key>CFBundleVersion</key>
    <string>1</string>
    <key>LSApplicationCategoryType</key>
    <string>public.app-category.games</string>
    <key>LSMinimumSystemVersion</key>
    <string>10.15</string>
    <key>NSHumanReadableCopyright</key>
    <string>Copyright © 2022</string>
    <key>NSPrincipalClass</key>
    <string>NSApplication</string>
    <key>LSRequiresNativeExecution</key>
    <true/>
    <key>LSArchitecturePriority</key>
    <array>
        <string>arm64</string>
    </array>
</dict>
</plist>
```

For more information about Info.plist files, see the [documentation](https://developer.apple.com/library/archive/documentation/General/Reference/InfoPlistKeyReference/Introduction/Introduction.html).

After completing these steps, your .app folder should appear as an executable application on macOS.
However it does need an icon. So we need to create an `.icns` file. We can use online tools to do this or you can use the following 

```cli
mkdir -p bin/Release/YourGame.iconset
sips -z 16 16 Icon.png --out bin/Release/YourGame.iconset/icon_16x16.png
sips -z 32 32 Icon.png --out bin/Release/YourGame.iconset/icon_16x16@2x.png
sips -z 32 32 Icon.png --out bin/Release/YourGame.iconset/icon_32x32.png
sips -z 64 64 Icon.png --out bin/Release/YourGame.iconset/icon_32x32@2x.png
sips -z 128 128 Icon.png --out bin/Release/YourGame.iconset/icon_128x128.png
sips -z 256 256 Icon.png --out bin/Release/YourGame.iconset/icon_128x128@2x.png
sips -z 256 256 Icon.png --out bin/Release/YourGame.iconset/icon_256x256.png
sips -z 512 512 Icon.png --out bin/Release/YourGame.iconset/icon_256x256@2x.png
sips -z 512 512 Icon.png --out bin/Release/YourGame.iconset/icon_512x512.png
sips -z 1024 1024 Icon.png  bin/Release/YourGame.iconset/icon_512x512@2x.png
iconutil -c icns bin/Release/YourGame.iconset --output bin/Release/YourGame.app/Contents/Resources/YourGame.icns
```

Note this code is expecting an `Icon.png` file to be in the same directory. This file should be `1024` x `1024` pixels. 

For archiving, we recommend using the .tar.gz format to preserve the execution permissions (you will likely run into permission issues if you use .zip at any point).

### [Ubuntu](#tab/ubuntu)

From the .NET CLI:

`dotnet publish -c Release -r linux-x64 -p:PublishReadyToRun=false -p:TieredCompilation=false -p:PublishAot=true --self-contained`

> [!IMPORTANT]
> Note we are making use of the `PublishAot` option. Using `Aot` has some restrictions which may require changes to your game code. Especially if you are using Reflection.

You can then archive the content of the publish folder and distribute the archive as-is.

We recommend using the .tar.gz archiving format to preserve the execution permissions.

---

## Special notes about .NET parameters

.NET proposes several parameters when publishing apps that may sound helpful, but have many issues when it comes to games (because they were never meant for games in the first place, but for small lightweight applications).

### PublishAot

This option optimises your game code "Ahead of Time". It allows you to ship your game without the need to JIT (Just In Time compile).
However you do need to currently add some additional settings to your .csproj.

```
  <ItemGroup>
    <TrimmerRootAssembly Include="MonoGame.Framework" />
    <TrimmerRootAssembly Include="mscorlib" />
  </ItemGroup>
```

The `TrimmerRootAssembly` stops the trimmer removing code from these assemblies. This will on the whole allow the game to run without 
any issues. However if you are using any Third Party or additional assemblies, you might need to add them to this list or fix your code to be `Aot` compliant.
It is recommended that you publish using AOT as it simplifies the app bundle. 

See [Trim self-contained deployments and executables](https://learn.microsoft.com/en-us/dotnet/core/deploying/trimming/trim-self-contained) for more information.

There are some known area's you need to watchout for. 

1. Using `XmlSerializer` in your game will probably cause issues. Since it uses reflection it will be difficult for the Trimmer to figure out what needs to be kept.
   It is recommended that instead of using the `Deserialize` method, you write your own custom deserializer using `XDocument` or `XmlReader`.
   Alternatively you can use the Content Pipeline and create a custom `Processor` and `Reader` to convert the Xml into a binary format that can be loaded via the usual `Content.Load<T>` method.
2. Dynamically loading assemblies via `Assembly.LoadFile`.
3. No run-time code generation, for example, System.Reflection.Emit.

### ReadyToRun (R2R)

[ReadyToRun](https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-core-3-0#readytorun-images) is advertised as improving application startup time, but slightly increasing binary size. We recommend not using it for games, because it produces micro stutters when your game is running.

Ready2Run code is of low quality and makes the Just-In-Time compiler (JIT) to trigger regularly to promote the code to a higher quality. Whenever the JIT runs, it produces potentially very visible stutters.

Disabling ReadyToRun solves this issue (at the cost of a slightly longer startup time, but typically very negligible).

ReadyToRun is disabled by default. You can configure it by setting the `PublishReadyToRun` property in your csproj file.

MonoGame templates for .NET projects explicitly set this to `false`.

### Tiered compilation

[Tiered compilation](https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-core-3-0#tiered-compilation) is a companion system to ReadyToRun and works on the same principle to enhance startup time. We suggest disabling it to avoid any stutter while your game is running.

Tiered compilation is **enabled by default**. To disable it set the `TieredCompilation` property to `false` in your csproj.
MonoGame templates for .NET projects explicitly set this to `false`.

### SingleFilePublish

SingleFilePublish packages your game into a single executable file with all dependencies and content integrated.

While it sounds very convenient, be aware that it's not magical and is in fact a hidden self-extracting zip archive. As such, it may make app startup take **a lot** longer if your game is large, and may fail to launch on systems where user permissions don't allow extracting files (or if there is not enough storage space available).

We highly recommend not using it for better compatibility across systems.

## Mobile games

Please refer to the Xamarin documentation:

- [Android](https://docs.microsoft.com/en-us/xamarin/android/deploy-test/publishing/)

- [iOS](https://docs.microsoft.com/en-us/xamarin/ios/deploy-test/app-distribution/app-store-distribution/publishing-to-the-app-store?tabs=windows)
