---
title: "Chapter 06: Working with Textures"
description: Learn how to load and render textures using the MonoGame content pipeline and SpriteBatch.
---

Textures are images that are used in your game to represent the visual graphics to the player, commonly referred to as *Sprites*.  In [Chapter 05](../05_content_pipeline/index.md#loading-assets), you learned how to use the **Content Pipeline** to load the MonoGame *logo.png* texture and render it to the screen.

In this chapter, you will:

- Learn how to render a texture with the [**SpriteBatch**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch).
- Explore how to manipulate the way the texture is rendered using the parameters of the [**SpriteBatch.Draw**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw(Microsoft.Xna.Framework.Graphics.Texture2D,Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Color)) method.

## Drawing a Texture

When rendering in MonoGame, *render states*, properties of the [**GraphicsDevice**](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice) that affect how rendering is performed, need to be set.  When rendering 2D sprites, the [**SpriteBatch**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) class simplifies rendering by managing these render states for you.

> [!IMPORTANT]
> Although the [**SpriteBatch**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) makes it easier to manage the render states for the [**GraphicsDevice**](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice), it can also change states that you may have set manually, such as when you are performing 3D rendering.  Keep this in mind when mixing 2D and 3D rendering.

Three methods are used when rendering with the [**SpriteBatch**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch):

1. [**SpriteBatch.Begin**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode,Microsoft.Xna.Framework.Graphics.BlendState,Microsoft.Xna.Framework.Graphics.SamplerState,Microsoft.Xna.Framework.Graphics.DepthStencilState,Microsoft.Xna.Framework.Graphics.RasterizerState,Microsoft.Xna.Framework.Graphics.Effect,System.Nullable{Microsoft.Xna.Framework.Matrix})) prepares the Graphics Device for rendering, including the render states.
2. [**SpriteBatch.Draw**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw(Microsoft.Xna.Framework.Graphics.Texture2D,Microsoft.Xna.Framework.Rectangle,Microsoft.Xna.Framework.Color)) tells the [**SpriteBatch**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) what to render. This is usually called multiple times before [**SpriteBatch.End**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.End) and batches the draw calls for efficiency.
3. [**SpriteBatch.End**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.End) submits the draw calls that were batched to the graphics device to be rendered.

> [!NOTE]
> The order of method calls when rendering using the [**SpriteBatch**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) is important.  [**SpriteBatch.Begin**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode,Microsoft.Xna.Framework.Graphics.BlendState,Microsoft.Xna.Framework.Graphics.SamplerState,Microsoft.Xna.Framework.Graphics.DepthStencilState,Microsoft.Xna.Framework.Graphics.RasterizerState,Microsoft.Xna.Framework.Graphics.Effect,System.Nullable{Microsoft.Xna.Framework.Matrix})) must be called before any [**SpriteBatch.Draw**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw(Microsoft.Xna.Framework.Graphics.Texture2D,Microsoft.Xna.Framework.Rectangle,Microsoft.Xna.Framework.Color)) calls are made.  When finished, [**SpriteBatch.End**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.End) must be called before another [**SpriteBatch.Begin**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode,Microsoft.Xna.Framework.Graphics.BlendState,Microsoft.Xna.Framework.Graphics.SamplerState,Microsoft.Xna.Framework.Graphics.DepthStencilState,Microsoft.Xna.Framework.Graphics.RasterizerState,Microsoft.Xna.Framework.Graphics.Effect,System.Nullable{Microsoft.Xna.Framework.Matrix})) can be called.  If these methods are called out of order, an exception will be thrown.

