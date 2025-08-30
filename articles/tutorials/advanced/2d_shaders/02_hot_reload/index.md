---
title: "Chapter 02: Hot Reload"
description: "Setup workflows to reload shaders without restarting the game"
---

Before we can dive in and start writing shader effects, we should first take a moment to focus on our development environment.

In this chapter, we will build a "hot-reload" system that will automatically detect changes to our shader files, recompile them, and load them into our running game on the fly. This is a huge time-saver that will let us iterate and experiment with our visual effects much more quickly. Let's get started!

If you're following along with code, here is the code from the end of the previous tutorial series, [Starting Code](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Tutorials/learn-monogame-2d/src/24-Shaders/) 

## Compiling Shaders

Our snake-like game already has a shader effect and we can use it to validate the _hot-reload_ system as we develop it. By default, MonoGame compiles the shader from a `.fx` file into a `.xnb` file when the game is compiled. Then, the built `.xnb` file is copied into the game's build directory so it is available to load when the game starts. Our goal is to recompile the `.fx` file, and copy the resulting `.xnb` file whenever the shader is changed. Luckily, we can re-use a lot of the existing capabilities of MonoGame. 

### MSBuild Targets

The existing automatic shader compilation is happening because the `DungeonSlime.csproj` file is referencing the `MonoGame.Content.Builder.Task` Nuget package.
[!code-xml[](./snippets/DungeonSlime.csproj?highlight=4)]

