---
title: "06: Publishing to iOS App Store"
description: "Learn how to publish your MonoGame mobile application to both the iOS App Store with step-by-step guidance."
---

# Publishing Overview

This chapter covers the complete publishing process for both major mobile app stores. You will learn the requirements, workflows, and common pitfalls for getting your MonoGame app successfully published on iOS platforms.

## Prerequisites

Before publishing, ensure you have:
- Completed development and testing of your cross-platform MonoGame app
- **For iOS:** Active Apple Developer Program membership (an annual fee)
- App icons, screenshots, and store assets prepared
- Final build configurations tested on physical devices

---

# iOS App Store Publishing

## Pre-Publishing Preparation

### Final iOS Build Configuration

Ensure your iOS project is properly configured for App Store submission:

#### Project Properties
```xml
<SupportedOSPlatformVersion>12.2</SupportedOSPlatformVersion>
<BundleIdentifier>com.companyname.gamename</BundleIdentifier>
<CFBundleIconName>AppIcon</CFBundleIconName>
```

### Understanding the Bundle Identifier

The **bundle identifier** is a unique string that identifies your app. It follows a reverse domain name notation, such as `com.companyname.gamename`.

This identifier is used by Apple and Google to distinguish your app from all others on the stores and on devices.

- It **must** be unique across all apps in the store.

- The bundle identifier you set in your project must exactly match the one registered in the stores.

- Changing the bundle identifier after publishing will create a new app entry, not update the existing app.

**Tip:** Choose a bundle identifier that reflects your organization and app name, and keep it consistent across your project files and provisioning profiles.

#### Code Signing for Release

Two entitlements - one for debug and one for publication.

```xml
<PropertyGroup Condition=" '$(Configuration)' == 'Release' ">
    <CodesignKey>iPhone Distribution</CodesignKey>
    <CodesignEntitlements>EntitlementsProduction.plist</CodesignEntitlements>
</PropertyGroup>
```

#### Production Entitlements

Verify your `EntitlementsProduction.plist` has debugging disabled:
```xml
<key>get-task-allow</key>
<false/>
```

### Required Assets

#### App Icons

Ensure your `Assets.xcassets/AppIcon.appiconset` contains all required sizes:

- **iPhone**: 40x40, 60x60, 87x87, 120x120, 180x180
- **iPad**: 40x40, 58x58, 80x80, 152x152, 167x167  
- **App Store**: 1024x1024

|Asset|Image|
|-|-|
|40x40|![40x40](images/icons/appicon20x20@2x.png)|
|58x58|![58x58](images/icons/appicon29x29@2x.png)|
|60x60|![60x60](images/icons/appicon20x20@3x.png)|
|80x80|![80x80](images/icons/appicon40x40@2x.png)|
|87x87|![87x87](images/icons/appicon29x29@3x.png)|
|120x120|![120x120](images/icons/appicon40x40@3x.png)|
|120x120|![120x120](images/icons/appicon60x60@2x.png)|
|152x152|![152x152](images/icons/appicon76x76@2x.png)|
|83.5x83.5|![167x167](images/icons/appicon83.5x83.5@2x.png)|
|180x180|![180x180](images/icons/appicon60x60@3x.png)|
|Artwork 1024x1024|![1024x1024](images/icons/appiconItunesArtwork.png)|

#### Info.plist Configuration

Verify essential properties are set:

The bundle identifier:

From the csproj to match the one in the info.plist file too.

> [!NOTE]
> Change the bundle identifier to match what was set up during the provisioning process.

IOS CSProj:

```xml
<BundleIdentifier>com.monogame.dungeonslime</BundleIdentifier>
```

Info.Plist setting:

```xml
	<key>CFBundleIdentifier</key>
	<string>com.monogame.dungeonslime</string>
```

The game name to appear on the device:

```xml
<key>CFBundleDisplayName</key>
<string>DungeonSlime</string>
```

Versioning of game:

```xml
<key>CFBundleVersion</key>
<string>11</string>
<key>CFBundleShortVersionString</key>
<string>1.10</string>
```

Typical settings for a game:

```
<key>UIRequiresFullScreen</key>
<true/>
<key>UIStatusBarHidden</key>
<true/>
```

### Asset Compilation Target

Ensure your iOS project includes the asset compilation target:

```xml
<Target Name="CompileAssets" BeforeTargets="_CompileAppManifest" Condition="$([MSBuild]::IsOSPlatform('OSX'))">
    <MakeDir Directories="$(OutputPath)Assets" />
    <Exec Command="xcrun actool --output-format human-readable-text --notices --warnings --platform iphoneos --minimum-deployment-target 12.2 --app-icon AppIcon --output-partial-info-plist partial-info.plist --compile $(OutputPath)Assets Assets.xcassets " />
</Target>
```

