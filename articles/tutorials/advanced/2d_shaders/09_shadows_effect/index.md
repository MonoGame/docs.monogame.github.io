---
title: "Chapter 09: Shadow Effect"
description: "Add dynamic shadows to the game"
---

Our lighting system is looking great, but the lights do not feel fully grounded in the world. They shine right through the walls, the bat, and even our slime! To truly sell the illusion of light, we need darkness. We need shadows.

In this, our final effects chapter, we're going to implement a dynamic 2D shadow system. The shadows will be drawn with a new vertex shader, and integrated into the point light shader from the previous chapter. After the effect is working, we will port the effect to use a more efficient approach using a tool called the _Stencil Buffer_. Lastly, we will explore some visual tricks to improve the look and feel of the shadows. 

At the end of this chapter, your game will look something like this:

| ![Figure 9-1: The final shadow effect](./gifs/overview.gif) |
| :---------------------------------------------------------: |
|         **Figure 9-1: The final shadow effect**             |


If you're following along with code, here is the code from the end of the [previous chapter](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Tutorials/2dShaders/src/08-Light-Effect).

## 2D Shadows

Take a look at the current lighting in _Dungeon Slime_. In this screenshot, there is a single light source. The bat and the slime do not cast shadows, and without these shadows, it is hard to visually identify where the light's position is. 

| ![Figure 9-2: A light with no shadows](./images/starting.png) |
| :-----------------------------------------------------------: |
|            **Figure 9-2: A light with no shadows**            |

If the slime was casting a shadow, then the position of the light would be a lot easier to decipher just from looking at the image. Shadows help ground the objects in the scene. Just to visualize it, this image is a duplicate of the above, but with a pink debug shadow drawn on top to illustrate the desired effect. 

![9.2: A hand drawn shadow](./images/dbg_light_shadow.png)

The pink section is called the _shadow hull_. We can split up the entire effect into two distinct stages. 
1. We need a way to calculate and render the shadow hulls from all objects that we want to cast shadows (such as bats and slime segments),
2. We need a way to _use_ the shadow hull to actually mask the lighting from the previous chapter. 


Step 2 is actually a lot easier to understand than step 1. Imagine that the shadow hulls were drawn to an off-screen texture, like the one in the image below. The black sections represent shadow hulls, and the white sections are places where no shadow hulls exist. This resource is called the `ShadowBuffer`. 

| ![Figure 9-3: A shadow map](./images/dbg_shadow_map.png) |
| :------------------------------------------------------: |
|               **Figure 9-3: A shadow map**               |

We would need to have a `ShadowBuffer` for each light source, but if we did, then when the light was being rendered, we could pass in the `ShadowBuffer` as an additional texture resource to the `_pointLightEffect.fx`, and use the pixel value of the `ShadowBuffer` to mask the light source. 

In the sequence below, the left image is the just the `LightBuffer`. The middle image is the `ShadowBuffer`, and the right image is the product of the two images. Any pixel in the `ShadowBuffer` that was `white` means the final image uses the color from the `LightBuffer`, and any `black` pixel from the `ShaodwBuffer` becomes black in the final image as well. The multiplication of the `LightBuffer` and `ShadowBuffer` complete the shadow effect.

| The `LightBuffer`                                         | The `ShadowBuffer`                                       | The multiplication of the two images                                     |
| --------------------------------------------------------- | -------------------------------------------------------- | ------------------------------------------------------------------------ |
| ![Figure 9-4: a light buffer](./images/dbg_light_map.png) | ![Figure 9-3: A shadow map](./images/dbg_shadow_map.png) | ![Figure 9-4: The multiplication](./images/dbg_light_map_multiplied.png) |

The mystery to unpack is step 1, how to render the `ShadowBuffer` in the first place. 

## Rendering the Shadow Buffer

To build some intuition, we will start by considering a shadow caster that is a single line segment. If we can generate a shadow for a single line segment, then we could compose multiple line segments to replicate the shape of the slime sprite. In the image blow, there is a single light source at position `L`, and a line segment between points `A`, and `B`. 

| ![Figure 9-4: A diagram of a simple light and line segment](./images/light_math.png) |
| :----------------------------------------------------------------------------------: |
|             **Figure 9-4: A diagram of a simple light and line segment**             |