Nuget packages can add custom build behaviours and the `MonoGame.Content.Builder.Task` package is adding a step to the game's build that runs the MonoGame Content Builder tool. These sorts of build extensions use a conventional `.prop` and `.target` file system. If you are interested, you can learn more about how Nuget packages may extend MSBuild systems on Microsoft's [documentation website](https://learn.microsoft.com/en-us/visualstudio/msbuild/msbuild?view=vs-2022). For reference, [this](https://github.com/MonoGame/MonoGame/blob/develop/Tools/MonoGame.Content.Builder.Task/MonoGame.Content.Builder.Task.targets#L172) is the `.targets` file for the `MonoGame.Content.Builder.Task`.


This line defines a new MSBuild step, called `IncludeContent`.
```xml
  <Target
    Name="IncludeContent"
    DependsOnTargets="RunContentBuilder"
    Condition="('$(EnableMGCBItems)' == 'true' OR '@(MonoGameContentReference)' != '') And '$(DesignTimeBuild)' != 'true'"
    Outputs="%(ExtraContent.RecursiveDir)%(ExtraContent.Filename)%(ExtraContent.Extension)"
    BeforeTargets="BeforeCompile"
    AfterTargets="ResolveProjectReferences">
```

You can learn more about what all the attributes do in MSBuild. Of particular note, the `BeforeTargets` attribute causes MSBuild to run the `IncludeContent` target before the `BeforeCompile` target is run, which is a standard target in the dotnet sdk. 

The `IncludeContent` target can run manually by invoking `dotnet build` by hand. In VSCode, open the embedded terminal to the _DungeonSlime_ project folder, and run the following command. 

```sh
dotnet build -t:IncludeContent
```

You should see log output indicating that the content for the _DungeonSlime_ game was built. 

### Dotnet Watch

There is a tool called [`dotnet watch`](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-watch) that comes with the standard installation of `dotnet`. Normally, `dotnet watch` is used to watch for changes to `.cs` code files, recompile, and reload those changes into a program without restarting the program. You can try out `dotnet watch`'s normal behaviour by opening VSCode's embedded terminal to the _DungeonSlime_ project, and running the following command. The game should start normally.

```sh
dotnet watch
```

> [!Tip]
> Use the `ctrl` + `c` key at the same time to quit the `dotnet watch` terminal process.

Then, comment out the `Clear()` function call in the title screen's `Draw()` method. Save the file, and you should see the title screen immediately stop clearing the background on each frame. If you restore the line and save again, the scene will start clearing the background again.

```csharp
public override void Draw(GameTime gameTime)  
{  
    //Core.GraphicsDevice.Clear(new Color(32, 40, 78, 255));
```


In our case, we don't want to recompile `.cs` files, but rather `.fx` files. First, `dotnet watch` can be configured to execute any MSBuild target rather than a recompile code. The following command uses the existing target provided by the `MonoGame.Content.Builder.Task`.

> [!Tip]
> All arguments passed after the `--` characters are passed to the `build` command itself, not `dotnet watch`.

```sh
dotnet watch build -- --target:IncludeContent
```

Now, when you change a _`.fx`_ file, all of the content files are rebuilt into `.xnb` files.

However, the `.xnb` files are not being copied from the `Content/bin` folder to _DungeonSlime_'s runtime folder. The `.xnb` files are only copied during the full MSBuild of the game. The `IncludeContent` target on its own doesn't have all the context it needs to know how to copy the files in the final game project. To solve this, we need to introduce a new `<Target>` that copies the final `.xnb` files into _DungeonSlime_'s runtime folder. 

The existing `MonoGame.Content.Builder.Task` system knows what the files are, so we can re-use properties defined in the MonoGame package.
Add this to your `.csproj` file.

```xml
<Target Name="BuildAndCopyContent" DependsOnTargets="IncludeContent">  
  <Message Text="Rebuilding Content..." Importance="High"/>  
  <Copy
      SourceFiles="%(ExtraContent.Identity)"  
      DestinationFiles="$(OutDir)%(ExtraContent.ContentDir)%(ExtraContent.RecursiveDir)%(ExtraContent.Filename)%(ExtraContent.Extension)"
      SkipUnchangedFiles="true"  
  />  
</Target>
```

Now, instead of calling the `IncludeContent` target directly, change your terminal command to invoke the new `BuildAndCopyContent` target.

```sh
dotnet watch build -- --target:BuildAndCopyContent
```

If you delete the `DungeonSlime/bin/Debug/net8.0/Content` folder, make an edit to a `.cs` file and save, you should see the `DungeonSlime/bin/Debug/net8.0/Content` folder be restored.

The next step is to only invoke the target when `.fx` files are edited instead of `.cs` files. These settings can be configured with custom MSBuild item configurations. Open the `DungeonSlime.csproj` file and add this `<ItemGroup>` to specify configuration settings.

```xml
<ItemGroup>  
  <!-- Adds .fx files to the `dotnet watch`'s file scope -->  
  <Watch Include="Content/**/*.fx;"/>  
  <!-- Removes the .cs files from `dotnet watch`'s compile scope -->  
  <Compile Update="**/*.cs" Watch="false" />  
</ItemGroup>
```

Now when you re-run the command from earlier, it will only run the `IncludeContent` target when `.fx` files have been changed. All edits to `.cs` files are ignored. Try adding a blank line to the `grayscaleEffect.fx` file, and notice the `dotnet watch` process re-build the content.

However, if you would like to use `dotnet watch` for anything else in your workflow, then the configuration settings are too aggressive, because they will be applied _all_ invocations of `dotnet watch`. The `ItemGroup` can be optionally included when a certain condition is met. We will introduce a new MSBuild property called `OnlyWatchContentFiles`.

```xml
<ItemGroup Condition="'$(OnlyWatchContentFiles)'=='true'">  
  <!-- Adds .fx files to the `dotnet watch`'s file scope -->  
  <Watch Include="Content/**/*.fx;"/>  
  <!-- Removes the .cs files from `dotnet watch`'s compile scope -->  
  <Compile Update="**/*.cs" Watch="false" />  
</ItemGroup>
```

And now when `dotnet watch` is invoked, it needs to specify the new parameter.

```sh
dotnet watch build --property OnlyWatchContentFiles=true -- --target:BuildAndCopyContent
```

The command is getting long and hard to type, and if we want to add more configuration, it will likely get even longer. Instead of invoking `dotnet watch` directly, it can be run as a new `<Target>` MSBuild step. Add this to your `DungeonSlime.csproj` file. 

```xml
<Target Name="WatchContent">  
  <Exec Command="dotnet watch build --property OnlyWatchContentFiles=true -- --target:BuildAndCopyContent"/>  
</Target>
```

And now from the terminal, run the following `dotnet build` command. 
```sh
dotnet build -t:WatchContent
```


We now have a way to dynamically recompile shaders on file changes and copy the `.xnb` files into the game folder! There are a few final adjustments to make to the configuration. 

First, you may notice some odd characters in the log output after putting the `dotnet watch` inside the `WatchContent` target. This is because there are _emoji_ characters in the standard `dotnet watch` log stream, and some terminals don't understand how to display those, especially when streamed between `dotnet build`. To disable the _emoji_ characters, a `DOTNET_WATCH_SUPPRESS_EMOJIS` environment variable needs to be set.

```xml
<Target Name="WatchContent">  
  <Exec Command="dotnet watch build --property OnlyWatchContentFiles=true -- --target:BuildAndCopyContent"  
        EnvironmentVariables="DOTNET_WATCH_SUPPRESS_EMOJIS=1"/>  
</Target>
```

Next, the `IncludeContent` target is doing a little too much work for our use case. It is trying to make sure the MonoGame Content Builder tools are installed. For our use case, we can opt out of that check by disabling the existing `AutoRestoreMGCBTool` MSBuild property. It also makes sense to pass `--restore:false` as well so that Nuget packages are not restored on each content file change.

```xml
<Target Name="WatchContent">  
  <Exec Command="dotnet watch build --property OnlyWatchContentFiles=true --property AutoRestoreMGCBTool=false  -- --target:BuildAndCopyContent --restore:false"  
        EnvironmentVariables="DOTNET_WATCH_SUPPRESS_EMOJIS=1"/>  
</Target>
```

To experiment with the system, re-run the following command, 

```sh
dotnet build -t:WatchContent
```

And then cause some sort of compiler-error in the `grayscaleEffect.fx` file, such as adding the line, `"tunafish"` to the top of the file. When you save it, you should see the terminal spit out an error containing information about the compilation failure, 

```
 error X3000: unrecognized identifier 'tunafish'
```

Remove the `"tunafish"` line and save again, and the watch program should log some lines similar to these, 
```
  dotnet watch : Started
    C:/proj/MonoGame.Samples/Tutorials/2dShaders/src/02-Hot-Reload-System/DungeonSlime/Content/effects/grayscaleEffect.fx
    Copying Content...
  
  Build succeeded.
      0 Warning(s)
      0 Error(s)

  Time Elapsed 00:00:01.40
  dotnet watch : Exited
  dotnet watch : Waiting for a file to change before restarting dotnet...

```


## Reload shaders in-game

Now anytime the `.fx` files are modified, they will be recompiled and copied into the game's runtime folder. However, the game itself doesn't know to _reload_ the `Effect` instances. In this section, we will create a utility over the `ContentManager` to respond to these dynamic file updates. 

It is important to make a distinction between assets the game _expects_ to be reloaded and assets that the game does _not_ care about reloading. This tutorial will demonstrate how to create an explicit system where individual assets opt _into_ being _hot reloadable_, rather than creating a system where all assets automatically handle dynamic reloading. 

### Extending Content Manager

Currently, the `grayscaleEffect.fx` is being loaded in the `GameScene` 's `LoadContent()` method like this, 

```csharp
// Load the grayscale effect  
_grayscaleEffect = Content.Load<Effect>("effects/grayscaleEffect");
```

The `.Load()` function in the existing `ContentManager` is almost sufficient for our needs, but it returns a regular `Effect`, which has no understanding of the dynamic nature of the new content workflow. 

Create a new _Content_ folder within the _MonoGameLibrary_ project, add a new file named `ContentManagerExtensions.cs`, and add the following code for the foundation of the new system.

```csharp
using System;  
using System.Diagnostics;  
using System.IO;  
using Microsoft.Xna.Framework.Content;  
using Microsoft.Xna.Framework.Graphics;  
using MonoGameLibrary.Graphics;  

namespace MonoGameLibrary.Content;  
  
public static class ContentManagerExtensions  
{

}
```

Now, you can create an [extension method](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) for the existing `MonoGame`'s `ContentManager` class, 

```csharp
public static T Watch<T>(this ContentManager manager, string assetName)  
{  
    var asset = manager.Load<T>(assetName);
    return asset;
}
```

This new `Watch` function is an opportunity to enhance how content is loaded. Use this new function to load the `_greyscaleEffect` effect in the `GameScene`. 

```csharp
_grayscaleEffect = Content.Watch<Effect>("effects/grayscaleEffect");
```


### The `WatchedAsset` class

The new system will need to keep track of additional information for each asset that we plan to be _hot-reloadable_. The new data will live in a new class, `WatchedAsset<T>`. 

Add a new file named `WatchedAsset.cs` in the _MonoGameLibrary/Content_ folder.

```csharp
using System;
using Microsoft.Xna.Framework.Content;

namespace MonoGameLibrary.Content;

public class WatchedAsset<T>
{
    /// <summary>
    /// The latest version of the asset.
    /// </summary>
    public T Asset { get; set; }
    
    /// <summary>
    /// The last time the <see cref="Asset"/> was loaded into memory.
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }
    
    /// <summary>
    /// The name of the <see cref="Asset"/>. This is the name used to load the asset from disk. 
    /// </summary>
    public string AssetName { get; init; }
}
```

The new `Watch` method should return a `WatchedAsset<T>` instead of the direct `Effect`.
```csharp
public static WatchedAsset<T> Watch<T>(this ContentManager manager, string assetName)
{
    var asset = manager.Load<T>(assetName);
    return new WatchedAsset<T>
    {
        AssetName = assetName,
        Asset = asset,
        UpdatedAt = DateTimeOffset.Now,
    };
}
```

This will require that the type of `_greyscaleEffect` change to a `WatchedAsset<Effect>` instead of simply an `Effect`. 
```csharp
// The grayscale shader effect.  
private WatchedAsset<Effect> _grayscaleEffect;
```

And that will cause a few compilation errors where the `_greyscaleEffect` is used throughout the rest of the `GameScene`. 
The compile errors appear because `_greyscaleEffect` used to be an `Effect`, but now the `Effect` is actually available as `_grayscaleEffect.Asset`. 

### Reload Extension

It is time to write the extension method that the game code will use to opt into reloading an asset. From the earlier section, anytime a `.fx` file is updated, the compiled `.xnb` file will be copied into the game's runtime folder. The operating system will keep track of the last time the `.xnb` file was written, and we can leverage that information with the `WatchedAsset<T>.UpdatedAt` property to understand if the `.xnb` file is _newer_ than the current loaded `Effect`. 

This method will take a `WatchedAsset<T>` and update the inner `Asset` property _if_ the `.xnb` file is newer. The method returns `true` when the asset is reloaded, which will be useful later. 

```csharp
public static bool TryRefresh<T>(this ContentManager manager, WatchedAsset<T> watchedAsset)
{
    oldAsset = default;

    // get the same path that the ContentManager would use to load the asset
    var path = Path.Combine(manager.RootDirectory, watchedAsset.AssetName) + ".xnb";

    // ask the operating system when the file was last written.
    var lastWriteTime = File.GetLastWriteTime(path);

    // when the file's write time is less recent than the asset's latest read time, 
    //  then the asset does not need to be reloaded.
    if (lastWriteTime <= watchedAsset.UpdatedAt)
    {
        return false;
    }

    // clear the old asset to avoid leaking
    manager.UnloadAsset(watchedAsset.AssetName);

    // load the new asset and update the latest read time
    watchedAsset.Asset = manager.Load<T>(watchedAsset.AssetName);
    watchedAsset.UpdatedAt = lastWriteTime;
    
    return true;
}    
```


At the top of the `GameScene.Update()` method, add the following line to opt into reloading the `_grayscaleEffect` asset.

```csharp
// Update the grayscale effect if it was changed  
Content.TryRefresh(_grayscaleEffect);
```

Now, when the `grayscaleEffect.fx` file is modified, the `dotnet watch` system will compile it to an `.xnb` file, copy it to the game's runtime folder, and then in the `Update()` loop, the `TryRefresh()` method will load the new effect and the new shader code will be running live in the game. Try it out by adding this line right before the `return` statement in the `grayscaleEffect.fx` file.

```hlsl
finalColor *= float3(1, 0, 0);
```

This video shows the effect changing.

![Figure 2-1: The reload system is working ](./videos/shader-reload.mp4)


## Final Touches

The _hot reload_ system is almost done. There are a few quality of life features to finish up.

### File Locking
There is an edge case bug in the `TryRefresh()` function that checks if the `.xnb` file is more recent than the in-memory asset. It is possible that the MonoGame Content Builder may be _actively_ writing the `.xnb` file when the function runs. The game will fail to read the file while it is being written. The solution to this problem is to simply wait and try loading the `.xnb` file the next frame. The trick is that C# does not have a standard way to check if a file is currently locked. The best way to check is to simply try and open the file, and if an `Exception` is thrown, assume the file is not readable. 

Add this function to the `ContentManagerExtensions` class.
```csharp
private static bool IsFileLocked(string path)
{
    try
    {
        using FileStream _ = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        // File is not locked
        return false;
    }
    catch (IOException)
    {
        // File is locked or inaccessible
        return true;
    }
}
```

And then modify the `TryRefresh` function by returning early if the file is locked. 
```csharp
if (IsFileLocked(path)) return false; // wait for the file to not be locked.
```

### Access the old Asset on reload
Anytime a new asset is loaded, the old asset is unloaded from the `ContentManager`. However, it will be helpful to be able to access in-memory data about the old asset version. For shaders, there is metadata and runtime configuration that should be applied to the new version. This will be more relevant in the next chapter. For now, modify the `TryRefresh` function to contain `out` parameter of the old asset. Change the method signature to the following,

```csharp
public static bool TryRefresh<T>(this ContentManager manager, WatchedAsset<T> watchedAsset, out T oldAsset)
```

And before updating the `watchedAsset.Asset`, set the `oldAsset` as the previous in-memory asset.
```csharp
oldAsset = watchedAsset.Asset;  
watchedAsset.Asset = manager.Load<T>(watchedAsset.AssetName);
```

Don't forget that the place where the `grayscaleEffect` calls the `TryRefresh()` function will need to include a no-op out variable.
```csharp
Content.TryRefresh(_grayscaleEffect, out _);
```
### Refresh Convenience Function 
Finally, we need to address a subtle usability bug in the existing code. The `TryRefresh` function may `Unload` an asset if a new version is loaded. However, it isn't obvious that the `ContentManager` instance doing the `Unload` operation is the same `ContentManager` instance that loaded the original asset in the first place. To solve this, add a `ContentManager` property to the `WatchedAsset<T>` class so that the asset itself knows which `ContentManager` is responsible for unloading old versions. 

```csharp
/// <summary>  
/// The <see cref="ContentManager"/> instance that loaded the asset.  
/// </summary>  
public ContentManager Owner { get; init; }
```

Adjust the `WatchAsset` function fill in this new property,
```csharp
public static WatchedAsset<T> Watch<T>(this ContentManager manager, string assetName)
{
    var asset = manager.Load<T>(assetName);
    return new WatchedAsset<T>
    {
        AssetName = assetName,
        Asset = asset,
        UpdatedAt = DateTimeOffset.Now,
        Owner = manager
    };
}
```

Then, in the `TryRefresh` function, a small assertion can be added to validate the `ContentManager` is the same.
```csharp
if (manager != watchedAsset.Owner)  
    throw new ArgumentException($"Used the wrong ContentManager to refresh {watchedAsset.AssetName}");
```

It is annoying to need use the `ContentManager` directly to call `TryRefresh` in the game loop. It would be easier to rely on the new `Owner` property, now. Add this method to the `WatchedAsset<T>` class.

```csharp
public bool TryRefresh(out T oldAsset)  
{  
    return Owner.TryRefresh(this, out oldAsset);  
}
```

Finally, update the `GameScene` to use the new convenience method to refresh the `_grayscaleEffect` .
```csharp
_grayscaleEffect.TryRefresh(out _);
```

## Conclusion

And with that, we have a powerful hot-reload system in place! In this chapter, you accomplished the following:

- Configured `dotnet watch` to monitor your `.fx` shader files.
- Created a custom MSBuild `<Target>` to automatically recompile and copy your built shaders.
- Wrote a C# wrapper class, `WatchedAsset<T>`, to track asset file changes.
- Extended `ContentManager` with a `TryRefresh` method to load new assets into the running game.

This new workflow is going to make the rest of our journey much more fun and productive. In the next chapter, we'll build on this foundation by creating a `Material` class to help us organize and safely interact with our shaders.

You can find the complete code sample for this chapter, [here](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Tutorials/2dShaders/src/02-Hot-Reload-System/). 

Continue to the next chapter, [Chapter 03: The Material Class](../03_the_material_class/index.md)