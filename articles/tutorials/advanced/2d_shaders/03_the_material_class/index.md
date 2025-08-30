Our hot-reload system is working, which is great! 

In this chapter, we will create a small wrapper class, called the `Material`, that will handle shader parameters, hot-reload, and serve as a baseline for future additions.

If you're following along with code, here is the code from the end of the [previous chapter](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Tutorials/2dShaders/src/02-Hot-Reload-System/).

## The Material Class

A `Material` represents a compiled `Effect` _and_ the runtime configuration for the `Effect`. For example, the `_grayscaleEffect` shader has a single property called `Saturation`, and the _value_ of that property is essential to the existence of the `_grayscaleEffect`. We will create a class called `Material` that manages all of our shader related metadata. 

Start by creating a new file in the _MonoGameLibrary/Graphics_ folder called `Material.cs`, 
```csharp
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Content;

namespace MonoGameLibrary.Graphics;

public class Material
{
    /// <summary>
    /// The hot-reloadable asset that this material is using
    /// </summary>
    public WatchedAsset<Effect> Asset;
    
    /// <summary>
    /// The currently loaded Effect that this material is using
    /// </summary>
    public Effect Effect => Asset.Asset;

    public Material(WatchedAsset<Effect> asset)
    {
        Asset = asset;
    }
}
```

In order to create instances of the `Material`, go ahead and create an additional method in the `ContentManagerExtensions` file, 
```csharp
/// <summary>  
/// Load an Effect into the <see cref="Material"/> wrapper class  
/// </summary>  
/// <param name="manager"></param>  
/// <param name="assetName"></param>  
/// <returns></returns>  
public static Material WatchMaterial(this ContentManager manager, string assetName)  
{  
    return new Material(manager.Watch<Effect>(assetName));  
}
```

And now, in the `GameScene`, adjust the `_grayscaleEffect` to use the new `Material` class. 
```csharp
// The grayscale shader effect.  
private Material _grayscaleEffect;
```

This will cause many compilation errors, because all the places where `_grayScaleEffect` is referenced need to use the new `Material` type.
When instantiating the `_grayscaleEffect`, use the new method,
```csharp
// Load the grayscale effect  
_grayscaleEffect = Content.WatchMaterial("effects/grayscaleEffect");
```

When checking if the asset needs to be reloaded for hot-reload, use the `.Asset` sub property,
```csharp
// Update the grayscale effect if it was changed  
_grayscaleEffect.Asset.TryRefresh(out _);
```

And in the `Draw()` method, use the `.Effect` shortcut property,
```csharp
// We are in a game over state, so apply the saturation parameter.  
_grayscaleEffect.Effect.Parameters["Saturation"].SetValue(_saturation);  
  
// And begin the sprite batch using the grayscale effect.  
Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, effect: _grayscaleEffect.Effect);
```

### Setting Shader Parameters

You already saw how to set a shader property by using the `Saturation` value in the `_grayscaleEffect` shader. However, as you develop shaders in MonoGame, you will eventually _accidentally_ try to set a shader property that doesn't exist in your shader. When this happens, the code will throw a `NullReferenceException` rather than fail silently. For example, if you tried to add this line of the code to the `Update` loop, 

```csharp
_grayscaleEffect.Effect.Parameters["DoesNotExist"].SetValue(0);
```

You'll see this type of `NullReference` error,
```
System.NullReferenceException: Object reference not set to an instance of an object.
```

> [!Caution]
> Don't actually add the `DoesNotExist` sample, because it will break your code.

On its own, this wouldn't be too difficult to accept. However, MonoGame's shader compiler will aggressively remove properties that aren't actually being _used_ in your shader code. Even if you wrote a shader that had a `DoesNotExist` property, if it wasn't being used to compute the return value of the shader, it will be removed. The compiler is good at optimizing away unused variables. For example, in the `grayscaleEffect.fx` file, change the last few lines of the `MainPS` function to the following,
```hlsl
// overwrite all existing operations and set the final color to white  
finalColor.rgb = 1;  
  
// Return the final color with the original alpha value  
return float4(finalColor, color.a);
```

If you run the game and enter the `GameScene`, you will see a `NullReferenceException` (and the game will hard-crash). The `Saturation` shader parameter no longer exists in the shader, so when the `Draw()` method tries to _set_ it, the game crashes. 

The aggressive optimization is good for your game's performance, but when combined with the hot-reload system, it will lead to unexpected bugs. As you iterate on shader code, it is likely that at some point a shader parameter will be optimized out of the compiled shader. The hot-reload system will automatically load the newly compiled shader, and if the C# code attempts to set the previously available parameter, the game may crash.

To solve this problem, the `Material` class can encapsulate setting shader properties and handle the potential error scenario. The `Effect.Parameters` variable is an instance of the  `EffectParameterCollection` class. Here is the class signature and fields. 