The shape we need to draw is the non-regular quadrilateral defined by `A`, `a`, `b`, and `B`. It is shaded in pink. These points are in world space. Given that we know where the line segment is, we know where `A` and `B` are, but we do not _yet_ know `a` and `b`'s location.

> [!note] 
> `A` and `a` naming convention.
> 
> The `A` and `a` points lay on the same ray from the light starting at `L`. The uppercase `A` denotes that the position is _first_ from the light's point of view. The same pattern holds for `B` and `b`. 

However, the `SpriteBatch` usually only renders rectangular shapes. Naively, it appears `SpriteBatch` cannot help us draw these sorts of shapes, but fortunately, since the shadow hull has exactly _4_ vertices, and `SpriteBatch` draws quads with exactly _4_ vertices, we can use a custom vertex function. 

This diagram shows an abstract pixel being drawn at some position `P`. The corners of the pixel may be defined as `G`, `S`, `D`, and `F`. 

| ![Figure 9-4: A diagram showing a pixel](./images/pixel_math.png) |
| :---------------------------------------------------------------: |
|             **Figure 9-4: A diagram showing a pixel**             |

Our goal is define a function that transforms the positions `S`, `D`, `F`, and `G` _into_ the positions, `A`, `a`, `b`, and `B`. The table below shows the desired mapping.

| Pixel Point | Shadow Hull Point |
| ----------- | ----------------- |
| S           | A                 |
| D           | a                 |
| F           | b                 |
| G           | B                 |


Each vertex (`S`, `D`, `F`, and `G`) have additional metadata beyond positional data. The diagram includes `P`, but that point is the point specified to _`SpriteBatch`_, and it isn't available in the shader function. The vertex shader runs once for each vertex, but completely in isolation of the other vertices. Remember, the input for the standard vertex shader is as follows:

[!code-hlsl[](./snippets/snippet-9-01.hlsl)]

The `TexCoord` data is a two dimensional value that tells the pixel shader how to map an image onto the rectangle. The values for `TexCoord` can be set by the `SpriteBatch`'s `sourceRectangle` field in the `Draw()`, but if left unset, they default to `0` through `1` values. The default mapping is in the table below,

| Vertex | TexCoord.x | TexCoord.y |
| ------ | ---------- | ---------- |
| S      | 0          | 0          |
| D      | 1          | 0          |
| F      | 1          | 1          |
| G      | 0          | 1          |

If we use the defaults, then we could use these values to compute a unique id for each vertex in the pixel. The function, `x + y*2` will produce a unique hash of the inputs for the domain we care about. The following table shows the unique values. 

| Vertex | TexCoord.x | TexCoord.y | Unique Id |
| ------ | ---------- | ---------- | --------- |
| S      | 0          | 0          | 0         |
| D      | 1          | 0          | 1         |
| F      | 1          | 1          | 3         |
| G      | 0          | 1          | 2         |

The unique value is important, because it gives the vertex shader the ability to know _which_ vertex is being processed, rather than _any_ arbitrary vertex. For example, now the shader can know if it is processing `S`, or `D` based on if the unique id is `0` and `1`. The math for mapping `S` --> `A` may be quite different than the math for mapping `D` --> `a`. 

Additionally, the default `TexCoord` values allow the vertex shader to take any arbitrary position,  (`S`, `D`, `F`, and `G`) , and produce the point `P` where the `SpriteBatch` is drawing the sprite in world space. Recall from the previous chapter that MonoGame uses the screen size as a basis for generating world space positions, and then the default projection matrix transforms those world space positions into clip space. Given a shader parameter, `float2 ScreenSize`,  the vertex shader can convert back from the world-space positions  (`S`, `D`, `F`, and `G`)  to the `P` position by subtracting `.5 * ScreenSize * TexCoord` from the current vertex. 

The `Color` data is used to tint the resulting sprite in the pixel shader, but in our use case for a shadow hull, we do not really need a color whatsoever. Instead, we can use this `float4` field as arbitrary data. The trick is that we will need to pack whatever data we need into a `float4` and pass it via the `Color` type in MonoGame. This color comes from the `Color` value passed to the `SpriteBatch`'s `Draw()` call. 