## App Store Connect Setup

### Creating Your App Record

1. **Log into App Store Connect** at [appstoreconnect.apple.com](https://appstoreconnect.apple.com)
2. **Create New App:**
   - Platform: iOS
   - Name: Your app's marketing name
   - Primary Language: English (or your primary market)
   - Bundle ID: Must match your project's `BundleIdentifier`
   - SKU: Unique identifier for your records

### App Information

#### Required Information

- **App Name:** Display name in the App Store
- **Subtitle:** Brief description
- **Category:** Primary and secondary categories
- **Content Rights:** Whether you own or have licensed all content

#### App Store Listing

- **Description:** Detailed app description
- **Keywords:** Search terms (comma-separated)
- **Marketing URL:** Your app's website
- **Privacy Policy URL:** Required for most apps

### Screenshots and Media

#### Required Screenshots

You need screenshots for each device type you support:

- **iPhone 6.7"** (iPhone 14 Pro Max): 1290 x 2796 pixels
- **iPhone 6.5"** (iPhone 11 Pro Max): 1242 x 2688 pixels  
- **iPhone 5.5"** (iPhone 8 Plus): 1242 x 2208 pixels
- **iPad Pro (6th gen)**: 2048 x 2732 pixels
- **iPad Pro (2nd gen)**: 2048 x 2732 pixels

#### App Preview Videos (Optional)

- 30 seconds maximum
- Same dimensions as screenshots
- Shows actual app gameplay

## Building and Uploading

### Archive Build Process

#### Using Command Line

The creation of an IPA file can be achieved on the terminal. Currently, Rider does **not** support this process.

```sh
dotnet clean
rm -rf bin/ obj/
dotnet publish -c Release -f net8.0-ios -r ios-arm64 -p:ArchiveOnBuild=true
```

### Upload Method

#### Transporter

- Standalone upload tool from Apple

| ![Figure 4-1: Transporter](images/ios/transporter.png) |
| :----------------------------------------------------------------------------------------------------------------------------------------: |
|                       **Figure 4-1: Transporter**                       |

- Useful for automated workflows

| ![Figure 4-2: Transporter Upload Step 1](images/ios/transporter-upload1.png) |
| :----------------------------------------------------------------------------------------------------------------------------------------: |
|                       **Figure 4-2: Transporter Upload Step 1**                       |

| ![Figure 4-3: Transporter Upload Step 2](images/ios/transporter-upload1.png) |
| :----------------------------------------------------------------------------------------------------------------------------------------: |
|                       **Figure 4-3: Transporter Upload Step 2**                       |

- Requires pre-signed IPA file

### Build Processing

After upload:

1. **Processing Time:** 10-60 minutes typically
2. **Build Appears:** In App Store Connect under "Activity"
3. **Status Check:** Wait for "Ready to Submit" status
4. **Build Selection:** Choose the build for your app version

| ![Figure 4-4: Transporter](images/ios/testflight-build.png) |
| :----------------------------------------------------------------------------------------------------------------------------------------: |
|                       **Figure 4-4: Transporter**                       |

| ![Figure 4-5: Transporter](images/ios/testflight-build-encryption.png) |
| :----------------------------------------------------------------------------------------------------------------------------------------: |
|                       **Figure 4-5 Transporter**    |

| ![Figure 4-6: Transporter](images/ios/testflight-build-assign-testers.png) |
| :----------------------------------------------------------------------------------------------------------------------------------------: |
|                       **Figure 4-6: Transporter** |    

## App Review Submission

### Pre-Submission Checklist

- [ ] All required screenshots uploaded
- [ ] App description and metadata complete
- [ ] Privacy policy URL provided (if required)
- [ ] Content rating completed
- [ ] Pricing and availability set
- [ ] Build selected and tested
- [ ] Export compliance information provided

### Review Information

#### App Review Contact Information

- First Name, Last Name
- Phone Number
- Email Address

#### Demo Account (If Required)

If your app requires login:
- Demo Username
- Demo Password
- Additional instructions for reviewers

#### Notes for Review

Additional information to help reviewers:
- Special instructions
- Feature explanations
- Known limitations

### Submission Process

1. **Select Build:** Choose your uploaded build
2. **Review Summary:** Check all information is correct
3. **Submit for Review:** Click "Submit for Review"
4. **Status Tracking:** Monitor in App Store Connect

## Review Timeline and Common Issues

### Typical Timeline

- **Review Time:** 24-48 hours
- **Processing:** Additional time for first-time developers
- **Holiday Delays:** Longer during Apple holidays, so plan accordingly.
