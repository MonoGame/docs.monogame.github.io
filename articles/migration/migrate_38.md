---
title: Migrating from 3.8.0
description: A guide on migrating a MonoGame v3.8.0 project to the current version of MonoGame.
---

# Migrating from 3.8.0

Migrating from 3.8.0 should be straightforward for most platforms.

The major difference is that 3.8.1 now requires .NET 6 and Visual Studio 2022. You can follow the [environment setup tutorial](../getting_started/index.md) to make sure that you are not missing any components.

The MGCB Editor is no longer a global .NET tool and we recommend that you use the new Visual Studio 2022 extension which helps with accessing it without the need of CLI commands.


> [!NOTE]
> It is also recommended that you uninstall the older global versions of the .NET tools as described below.

## WindowsDX, DesktopGL, and UWP

Upgrading from 3.8.0 should be as straightforward as upgrading your ```TargetFramework``` and MonoGame version.

Edit your csproj file to change your ```TargetFramework```:

```xml
<TargetFramework>net6.0</TargetFramework>
```

Then edit your MonoGame ```PackageReference``` to point to 3.8.1:

```xml
<PackageReference Include="MonoGame.Framework.{Platform}" Version="3.8.1.*" />
<PackageReference Include="MonoGame.Content.Builder.Task" Version="3.8.1.*" />
```

### Accessing MGCB and MCGB Editor without a global tool

The MGCB Editor is no longer a .NET global tool, and does not need to be installed or registered. When migrating from 3.8.0, it is recommended that you **uninstall** the global versions of the tools. You can accomplish that with these commands:

```
dotnet tool uninstall dotnet-mgcb -g
dotnet tool uninstall dotnet-2mgfx -g
dotnet tool uninstall dotnet-mgcb-editor -g
```

::: tip
**Do not** run the ``` dotnet tool install ``` on 3.8.1, as it would break 3.8.1.
:::

You will also need to setup a dotnet-tools configuration file. 

- Next to your ```.csproj```create a folder named ```.config```
- Add a file within the folder named ```dotnet-tools.json``` with the following content:

```json
{
  "version": 1,
  "isRoot": true,
  "tools": {
    "dotnet-mgcb": {
      "version": "3.8.1.303",
      "commands": [
        "mgcb"
      ]
    },
    "dotnet-mgcb-editor": {
      "version": "3.8.1.303",
      "commands": [
        "mgcb-editor"
      ]
    },
    "dotnet-mgcb-editor-linux": {
      "version": "3.8.1.303",
      "commands": [
        "mgcb-editor-linux"
      ]
    },
    "dotnet-mgcb-editor-windows": {
      "version": "3.8.1.303",
      "commands": [
        "mgcb-editor-windows"
      ]
    },
    "dotnet-mgcb-editor-mac": {
      "version": "3.8.1.303",
      "commands": [
        "mgcb-editor-mac"
      ]
    }
  }
}
```

Please note that you cannot use the ```3.8.1.*``` wildcard in the ```dotnet-tools.json``` file (tool versions have to be fully qualified). We strongly recommand that the versions match the MonoGame version referenced in your ```.csproj``` (if you are using the ```*``` wildcard, make sure that they do not end up mismatching if the nugets are updated without you noticing).

You will also need to add this to your ```.csproj```:

```xml
  <Target Name="RestoreDotnetTools" BeforeTargets="Restore">
    <Message Text="Restoring dotnet tools" Importance="High" />
    <Exec Command="dotnet tool restore" />
  </Target>
```

With these changes, .NET will automatically install the MGCB Editor for you when launching Visual Studio 2022 (if you want to install it manually and skip adding the Target, run ```dotnet tool restore``` within the project directory).

Then, if you installed the Visual Studio extension, you should also be able to just double-click an ```.mgcb``` file to open the MGCB Editor. You can also open the MGCB Editor with the CLI via ```dotnet mgcb-editor``` when executed from within the project directory.

This new configuration has the advantage of allowing to have per-project versions of MGCB and its Editor (instead of per-machine like a global tool).

## iOS/iPadOS, and Android

.NET 6 introduced breaking changes in how csproj are defined for iOS/iPadOS and Android. We recommand that you create new projects using the 3.8.1 templates and that you copy over your project files there.