The `Position` and `Color` both use `float4` in the standard vertex shader input, and it _may_ appear as though they should have the same amout precision. However they are not passed from MonoGame's `SpriteBatch` as the same type. When `SpriteBatch` goes to draw a sprite, it uses a `Color` for the `Color`, and a `Vector3` for the `Position`. A `Color` has 4 `bytes`, but a `Vector3` has 12 `bytes`. This can be seen in the [`VertexPositionColorTexture`](https://github.com/MonoGame/MonoGame/blob/develop/MonoGame.Framework/Graphics/Vertices/VertexPositionColorTexture.cs#L103) class. The takeaway is that we can only pack a third as much data into the `Color` semantic as the `Position` gets, and that may limit the types of values we want to pack into the `Color` value. 

Finally, the light's position must be provided as a shader parameter, `float2 LightPosition`. The light's position should be in the same world-space coordinate system that the light is being drawn at itself. 


### Vertex Shader Theory

Now that we have a good understanding of the available inputs, and the goal of the vertex function, we can begin moving towards a solution. Unlike the previous chapters, we are going to build up a fair bit of math before converting any of this is to a working shader. To begin, we draw the pixel _at_ the start of the line segment itself, `A`. The position where the pixel is drawn is definitionally `P`, so by drawing the pixel at `A`, we have set `A = P`. 

Every point (`S`, `D`, `F`, and `G`) needs to find `P`. To do that, the `TexCoord` can be treated as a direction from `P` to the current point, and the `ScreenSize` shader parameter can be used to find the right amount of distance to travel along that direction:

[!code-hlsl[](./snippets/snippet-9-02.hlsl)]

Next, we pack the `Color` value as the vector `(B - A)`. The `x` component of the vector can live in the `red` and `green` channels of the `Color`, and the `y` component will live in the `blue` and `alpha` channels. In the vertex shader, `B` can be derived by unpacking the `(B - A)` vector from the `COLOR` semantic and _adding_ it to the `A`. The reason we pack the _difference_ between `B` and `A` into the `Color`, and not `B` itself is due the lack of precision in the `Color` type. There are only 4 `bytes` to pack all the information, which means 2 `bytes` per `x` and `y`. Likely, the line segment will be small, so the values of `(B - A)` will fit easier into a 2 `byte` channel:

[!code-hlsl[](./snippets/snippet-9-03.hlsl)]

The point `a` must lay _somewhere_ on the ray cast from the `LightPosition` to the start of the line segment, `A`. Additionally, the point `a` must lay _beyond_ `A` from the light's perspective. The direction of the ray can be calculated as:

[!code-hlsl[](./snippets/snippet-9-04.hlsl)]

Then, given some `distance`,  beyond `A`, the point `a` can be produced as:

[!code-hlsl[](./snippets/snippet-9-05.hlsl)]

The same can be said for `b`:

[!code-hlsl[](./snippets/snippet-9-06.hlsl)]

Now the vertex shader function knows all positions, `A`, `a`, `b`, and `B`. The `TexCoord` can be used to derive a unique id, and the unique id can be used to select one of the points:

[!code-hlsl[](./snippets/snippet-9-07.hlsl)]

Once all of the positions are mapped, our goal is complete! We have a vertex function and strategy to convert a single pixel's 4 vertices into the 4 vertices of a shadow hull! 

### Implementation

To start implementing the effect, create a new Sprite Effect in the `MonoGameLibrary`'s common content effect folder called `shadowHullEffect.fx`. Load it into the `Core` class as before in the previous chapters. 

| ![Figure 9-5: Create the `shadowHullEffect` in MGCB](./images/mgcb.png) |
| :---------------------------------------------------------------------: |
|          **Figure 9-5: Create the `shadowHullEffect` in MGCB**          |

1. Add it as a class member:

[!code-csharp[](./snippets/snippet-9-08.cs)]

2. Load it as watched content in the `LoadContent()` method:

[!code-csharp[](./snippets/snippet-9-09.cs)]

3. Make sure to call `Update()` on the `Material` in the `Core`'s `Update()` method. Without this, hot-reload will not work:

[!code-csharp[](./snippets/snippet-9-10.cs)]

To represent the shadow casting objects in the game, we will create a new class called `ShadowCaster` in the _MonoGameLibrary_'s graphics folder. For now, keep the `ShadowCaster` class as simple as possible while we build the basics. It will just hold the positions of the line segment from the theory section, `A`, and `B`. Create the class and follow the steps to integrate it with the rest of the project.