```csharp
public class EffectParameterCollection : IEnumerable<EffectParameter>, IEnumerable  
{  
  internal static readonly EffectParameterCollection Empty = new EffectParameterCollection(new EffectParameter[0]);  
  private readonly EffectParameter[] _parameters;  
  private readonly Dictionary<string, int> _indexLookup;
  // the rest of the class has been omitted 
```

The `_indexLookup` is a `Dictionary<string, int>` containing a mapping of property _name_ to parameter. The `Dictionary` class has methods for checking if a given property name exists, but unfortunately we cannot access it due to the `private` access modifier. Luckily, the entire `EffectParameterCollection` inherits from `IEnumerable`, so we can use the existing dotnet utilities to convert the entire structure into a `Dictionary`. Once we have the parameters in a `Dictionary` structure, we'll be able to check what parameters exist _before_ trying to access them, thus avoiding potential `NullReferenceExceptions`. 

Add this new property to the `Material` class, 
```csharp
/// <summary>  
/// A cached version of the parameters available in the shader  
/// </summary>  
public Dictionary<string, EffectParameter> ParameterMap;
```

And now you can add the following method that will convert the `EffectParameterCollection` into the new `Dictionary` property. 
```csharp
/// <summary>  
/// Rebuild the <see cref="ParameterMap"/> based on the current parameters available in the effect instance  
/// </summary>  
public void UpdateParameterCache()  
{  
    ParameterMap = Effect.Parameters.ToDictionary(p => p.Name);  
}
```

Don't forget to invoke this method during the constructor of the `Material`, 
```csharp
public Material(WatchedAsset<Effect> asset)  
{  
    Asset = asset;  
    UpdateParameterCache();  
}
```

With the new `ParameterMap` property, create a helper function that checks if a given parameter exists in the shader.
```csharp
/// <summary>  
/// Check if the given parameter name is available in the compiled shader code.  
/// Remember that a parameter will be optimized out of a shader if it is not being used  
/// in the shader's return value.  
/// </summary>  
/// <param name="name"></param>  
/// <param name="parameter"></param>  
/// <returns></returns>  
public bool TryGetParameter(string name, out EffectParameter parameter)  
{  
    return ParameterMap.TryGetValue(name, out parameter);  
}
```

Now you can create a helper method that sets a parameter value for the `Material`, but won't crash if the parameter doesn't actually exist.
```csharp
public void SetParameter(string name, float value)
{
    if (TryGetParameter(name, out var parameter))
    {
        parameter.SetValue(value);
    }
    else
    {
        Console.WriteLine($"Warning: cannot set parameter=[{name}] as it does not exist in the shader=[{Asset.AssetName}]");
    }
}
```

Instead of setting the `Saturation` for the `_grayscaleEffect` manually as before, change the `Draw()` code to use the new method,
```csharp
// We are in a game over state, so apply the saturation parameter.  
_grayscaleEffect.SetParameter("Saturation", _saturation);
```

To verify it is working, re-run the game, and instead of seeing a crash, you should see the `GameScene`'s gameover menu show a completely _white_ background. This is because the shader is setting the `finalColor` to `1`. Delete the following line from the shader, wait for the hot-reload system to kick in, and the game should return to normal.

![Figure 3-1: The game does not crash](./videos/shader-properties.mp4)


### Reloading Properties

When the hot-reload system loads a new compiled shader into the game's memory, the new shader doesn't have any of the existing shader parameter _values_ that the previous shader instance had. To demonstrate the problem, we will purposefully break the `_grayscaleEffect` a bit. For now, comment out the line the `GameScene`'s `Draw()` method,

```csharp
// _grayscaleEffect.SetParameter("Saturation", _saturation);
```

And instead, add the following line to the end of the `LoadContent()` method.
```chsharp
_grayscaleEffect.SetParameter("Saturation", 1);
```

The net outcome is that the `_grayscaleEffect` won't actually work for its designed purpose, but it should be always fully saturated with a value of `1`, and _never_ change. Run the game, enter the `GameScene`, and hit the pause button. Now, if you cause _any_ reload of the shader, the background of the `GameScene` will desaturate and turn grayscale. The newly compiled shader instance has no value for the `Saturation` parameter, and since `0` is the default value for numbers, it appears grayscale. 

To solve this problem, the `Material` class can encapsulate the handling of applying new hot-reload updates. Anytime a new shader is available to swap in, the `Material` class needs to handle re-applying the old shader parameters to the new instance. 
Add the following method to the `Material` class,

