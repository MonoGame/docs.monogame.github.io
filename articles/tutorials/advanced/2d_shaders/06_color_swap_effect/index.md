---
title: "Chapter 06: Color Swap Effect"
description: "Create a shader to change the colors of the game"
---

In this chapter, we will create a powerful color-swapping effect. we will learn about a common and flexible technique that uses a texture as a Look-Up Table (LUT) to map original colors to new ones. This will give us precise control over the look and feel of our game's sprites.

At the end of this chapter, we will be able to fine-tune the colors of the game. Here are a few examples: 

| ![Figure 6-1: The default colors](./images/overview-1.png) | ![Figure 6-2: A green variant of the game](./images/overview-2.png) | ![Figure 6-3: A pink variant of the game](./images/overview-3.png) | ![Figure 6-4: A purple variant of the game](./images/overview-4.png) |
| :--------------------------------------------------------: | :-----------------------------------------------------------------: | :----------------------------------------------------------------: | :------------------------------------------------------------------: |
|             **Figure 6-1: The default colors**             |             **Figure 6-2: A green variant of the game**             |             **Figure 6-3: A pink variant of the game**             |             **Figure 6-4: A purple variant of the game**             |


If you are following along with code, here is the code from the end of the [previous chapter](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Tutorials/2dShaders/src/05-Transition-Effect).

## The Basic Color Swap Effect

At the moment, the game uses a lot of blue and gray textures. A common feature in retro-style games is to be able to change the color palette of the game. Another common feature of a game is to change the character's color during certain in-game events. For example, maybe the character flashes white when taking damage, or sparkles a gold color when picking up a combo. There are 2 broad categories for implementing these styles of features, 
1. re-draw all of the game assets using each color palette,
2. use some sort of _color swap_ shader effect to dynamically control the colors of sprites at runtime. 

For some simple use cases, sometimes it makes sense to simply re-draw the assets with different colors. However, the second option is more flexible and will enable more features, and since this is a shader tutorial, we will explore option 2. 

### Getting Started

Start by creating a new `Sprite Effect` in the _SharedContent_ MonoGame Content Builder file, and name it `colorSwapEffect.fx`. 

| ![Figure 6-5: Add the `colorSwapEffect.fx` to MGCB Editor](./images/mgcb.png) |
| :---------------------------------------------------------------------------: |
|          **Figure 6-5: Add the `colorSwapEffect.fx` to MGCB Editor**          |

Switch back to your code editor, and in the `GameScene`, we need to do the following steps to start working with the new `colorSwapEffect.fx`, 
1. Add a class variable for the new `Material` instance:

[!code-csharp[](./snippets/snippet-6-01.cs)]

2. Load the shader in the `LoadContent()` method:

[!code-csharp[](./snippets/snippet-6-02.cs)]

3. Update the `Material` in the `Update()` method to enable hot-reload support:

[!code-csharp[](./snippets/snippet-6-03.cs)]

4. And finally, _use_ the `colorSwapMaterial` when drawing the sprites for the `GameScene`. For now, as we explore the color swapping effect, we are going to disable the `grayscaleEffect` functionality. In the `Draw()` method, start the `SpriteBatch` like this:

[!code-csharp[](./snippets/snippet-6-04.cs)]

Now when you run the game, it will look the same, but the new shader is being used to draw all the sprites in the `GameScene`. To verify, you can try changing the shader function to force the red channel to be `1`, just to see some visually striking confirmation the new shader is being used:

[!code-hlsl[](./snippets/snippet-6-05.hlsl)]

| ![Figure 6-6: Confirm the shader is being used](./images/test.png) |
| :----------------------------------------------------------------: |
|          **Figure 6-6: Confirm the shader is being used**          |

> [!warning] 
> The menu will not use the color swapper.
> 
> The game's menu is being drawn with GUM, and we aren't configuring any shaders on the GUM menu yet. For now, it will continue to draw with its old colors. 

For debugging purposes, we will disable the game's update logic so the player and bat aren't moving. This will let us focus on developing the look of the shader without getting distracted by the movement and game logic of game-over menus and score. 

The easiest way to disable all of the game logic is to `return` early from the `GameScene`'s `Update()` method, thus short circuiting all of the game logic:

[!code-csharp[](./snippets/snippet-6-06.cs)]


### Hard Coding Color Swaps

The goal is to be able to change the color of the sprites drawn with the `_colorSwapMaterial`. To build some intuition, one of the most straightforward ways to change the color is to hard-code a table of colors in the `colorSwapEffect.fx` file. The texture atlas used to draw the slime character uses a color value of `rgb(32, 40, 78)` for the body of the slime. 

| ![Figure 6-7: The slime uses a dark blue color](./images/slime-blue-color.png) |
| :----------------------------------------------------------------------------: |
|                **Figure 6-7: The slime uses a dark blue color**                |

The shader code _could_ just do an `if` check for this color, and when any of the pixels are that color, return a hot-pink color instead:

[!code-hlsl[](./snippets/snippet-6-07.hlsl)]

That would produce an image like this,

| ![Figure 6-8: The blue color is hardcoded to pink](./images/pink.png) |
| :-------------------------------------------------------------------: |
|          **Figure 6-8: The blue color is hardcoded to pink**          |

### Using a Color Map

The problem with the hard-coded approach is that we would need to have an `if` check for _each_ color that should be swapped. Depending on your work ethic, there are already too many colors in the _Dungeon Slime_ assets to hardcode them all in a shader. Instead of hard coding the color swaps as `if` statements, we can create a _table_ of colors that maps asset color to final color. 

Conceptually, a _table_ structure is a series of `key` -> `value` pairs. We could represent each asset color as a `key`, and store the swap color as a `value`. To build up a good example, let's find a few more colors from the _Dungeon Slime_ assets. 

| ![Figure 6-9: The colors from the assets](./images/color-map.png) |
| :---------------------------------------------------------------: |
|            **Figure 6-9: The colors from the assets**             |

And here they are written out,
1. dark-blue - `rgb(32, 40, 78)`
2. gray-blue - `rgb(115, 141, 157)`
3. white - `rgb(255, 255, 255)`
4. light gray-blue - `rgb(214, 225, 233)`

Our goal is to treat those colors as `keys` into a table that results in a final color `value`. Fortunately, all of the `red` channels are unique across all 4 input colors. The `red` channels are `32`, `115`, `255`, and `214`. 

As a demonstration, if we were using C# to create a table, it might look like this:

[!code-csharp[](./snippets/snippet-6-08.cs)]

Unfortunately, shaders do not support the `Dictionary<>` type, so we need to find another way to represent the table in a shader friendly format. Shaders are good at reading data from textures, so we will encode the table information inside a custom texture. Imagine a custom texture that was 256 pixels wide, but only 1 pixel _tall_. We could treat the `key` values from above (`32`, `115`, `255`, and `214`) as _locations_ along the x-axis of the image, and the color of each pixel as the `value`. 

These images are not to scale, because a 256x1 pixel image would not show well on a web browser. Here are the original colors laid out in a 256x1 pixel image, with the color's red channel value written below the pixel.

| ![Figure 6-10: The original colors](./images/texture-map-original.png) |
| :-------------------------------------------------------------------: |
|                  **Figure 6-10: The original colors**                  |

We could produce a second texture that puts different color values in the same key positions.

| ![Figure 6-11: An abstract view of a 255 x 1 texture](./images/texture-map.png) |
| :----------------------------------------------------------------------------: |
|             **Figure 6-11: An abstract view of a 255 x 1 texture**              |



Here is the actual texture with the swapped colors. Download [this image](./images/color-map-1.png) and add it to your MonoGame Content file.

> [!note]
> This image is not to scale. Remember, it is a 256x1 pixel image. This preview is being scaled up to a height of 32 pixels just for visualization.

| <img src="./images/color-map-1.png" alt="Figure 6-12: The color table texture" style=" height: 32px; image-rendering: pixelated;"/> |
| :--------------------------------------------------------------: |
|             **Figure 6-12: The color table texture**              |

We need to load and pass the the texture to the `colorSwapEffect` shader.
Add this code after loading the `_colorSwapMaterial` in the `LoadContent()` method:

[!code-csharp[](./snippets/snippet-6-09.cs)]

And the `colorSwapEffect.fx` shader needs to be updated to accept the color map:

[!code-hlsl[](./snippets/snippet-6-10.hlsl)]

The `Texture2D` and `sampler2D` declarations are required to read from textures in a MonoGame shader. A `Texture2D` represents the pixel data of the image. A `sampler2D` defines _how_ the shader is allowed to read data from the `Texture2D`. 

The `ColorMapSampler` has a lot of extra properties (`MinFilter`, `MagFilter`, `MipFilter`, `AddressU`, and `AddressV`) that control exactly how the texture data is read from the `ColorMap`. 

By default, when a sampler reads data from a texture in a shader, it will subtly blend nearby pixel values to increase the visual quality. However, when a texture is being used as a lookup table, this blending is problematic because it will distort the data stored in the texture. The `Point` value given to the `Filter` properties tells the sampler to only read _one_ pixel value. 

When a sampler is reading a texture, there is always some _location_ being used to read pixel data from. The texture coordinate space is from 0 to 1, in both the `u` and `v` axes. By default, if a value greater than 1, or less than 0 is given, the sampler will _wrap_ the value around to be within the range 0 to 1. For example, `1.15` would become `0.15`.  The `Clamp` value prevents the wrapping and cuts the input off at the min and max values. For example, `1.15` becomes `1.0`. 

> [!tip] 
> More information on Samplers.
> 
> The [MonoGame Docs](https://docs.monogame.net/articles/getting_to_know/whatis/graphics/WhatIs_Sampler.html) have more details on samplers. 

The shader function can now do 2 steps to perform the color swap,
1. read the original color value of the pixel,
2. use the original color's red value as the `key` in the lookup texture to extract the swap color `value`. 

To help visualize the effect, it will be helpful to visualize the original color _and_ the swap color. Add a control parameter that can be used to select between the two colors:

[!code-hlsl[](./snippets/snippet-6-11.hlsl)]

Change the shader function to the following:

[!code-hlsl[](./snippets/snippet-6-12.hlsl)]

Now in the game, we can visualize the color swap by adjusting the control parameter. Perhaps the colors we picked do not look very nice.

| ![Figure 6-13: The color swap effect is working!](./gifs/color-swap.gif) |
| :---------------------------------------------------------------------: |
|            **Figure 6-13: The color swap effect is working!**            |

That looks pretty good, but changing between original and swap colors reveals a visual glitch. The color table didn't account for _some_ of the original colors. All of the colors get mapped, and our default color in the map was _white_, so some of the game's art is just turning white. For example, look at the torches on the top-wall. 

To fix this, we can adjust the color lookup map to use transparent values by default. Use [this texture](./images/color-map-2.png) instead.

| <img src="./images/color-map-2.png" alt="Figure 6-14: The color map with a transparent default color" style=" height: 32px; image-rendering: pixelated;"/> |
| :-------------------------------------------------------------------------------------: |
|             **Figure 6-14: The color map with a transparent default color**              |

Now, anytime the swapped color value has an `alpha` value of zero, the implication is that the color was not part of the table. In that case, the shader should default to the original color instead of the non-existent mapped value.

In the shader, before the final `return` line, add this snippet:

[!code-hlsl[](./snippets/snippet-6-13.hlsl)]

| ![Figure 6-15: Colors that are not in the map do not change color](./gifs/color-swap-2.gif) |
| :-----------------------------------------------------------------------------------------: |
|             **Figure 6-15: Colors that are not in the map do not change color**             |

One final glitch becomes apparent if you stare at that long enough, which is that the center pixel in the torch is changing color from its original _white_, to our mapped orange color. In a way, that is _by design_, because the white values are being mapped. Fixing this would require a modification to the original assets to change the color the torch center, but that is left as an exercise for the reader. 

### Nicer Colors

The colors used above aren't the nicest. They were used for demonstration purposes. Here are some nicer textures to use that produce better results. 

Dark Purple - Here is the color map for a [dark-purple](./images/color-map-dark-purple.png) color scheme.

| ![Figure 6-16: A dark purple look](./images/example-dark-purple.png) |
| :------------------------------------------------------------------: |
|                 **Figure 6-16: A dark purple look**                  |

Green - Here is the color map for a [green](./images/color-map-green.png) color scheme.

| ![Figure 6-17: A green look](./images/example-green.png) |
| :------------------------------------------------------: |
|              **Figure 6-17: A green look**               |

Pink - Here is the color map for a [pink](./images/color-map-pink.png) color scheme.

| ![Figure 6-18: A pink look](./images/example-pink.png) |
| :----------------------------------------------------: |
|              **Figure 6-18: A pink look**              |


## Creating Dynamic Color Maps

So far, we have created color maps and brought them into the game as content. However, it would be cool to create these color maps dynamically with a C# script based on gameplay values and provide that texture to the shader in realtime. Our goal will be to modify the snake's color piece by piece when a the player eats a bat. 

To get started, we first need to devise a way to create a custom color map and pass it to the shader. 

Create a new class under the _MonoGameLibrary/Graphics_ folder called `RedColorMap`:

[!code-csharp[](./snippets/snippet-6-14.cs)]

And now to check if its working, create a temporary variable at the end of the `LoadContent()` in the `GameScene`
[!code-csharp[](./snippets/snippet-6-15.cs)]

| ![Figure 6-19: Changing the colors from runtime](./images/example-runtime.png) |
| :----------------------------------------------------------------------------: |
|               **Figure 6-19: Changing the colors from runtime**                |

### Changing Slime Color

The goal is to change the color of the slime independently from the rest of the game. The `SpriteBatch` will try to make as few draw calls as possible and because all of the game assets are in a sprite-atlas, any shader parameters will be applied for _all_ sprites. However, you can change the `sortMode` to `Immediate` to change the `SpriteBatch`'s optimization to make it draw sprites immediately with whatever _current_ shader parameters exist. 

Change the `SpriteBatch.Begin()` call to look like this:

[!code-csharp[](./snippets/snippet-6-16.cs)]

And then update the draw code itself to update the shader parameter between drawing the slime and the rest of the game:

[!code-csharp[](./snippets/snippet-6-17.cs)]

Now the slime appears with one color swap configuration and the rest of the scene uses the color swap configured via the content.

| ![Figure 6-20: The slime is a different color configuration than the game](./images/example-multi.png) |
| :----------------------------------------------------------------------------------------------------: |
|              **Figure 6-20: The slime is a different color configuration than the game**               |

We want to swap the color of the slime between two color maps, so first, we need a way to clone an existing color map into the dynamic color table. Add this method to the `RedColorMap` class:

[!code-csharp[](./snippets/snippet-6-18.cs)]

Then modify the instance in the `GameScene` to start the color map based off whatever color map texture was loaded:

[!code-csharp[](./snippets/snippet-6-19.cs)]


Now in the `Draw()` method, we can _optionally_ change the color map based on some condition. In this example, the color map only being set on every other second:

[!code-csharp[](./snippets/snippet-6-20.cs)]

| ![Figure 6-21: The slime's color changes based on time](./gifs/color-swap-even-seconds.gif) |
| :-----------------------------------------------------------------------------------------: |
|                  **Figure 6-21: The slime's color changes based on time**                   |

Ultimately, it would be nice to control the color value _per_ slime segment, not the entire slime. When the player eats a bat, the slime segments should change color in an animated way so that it looks like the color is "moving" down the slime segments. To do this, modify the `Slime.Draw()` method to look like this:

[!code-csharp[](./snippets/snippet-6-21.cs)]

Then, in the `GameScene`'s logic, we need to add a local field to remember when the last time the slime's `Grow()` method was called. Add a class field:

[!code-csharp[](./snippets/snippet-6-22.cs)]


In the `CollisionCheck()` method, add this line after the `Grow()` method is invoked:

[!code-csharp[](./snippets/snippet-6-23.cs)]

Now, in the `Draw()` method, modify the _slime_'s draw invocation to use the new `configureSpriteBatch` callback:

[!code-csharp[](./snippets/snippet-6-24.cs)]

Play around with the colors until you find something you like.

| ![Figure 6-22: The slime's color changes when it eats](./gifs/color-swap-slime.gif) |
| :---------------------------------------------------------------------------------: |
|               **Figure 6-22: The slime's color changes when it eats**               |

## Fixing the Gray Scale

The color swap shader is working well, but to experiment with it, we had previously _removed_ the pause screen's grayscale effect. Both effects are trying to modify the color of the game, so they naturally conflict with each other. To solve the problem, the shaders can be merged together into a single effect.

Extract the logic of the grayscale effect into a separate function and copy it into the `colorSwapEffect.fx` file:

[!code-hlsl[](./snippets/snippet-6-25.hlsl)]

In order for this to work, do not forget to add the `Saturation` shader parameter to the `colorSwapEffect.fx` file:

[!code-hlsl[](./snippets/snippet-6-26.hlsl)]

For readability, extract the logic of the color swap effect into a new function as well:

[!code-hlsl[](./snippets/snippet-6-27.hlsl)]

And now the main shader function can chain these methods together:

[!code-hlsl[](./snippets/snippet-6-28.hlsl)]

> [!warning] 
> Function Order Matters!
> 
> Make sure that the `Grayscale` and `SwapColors` functions appear _before_ the `MainPS` function in the shader, otherwise the compiler will not be able to resolve the functions.

Now you can control the saturation manually with the debug slider,

| ![Figure 6-23: Combining the color swap and saturation effect](./gifs/color-saturation.gif) |
| :-----------------------------------------------------------------------------------------: |
|               **Figure 6-23: Combining the color swap and saturation effect**               |

The last thing to do is remove the old `grayscaleEffect` and re-write the game logic to set the `Saturation` parameter on the new effect. 
In the `Draw()` method, instead of having an `if` case to start the `SpriteBatch` with different settings, it can always be configured to start with the `_colorSwapMaterial`:

[!code-csharp[](./snippets/snippet-6-29.cs)]

In the `Update()` method, we just need to set the `_saturation` back to `1` if the game is being played:

[!code-csharp[](./snippets/snippet-6-30.cs)]

| ![Figure 6-24: The grayscale effect has been restored](./gifs/grayscale.gif) |
| :--------------------------------------------------------------------------: |
|           **Figure 6-24: The grayscale effect has been restored**            |


## Color Look Up Textures (LUTs)

The approach we used above is a simplified version of a broader technique called _Color Look Up Tables_, or Color LUTs. In the version we wrote above, there is a large limitation about which colors can be used in the table. The `key` in the color table was the `red` channel value of the input colors. If you had two different input colors that shared the same `red` channel value, the technique wouldn't work. 

The limitation is _often_ acceptable in game assets because you own the assets themselves and can author the textures to avoid the case where colors overlap on key values. However, when it is unavoidable, the `key` must be more complex than only the `red` value. For example, it could be unavoidable if your game needed more than 256 _unique_ colors. 

The next logical step is to make the `key` _2_ color channels like `red` _and_ `green`. In that case, the color texture wouldn't be a 256x1 texture, it would be a 256x256 texture. The `x` axis would still represent the `red` channel, and now the `y` axis would represent the `green` channel. Now the game could have `256 * 256` unique colors, or `65,536`. 

Finally, if you need _more_ colors, the final color channel can be included in the `key`, making the `key` be the combination of `red`, `green`, and `blue` channels. In the first case, the look up texture was 256x1 pixels. In the second case, it was 256x256 pixels. The final case is a texture of size 2048x2048 pixels. Imagine a texture made up of smaller 256x256 textures, stored in an 8x8 grid. 

Color LUTs are used in post-processing to adjust the final look and feel of games across the industry. The technique is called _Tone-Mapping_. 

## Conclusion

That was a really powerful technique! In this chapter, you accomplished the following:

- Implemented a color-swapping system using a 1D texture as a Look-Up Table (LUT).
- Created a `RedColorMap` class to dynamically generate these LUTs from C# code.
- Used `SpriteSortMode.Immediate` to apply different materials to different sprites in the same frame.
- Combined the color swap and grayscale effects into a single, more versatile shader.

So far, all of our work has been in the pixel shader, which is all about changing the color of pixels. In the next chapter, we will switch gears and explore the vertex shader to manipulate the geometry of our sprites and add some surprising 3D flair to our 2D game.

You can find the complete code sample for this chapter, [here](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Tutorials/2dShaders/src/06-Color-Swap-Effect). 

Continue to the next chapter, [Chapter 07: Sprite Vertex Effect](../07_sprite_vertex_effect/index.md)