1. Create the class:

[!code-csharp[](./snippets/snippet-9-11.cs)]

2. In the `GameScene`, add a class member to hold all the various `ShadowCasters` that will exist in the game:

[!code-csharp[](./snippets/snippet-9-12.cs)]

3. For now, for simplicity, re-configure the `InitializeLights()` function in the `GameScene` to have a single `PointLight` and a single `ShadowCaster`:

[!code-csharp[](./snippets/snippet-9-13.cs)]


Every `PointLight` needs its own `ShadowBuffer`. Recall that the `ShadowBuffer` is the off-screen texture that will have _white_ pixels where the light is visible, and _black_ pixels where the light is not visible due to shadow. 

1. Add a new `RenderTarget2D` field to the `PointLight` class:

[!code-csharp[](./snippets/snippet-9-14.cs)]

2. And instantiate the `ShaderBuffer` in the `PointLight`'s constructor:

[!code-csharp[](./snippets/snippet-9-15.cs)]


Now, we need to find a place to render the `ShadowBuffer` _per_ `PointLight` before the deferred renderer draws the light itself. 

1. Copy this function into the `PointLight` class:

[!code-csharp[](./snippets/snippet-9-16.cs)]

> [!warning] 
> 
> The `(B-A)` vector isn't being packed into the color channel, _yet_.
> We will come back to that soon!

2. Next, create a second method that will call the `DrawShadowBuffer` function for a list of lights and shadow casters:

[!code-csharp[](./snippets/snippet-9-17.cs)]

3. And finally, call the `DrawShadows()` method right before the `GameScene` calls the `DeferredRenderer`'s `StartLightPass()` method:

[!code-csharp[](./snippets/snippet-9-18.cs)]

4. For debug visualization purposes, add this snippet at the end of the `GameScene`'s `Draw()` just so you can see the `ShaderBuffer` as we debug it:

[!code-csharp[](./snippets/snippet-9-19.cs)]

When you run the game, you will see a totally blank game (other than the GUI). This is because the shadow map is currently being cleared to `black` to start, and the debug view renders that on top of everything else.

| ![Figure 9-6: A blank shadow buffer](./images/shadow_map_blank.png) |
| :-----------------------------------------------------------------: |
|                **Figure 9-6: A blank shadow buffer**                |

We cannot implement the vertex shader theory until we can pack the `(B-A)` vector into the `Color` argument for the `SpriteBatch`. For the sake of brevity, we will skip over the derivation of these functions. If you would like to know more, research bit-packing. 

1. Add this function to your `PointLight` class:

[!code-csharp[](./snippets/snippet-9-20.cs)]

2. And then to use the packing function, in the `DrawShadowBuffer` function, instead of passing `Color.White` like before, we need to create the `bToA` vector, pack it a `Color`, and then pass it to the `SpriteBatch`:

[!code-csharp[](./snippets/snippet-9-21.cs)]

3. On the shader side, add this function to your `shadowHullEffect.fx` file:

[!code-hlsl[](./snippets/snippet-9-22.hlsl)]

Now we have the tools to start implementing the vertex shader! Of course, anytime you want to override the default `SpriteBatch` vertex shader, the shader needs to fulfill the world-space to clip-space transformation. We can re-use the work done in the [previous chapter](./../08_light_effect/index.md). 

1. Replace the `VertexShaderOutput` struct with the `#include "3dEffect.fxh"` line. 

2. Create a basic template for the vertex shader function:

[!code-hlsl[](./snippets/snippet-9-23.hlsl)]

3. And do not forget to set the technique for the vertex shader function:

[!code-hlsl[](./snippets/snippet-9-24.hlsl)]

4. The last step to make sure the default vertex shader works is to pass the `MatrixTransform` and `ScreenSize` shader parameters in the `GameScene`'s `Update()` loop, next to where they are being configured for the existing `PointLightMaterial`:

[!code-csharp[](./snippets/snippet-9-25.cs)]

The pixel shader function for the `shadowHullEffect` needs to ignore the `input.Color` and just return a solid color:

[!code-hlsl[](./snippets/snippet-9-26.hlsl)]

The vertex shader function is derived from the theory section above, but is written out in complete form below:

[!code-hlsl[](./snippets/snippet-9-27.hlsl)]

