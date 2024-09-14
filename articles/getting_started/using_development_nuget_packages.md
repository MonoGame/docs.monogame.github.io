# Using the Development Nuget Packages

When the MonoGame develop branch builds, it publishes development NuGet packages to the 
MonoGame NuGet Feed on GitHub. If you want to test a new feature or just be on the 
very latest code you can use this Feed to do that. 

## Adding a NuGet Source 

Create a `NuGet.config` in the root or top level directory of your project.
NuGet will automattically walk up the directory tree to find `NuGet.config` files. 
Then add the following content.

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <packageSources>
        <add key="MonoGameGitHub" value="https://nuget.pkg.github.com/MonoGame/index.json" />
    </packageSources>
</configuration>
```

Next, again in the root or top level directory, create a `Directory.Build.props` file and add the following content.

```xml
<Project>
    <PropertyGroup>
        <MonoGamePackageVersion>1.0.0.1233-develop</MonoGamePackageVersion>
    </PropertyGroup>
</Project>
```
`Directory.Build.props` is an MSBuild file which will be imported by all projects in your game. 
It is like a file that contains global variables. In this case the version of MonoGame we want to use.

To find out the latest version number, you can look at one of the packages at [https://github.com/orgs/MonoGame/packages?repo_name=MonoGame](https://github.com/orgs/MonoGame/packages?repo_name=MonoGame). Or to get the information from the GitHub feed, you can run the following command.

```CLI
nuget search "MonoGame.Framework" -PreRelease -Source MonoGameGitHub
```

This will give you the following output 

```
====================
Source: MonoGameGitHub
--------------------
> MonoGame.Framework.Android | 1.0.0.1278-develop | Downloads: 164
  The MonoGame runtime for Android.
--------------------
> MonoGame.Framework.Content.Pipeline | 1.0.0.1278-develop | Downloads: 69
  The Monogame Content Pipeline for Windows, Mac and Linux is used to compile raw content to xnb files...
--------------------
> MonoGame.Framework.DesktopGL | 1.0.0.1278-develop | Downloads: 337
  The MonoGame runtime supporting Windows, Linux and macOS using SDL2 and OpenGL.
--------------------
> MonoGame.Framework.Native | 1.0.0.1278-develop | Downloads: 1
  The MonoGame Native platform.
--------------------
> MonoGame.Framework.WindowsDX | 1.0.0.1278-develop | Downloads: 76
  The MonoGame runtime for Windows using DirectX API's.
--------------------
> MonoGame.Framework.WindowsUniversal | 3.8.1.1128-develop | Downloads: 54
  The MonoGame runtime for UWP (Universal Windows Platform) which supports Windows 10 and Xbox One.
--------------------
> MonoGame.Framework.iOS | 1.0.0.1278-develop | Downloads: 153
  The MonoGame runtime for iOS amd iPadOS.
--------------------
```

The version number you want to use is listed in the output.

> [!IMPORTANT]
> As packages are published, the version number will always change. Unfortunately, due to limitations in the way NuGet works, we cannot 
> use a wildcard with a pre-release package (so you cannot do `1.0.0.*-develop`). So this is the best way to find the latest verion you want to use.

Next update all the `PackageReference` entries in your csproj's which use MonoGame to use `$(MonoGamePackageVersion)` MSBuild property.
For example:

```xml
<ItemGroup>
    <PackageReference Include="MonoGame.Framework.DesktopGL" Version="$(MonoGamePackageVersion)" />
    <PackageReference Include="MonoGame.Content.Builder.Task" Version="$(MonoGamePackageVersion)" />
</ItemGroup>
```

If you try to build now you will get an error. This is because the NuGet feeds on GitHub are not public. You need
to be a valid GitHub user to use them. 

## Authentication

You need to create a Personal Access Token (PAT) on your GitHub account in order to use the NuGet feed.
See the following documentation on how to create your PAT. 
[managing-your-personal-access-tokens](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens)

> [!IMPORTANT]
> You need to create a "PAT (Classic)" token in order for it to work with the Nuget feed. See [creating-a-personal-access-token-classic](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens#creating-a-personal-access-token-classic) for details.

Once you have your PAT, you can create a new `NuGet.config` file in the directory ABOVE your game project directory.
To be clear, this file should NOT be in your source tree. It should be outside of any directory which is under source control.

```
 Projects
      NuGet.confing <-- THIS IS WHERE YOU PUT THE FILE.
      MyGame
        .git
        Directory.Build.props
        NuGet.config
        MyGame.DesktopGL
          MyGame.DesktopGL.csproj

```

> [!IMPORTANT]
> Do Not... DO NOT place a NuGet.config file with valid `packageSourceCredentials` in your source control. 

The contents of the file are as follows, replace `%GITHUB_USER%` with your GitHub username and the `%GITHUB_TOKEN%` with your token.

```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <packageSourceCredentials>
        <MonoGameGitHub>
            <add key="Username" value="%GITHUB_USER%" />
            <add key="ClearTextPassword" value="%GITHUB_TOKEN%" />
        </MonoGameGitHub>
    </packageSourceCredentials>
</configuration>
```

The really good thing about placing these credentials outside of source control is that they are safe. But also any
new projects you create under that folder can also make use of these createntials. So it is a good idea to keep them in one place.
For more information, you can read [consuming-packages-authenticated-feeds](https://learn.microsoft.com/en-us/nuget/consume-packages/consuming-packages-authenticated-feeds#credentials-in-nugetconfig-files).
