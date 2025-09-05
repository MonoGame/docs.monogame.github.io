---
title: "Chapter 03: The Material Class"
description: "Create a wrapper class to help manage shaders"
---

In [Chapter 24](../../../building_2d_games/24_shaders/index.md) of the original [2D Game Series](../../../building_2d_games/index.md), you learned about MonoGame's [`Effect`](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) class. When `.fx` shaders are compiled, the compiled code is loaded into MonoGame as an `Effect` instance. The `Effect` class has a few powerful utilities for setting shader parameters, but otherwise, it is a fairly bare-bones container for the compiled shader code. 

> [!note]
> MonoGame also ships with standard `Effect` sub-classes that can be useful for bootstrapping a game without needing to write any custom shader code. However, all of these standard `Effect` types are geared towards 3D games, except for _1_, called the [`SpriteEffect`](xref:Microsoft.Xna.Framework.Graphics.SpriteEffect). This will be discussed in [Chapter 7: Sprite Vertex Effect](../07_sprite_vertex_effect/index.md). 

In this chapter, we will create a small wrapper class, called the `Material`, that will handle shader parameters, hot-reload, and serve as a baseline for future additions.

If you are following along with code, here is the code from the end of the [previous chapter](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Tutorials/2dShaders/src/02-Hot-Reload-System/).

## The Material Class

The `_grayscaleEffect` serves a very specific purpose, but imagine instead of just _decreasing_ the saturation, the effect could also _increase_ the saturation. In that hypothetical, then calling it a "grayscale" effect only captures _some_ of the shader's value. Setting the `Saturation` to `0` would configure the shader to be a grayscale effect, but setting the `Saturation` really high would configure the shader to be a super-saturation effect. A single shader can configured to create multiple distinct visuals. Many game engines use the term, _Material_, to recognize each _configuration_ of a shader effect. 

A material represents a compiled `Effect` _and_ the runtime configuration for the `Effect`. For example, the `_grayscaleEffect` shader has a single property called `Saturation`. The _value_ of that property is essential to the existence of the `_grayscaleEffect`. It would be useful to have logic that owns these shader parameter values. We will create a class called `Material` that manages all of our shader related metadata. 

Start by creating a new file in the _MonoGameLibrary/Graphics_ folder called `Material.cs`:

[!code-csharp[](./snippets/snippet-3-01.cs)]

In order to create instances of the `Material`, go ahead and create an additional method in the `ContentManagerExtensions` file:

[!code-csharp[](./snippets/snippet-3-02.cs)]

> [!note]
> The `Material` has been built to be hot-reloadable. Later in this chapter, we will see how the performance cost for supporting hot-reload is negligible by using the `[Conditional("DEBUG")]` attribute. However, if you do not want the `Material` to be hot-reloadable, then change the `Material`'s `Asset` field to be an `Effect` rather than a `WatchedAsset<Effect>`. There will be some minor differences in the rest of the tutorial series, but the only major difference is that the `Material` will not be hot-reloadable during development.


And now, in the `GameScene`, adjust the `_grayscaleEffect` to use the new `Material` class:

[!code-csharp[](./snippets/snippet-3-03.cs)]

Changing the `_grayscaleEffect` from an `Effect` to `Material` is going to cause a few compilation errors. The fixes are listed below.

1. When instantiating the `_grayscaleEffect`, use the new method:

	[!code-csharp[](./snippets/snippet-3-04.cs)]

2. When checking if the asset needs to be reloaded for hot-reload, use the `.Asset` sub property:

	[!code-csharp[](./snippets/snippet-3-05.cs)]

3. And in the `Draw()` method, use the `.Effect` shortcut property:

	[!code-csharp[](./snippets/snippet-3-06.cs)]

### Setting Shader Parameters

You already saw how to set a shader property by using the `Saturation` value in the `_grayscaleEffect` shader. However, as you develop shaders in MonoGame, you will eventually _accidentally_ try to set a shader property that does not exist in your shader. When this happens, the code will throw a `NullReferenceException` rather than fail silently. 
For example, if you tried to add this line of the code to the `Update` loop:

[!code-csharp[](./snippets/snippet-3-07.cs)]

You will see this type of `NullReference` error:

```
System.NullReferenceException: Object reference not set to an instance of an object.
```

> [!Caution]
> do not actually add the `DoesNotExist` sample, because it will break your code.

On its own, this would not be too difficult to accept. However, MonoGame's shader compiler will aggressively remove properties that are not actually being _used_ in your shader code. Even if you wrote a shader that had a `DoesNotExist` property, if it was not being used to compute the return value of the shader, it will be removed. The compiler is good at optimizing away unused variables. 
For example, in the `grayscaleEffect.fx` file, change the last few lines of the `MainPS` function to the following:

[!code-hlsl[](./snippets/snippet-3-08.hlsl)]

If you run the game and enter the `GameScene`, you will see a `NullReferenceException` (and the game will hard-crash). The `Saturation` shader parameter no longer exists in the shader, so when the `Draw()` method tries to _set_ it, the game crashes. 

The aggressive optimization is good for your game's performance, but when combined with the hot-reload system, it will lead to unexpected bugs. As you iterate on shader code, it is likely that at some point a shader parameter will be optimized out of the compiled shader. The hot-reload system will automatically load the newly compiled shader, and if the C# code attempts to set the previously available parameter, the game may crash.

To solve this problem, the `Material` class can encapsulate setting shader properties and handle the potential error scenario. The `Effect.Parameters` variable is an instance of the  [`EffectParameterCollection`](xref:Microsoft.Xna.Framework.Graphics.EffectParameterCollection) class. Here is the class signature and fields:

[!code-csharp[](./snippets/snippet-3-09.cs)]

The `_indexLookup` is a `Dictionary<string, int>` containing a mapping of property _name_ to parameter. The `Dictionary` class has methods for checking if a given property name exists, but unfortunately we cannot access it due to the `private` access modifier. Luckily, the entire `EffectParameterCollection` inherits from `IEnumerable`, so we can use the existing dotnet utilities to convert the entire structure into a `Dictionary`. Once we have the parameters in a `Dictionary` structure, we will be able to check what parameters exist _before_ trying to access them, thus avoiding potential `NullReferenceExceptions`. 

Add this new property to the `Material` class:

[!code-csharp[](./snippets/snippet-3-10.cs)]

And now you can add the following method that will convert the `EffectParameterCollection` into the new `Dictionary` property:

[!code-csharp[](./snippets/snippet-3-11.cs)]

Do not forget to invoke this method during the constructor of the `Material`:

[!code-csharp[](./snippets/snippet-3-12.cs)]

With the new `ParameterMap` property, create a helper function that checks if a given parameter exists in the shader:

[!code-csharp[](./snippets/snippet-3-13.cs)]

Now you can create a helper method that sets a parameter value for the `Material`, but will not crash if the parameter does not actually exist:

[!code-csharp[](./snippets/snippet-3-14.cs)]

Instead of setting the `Saturation` for the `_grayscaleEffect` manually as before, change the `Draw()` code to use the new method:

[!code-csharp[](./snippets/snippet-3-15.cs)]

To verify it is working, re-run the game, and instead of seeing a crash, you should see the `GameScene`'s gameover menu show a completely _white_ background. This is because the shader is setting the `finalColor` to `1`. Delete the following line from the shader, wait for the hot-reload system to kick in, and the game should return to normal.

| ![Figure 3-1: The game does not crash](./videos/shader-properties.mp4) |
| :--------------------------------------------------------------------: |
|                **Figure 3-1: The game does not crash**                 |


### Reloading Properties

When the hot-reload system loads a new compiled shader into the game's memory, the new shader does not have any of the existing shader parameter _values_ that the previous shader instance had. To demonstrate the problem, we will purposefully break the `_grayscaleEffect` a bit. For now, comment out the line the `GameScene`'s `Draw()` method:

[!code-csharp[](./snippets/snippet-3-16.cs)]

And instead, add the following line to the end of the `LoadContent()` method:

```chsharp
_grayscaleEffect.SetParameter("Saturation", 1);
```

The net outcome is that the `_grayscaleEffect` will not actually work for its designed purpose, but it should be always fully saturated with a value of `1`, and _never_ change. Run the game, enter the `GameScene`, and hit the pause button. Now, if you cause _any_ reload of the shader, the background of the `GameScene` will desaturate and turn grayscale. The newly compiled shader instance has no value for the `Saturation` parameter, and since `0` is the default value for numbers, it appears grayscale. 

To solve this problem, the `Material` class can encapsulate the handling of applying new hot-reload updates. Anytime a new shader is available to swap in, the `Material` class needs to handle re-applying the old shader parameters to the new instance. 
Add the following method to the `Material` class:

[!code-csharp[](./snippets/snippet-3-17.cs)]

And now instead of using the `TryRefresh()` method directly on the `_grayscaleEffect`, use the new method:

[!code-csharp[](./snippets/snippet-3-18.cs)]

If you repeat the same test as before, the game will not become grayscale after a new shader is loaded. Once you have validated this, make sure to undo the changes in the `LoadContent()` and `Draw()` method, so that the `_grayscaleEffect` is still setting the `Saturation` value in the `Draw()` method.


### Debug Builds

When the _DungeonSlime_ game is published, it would not make sense to run the new `Material.Update()` method, because no shaders would ever be hot-reloaded in a release build. We can strip the method from the game when it is being built for _Release_. Add the following attribute to the `Material.Update()` method:

[!code-csharp[](./snippets/snippet-3-19.cs)]

The built _DungeonSlime_ executable will no longer contain the compiled code for the `Material.Update()` method, or any place in the code that _invoked_ the method. This means that the hot-reload system will never attempt to read the file timestamps of your `.xnb` files. There is still a _tiny_ cost for keeping the extra fields on the `WatchedAsset` type, rather than using the `Effect` directly. However, given the huge wins for your shader development workflow, paying the memory cost for a few mostly unused fields is a worthwhile trade-off. 

> [!Tip]
> Adding the [`[Conditional]`](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.conditionalattribute?view=net-9.0) attribute is optional. It will slightly increase the performance of your game, and disable `Material` hot-reload for released games.

### Supporting more Parameter Types

In the previous sections, we have only dealt with the `grayscaleEffect.fx` shader, and that shader only has a single shader parameter called `Saturation`. The `Saturation` parameter is a `float`, and so far, the `Material` class only handles `float` based parameters. However, shaders may have many more types of parameters. In this tutorial, we will add support for the following parameter types, and leave it as an exercise to the reader to add support for more.
- `Matrix`,
- `Vector2`,
- `Texture2D` 

Add the following methods to the `Material` class:

[!code-csharp[](./snippets/snippet-3-20.cs)]

And then in the `Material.Update()` method, change the `switch` statement to handle the following cases:

[!code-csharp[](./snippets/snippet-3-21.cs)]

## Conclusion

Excellent work! Our new `Material` class makes working with shaders much safer and more convenient. In this chapter, you accomplished the following:

- Created a `Material` class to encapsulate shader effects and their parameters.
- Solved the `NullReferenceException` that can happen when the compiler optimizes away unused parameters.
- Handled the state of shader parameters so they are automatically reapplied during a hot-reload.
- Added support for multiple parameter types like `Matrix`, `Vector2`, and `Texture2D`.

Now that we have a solid and safe foundation for our effects, we will make them easier to tweak. In the next chapter, we will build a real-time debug UI that will let us change our shader parameters with sliders and buttons right inside the game!

You can find the complete code sample for this chapter, [here](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Tutorials/2dShaders/src/03-The-Material-Class). 

Continue to the next chapter, [Chapter 04: Debug UI](../04_debug_ui/index.md)