---
title: "Chapter 05: Sprite Batching"
description: TODO: Write Description
---
# Chapter 05: Sprite Batching

In this chapter, we'll learn about texture atlases, an optimization technique for game rendering and how it can improve our game's performance. Building on the source rectangle concept from [Chapter 04](04-working-with-textures.md#drawing-texture-regions), we'll take that and the texture atlas concept and extend it to create a `Sprite` class.

When rendering 2D graphics in MonoGame using [**SpriteBatch**](), the goal is to minimize state changes by batching similar draw calls together.  You can do this by putting as many of your draw calls as possible between a single [**SpriteBatch.Begin**]() and [**SpriteBatch.End**]() block.

For example, here what the draw logic may look like for a simple pong game:

```cs
// Use the paddle texture to draw the left player paddle.
_spriteBatch.Draw(paddleTexture, leftPaddlePosition, Color.White);

// Use the ball texture to draw the ball.
_spriteBatch.Draw(ballTexture, ballPosition, Color.White);

// Reuse the paddle texture to draw the left player paddle.
_spriteBatch.Draw(paddleTexture, rightPaddlePosition, Color.White);
```

Here, the left paddle, the ball, and the right paddle are all drawn together in the same batch.  While putting them all within the same batch is important, another factor to consider is *texture swapping*.

## Texture Swapping
Every time the [**SpriteBatch.Draw**]() method is executed when a different *texture* parameter than the previous [**SpriteBatch.Draw**]() method call, MonoGame must perform what's called a texture swap on the GPU; an expensive operation that can impact performance.  Each time a texture swap occurs, the GPU has to unbind the current texture and bind the new texture.  For example, let's break down what happen on the GPu during the draw calls frm the Pong example above

1. First paddle draw: GPU binds the paddle texture
2. Ball draw: GPU must unbind paddle texture and bind ball texture (Texture Swap #1)
3. Second paddle draw: GPU must unbind ball texture and rebind paddle texture (Texture Swap #2)

These texture swaps, while negligible in our simple Pong example, can become a performance issue in a full game where you might be drawing hundreds or thousands of sprites per frame.

You might think we could optimize this by reordering our draw calls to minimize texture swaps:

```cs
// Optimized draw order - only one texture swap needed
_spriteBatch.Draw(_paddle, _leftPaddlePosition, Color.White);
_spriteBatch.Draw(_paddle, _rightPaddlePosition, Color.White);
_spriteBatch.Draw(_ball, _ballPosition, Color.White);
```

While this reduces texture swaps from two to one in our example, it's not a scalable solution. In a real game with dozens of different textures and complex draw orders (think layered sprites, UI elements, particles, etc.), managing draw order by texture becomes impractical and can conflict with desired visual layering.

A better solution to this is to use a *texture atlas*.

## What is a Texture Atlas

A texture atlas (also known as a sprite sheet) is a large image file that contains multiple smaller images packed together. Instead of loading separate textures for each game element, you load the single texture file with all the images combined like a scrapbook where all your photos are arranged on the same page.

In the Pong example, imagine taking the paddle and ball image and combining them into a single image file like in Figure 5-1 below:

<figure><img src="../images/05-texture-atlas/pong-atlas.png" alt="Figure 5-1: Pong Texture Atlas Example."><figcaption><p><strong>Figure 5-1: Pong Texture Atlas Example.</strong></p></figcaption></figure>

Now when we draw these images, we would be using the same texture and just specify the source rectangles for the paddle or ball when needed, completely eliminating texture swaps.

```cs
private Texture2D _textureAtlas;
private Rectangle _paddleSourceRect;
private Rectangle _ballSourceRect;

protected override void LoadContent()
{
    _textureAtlas = Content.Load<Texture2D>("pong-atlas");
    _paddleSourceRect = new Rectangle(0, 0, 32, 32);
    _ballSourceRect = new Rectangle(32, 0, 32, 32);
}

protected override void Draw(GameTime gameTime)
{
    GraphicsDevice.Clear(Color.CornflowerBlue);

    _spriteBatch.Begin();

    //  All draw calls use the same texture, so there is no texture swapping!
    _spriteBatch.Draw(_textureAtlas, _leftPaddlePosition, _paddleSourceRect, Color.White);
    _spriteBatch.Draw(_textureAtlas, _rightPaddlePosition, _paddleSourceRect, Color.White);
    _spriteBatch.Draw(_textureAtlas, _ballPosition, _ballSourceRect, Color.White);
    _spriteBatch.End();
}

```

## The `Sprite` class
In the examples above, we applied the concept of using a source rectangle to define a single image to be drawn from a source texture.  In a 2D game, we can think of each of these images we draw with the [**SpriteBatch.Draw**]() method as a *sprite*.  Even though they all use the same source `Texture2D` (texture atlas), each sprite will have different parameters to use, like the source rectangle, and other parameters such as *rotation*, *scale*, and *origin*.

Instead of creating ton of variables to track all of this information, we can apply object-oriented programming design and create a `Sprite` class that represents each sprite.  The properties of a `Sprite` should match the parameters used in the [**SpriteBatch.Draw**]() call so they can be used when rendering the sprite.  The following tables lists the field, properties, and methods the `Sprite` class well created.


| Property          | Type            | Description                                                                                                          |
| ----------------- | --------------- | -------------------------------------------------------------------------------------------------------------------- |
| `Texture`         | `Texture2D`     | The source texture used when rendering the sprite.                                                                   |
| `SourceRectangle` | `Rectangle`     | The boundary within the source texture to render.                                                                    |
| `Color`           | `Color`         | The color tint to apply when rendering the sprite.                                                                   |
| `Rotation`        | `float`         | The amount of rotation, in radians, to apply when rendering the sprite.                                              |
| `Scale`           | `Vector2`       | The scale factor to apply to the x- and y-axes when rendering the sprite.                                            |
| `Origin`          | `Vector2`       | The xy-coordinate origin point, relative to the top-left corner, of the sprite.                                      |
| `Effects`         | `SpriteEffects` | The `SpriteEffects` value to apply when rendering to flip the sprite horizontally, vertically, or both.              |
| `LayerDepth`      | `float`         | The depth at which the sprite is rendered.                                                                           |
| `Width`           | `float`         | The width of the sprite, calculated by multiplying the width of the `_sourceRectangle` by the x-axis scale factor.   |
| `Height`          | `float`         | The height of the sprite, calculated by multiplying the heigh tof the `_sourceRectangle` by the y-axis scale factor. |

| Methods                        | Return Type | Description                                                                                              |
| ------------------------------ | ----------- | -------------------------------------------------------------------------------------------------------- |
| `Sprite(Texture2D, Rectangle)` | `Sprite`    | Creates a new instance of the `Sprite` class using source texture and source rectangle parameters given. |
| `Draw(SpriteBatch, Vector2)`   | `void`      | Draws the sprite using the [**SpriteBatch**]() provided at the specified position.                             |

Knowing how to define our `Sprite` class, let's add it to our game project.  Add a new file named *Sprite.cs*, then replace the contents of the file with the following

```cs
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameSnake;

public class Sprite
{
    /// <summary>
    /// Gets the source texture used when rendering this Sprite.
    /// </summary>
    public Texture2D Texture { get; }

    /// <summary>
    /// Gets the source rectangle that represents the region within the source texture to use
    /// when rendering this Sprite.
    /// </summary>
    public Rectangle SourceRectangle { get; protected set; }

    /// <summary>
    /// Gets or Sets the color tint to apply when rendering this sprite.
    /// Default value is Color.White.
    /// </summary>
    public Color Color { get; set; } = Color.White;

    /// <summary>
    /// Gets or Sets the amount of rotation, in radians, to apply when rendering this sprite.
    /// Sprite is rotated around the Origin.
    /// Default value is 0.0f
    /// </summary>
    public float Rotation { get; set; } = 0.0f;

    /// <summary>
    /// Gets or Sets the scale factor to apply to the x- and y-axes when rendering this sprite.
    /// Sprite is scaled from the Origin.
    /// Default value is Vector2.One.
    /// </summary>
    public Vector2 Scale { get; set; } = Vector2.One;

    /// <summary>
    /// Gets or Sets the xy-coordinate origin point, relative to the top-left corner, of this sprite.
    /// Default value is Vector2.Zero
    /// </summary>
    public Vector2 Origin { get; set; } = Vector2.Zero;

    /// <summary>
    /// Gets or Sets whether this sprite should be flipped horizontally, vertically, or both, when rendered.
    /// Default value is SpriteEffects.None.
    /// </summary>
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;

    /// <summary>
    /// Gets or Sets the depth at which this sprite is rendered.
    /// Default value is 0.0f.
    /// </summary>
    public float LayerDepth { get; set; } = 0.0f;

    /// <summary>
    /// Gets the width of this sprite multiplied by the x-axis scale factor.
    /// </summary>
    public float Width => SourceRectangle.Width * Scale.X;

    /// <summary>
    /// Gets the height of this sprite, multiplied by the y-axis scale factor.
    /// </summary>
    public float Height => SourceRectangle.Height * Scale.Y;

    /// <summary>
    /// Creates a new Sprite instance using the source texture and source rectangle provided.
    /// </summary>
    /// <param name="texture">The source texture of the sprite.</param>
    /// <param name="sourceRectangle">The source rectangle to use when rendering the sprite.</param>
    public Sprite(Texture2D texture, Rectangle sourceRectangle)
    {
        Debug.Assert(texture is not null);
        Debug.Assert(!texture.IsDisposed);

        Texture = texture;
        SourceRectangle = sourceRectangle;
    }

    /// <summary>
    /// Draws this sprite using the SpriteBatch given at the position specified.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch to use when rendering this sprite.</param>
    /// <param name="position">The xy-coordinate position to render this sprite at.</param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        spriteBatch.Draw(Texture, position, SourceRectangle, Color, Rotation, Origin, Scale, Effects, LayerDepth);
    }
}
```

The first two properties of the `Sprite` class are `Texture` and `SourceRectangle`.  `Texture` is a *get-only* property since there is no design reason it should ever change once the `Sprite` is created.  `SourceRectangle` however is a *get* property the setter is *protected*.  This will allow us to extend the use of this property later when we create animated sprites.  Following the instance members are the public properties for `Color`, `Rotation`, `Scale`, `Origin`, `Effects` and `LayerDepth`.  Each of these properties mirror the parameter values that can be supplied to the [**SpriteBatch.Draw**]() method, including the default values that are given.

Two additional properties are added for the `Width` and `Height`.  Since the `Sprite` itself represents the image within the texture atlas, the width and height should be based on the `SourceRectangle` width and height.  However, we have a property to scale the sprite, so the final width and height values need to take that into account by multiplying the values by the scale factor for that axis.

The constructor specifies that it requires a `Texture2D` and a `Rectangle` value be given, with these representing the source texture (texture atlas) and the boundary within the atlas where the sprite is.  Before storing the references, checks are made to ensure:

1. The `Texture2D` given is not null
2. The `Texture2D` given was not previously disposed of
3. The `Rectangle` given is a boundary that is contained within the `Texture2D`.

You might think that adding these checks are pointless, because when would you ever pass in a null or disposed texture, or provide a source rectangle that is out of bounds of the texture bounds. Of course you would never do this right?  Well, we're all human and sometimes we make mistakes.  It's always best to check yourself to be sure before you publish your game with bugs that could have been avoided.

> [!TIP]  
> Instead of throwing exceptions in the constructor when performing these checks,  `Debug.Assert` is used here.  This has a similar result as throwing an exception, except that the line of code is only ever executed when you run the code in a Debug build.  It asserts that the statement provided is true.  If the statement is false, then code execution will be paused at that line of code similar to if you add a breakpoint to debug.  This allows you to catch any issues while developing your game and running in a Debug build without needing to throw exceptions.  
>
> The `Debug.Assert` lines of code are also removed completely when you compile the project in a Release build, so you don't have to worry about debug specific code making its way into your final release.

Finally, we have the `Draw` method.  This method is responsible for rendering the sprite.  It requires a [**SpriteBatch**]() as parameter which will be the [**SpriteBatch**]() instance used to render the sprite, and a `Vector2` value that represents the position to render the sprite at. Then the [**SpriteBatch.Draw**]() method is executed using the [**SpriteBatch**]() given, using the fields and properties of the `Sprite` as the parameters, to render it at the position specified.

## Using the `Sprite` Class
Now that we have the `Sprite` class defined, let's see it in action.  Recall the MonoGame logo that we added to our game in [Chapter 04](04-working-with-textures.md#drawing-texture-regions) when we broke it down into texture regions:

<figure><img src="../images/04-working-with-textures/logo-texture-regions.png" alt="Figure 5-2: The MonoGame logo broken down into texture regions."><figcaption><p><strong>Figure 5-2: The MonoGame logo broken down into texture regions.</strong></p></figcaption></figure>

This image contains is similar to a texture atlas.  Within the single image, it contains two separate images:

- The MonoGame Logo icon at region (x:0, y:0, width: 128px, height: 128px)
- The MonoGame Wordmark at region (x: 150, y:34, width: 458px, height: 58px)

Let's adjust the code we currently have to create `Sprite` instances from this texture.  First, open the *Game1.cs* file, find the `_logo` instance member field at the top and add the following:

```cs
private Sprite _monogameIcon;
private Sprite _monogameWordmark;
```

These will be the two `Sprite` instances we'll create for the icon and wordmark regions of the icon texture.  Next, locate the `LoadContent` method, and add the following after the icon texture is loaded:

```cs
_monogameIcon = new Sprite(_logo, new Rectangle(0, 0, 128, 128));
_monogameWordmark = new Sprite(_logo, new Rectangle(150, 34, 458, 58));
```

The above code takes the single `_logo` texture and uses it to create the `_monogameIcon` and `_monogameWordMark` sprites.The *sourceRectangle* parameter provided for each are based on the region boundaries shown in Figure 5-2 above.

Next, find the `Draw` method and replace the entire method with the following:

```cs
protected override void Draw(GameTime gameTime)
{
    GraphicsDevice.Clear(Color.CornflowerBlue);

    //  Calculates the center xy-coordinate of the game window
    Vector2 center = new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height) * 0.5f;

    _spriteBatch.Begin();
    _monogameIcon.Draw(_spriteBatch, center);
    _monogameWordmark.Draw(_spriteBatch, new Vector2(center.X, center.Y + _monogameIcon.Height));
    _spriteBatch.End();

    base.Draw(gameTime);
}
```

The new code for the `Draw` method is drawing both sprites using the [**SpriteBatch**](), but since both are using the same source `Texture2D`, no texture swapping occurs between the calls.  The `_monoGameIcon` is drawn at the center xy-coordinate of the game window.  The `_monogameWordmark` uses the `Height` property of the `_monogameIcon` to draw it directly below it.

If you run the game now, you should see something similar to the following:


<figure><img src="../images/05-sprite-batching/logo-wordmark-offcenter.png" alt="Figure 5-3: The MonoGame icon and wordmark."><figcaption><p><strong>Figure 5-3: The MonoGame icon and wordmark.</strong></p></figcaption></figure>








