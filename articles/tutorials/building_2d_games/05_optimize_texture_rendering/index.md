---
title: "Chapter 05: Optimizing Texture Rendering"
description: TODO: Write Description
---

In [Chapter 04](04-working-with-textures.md), you learned how to load and render textures using [**SpriteBatch**](). While rendering individual textures works well for simple games, it can lead to performance issues as your game grows more complex. In this chapter, we'll explore how to optimize texture rendering by reducing texture swaps and creating reusable components for better organization.

In this chapter, you will:
- Learn about texture swapping and its impact on performance.
- Understand what texture atlases are and how they can improve rendering efficiency.
- Set up a game library for sharing code across projects.
- Create a reusable `Sprite` class to simplify texture atlas usage.

By the end of this chapter, you'll understand how to organize your game's textures for optimal performance and have a flexible sprite system for your future game projects.

## Texture Swapping
Every time the [**SpriteBatch.Draw**]() method is executed when a different *texture* parameter than the previous [**SpriteBatch.Draw**]() method call, a *texture swap* occurs, unbinding the current texture on the GPU and binding the new texture.

> [!NOTE]
> A texture swap occurs when the GPU needs to switch between different textures during rendering. While each individual swap may seem trivial, the cumulative effect in a complex game can significantly impact performance.

For example, let's explore the following simplified draw calls for an example Pong game:

```cs
// Using the paddle texture to render the left player paddle.
// The paddle texture is bound to the GPU.
_spriteBatch.Draw(paddleTexture, leftPaddlePosition, Color.White);

// Using the ball texture to render the ball
// A texture swap occurs, unbinding the paddle texture to bind the ball texture.
_spriteBatch.Draw(ballTexture, ballPosition, Color.White);

// Reusing the paddle texture to draw the right player paddle.
// A texture swap occurs again, unbinding the ball texture to bind the paddle texture.
_spriteBatch.Draw(paddleTexture, rightPaddlePosition, Color.White);
```

In this example, the following occurs:

1. First paddle draw: The paddle texture is bound to the GPU.
2. Ball draw: The paddle texture is unbound from the GPU and then the ball texture is bound (Texture Swap #1).
3. Second paddle draw: The ball texture is unbound from the GPU and the paddle texture is bound (Texture Swap #2).

These texture swaps, while negligible in our simple Pong example, can become a performance issue in a full game where you might be drawing hundreds or thousands of sprites per frame.

> [!TIP]
> When profiling your game's performance, pay attention to the number of texture swaps occurring per frame. A high number of swaps can be an early indicator that your rendering pipeline needs optimization.

### Attempting to Optimize Draw Order
One approach to get around this could be to optimize the order of the draw calls to minimize texture swaps:

```cs
// Render the left and right paddles first.
// This reduces the number of texture swaps needed from two to one.
_spriteBatch.Draw(paddleTexture, _leftPaddlePosition, Color.White);
_spriteBatch.Draw(paddleTexture, _rightPaddlePosition, Color.White);
_spriteBatch.Draw(ballTexture, _ballPosition, Color.White);
```

This approach does reduce the texture swaps, however it is not a scalable solution. In a real game with dozens of different textures and complex draw orders for layered sprites, UI elements, particles, etc., managing draw order by texture becomes impractical and will conflict with desired visual layering.

## What is a Texture Atlas
A texture atlas (also known as a sprite sheet) is a large image file that contains multiple smaller images packed together. Instead of loading separate textures for each game element, you load the single texture file with all the images combined like a scrapbook where all your photos are arranged on the same page.

> [!NOTE]
> Using a texture atlas not only eliminates texture swaps but also reduces memory usage and simplifies asset management since you're loading and tracking a single texture instead of many individual ones.

In the Pong example, imagine taking the paddle and ball image and combining them into a single image file like in Figure 5-1 below:

<figure><img src="./images/pong-atlas.png" alt="Figure 5-1: Pong Texture Atlas Example."><figcaption><p><strong>Figure 5-1: Pong Texture Atlas Example.</strong></p></figcaption></figure>

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
    
    // All draw calls use the same texture, so there is no texture swapping!
    _spriteBatch.Draw(_textureAtlas, _leftPaddlePosition, _paddleSourceRect, Color.White);
    _spriteBatch.Draw(_textureAtlas, _rightPaddlePosition, _paddleSourceRect, Color.White);
    _spriteBatch.Draw(_textureAtlas, _ballPosition, _ballSourceRect, Color.White);
    
    _spriteBatch.End();
}
```

> [!TIP]
> When creating a texture atlas, organize related sprites close together and try to minimize empty space between sprites. This makes the atlas more efficient and easier to maintain.

## Defining `Sprite` Class
While using texture atlases solves the performance issues of texture swapping, managing multiple source rectangles and draw parameters for each sprite can become complex as your game grows. In the Pong game we are already tracking source rectangles for both the paddle and ball sprites. Imagine scaling this up to a game with dozens of different sprites, each potentially needing their own position, rotation, scale, and other rendering properties. 

To better organize this complexity, we can apply object-oriented design principles by creating a `Sprite` class that encapsulates all the information needed to render a sprite from our texture atlas. 

The following table lists the fields, properties, and methods needed for the `Sprite` class we will create:

| Property          | Type                  | Description                                                                                                          |
| ----------------- | --------------------- | -------------------------------------------------------------------------------------------------------------------- |
| `Texture`         | [**Texture2D**]()     | The source texture used when rendering the sprite.                                                                   |
| `SourceRectangle` | [**Rectangle**]()     | The boundary within the source texture to render.                                                                    |
| `Color`           | [**Color**]()         | The color tint to apply when rendering the sprite.                                                                   |
| `Rotation`        | `float`               | The amount of rotation, in radians, to apply when rendering the sprite.                                              |
| `Scale`           | [**Vector2**]()       | The scale factor to apply to the x- and y-axes when rendering the sprite.                                            |
| `Origin`          | [**Vector2**]()       | The xy-coordinate origin point, relative to the top-left corner, of the sprite.                                      |
| `Effects`         | [**SpriteEffects**]() | The [**SpriteEffects**]() value to apply when rendering to flip the sprite horizontally, vertically, or both.        |
| `LayerDepth`      | `float`               | The depth at which the sprite is rendered.                                                                           |
| `Width`           | `float`               | The width of the sprite, calculated by multiplying the width of the `_sourceRectangle` by the x-axis scale factor.   |
| `Height`          | `float`               | The height of the sprite, calculated by multiplying the height of the `_sourceRectangle` by the y-axis scale factor. |

| Method                         | Returns  | Description                                                                                              |
| ------------------------------ | -------- | -------------------------------------------------------------------------------------------------------- |
| `Sprite(Texture2D, Rectangle)` | `Sprite` | Creates a new instance of the `Sprite` class using source texture and source rectangle parameters given. |
| `Draw(SpriteBatch, Vector2)`   | `void`   | Draws the sprite using the [**SpriteBatch**]() provided at the specified position.                       |

> [!NOTE]
> The properties of the `Sprite` class directly correspond to the parameters used in [**SpriteBatch.Draw**](). This design makes it simple to encapsulate all rendering information while maintaining flexibility in how each sprite is displayed.

> [!TIP]
> The `Width` and `Height` properties automatically account for scaling, making it easier to perform calculations like collision detection or positioning without manually applying the scale factor each time.

## Creating a Game Library
Throughout this tutorial series, we'll be creating reusable classes like `Sprite` class defined above. Instead of adding these classes directly to our game project, we can create a separate class library project to contain them. This approach offers several advantages:

1. **Reusability**: The classes can be easily reused in future game projects by simply adding a reference to the library.
2. **Organization**: Keeps game-specific code separate from reusable framework code.
3. **Maintainability**: Changes to the library can benefit all games that use it.
4. **Testing**: Framework code can be tested independently of any specific game.

A class library is a project type that compiles into a Dynamic Link Library (DLL) instead of an executable. It contains reusable code that can be referenced by other projects, making it perfect for sharing functionality across multiple games.  MonoGame offers the *MonoGame Game Library* project template that can be used to create a class library.   

> [!NOTE]
> By using the *MonoGame Game Library* template instead of a standard class library template, we get the correct MonoGame framework references and configuration automatically set up. This saves time and ensures compatibility.

Let's use this template to add a new class library project that will house our game framework code:

1. In VSCode, open the *Command Palette* (View > Command Palette).   
2. Enter ".NET New Project" and select the command.
3. Enter "MonoGame Game Library" and select it as the template to use.
4. Name the project "MonoGameLibrary".
5. When prompted for a location, use the default option which will put the new project in a folder next to your game project.
6. Select "Create Project".

You should now see the *MonoGameLibrary* project listed as part of your solution in the Solution Explorer panel in VSCode

<figure><img src="./images/library-in-solution.png" alt="Figure 5-2: The MonoGameLibrary project added to the solution."><figcaption><p><strong>Figure 5-2: The MonoGameLibrary project added to the solution.</strong></p></figcaption></figure>

> [!TIP]
> The Solution Explorer panel in VSCode is provided by the C# Dev Kit extension that was installed in [Chapter 02](../02_getting_started/index.md#install-the-c-dev-kit-extension).  If you do not see this panel because you accidentally closed it, you can reopen it by opening the *Command Palette* (View > Command Palette), entering "Explorer: Focus on Solution ExplorerView", and selecting that command.

Now that we have the game library project created we need to add a reference to the library in our game project.  To do this:

1. In VSCode, in the Solution Explorer panel, right-click the MonoGameSnake project and choose *Add Project Reference*.
2. Choose *MonoGameLibrary* from the available options.

## Adding the `Sprite` class

Now that we have our library project set up and referenced, let's create the `Sprite` class we [defined in the previous section](#defining-sprite-class). Add a new folder to the MonoGameLibrary project called *Graphics*, then create a new file in that folder named *Sprite.cs* and add the following code:

```cs
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics;

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

> [!NOTE]
> The `Sprite` class is placed in the `MonoGameLibrary.Graphics` namespace to keep rendering-related classes organized together. As we add more functionality to our library, we'll continue to use namespaces to maintain a clean structure.

### Properties
The first two properties of the `Sprite` class are `Texture` and `SourceRectangle`.  `Texture` is a *get-only* property since there is no design reason it should ever change once the `Sprite` is created.  `SourceRectangle` however is a *get* property the setter is *protected*.  This will allow us to extend the use of this property later when we create animated sprites.  Following the instance members are the public properties for `Color`, `Rotation`, `Scale`, `Origin`, `Effects` and `LayerDepth`.  Each of these properties mirror the parameter values that can be supplied to the [**SpriteBatch.Draw**]() method, including the default values that are given.

Two additional properties are added for the `Width` and `Height`.  Since the `Sprite` itself represents the image within the texture atlas, the width and height should be based on the `SourceRectangle` width and height.  However, we have a property to scale the sprite, so the final width and height values need to take that into account by multiplying the values by the scale factor for that axis.

### Constructor
The constructor specifies that it requires a `Texture2D` and a `Rectangle` value be given, with these representing the source texture (texture atlas) and the boundary within the atlas where the sprite is.  Before storing the references, checks are made to ensure:

1. The `Texture2D` given is not null
2. The `Texture2D` given was not previously disposed of
3. The `Rectangle` given is a boundary that is contained within the `Texture2D`.

You might think that adding these checks are pointless, because when would you ever pass in a null or disposed texture, or provide a source rectangle that is out of bounds of the texture bounds. Of course you would never do this right?  Well, we're all human and sometimes we make mistakes.  It's always best to check yourself to be sure before you publish your game with bugs that could have been avoided.

> [!TIP]  
> Instead of throwing exceptions in the constructor when performing these checks,  `Debug.Assert` is used here.  This has a similar result as throwing an exception, except that the line of code is only ever executed when you run the code in a Debug build.  It asserts that the statement provided is true.  If the statement is false, then code execution will be paused at that line of code similar to if you add a breakpoint to debug.  This allows you to catch any issues while developing your game and running in a Debug build without needing to throw exceptions.  
>
> The `Debug.Assert` lines of code are also removed completely when you compile the project in a Release build, so you don't have to worry about debug specific code making its way into your final release.

### The Draw Method
Finally, we have the `Draw` method.  This method is responsible for rendering the sprite.  It requires a [**SpriteBatch**]() as parameter which will be the [**SpriteBatch**]() instance used to render the sprite, and a `Vector2` value that represents the position to render the sprite at. Then the [**SpriteBatch.Draw**]() method is executed using the [**SpriteBatch**]() given, using the fields and properties of the `Sprite` as the parameters, to render it at the position specified.

## Using the `Sprite` Class
