---
title: Upgrading MonoGame projects from 3.8.0 or later to 3.8.4.1
description: A guide on upgrading a MonoGame v3.8 project to the current 3.8.4.1 + versions of MonoGame.
---

Upgrading existing projects from earlier 3.8 releases should be straightforward for most platforms.

The major difference is that 3.8.4 onward recommends using .NET 9 in your client project (mandatory for Mobile projects by the DotNet Framework). You can follow the [environment setup tutorial](../getting_started/index.md) to make sure that you are not missing any components.

The MGCB Editor is no longer a global .NET tool and the MGCB editor is included as part of the specific project through the use of the dotnet tooling configuration `dotnet-tools.json` file located in the `.config` folder in your solution/project.

> [!NOTE]
> If you are using Visual Studio 2022, we recommend that you use the MonoGame extension which helps with accessing the MGCB editor without the need of CLI commands.

The process of updating your project should be fairly quick and painless without having to change your code or your content project.

> [!IMPORTANT]
> It is also recommended that you uninstall the older global versions of the .NET tools as described below.

## Contents

- [Updating DotNet Target Framework](#updating-dotnet-target-framework)
- [Update MonoGame references](#update-monogame-references)
- [Add/Update `dotnet-tools.json` Configuration](#addupdate-dotnet-toolsjson-configuration)
- [Remove `RestoreDotnetTools` section from csproj](#remove-restoredotnettools-section-from-csproj)
- [iOS/iPadOS, and Android Considerations](#iosipados-and-android-considerations)

## Updating DotNet Target Framework

Modern MonoGame projects now use the DotNet framework (older project used to rely on the NetCore/NetFramework libraries).  As MonoGame has used DotNet project templates since 3.8.x, you only need to replace the `TargetFramework` element of any `csproj` files referencing MonoGame libraries, namely:

- Replace any `<TargetFramework>` entries to the following:

```xml
<TargetFramework>net9.0</TargetFramework>
```

> [!NOTE]
> Using `DotNet 9` is only a **recommendation**, you can use any version of DotNet (from 8.0 and above) so long as it supports the MonoGame Dotnet 8 dependency.  This includes the upcoming DotNet 10 LTS.

Make sure to update any and all projects in your solution, especially you have a multi-project solution similar to those in the [MonoGame.Samples](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Platformer2D)

## Update MonoGame references

Ensure you update the MonoGame references to the latest version, this can be achieved by either:

- Editing the `csproj` files that reference MonoGame to the latest version

    > [!NOTE]
    > The MonoGame templates set the Version number as `Version="3.8.*"`, this means that it will use the LATEST public version of MonoGame (not including preview releases) that is available.  However, this does mean your `tools` configuration can become out of sync with your project and potentially cause issue.

- Use the [Updating NuGet package dependencies](../getting_to_know/howto/HowTo_Install_Preview_Release.md#updating-nuget-package-dependencies) documentation as part of the "Preview Release installation instructions", which states you just need to run the following commands (for `DesktopGL`, use other platforms accordingly):

    ```dotnetcli
    dotnet add package MonoGame.Framework.DesktopGL -v 3.8.4.1
    dotnet add package MonoGame.Content.Builder.Task -v 3.8.4.1
    ```

    > [!NOTE]
    > The `MonoGame.Content.Builder.Task` is only needed for client projects and not libraries.

This will ensure your project is using the intended version of MonoGame.

> [!IMPORTANT]
> Always ensure your `dotnet-tools.json` version matches any updates to the version of MonoGame you are using, see next section.

## Add/Update `dotnet-tools.json` Configuration

MonoGame DotNet projects currently **Require** DotNet tools configuration to be able to locate and run the `MGCB` editor which is installed locally per project (preventing issues when working with multiple projects using different versions of MonoGame).

> [!IMPORTANT]
> The MGCB Editor is no longer a .NET global tool, and does not need to be installed or registered. When migrating from 3.8.0, it is recommended that you **uninstall** the global versions of the tools. You can accomplish that with these commands:
>
> ```sh
> dotnet tool uninstall dotnet-mgcb -g
> dotnet tool uninstall dotnet-2mgfx -g
> dotnet tool uninstall dotnet-mgcb-editor -g
> ```

You can either copy the configuration from a new project template (e.g. `dotnet new mgdesktopgl -o <project name>`),
alternatively you can do the following:

- Create a new folder in the root of your project named `.config`
- Add a new file called `dotnet-tools.json` and replace its contents with the following:

```text
{
  "version": 1,
  "isRoot": true,
  "tools": {
    "dotnet-mgcb": {
      "version": "3.8.4.1",
      "commands": [
        "mgcb"
      ]
    },
    "dotnet-mgcb-editor": {
      "version": "3.8.4.1",
      "commands": [
        "mgcb-editor"
      ]
    },
    "dotnet-mgcb-editor-linux": {
      "version": "3.8.4.1",
      "commands": [
        "mgcb-editor-linux"
      ]
    },
    "dotnet-mgcb-editor-windows": {
      "version": "3.8.4.1",
      "commands": [
        "mgcb-editor-windows"
      ]
    },
    "dotnet-mgcb-editor-mac": {
      "version": "3.8.4.1",
      "commands": [
        "mgcb-editor-mac"
      ]
    }
  }
}
```

> [!NOTE]
> Please note that you cannot use the ```3.8.*``` wildcard in the ```dotnet-tools.json``` file (tool versions have to be fully qualified). We strongly recommand that the versions match the MonoGame version referenced in your ```.csproj``` (if you are using the ```*``` wildcard, make sure that they do not end up mismatching if the nuget's are updated without you noticing).

The file is the same regardless of which platform / target you are intending to use.  If you have a solution, you only need a **SINGLE** configuration at the root of the project for all client projects.

## Remove `RestoreDotnetTools` section from csproj

From `3.8.4.1` and above, the `RestoreDotnetTools` section is no longer required in client project `csproj` files, as the processing is now handled within the MonoGame deliverables.

> [!NOTE]
> Earlier versions of MonoGame, e.g. 3.8.0 does not have this configuration in the project template, if your `csproj` does not have a `RestoreDotnetTools` element, you can safely ignore this section.

```xml
  <Target Name="RestoreDotnetTools" BeforeTargets="Restore">
    <Message Text="Restoring dotnet tools" Importance="High" />
    <Exec Command="dotnet tool restore" />
  </Target>
```

> [!NOTE]
> The XML has changed over versions with various messages and layout, but the section to remove is always titled `Name="RestoreDotnetTools"`

Simply remove this section safely from any and all `csproj` files located in your solution that are dependent on MonoGame.

## iOS/iPadOS, and Android Considerations

DotNet 9 is MANDATORY for iOS and Android due to platform requirements, as well as the following configurations:

- MonoGame 3.8.4.1 is **REQUIRED** to comply with the Google Policies on 16kb pages and other affordances.
- The Android `targetSdkVersion` in the `AndroidManifest.xml` **MUST** be a minimum "35" to comply with the latest Google policies.
- iOS **MUST** use a minimum `SupportedOSPlatformVersion` of `12.2` in both the `csproj` and in the `info.plist` configuration for the project.
