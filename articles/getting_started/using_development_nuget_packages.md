# Using the Development Nuget Packages

When the MonoGame develop branch builds it publishes development NuGet packages to the 
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

Next again in the root or top level directory create a `Directory.Build.props` file and add the following content.

```xml
<Project>
    <PropertyGroup>
        <MonoGamePackageVersion>1.0.0.1233-develop</MonoGamePackageVersion>
    </PropertyGroup>
</Project>
```

`Directory.Build.props` is a MSBuild file which will be imported by all projects in your game. 
Its like a file to contain global variables. In this case the version of MonoGame we want to use.

Next update all the `PackageReference` entires in your csprojs which use MonoGame to use `$(MonoGamePackageVersion)` MSBuild property.
For example

```xml
<ItemGroup>
    <PackageReference Include="MonoGame.Framework.DesktopGL" Version="$(MonoGamePackageVersion)" />
    <PackageReference Include="MonoGame.Content.Builder.Task" Version="$(MonoGamePackageVersion)" />
  </ItemGroup>
```

If you try to build now you will get an error. This is because the NuGet feeds on GitHub are not open. You need
to be a valid GitHub user to use them. 

## Authentication

You need to create an Personal Access Token (PAT) on your GitHub account in order to use the NuGet feed.
See the following documentation on how to create your PAT. 
(https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens)[https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens]
Note: You need to create a "PAT (Classic)" token in order for it to work with the Nuget feed. See (https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens#creating-a-personal-access-token-classic)[https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens#creating-a-personal-access-token-classic] for details.

Once you have your PAT you can create a new `NuGet.config` file in the directory ABOVE your game project directory.
To be clean this file should NOT be in your source tree. It should be outside of any directory which is under source control.

```
 Projects
      NuGet.confing <-- THIS IS WHERE YOU PUT THE FILE.
      MyGame
        NuGet.config
        MyGame.DesktopGL
          MyGame.DesktopGL.csproj

```
Do Not... DO NOT place this NuGet.config file in your source control. 

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

The really good thing about placing these credentials outside of source control is they are safe, but also any
new projects you create under that folder can also make use of these createntials. So its a good idea to keep them in one place. 
you