Now if you run the game, you will see the white shadow hull. 

| ![Figure 9-7: A shadow hull](./images/shadow_map.png) |
| :---------------------------------------------------: |
|             **Figure 9-7: A shadow hull**             |

To get the basic shadow effect working with the rest of the renderer, we need to do the multiplication step between the `ShadowBuffer` and the `LightBuffer` in the `pointLightEffect.fx` shader. 

1. Add a second texture and sampler for the `pointLightEffect.fx` file:

[!code-hlsl[](./snippets/snippet-9-28.hlsl)]

2. Then, in the `MainPS` of the light effect, read the current value from the shadow buffer:

[!code-hlsl[](./snippets/snippet-9-29.hlsl)]

3. And use it as a multiplier at the end when calculating the final light color:

[!code-hlsl[](./snippets/snippet-9-30.hlsl)]

4. Before running the game, make sure to pass the `ShadowBuffer` to the point light's draw invocation. 
   
   In the `Draw()` method in the `PointLight` class, change the `SpriteBatch` to use `Immediate` sorting, and forward the `ShadowBuffer` to the shader parameter for each light:

[!code-csharp[](./snippets/snippet-9-31.cs)]

Disable the debug visualization to render the `ShadowMap` on top of everything else, and run the game. 

| ![Figure 9-8: The light is appearing inverted](./images/shadow_map_backwards.png) |
| :--------------------------------------------------------------------------------: |
|                  **Figure 9-8: The light is appearing inverted**                  |

Oops, the shadows and lights are appearing opposite of where they should! That is because the `ShadowBuffer` is inverted. 

1. Change the clear color for the `ShadowBuffer` to _white_:

[!code-csharp[](./snippets/snippet-9-32.cs)]

2. And change the pixel shader to return a solid black rather than white:

[!code-hlsl[](./snippets/snippet-9-33.hlsl)]

And now the shadow appears correctly for our simple single line segment!

| ![Figure 9-9: A working shadow!](./images/shadow_map_working.png) |
| :----------------------------------------------------------------: |
|                 **Figure 9-9: A working shadow!**                 |

## More Segments

So far, we have built up an intuition for the shadow caster system using a single line segment. Now, we will combine many line segments to create primitive shapes. We will approximate the slime character as a hexagon. 

1. Instead of only having `A` and `B` in the `ShadowCaster` class, change the class body to have a `Position` and a list of points:

[!code-csharp[](./snippets/snippet-9-34.cs)]

2. To create simple polygons, add this method to the `ShadowCaster` class:

[!code-csharp[](./snippets/snippet-9-35.cs)]

3. Then, in the `InitializeLights()` function in the `GameScene`, instead of constructing a `ShadowCaster` with the `A` and `B` properties, we can use the new `SimplePolygon` method:

[!code-csharp[](./snippets/snippet-9-36.cs)]

Finally, the last place we need to change is the `DrawShadowBuffer()` method. Currently it is just drawing a single pixel with the `ShadowHullMaterial`, but now we need to draw a pixel _per_ line segment. 

Update the `foreach` block to loop over all the points in the `ShadowCaster`, and connect the points as line segments:

[!code-csharp[](./snippets/snippet-9-37.cs)]

When you run the game, you will see a larger shadow shape.

| ![Figure 9-10: A shadow hull from a hexagon](./images/shadow_map_hex.png) |
| :-----------------------------------------------------------------------: |
|               **Figure 9-10: A shadow hull from a hexagon**               |

There are a few problems with the current effect. First off, there is a visual artifact going horizontally through the center of the shadow caster where it appears light is "leaking" in. This is likely due to numerical accuracy issues in the shader. A simple solution is to slightly extend the line segment in the vertex shader. After both `A` and `B` are calculated, but before `a` and `b`, add this to the shader:

[!code-hlsl[](./snippets/snippet-9-38.hlsl)]

And now the visual artifact has gone away.

| ![Figure 9-11: The visual artifact has been fixed](./images/shadow_map_hex_2.png) |
| :-------------------------------------------------------------------------------: |
|                **Figure 9-11: The visual artifact has been fixed**                |

The next item to consider is that the the "inside" of the slime isn't being lit. All of the segments are casting shadows, but it would be nice if only the segments on the far side of the slime cast shadows. We can take advantage of the fact that all of the line segments making up the shadow caster are _wound_ in the same direction:

[!code-hlsl[](./snippets/snippet-9-39.hlsl)]

> [!tip]
> This technique is called [back-face culling](https://en.wikipedia.org/wiki/Back-face_culling).


And then in the pixel shader function, add this line to the top. The [`clip`](https://learn.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-clip) function will completely discard the fragment and not draw anything to the `ShadowBuffer`:

[!code-hlsl[](./snippets/snippet-9-40.hlsl)]

Now the slime looks well lit and shadowed! In the next section, we will line up the lights and shadows with the rest of the level. 

| ![Figure 9-12: The slime is well lit](./images/shadow_map_hex_3.png) |
| :------------------------------------------------------------------: |
|                **Figure 9-12: The slime is well lit**                |

## Gameplay

Now that we can draw shadows in the lighting system, we should rig up shadows to the slime, the bat, and the walls of the dungeon. 

1. First, start by adding back the `InitializeLights()` method as it existed at the start of the chapter. Feel free to add or remove lights as you see fit. Here is a version of the function:

[!code-csharp[](./snippets/snippet-9-41.cs)]

2. Now, we will focus on the slime shadows. Add a new `List<ShadowCaster>` property to the `Slime` class:

[!code-csharp[](./snippets/snippet-9-42.cs)]

3. And in the `Slime`'s `Update()` method, add this snippet:

[!code-csharp[](./snippets/snippet-9-43.cs)]

4. Now, modify the `GameScene`'s `Draw()` method to create a master list of all the `ShadowCasters` and pass that into the `DrawShadows()` function:

[!code-csharp[](./snippets/snippet-9-44.cs)]

And now the slime has shadows around the segments!

| ![Figure 9-13: The slime has shadows](./gifs/snake_shadow.gif) |
| :------------------------------------------------------------: |
|             **Figure 9-13: The slime has shadows**             |

Next up, the bat needs some shadows! 

1. Add a `ShadowCaster` property to the `Bat` class:

[!code-csharp[](./snippets/snippet-9-45.cs)]

2. And instantiate it in the constructor:

[!code-csharp[](./snippets/snippet-9-46.cs)]

3. In the `Bat`'s `Update()` method, update the position of the `ShadowCaster`:

[!code-csharp[](./snippets/snippet-9-47.cs)]

4. And finally add the `ShadowCaster` to the master list of shadow casters during the `GameScene`'s `Draw()` method:

[!code-csharp[](./snippets/snippet-9-48.cs)]

And now the bat is casting a shadow as well!

| ![Figure 9-14: The bat casts a shadow](./gifs/bat_shadow.gif) |
| :-----------------------------------------------------------: |
|            **Figure 9-14: The bat casts a shadow**            |

Lastly, the walls should cast shadows to help ground the lighting in the world. 
Add a shadow caster in the `InitializeLights()` function to represent the edge of the playable tiles:

[!code-csharp[](./snippets/snippet-9-49.cs)]

| ![Figure 9-15: The walls have shadows](./gifs/wall_shadow.gif) |
| :------------------------------------------------------------: |
|            **Figure 9-15: The walls have shadows**             |

## The Stencil Buffer

The light and shadow system is working! However, there is a non-trivial amount of memory overhead for the effect. Every light has a full screen sized `ShadowBuffer`. At the moment, each `ShadowBuffer` is a `RenderTarget2D` with `32` bits of data per pixel. At our screen resolution of `1280` x `720`, that means every light adds roughly (`1280 * 720 * 32bits`) 3.6 _MB_ of overhead to the game! Our system is not taking full advantage of those 32 bits per pixel. Instead, all we really need is a _single_ bit, for "in shadow" or "not in shadow". In fact, all the `ShadowBuffer` is doing is operating as a _mask_ for the point light. 

Image masking is a common task in computer graphics. There is a built-in feature of MonoGame called the _Stencil Buffer_ that handles image masking without the need for any custom `RenderTarget` or shader logic. In fact, we will be able to remove a lot of the existing code and leverage the stencil instead. 

The stencil buffer is a part of an existing `RenderTarget`, but we need to opt into using it. In the `DeferredRenderer` class, where the `LightBuffer` is being instantiated, change the `preferredDepthFormat` to `DepthFormat.Depth24Stencil8`:

[!code-csharp[](./snippets/snippet-9-50.cs)]

The `LightBuffer` itself has `32` bits per pixel of `Color` data, _and_ an additional `32` bits of data split between the depth and stencil buffers. As the name suggests, the `Depth24Stencil8` format grants the depth buffer `24` bits of data, and the stencil buffer `8` bits of data. `8` bits are enough for a single `byte`, which means it can represent integers from `0` to `255`. 

For our use case, we will deal with the stencil buffer in two distinct steps. First, all of the shadow hulls will be drawn into the stencil buffer _instead_ of a unique `ShadowBuffer`. Anywhere a shadow hull is drawn, the stencil buffer will have a value of `1`, and anywhere without a shadow hull will have a value of `0`. Then, in the second step, when the point lights are drawn, the stencil buffer can be used as a mask where pixels are only drawn where the stencil buffer has a value of `0` (which means there was no shadow hull present in the previous step). 

The stencil buffer can be cleared and re-used between each light, so there is no need to have a buffer _per_ light. We will be able to completely remove the `ShadowBuffer` from the `PointLight` class. That also means we will not need to send the `ShadowBuffer` to the point light shader or read from it in shader code any longer. 

1. To get started, create a new method in the `DeferredRenderer` class called `DrawLights()`. This new method is going to completely replace some of our existing methods, but we will clean the unnecessary ones up when we are done with the new approach:

[!code-csharp[](./snippets/snippet-9-51.cs)]

2. In the `GameScene`'s `Draw()` method, call the new `DrawLights()` method instead of the `DrawShadows()`, `StartLightPhase()` _and_ `PointLight.Draw()` methods. Here is a snippet of the `Draw()` method:

[!code-csharp[](./snippets/snippet-9-52.cs)]

3. Next, in the `pointLightEffect.fx` shader, we will not be using the `ShadowBuffer` anymore, so remove the `Texture2D ShadowBuffer` and `sampler2D ShadowBufferSampler`. Remove the `tex2D` read from the shadow image, and remove the final multiplication of the `shadow`. The end of the `pointLightEffect.fx` shader should read as follows:

[!code-hlsl[](./snippets/snippet-9-53.hlsl)]

If you run the game now, you will not see any of the lights anymore. 

| ![Figure 9-16: Back to square one](./images/stencil_blank.png) |
| :------------------------------------------------------------: |
|              **Figure 9-16: Back to square one**               |

In the new `DrawLights()` method, we need to iterate over all the lights, and draw them. First, we need to set the current render target to the `LightBuffer` so it can be used in the deferred renderer composite stage:

[!code-csharp[](./snippets/snippet-9-54.cs)]

Now the lights are back, but of course no shadows yet. 

| ![Figure 9-17: Welcome back, lights](./images/stencil_lights.png) |
| :---------------------------------------------------------------: |
|               **Figure 9-17: Welcome back, lights**               |

As each light is about to draw, we need to draw the shadow hulls. Add this snippet to the top of the `foreach` loop. This code is mainly copied from our previous approach:

[!code-csharp[](./snippets/snippet-9-55.cs)]

This produces strange results. So far, the stencil buffer isn't being used yet, so all we are doing is rendering the shadow hulls onto the same image as the light data itself. Worse, the alternating order from rendering shadows to lights, back to shadows, and so on produces very visually decoherent results.

| ![Figure 9-18: Worse shadows](./images/stencil_pre.png) |
| :-----------------------------------------------------: |
|             **Figure 9-18: Worse shadows**              |

Instead of writing the shadow hulls as _color_ into the color portion of the `LightBuffer`, we only need to render the `1` or `0` to the stencil buffer portion of the `LightBuffer`. To do this, we need to create a new `DepthStencilState` variable. The `DepthStencilState` is a MonoGame primitive that describes how draw call operations should interact with the stencil buffer. 

1. Create a new class variable in the `DeferredRenderer` class: 

[!code-csharp[](./snippets/snippet-9-56.cs)]

2. And initialize it in the constructor:

[!code-csharp[](./snippets/snippet-9-57.cs)]

3. The `_stencilWrite` variable is a declarative structure that tells MonoGame how the stencil buffer should be used during a `SpriteBatch` draw call. The next step is to actually pass the `_stencilWrite` declaration into the `SpriteBatch`'s `Draw()` call when the shadow hulls are being rendered:

[!code-csharp[](./snippets/snippet-9-58.cs)]

Unfortunately, there is not a good way to visualize the state of the stencil buffer, so if you run the game, it is hard to tell if the stencil buffer contains any data. Instead, we will try and _use_ the stencil buffer's data when the point lights are drawn. The point lights will not interact with the stencil buffer in the same way the shadow hulls did. 

1. To capture the new behavior, create a second `DepthStencilState` class variable:

[!code-csharp[](./snippets/snippet-9-59.cs)]

2. And initialize it in the constructor:

[!code-csharp[](./snippets/snippet-9-60.cs)]

3. And now pass the new `_stencilTest` state to the `SpriteBatch` `Draw()` call that draws the point lights:

[!code-csharp[](./snippets/snippet-9-61.cs)]

The shadows look _better_, but something is still broken. It looks eerily similar to the previous iteration before passing the `_stencilTest` and `_stencilWrite` declarations to `SpriteBatch`...

| ![Figure 9-19: The shadows still look funky](./images/stencil_blend.png) |
| :----------------------------------------------------------------------: |
|              **Figure 9-19: The shadows still look funky**               |
This happens because the shadow hulls are _still_ being drawn as colors into the `LightBuffer`. The shadow hull shader is rendering a black pixel, so those black pixels are drawing on top of the `LightBuffer` 's previous point lights. To solve this, we need to create a custom `BlendState` that ignores all color channel writes. 

1. Create a new class variable in the `DeferredRenderer`:

[!code-csharp[](./snippets/snippet-9-62.cs)]

2. And initialize it in the constructor:

[!code-csharp[](./snippets/snippet-9-63.cs)]

> ![tip]
> 
> Setting the `ColorWriteChannels` to `.None` means that the GPU still rasterizes the geometry, but no color will be written to the `LightBuffer`.


3. Finally, pas it to the shadow hull `SpriteBatch` call:

[!code-csharp[](./snippets/snippet-9-64.cs)]

Now the shadows look closer, but there is one final issue. 

| ![Figure 9-20: The shadows are back](./images/stencil_noclear.png) |
| :----------------------------------------------------------------: |
|               **Figure 9-20: The shadows are back**                |

The `LightBuffer` is only being cleared at the start of the entire `DrawLights()` method. This means the `8` bits for the stencil data aren't being cleared between lights, so shadows from one light are overwriting into all subsequent lights. To fix this, we just need to clear the stencil buffer data before rendering the shadow hulls:

[!code-csharp[](./snippets/snippet-9-65.cs)]

And now the lights are working again! The final `DrawLights()` method is written below:

[!code-csharp[](./snippets/snippet-9-66.cs)]

| ![Figure 9-21: Lights using the stencil buffer](./gifs/stencil_working.gif) |
| :-------------------------------------------------------------------------: |
|              **Figure 9-21: Lights using the stencil buffer**               |

We can remove a lot of unnecessary code.
1. The `DeferredRenderer.StartLightPhase()` function is no longer called. Remove it.
2. The `PointLight.DrawShadows()` function is no longer called. Remove it.
3. The `PointLight.Draw()` function is no longer called. Remove it.
4. The `PointLight.DrawShadowBuffer()` function is no longer called. Remove it.
5. The `PointLight.ShadowBuffer` `RenderTarget` is no longer used. Remove it. Anywhere that referenced the `ShadowBuffer` can also be removed. 

## Improving The Look and Feel
TODO

- dither fall off
- box-blur
- stencil mask
- scissor test

## Conclusion

And with that, our lighting and shadow system is complete! In this chapter, you accomplished the following:

- Learned the theory behind generating 2D shadow geometry from a light and a line segment.
- Wrote a vertex shader to generate a "shadow hull" quad on the fly.
- Implemented a shadow system using a memory-intensive texture-based approach.
- Refactored the system to use the Stencil Buffer for masking.

In the final chapter, we will wrap up the series and discuss some other exciting graphics programming topics you could explore from here.

You can find the complete code sample for this tutorial series, [here](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Tutorials/2dShaders/src/09-Shadows-Effect/). 

Continue to the next chapter, [Chapter 10: Next Steps](../10_next_steps/index.md)