```csharp
public void Update()
{
    if (Asset.TryRefresh(out var oldAsset))
    {
        UpdateParameterCache();
        
        foreach (var oldParam in oldAsset.Parameters)
        {
            if (!TryGetParameter(oldParam.Name, out var newParam))
            {
                continue;
            }
            
            switch (oldParam.ParameterClass)
            {
                case EffectParameterClass.Scalar:
                    newParam.SetValue(oldParam.GetValueSingle());
                    break;
                default:
                    Console.WriteLine("Warning: shader reload system was not able to re-apply property. " +
                                      $"shader=[{Effect.Name}] " +
                                      $"property=[{oldParam.Name}] " +
                                      $"class=[{oldParam.ParameterClass}]");
                    break;
            }
        }
    }
}
```

And now instead of using the `TryRefresh()` method directly on the `_grayscaleEffect`, use the new method,
```csharp
// Update the grayscale effect if it was changed  
_grayscaleEffect.Update();
```

If you repeat the same test as before, the game won't become grayscale after a new shader is loaded. Once you have validated this, make sure to undo the changes in the `LoadContent()` and `Draw()` method, so that the `_grayscaleEffect` is still setting the `Saturation` value in the `Draw()` method.


#### Debug Builds

When the _DungeonSlime_ game is published, it would not make sense to run the new `Material.Update()` method, because no shaders would ever be hot-reloaded in a release build. We can strip the method from the game when it is being built for _Release_. Add the following attribute to the `Material.Update()` method, 

```csharp
[Conditional("DEBUG")]  
public void Update()
{
 // implementation left out for brevity
}
```

> [!Tip]
> Adding the [`[Conditional]`](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.conditionalattribute?view=net-9.0) attribute is optional. It won't seriously impact your game's performance one way or another.

### Supporting more Parameter Types

In the previous sections, we have only dealt with the `grayscaleEffect.fx` shader, and that shader only has a single shader parameter called `Saturation`. The `Saturation` parameter is a `float`, and so far, the `Material` class only handles `float` based parameters. However, shaders may have many more types of parameters. In this tutorial, we will add support for the following parameter types, and leave it as an exercise to the reader to add support for more.
- `Matrix`,
- `Vector2`,
- `Texture2D` 

Add the following methods to the `Material` class.
```csharp
public void SetParameter(string name, Matrix value)
{
    if (TryGetParameter(name, out var parameter))
    {
        parameter.SetValue(value);
    }
    else
    {
        Console.WriteLine($"Warning: cannot set shader parameter=[{name}] because it does not exist in the compiled shader=[{Asset.AssetName}]");
    }
}

public void SetParameter(string name, Vector2 value)
{
    if (TryGetParameter(name, out var parameter))
    {
        parameter.SetValue(value);
    }
    else
    {
        Console.WriteLine($"Warning: cannot set shader parameter=[{name}] because it does not exist in the compiled shader=[{Asset.AssetName}]");
    }
}

public void SetParameter(string name, Texture2D value)
{
    if (TryGetParameter(name, out var parameter))
    {
        parameter.SetValue(value);
    }
    else
    {
        Console.WriteLine($"Warning: cannot set shader parameter=[{name}] because it does not exist in the compiled shader=[{Asset.AssetName}]");
    }
}
```

And then in the `Material.Update()` method, change the `switch` statement to handle the following cases, 
```csharp
switch (oldParam.ParameterClass)  
{  
    case EffectParameterClass.Scalar:  
        newParam.SetValue(oldParam.GetValueSingle());  
        break;  
    case EffectParameterClass.Matrix:  
        newParam.SetValue(oldParam.GetValueMatrix());  
        break;  
    case EffectParameterClass.Vector when oldParam.ColumnCount == 2: // float2  
        newParam.SetValue(oldParam.GetValueVector2());  
        break;  
    case EffectParameterClass.Object:  
        newParam.SetValue(oldParam.GetValueTexture2D());  
        break;  
    default:  
        Console.WriteLine("Warning: shader reload system was not able to re-apply property. " +  
                          $"shader=[{Effect.Name}] " +  
                          $"property=[{oldParam.Name}] " +  
                          $"class=[{oldParam.ParameterClass}]");  
        break;  
}
```

## Conclusion

Excellent work! Our new `Material` class makes working with shaders much safer and more convenient. In this chapter, you accomplished the following:

- Created a `Material` class to encapsulate shader effects and their parameters.
- Solved the `NullReferenceException` that can happen when the compiler optimizes away unused parameters.
- Handled the state of shader parameters so they are automatically reapplied during a hot-reload.
- Added support for multiple parameter types like `Matrix`, `Vector2`, and `Texture2D`.

Now that we have a solid and safe foundation for our effects, let's make them easier to tweak. In the next chapter, we'll build a real-time debug UI that will let us change our shader parameters with sliders and buttons right inside the game!

You can find the complete code sample for this chapter, [here](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Tutorials/2dShaders/src/03-The-Material-Class). 

Continue to the next chapter, [Chapter 04: Debug UI](../04_debug_ui/index.md)