As mentioned in [Chapter 03](../03_the_game1_file/index.md#the-game-loop), all rendering should be done inside the [**Draw**](xref:Microsoft.Xna.Framework.Game.Draw(Microsoft.Xna.Framework.GameTime)) method. The [**Draw**](xref:Microsoft.Xna.Framework.Game.Draw(Microsoft.Xna.Framework.GameTime)) method's responsibility is to render the game state that was calculated in [**Update**](xref:Microsoft.Xna.Framework.Game.Update(Microsoft.Xna.Framework.GameTime)); it should not contain any game logic or complex calculations.

At the end of [Chapter 05](../05_content_pipeline/index.md#loading-assets), you added the following code to the [**Draw**](xref:Microsoft.Xna.Framework.Game.Draw(Microsoft.Xna.Framework.GameTime)) method inside the `Game1.cs` file:

[!code-csharp[](./snippets/draw.cs?highlight=6-7,9-10,12-13)]

These lines initialize the [**SpriteBatch**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch), draw the logo at [**Vector2.Zero**](xref:Microsoft.Xna.Framework.Vector2.Zero) (0, 0), and submit the batch. When you ran the game, the logo appeared in the upper-left corner of the window:

| ![Figure 6-1: The MonoGame logo drawn to the game window](./images/logo-drawn.png) |
| :--------------------------------------------------------------------------------: |
|             **Figure 6-1: The MonoGame logo drawn to the game window**             |

The [**SpriteBatch.Draw**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw(Microsoft.Xna.Framework.Graphics.Texture2D,Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Color)) method we just used can be given the following parameters:

| Parameter  | Type                                                             | Description                                                                                                                                                     |
| ---------- | ---------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| *texture*  | [**Texture2D**](xref:Microsoft.Xna.Framework.Graphics.Texture2D) | The [**Texture2D**](xref:Microsoft.Xna.Framework.Graphics.Texture2D) to draw.                                                                                   |
| *position* | [**Vector2**](xref:Microsoft.Xna.Framework.Vector2)              | The X and Y coordinates at which the texture will be rendered, with the texture's origin being the upper-left corner of the image.                              |
| *color*    | [**Color**](xref:Microsoft.Xna.Framework.Color)                  | The color mask (tint) to apply to the image drawn. Specifying [**Color.White**](xref:Microsoft.Xna.Framework.Color.White) will render the texture with no tint. |

> [!TIP]
> Try adjusting the position and color parameters and see how they affect the image being drawn.

MonoGame uses a coordinate system where (0, 0) is at the screen's upper-left corner. X values increase moving right, and Y values increase moving down. Understanding this, we will try to center the logo on the game window.

To center content on the screen, we need to find the window's center point. We can access this using the [**Window.ClientBounds**](xref:Microsoft.Xna.Framework.GameWindow.ClientBounds) property from the [**Game**](xref:Microsoft.Xna.Framework.Game) class, which represents the rectangular bounds of the game window.  [**Window.ClientBounds**](xref:Microsoft.Xna.Framework.GameWindow.ClientBounds) exposes both  [**Width**](xref:Microsoft.Xna.Framework.Rectangle.Width) and [**Height**](xref:Microsoft.Xna.Framework.Rectangle.Height) properties for the window's dimensions in pixels.  By dividing these dimensions in half, we can calculate the window's center coordinates.  We can update our [**Draw**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw(Microsoft.Xna.Framework.Graphics.Texture2D,Microsoft.Xna.Framework.Rectangle,Microsoft.Xna.Framework.Color)) method to use this:

[!code-csharp[](./snippets/draw_center_wrong.cs?highlight=9-16)]

> [!TIP]  
> In the example above, we multiply the [**Vector2**](xref:Microsoft.Xna.Framework.Vector2) created by `0.5f` to halve the values instead of dividing it by `2.0f`. If you are not used to seeing this, it might seem strange at first, but it is actually an optimization technique. CPUs are able to perform multiplication operations much faster than division operations and reading `* 0.5f` is easily understood to be the same thing as `/ 2.0f`.

We have now set the position to half the window's dimensions, which should center the logo. Run the game to see the result.

| ![Figure 6-2: Attempting to draw the MonoGame logo centered on the game window](./images/logo-off-center.png) |
| :-----------------------------------------------------------------------------------------------------------: |
|               **Figure 6-2: Attempting to draw the MonoGame logo centered on the game window**                |

The logo is not centered as we expected it to be.  Even though we set the *position* parameter to the center of the game window, the texture starts drawing from its *origin*, which is the upper-left corner in this example.  So when we set the position to the screen's center, we are actually placing the logo's upper-left corner at that point, not the center of the texture.

One way to solve this is to subtract half the width and height of the texture from the center position of the game window, as shown below:

[!code-csharp[](./snippets/draw_center.cs?highlight=12-14)]

This offsets the position so that it correctly centers the image inside the game window.

| ![Figure 6-3: The MonoGame logo drawn centered on the game window](./images/logo-centered.png) |
| :--------------------------------------------------------------------------------------------: |
|              **Figure 6-3: The MonoGame logo drawn centered on the game window**               |

While this works, there is a better approach.  There is a different overload of the [**SpriteBatch.Draw**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw(Microsoft.Xna.Framework.Graphics.Texture2D,Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Color)) method that provides additional parameters for complete control over the draw operation, which we will use in the upcoming sections.

Update your code to:

[!code-csharp[](./snippets/draw_all_params.cs?highlight=10-22)]

This overload produces the same centered result but exposes all parameters that control rendering for a draw operation.  Unlike engines that abstract much of these details away, MonoGame provides explicit control for a flexible custom rendering pipeline.  Here is what each parameter does:

| Parameter         | Type                                                                     | Description                                                                                                                                                                                                                                                                                            |
| ----------------- | ------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| *texture*         | [**Texture2D**](xref:Microsoft.Xna.Framework.Graphics.Texture2D)         | The [**Texture2D**](xref:Microsoft.Xna.Framework.Graphics.Texture2D) to draw.                                                                                                                                                                                                                          |
| *position*        | [**Vector2**](xref:Microsoft.Xna.Framework.Vector2)                      | The X and Y position at which the texture will be rendered, relative to the *origin* parameter.                                                                                                                                                                                                        |
| *sourceRectangle* | [**Rectangle**](xref:Microsoft.Xna.Framework.Rectangle)                  | An optional region within the texture to be rendered in order to draw only a portion of the texture. Specifying `null` will render the entire texture.                                                                                                                                                 |
| *color*           | [**Color**](xref:Microsoft.Xna.Framework.Color)                          | The color mask (tint) to apply to the image drawn. Specifying [**Color.White**](xref:Microsoft.Xna.Framework.Color.White) will render the texture with no tint.                                                                                                                                        |
| *rotation*        | `float`                                                                  | The amount of rotation, in radians, to apply to the texture when rendering. Specifying `0.0f` will render the image with no rotation.                                                                                                                                                                  |
| *origin*          | [**Vector2**](xref:Microsoft.Xna.Framework.Vector2)                      | The X and Y coordinates defining the position of the texture origin. This affects the texture's offset during rendering and serves as a reference point around which the texture is rotated and scaled.                                                                                                |
| *scale*           | `float`                                                                  | The scale factor applied to the image across the x- and y-axes. Specifying `1.0f` will render the image at its original size with no scaling.                                                                                                                                                          |
| *effects*         | [**SpriteEffects**](xref:Microsoft.Xna.Framework.Graphics.SpriteEffects) | A [**SpriteEffects**](xref:Microsoft.Xna.Framework.Graphics.SpriteEffects) enum value that specifies if the texture should be rendered flipped across the horizontal axis, the vertical axis, or both axes.                                                                                            |
| *layerDepth*      | `float`                                                                  | Specifies the depth at which the texture is rendered. Textures with a higher layer depth value are drawn on top of those with a lower layer depth value. **Note: This value will only apply when using `SpriteSortMode.FrontToBack` or `SpriteSortMode.BackToFront`. We will cover this in a moment.** |

### Rotation

First we will explore the `rotation` parameter.  This value is the amount of rotation to apply to the sprite when rendering it.  We will rotate the texture 90° to make it vertical. Since rotation is measured in radians, not degrees, we can use the built-in math library in MonoGame to make the conversion for us by calling [**MathHelper.ToRadians**](xref:Microsoft.Xna.Framework.MathHelper.ToRadians(System.Single)).  Update the code to:

[!code-csharp[](./snippets/rotation.cs?highlight=17)]

Running the code now shows the rotated image, but not in the expected position:

| ![Figure 6-4: Attempting to draw the MonoGame logo rotated 90° and centered on the game window](./images/logo-rotated-offcenter.png) |
| :----------------------------------------------------------------------------------------------------------------------------------: |
|                   **Figure 6-4: Attempting to draw the MonoGame logo rotated 90° and centered on the game window**                   |

The reason the sprite did not rotate as expected is because of the `origin` parameter.  

### Origin

The `origin` parameter determines the reference point from which the sprite is rendered, rotated, and scaled. By default, if no origin is set, it will be [**Vector2.Zero**](xref:Microsoft.Xna.Framework.Vector2.Zero), the upper-left corner of the sprite. To visualize this, see *Figure 6-5* below. The red square represents where the origin is for the sprite, and we can see how it is rotated around this origin point.

| ![Figure 6-5: Demonstration of how a sprite is rotated around its origin](./videos/top-left-origin-rotation-example.webm) |
| :-----------------------------------------------------------------------------------------------------------------------: |
|                        **Figure 6-5: Demonstration of how a sprite is rotated around its origin**                         |

To resolve this rotation issue, we need to need to change two things:

1. Set the `origin` parameter to the center of the sprite instead of defaulting to the upper-left corner.
2. Change the `position` parameter back to the center of the screen.

Update the code to:

[!code-csharp[](./snippets/origin.cs?highlight=12-14,18-20)]

Moving the sprite's origin to its center not only solves the rotation problem, but also allows us to use the screen's center position directly, without the need for additional offset calculations. Running the game now shows the logo properly centered and rotated by 90°.

> [!NOTE]
> The `origin` parameter is based on the sprite's width and height, so the center will be half the width and half the height of the sprite.

| ![Figure 6-6: The MonoGame logo drawn rotated 90° and centered on the game window](./images/logo-rotated-centered.png) |
| :--------------------------------------------------------------------------------------------------------------------: |
|                  **Figure 6-6: The MonoGame logo drawn rotated 90° and centered on the game window**                   |

### Scale

The `scale` parameter specifies the amount of scaling to apply to the sprite when it is rendered.  The default value is `1.0f`, which can be read as "rendering the sprite at 1x the size".  Increasing this will scale up the size of the sprite and decreasing it will scale down the sprite.  

For this example, we will first reset the rotation back to `0.0f` (removing the 90° rotation we applied above) so we can clearly see the scaling effect.  Then we will set the scale of the logo sprite to `1.5f`

[!code-csharp[](./snippets/scale.cs?highlight=17,21)]

| ![Figure 6-7: The MonoGame logo drawn scaled at 1.5x the size](./images/logo-scaled-1.5x.png) |
| :-------------------------------------------------------------------------------------------: |
|                **Figure 6-7: The MonoGame logo drawn scaled at 1.5x the size**                |

Note that the sprite is scaled up from the center.  This is because the `origin` parameter is still set to the center of the sprite.  If we adjust the code so that the origin parameter is back in the upper-left corner, as in the listing below:

[!code-csharp[](./snippets/scale_no_origin.cs?highlight=18-19)]

Then the scaling is applied from the origin in the upper-left corner, producing the following result:

| ![Figure 6-8: The MonoGame logo drawn with a scale factor of 1.5x and the origin set in the upper-left corner](./images/logo-scaled-1.5x-zero-origin.png) |
| :-------------------------------------------------------------------------------------------------------------------------------------------------------: |
|                      **Figure 6-8: The MonoGame logo drawn with a scale factor of 1.5x and the origin set in the upper-left corner**                      |

Scaling can also be applied to the x- and y-axes independently by providing a [**Vector2**](xref:Microsoft.Xna.Framework.Vector2) value instead of a float value.  For instance, we can scale the x-axis of the sprite by 1.5x and reduce the scale of the y-axis to 0.5x:

[!code-csharp[](./snippets/scale_vector2.cs?highlight=21)]

Which will produce the following result:

| ![Figure 6-9: The MonoGame logo drawn scaled at 1.5x the size on the x-axis and 0.5x on the y-axis](./images/logo-scaled-1.5x-0.5x.png) |
| :-------------------------------------------------------------------------------------------------------------------------------------: |
|                  **Figure 6-9: The MonoGame logo drawn scaled at 1.5x the size on the x-axis and 0.5x on the y-axis**                   |

### SpriteEffects

The `effects` parameter is used to flip the sprite when rendered on either the horizontal axis, vertical axis, or both. The value for this parameter will be one of the [**SpriteEffects**](xref:Microsoft.Xna.Framework.Graphics.SpriteEffects) enum values.

| SpriteEffect                                                                                               | Description                                               |
| ---------------------------------------------------------------------------------------------------------- | --------------------------------------------------------- |
| [**SpriteEffects.None**](xref:Microsoft.Xna.Framework.Graphics.SpriteEffects.None)                         | No effect is applied and the sprite is rendered normally. |
| [**SpriteEffects.FlipHorizontally**](xref:Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally) | The sprite is rendered flipped along the horizontal axis. |
| [**SpriteEffects.FlipVertically**](xref:Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipVertically)     | The sprite is rendered flipped along the vertical axis.   |

For this example, we will reset the scale back to `1.0f` and apply the  [**SpriteEffects.FlipHorizontally**](xref:Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally) value to the sprite:

[!code-csharp[](./snippets/spriteeffects.cs?highlight=21,22)]

Which will produce the following result:

| ![Figure 6-10: The MonoGame logo flipped horizontally](./images/logo-flipped-horizontally.png) |
| :--------------------------------------------------------------------------------------------: |
|                    **Figure 6-10: The MonoGame logo flipped horizontally**                     |

The [**SpriteEffects**](xref:Microsoft.Xna.Framework.Graphics.SpriteEffects) enum value also uses the [`[Flag]`](https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-flagsattribute) attribute, which means we can combine both horizontal and vertical flipping together.  To do this, we use the [bitwise OR operator](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/bitwise-and-shift-operators#logical-or-operator-) `|`.  Update the `effect` parameter value to the following:

[!code-csharp[](./snippets/spriteeffects_flags.cs?highlight=22-23)]

Now the sprite is flipped both horizontally and vertically

| ![Figure 6-11: The MonoGame logo flipped horizontally and vertically](./images/logo-flipped-horizontally-and-vertically.png) |
| :--------------------------------------------------------------------------------------------------------------------------: |
|                            **Figure 6-11: The MonoGame logo flipped horizontally and vertically**                            |

### Color and Opacity

The `color` parameter applies a color mask to the sprite when it is rendered.  Note that this is not setting the actual color of the image, just a mask that is applied on top of the image.  The default value is [**Color.White**](xref:Microsoft.Xna.Framework.Color.White).  So if we are setting it to [**Color.White**](xref:Microsoft.Xna.Framework.Color.White), why does this not affect the tinting of the sprite drawn?

When the `color` parameter is applied, each color channel (Red, Green, Blue) of the sprite is multiplied by the corresponding channel in the `color` parameter, where each channel is represented as a value between `0.0f` and `1.0f`.  For [**Color.White**](xref:Microsoft.Xna.Framework.Color.White), all color channels are set to `1.0f` (255 in byte form), so the multiplication looks like this:

```sh
Final Red = Sprite Red * 1.0f
Final Green = Sprite Green * 1.0f
Final Blue = Sprite Blue * 1.0f;
```

Since multiplying by `1.0f` does not change the value, [**Color.White**](xref:Microsoft.Xna.Framework.Color.White) essentially preserves the original colors of the sprite.

For this example, we will reset the `effects` parameter back to [**SpriteEffects.None**](xref:Microsoft.Xna.Framework.Graphics.SpriteEffects.None) and update the `color` parameter to use [**Color.Green**](xref:Microsoft.Xna.Framework.Color.Green):

[!code-csharp[](./snippets/color.cs?highlight=16,22)]

This produces the following result:

| ![Figure 6-12: The MonoGame logo with a green color tint applied](./images/logo-green-tint.png) |
| :---------------------------------------------------------------------------------------------: |
|               **Figure 6-12: The MonoGame logo with a green color tint applied**                |

> [!NOTE]
> The icon and the word "GAME" in the logo look black after using [**Color.Green**](xref:Microsoft.Xna.Framework.Color.Green) because the Red, Blue and Green components of that color are (`0.0f`, `0.5f`, `0.0f`).  The Orange color used in the logo is [**Color.MonoGameOrange**](xref:Microsoft.Xna.Framework.Color.MonoGameOrange), which has the component values of (`0.9f`, `0.23f`, `0.0f`).  When multiplying the component values, the result is (`0.0f`, `0.125f`, `0.0f`) which would be Red 0, Green 31, Blue 0 in byte values.  So it is not quite fully black, but it is very close.
>
> This is why it is important to understand how the `color` parameter values are applied to the sprite when it is rendered.

To adjust the opacity of a sprite, we can multiply the `color` parameter value by a value between `0.0f` (fully transparent) and `1.0f` (fully opaque).  For instance, if we wanted to render the logo with 50% transparency we can multiply the `color` parameter by `0.5f` like this:

[!code-csharp[](./snippets/opacity.cs?highlight=16)]

Which will produce the following result:

| ![Figure 6-13: The MonoGame logo with half transparency](./images/logo-half-transparency.png) |
| :-------------------------------------------------------------------------------------------: |
|                   **Figure 6-13: The MonoGame logo with half transparency**                   |

### Source Rectangle

The `sourceRectangle` parameter specifies a region within the texture that should be rendered. So far, we have just set this parameter to `null`, which specifies that the full texture should be rendered. If we only wanted to render a portion of the texture as the sprite, we can set this parameter value.

For instance, take the logo image we have been using.  We can break it down into two distinct regions; the MonoGame icon and the MonoGame wordmark.

| ![Figure 6-14: The MonoGame logo broken down into the icon and wordmark regions](./images/logo-texture-regions.png) |
| :-----------------------------------------------------------------------------------------------------------------: |
|                  **Figure 6-14: The MonoGame logo broken down into the icon and wordmark regions**                  |

We can see from *Figure 6-14* above that the actual icon starts at position (0, 0) and is 128px wide and 128px tall. Likewise, the wordmark starts at position (150, 34) and is 458px wide and 58px tall. Knowing the starting position and the width and height of the region gives us a defined rectangle that we can use as the `sourceRectangle`.

We can see this in action by drawing the icon and the wordmark separately from the same texture. Update the code to the following:

[!code-csharp[](./snippets/sourcerectangle.cs?highlight=6-7,9-10,15-30,32-47)]

The following changes were made:

- Two new [**Rectangle**](xref:Microsoft.Xna.Framework.Rectangle) values called `iconSourceRect` and `wordmarkSourceRect` that represent the boundaries of the MonoGame icon and wordmark regions within the logo texture were added.
- The *sourceRectangle* parameter of the `_spriteBatch.Draw` call was updated to use the new `iconSourceRect` value. **Notice that we are still telling it to draw the `_logo` for the *texture*, we have just supplied it with a source rectangle this time.**
- The *origin* parameter was updated to use the width and height of the `iconSourceRect`. Since the overall dimensions of what we will be rendering has changed due to supplying a source rectangle, the origin needs to be adjusted to those dimensions as well.
- Finally, a second `_spriteBatch.Draw` call is made, this time using the `wordmarkSourceRect` as the source rectangle so that the wordmark is drawn.

If you run the game now, you should see the following:

| ![Figure 6-15: The MonoGame icon and wordmark, from the logo texture, centered in the game window](./images/icon-wordmark-centered.png) |
| :-------------------------------------------------------------------------------------------------------------------------------------: |
|                   **Figure 6-15: The MonoGame icon and wordmark, from the logo texture, centered in the game window**                   |

> [!NOTE]
> Making use of the `sourceRectangle` parameter to draw different sprites from the same texture is an optimization technique that we will explore further in the next chapter.

### Layer Depth

The final parameter to discuss is the `layerDepth` parameter. Notice that in *Figure 6-15* above, the word mark is rendered on top of the icon.  This is because of the order the draw calls were made; first the icon was rendered, then the word mark was rendered.

The [**SpriteBatch.Begin**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode,Microsoft.Xna.Framework.Graphics.BlendState,Microsoft.Xna.Framework.Graphics.SamplerState,Microsoft.Xna.Framework.Graphics.DepthStencilState,Microsoft.Xna.Framework.Graphics.RasterizerState,Microsoft.Xna.Framework.Graphics.Effect,System.Nullable{Microsoft.Xna.Framework.Matrix})) method contains several optional parameters, one of which is the `sortMode` parameter.  By default, this value is [**SpriteSortMode.Deferred**](xref:Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred), which means what is drawn is done so in the order of the [**SpriteBatch.Draw**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.DrawString(Microsoft.Xna.Framework.Graphics.SpriteFont,System.Text.StringBuilder,Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Color,System.Single,Microsoft.Xna.Framework.Vector2,System.Single,Microsoft.Xna.Framework.Graphics.SpriteEffects,System.Single)) calls.  Each subsequent call will be drawn visually on top of the previous call.

When [**SpriteSortMode.Deferred**](xref:Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred) is used, then the `layerDepth` parameter in the [**SpriteBatch.Draw**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.DrawString(Microsoft.Xna.Framework.Graphics.SpriteFont,System.Text.StringBuilder,Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Color,System.Single,Microsoft.Xna.Framework.Vector2,System.Single,Microsoft.Xna.Framework.Graphics.SpriteEffects,System.Single)) call is essentially ignored.  For instance, in the first `_spriteBatch.Draw` method call, update the `layerDepth` parameter to `1.0f`.

[!code-csharp[](./snippets/layerdepth.cs?highlight=29)]

Doing this should tell it to render on a layer above the wordmark since the icon is at `1.0f` and the wordmark is at `0.0f` for the `layerDepth`.  However, if you run the game now, you will see that no change actually happens; the wordmark is still drawn on top of the icon.

To make use of the `layerDepth` parameter, you need to set the `sortMode` to either [**SpriteSortMode.BackToFront**](xref:Microsoft.Xna.Framework.Graphics.SpriteSortMode.BackToFront) or [**SpriteSortMode.FrontToBack**](xref:Microsoft.Xna.Framework.Graphics.SpriteSortMode.FrontToBack).

| Sort Mode                                                                                          | Description                                                          |
| -------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------- |
| [**SpriteSortMode.BackToFront**](xref:Microsoft.Xna.Framework.Graphics.SpriteSortMode.BackToFront) | Sprites are sorted by depth in back-to-front order prior to drawing. |
| [**SpriteSortMode.FrontToBack**](xref:Microsoft.Xna.Framework.Graphics.SpriteSortMode.FrontToBack) | Sprites are sorted by depth in front-to-back order prior to drawing. |

Now we can see this in action. We have already set the `layerDepth` parameter of the icon to `1.0f`. Find the `_spriteBatch.Begin()` method call and update it to the following:

[!code-csharp[](./snippets/sortmode.cs?highlight=13)]

Now we are telling it to use the [**SpriteSortMode.FrontToBack**](xref:Microsoft.Xna.Framework.Graphics.SpriteSortMode.FrontToBack) sort mode, which will sort the draw calls so that those with a higher `layerDepth` will be drawn on top of those with a lower one.  Even though we did not change the order of the `_spriteBatch.Draw` calls, if you run the game now, you will see the following:

| ![Figure 6-16: The MonoGame icon drawn on top of the wordmark](./images/icon-on-top-of-wordmark.png) |
| :--------------------------------------------------------------------------------------------------: |
|                   **Figure 6-16: The MonoGame icon drawn on top of the wordmark**                    |

There are also two additional [**SpriteSortMode**](xref:Microsoft.Xna.Framework.Graphics.SpriteSortMode) values that can be used.  These, however, are situational and can have draw backs when using them, so understanding what they are for is important.

The first is [**SpriteSortMode.Texture**](xref:Microsoft.Xna.Framework.Graphics.SpriteSortMode.Texture).  This works similar to [**SpriteSortMode.Deferred**](xref:Microsoft.Xna.Framework.Graphics.SpriteSortMode.Deferred) in that draw calls happen in the order they are made.  However, before the draw calls are made, they are sorted by texture.  This can be helpful when using multiple textures to reduce texture swapping, however it can have unintended results with layering if you are not careful.

The second is [**SpriteSortMode.Immediate**](xref:Microsoft.Xna.Framework.Graphics.SpriteSortMode.Immediate).  When using this sort mode, when a draw call is made, it is immediately flushed to the GPU and rendered to the screen, ignoring the layer depth, instead of batched and drawn when [**SpriteBatch.End**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.End) is called. Using this can cause performance issues and should only be used when necessary.  We will discuss an example of using this in a later chapter when we discuss shaders, since with [**SpriteSortMode.Immediate**](xref:Microsoft.Xna.Framework.Graphics.SpriteSortMode.Immediate) you can adjust shader parameters for each individual draw call.

## Conclusion

In this chapter, you accomplished the following:

- You learned about the different parameters of the [**SpriteBatch.Draw**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw(Microsoft.Xna.Framework.Graphics.Texture2D,Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Color)) method and how they affect sprite rendering.
- You learned how the `rotation` parameter works and how to convert between degrees and radians using [**MathHelper.ToRadians**](xref:Microsoft.Xna.Framework.MathHelper.ToRadians(System.Single)).
- You learned how the `origin` parameter affects sprite positioning, rotation, and scaling.
- You learned how to use the `scale` parameter to resize sprites uniformly or along individual axes.
- You explored the [**SpriteEffects**](xref:Microsoft.Xna.Framework.Graphics.SpriteEffects) enum to flip sprites horizontally and vertically.
- You learned how the `color` parameter can be used to tint sprites and adjust their opacity.
- You used the `sourceRectangle` parameter to draw specific regions from a texture.
- You explored sprite layering using the `layerDepth` parameter and different [**SpriteSortMode**](xref:Microsoft.Xna.Framework.Graphics.SpriteSortMode) options.

In the next chapter, we will take what we have learned about working with textures and learn techniques to optimize rendering to reduce texture swapping.

## Test Your Knowledge

1. What is the purpose of the `origin` parameter in SpriteBatch.Draw, and how does it affect position, rotation and scaling?

    :::question-answer
    The `origin` parameter determines the reference point for the sprite's position, rotation, and scaling. When set to [**Vector2.Zero**](xref:Microsoft.Xna.Framework.Vector2.Zero), the sprite rotates and scales from its upper-left corner. When set to the center of the sprite, the sprite rotates and scales from its center. The origin point also affects where the sprite is positioned relative to the `position` parameter.
    :::

2. How can you adjust a sprite's opacity using [**SpriteBatch.Draw**](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.DrawString(Microsoft.Xna.Framework.Graphics.SpriteFont,System.Text.StringBuilder,Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Color,System.Single,Microsoft.Xna.Framework.Vector2,System.Single,Microsoft.Xna.Framework.Graphics.SpriteEffects,System.Single))?

    :::question-answer
    A sprite's opacity can be adjusted by multiplying the `color` parameter by a value between `0.0f` (fully transparent) and `1.0f` (fully opaque). For example, `Color.White * 0.5f` will render the sprite at 50% opacity.
    :::

3. How can you flip a sprite horizontally and vertically at the same time using SpriteEffects?

    :::question-answer
    To flip a sprite both horizontally and vertically, you can combine the SpriteEffects values using the bitwise OR operator (`|`):

    ```cs
    SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically
    ```

    :::

4. When using the `sourceRectangle` parameter, what information do you need to specify, and what is its purpose?

    :::question-answer
    The `sourceRectangle` parameter requires a [**Rectangle**](xref:Microsoft.Xna.Framework.Rectangle) value where the x- and y-coordinates specify the upper-left corner of the region within the texture and the width and height, in pixels, of the region.

    Its purpose is to specify a specific region within a texture to draw, allowing multiple sprites to be drawn from different parts of the same texture.
    :::
