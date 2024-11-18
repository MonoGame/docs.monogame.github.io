---
title: "Chapter 03: The Game1 File"
description: In this chapter, we'll explore the contents of the Game1 file generated when a new MonoGame game project is created.
---

In the previous chapter, you created a new MonoGame project using the *MonoGame Cross-Platform Desktop Application* project template. Using the templates to create a new project will automatically generate files and a project structure as a starting point for a new MonoGame game application. While MonoGame offers different templates to create a new project based on target platform, all projects will contain the *Game1.cs* file.

> [!TIP]  
> For an in-depth look at all files created in a MonoGame project when using the MonoGame templates, refer to [Appendix 02: MonoGame Project Overview](../appendix_02_monogame_project_overview/index.md).

## Exploring the Game1 Class

At the core of a MonoGame project is the `Game` class.  This class handles the initialization of graphics services, initialization of the game, loading content, updating, and rendering the game.  When you create a new Monogame project, this `Game` class is implemented as the `Game1` class that you can customize as needed for your specific game.

> [!TIP]  
> While the default template names the class `Game1`, you're free to rename it to something more appropriate for your project.  However, for consistency, the documentation will continue to refer to it as `Game1`.

Locate the *Game1.cs* file that was generated when you created the MonoGame project and open it.  The default content will be:

```cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameSnake;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        base.Draw(gameTime);
    }
}

```

Let's take a closer look at what's happening here:
1. **Graphics and Rendering**: The class has two important member fields; `GraphicsDeviceManager` and `SpriteBatch`.  The `GraphicsDeviceManager` is responsible for initializing and managing the graphics device, which is the connection between your game and the computer's graphics processing unit (GPU).  The `SpriteBatch` provides an optimized method of rendering 2D graphics, like sprites, onto the screen.
2. **Initialization**: The `Game1` constructor sets up the initial configuration, such as the graphics manager, content directory, and mouse visibility.  The `Initialize` method is called once at the start, after the constructor, and allows you to perform additional setup tasks.
3. **Content Loading**: The `LoadContent` method is where you'll load assets needed for your game, such as images, audio, fonts, and models.  This method is only called once at the start of the game.
4. **Game Loop**: After initialization and content loading, the game enters a *game loop*.  A game loop is a series of methods that are called in sequence over and over until the game is told to exit.  In MonoGame, the game loop consists of two main methods:
    - `Update`: This is where you handle user input, update game state, and perform calculations such as physics and collisions.
    - `Draw`: This is where you render the game's visuals, drawing sprite and other graphics to the screen.

> [!CAUTION]
> The `Initialize` method includes a call to the `base.Initialize` method.  This call is very important in terms of what you can do before and after it.  When the base method is called, this is when the `GraphicsDevice` object is instantiated.  During the base method call is also when the `LoadContent` method is called.  So any initializations   that rely on either the `GraphicsDevice` object or any loaded assets should be done **after** the base method call and not **before** it.

Figure 3-1 below shows the lifecycle of a MonoGame game including the `Update` and `Draw` methods that make up the *game loop*.

<figure><img src="./images/monogame-lifecycle.png" alt="Figure 3-1: Lifecycle of a MonoGame game."><figcaption><p><em>Figure 3-1: Lifecycle of a MonoGame game.</em></p></figcaption></figure>

## The Game Loop

As just mentioned, MonoGame implements a *game loop* by calling `Update` and `Draw` over and over until the game is told to exit. Recall at the end of the previous chapter when you ran the project for the first time, I mentioned that there is a lot going on behind the scenes? This game loop is what I was referring to.

MonoGame is executing the `Update` method and then the `Draw` method 60 times per second. The `Update` method at the moment is not doing much, only checking for input from a controller or keyboard to determine if the game should exit. However, the `Draw` method is doing more than what it appears to at first glance.

Take a look at the `Draw` method

```cs
protected override void Draw(GameTime gameTime)
{
    GraphicsDevice.Clear(Color.CornflowerBlue);

    base.Draw(gameTime);
}
```

The first line is executing the `Clear` method of the `GraphicsDevice` property using the color `CornflowerBlue`. Recall that the `GraphicsDevice` object is your direct interface between the game and what is rendered to the screen. Every time the `Draw` method is called, this line of code of erasing the contents of the game window and refilling it with the color specified. Without clearing the contents of the screen first, every draw call would draw the new frame render over top of the previous render, and you'd end up with something like the old solitaire win screen

<figure><img src="./images/solitaire.webp" alt="Figure 3-2: Windows XP Solitaire Win Screen."><figcaption><p><em>Figure 3-2: Windows XP Solitaire Win Screen.</em></p></figcaption></figure>

While this can make for a neat effect, it's not something you want all the time. So, the screen is cleared and refilled with a solid color. You can test this yourself by modifying the code to use a different color, such as `Color.MonoGameOrange`, then running the game. (yes, there is a MonoGame Orange color).

Each time the game loops completes and the game is drawn to the screen, we call this a *frame*. So if MonoGame is running the game loop at 60 frames per second, that means it is performing and update and a render of each frame in 16ms. Notice that both the `Update` and the `Draw` methods both receive a parameter of the type `GameTime`. The `GameTime` parameter provides a snapshot of the timing values for the game, including the amount of time that it took for the previous frame to execute. This is commonly referred to as the *delta time*.

*Delta time* allows you to track time accurately for things such as animations and events based on *game time* and not the speed of the processor (CPU) on the machine running the game. While in ideal circumstances, the delta time will always be 16ms, there are any number of things that could cause a temporary slow down or hiccup in a frame, and using the delta time ensures that timing based events are always correct.

## Conclusion

Let's review what you accomplished in this chapter:

- You read through the default code provided in a *Game1.cs* file created by a MonoGame template.
- You learned about the lifecycle of a MonoGame game project.
- You learned what a game loop is and how it is implemented in MonoGame.

In the next chapter, you'll start working with sprites and learn how to load and render them.

## See Also

This chapter briefly touched on the *Game1.cs* file and the `Game` class. For an in-depth detailed discussion of all files created in a MonoGame project, including a full overview of the order of execution for a MonoGame game, see [Appendix 02: MonoGame Project Overview](../appendix_02_monogame_project_overview/index.md).

## Test Your Knowledge

1. Can the `Game1` class be renamed or is it required to be called `Game1`

    <details>
    <summary>Question 1 Answer</summary>

    > It is not a requirement that it be called `Game1`.  This is just the default name given to it by the templates when creating a new MonoGame game project.  

    </details><br />

2. What is the `SpriteBatch` used for?

    <details>
    <summary>Question 2 Answer</summary>

    > The `SpriteBatch` provides an optimized method of rendering 2D graphics, like sprites, onto the screen
    
    </details><br />

3. When is the `LoadContent` method executed and why is it important to know this?

    <details>
    <summary>Question 3 Answer</summary>

    > `LoadContent` is executed during the `base.Initialize()` method call within the `Initialize` method.  It's important to know this because anything being initialized that is dependent on content loaded should be done **after** the `base.Initialize()` call and not **before**. 
    
    </details><br />

4. How does MonoGame provide a *delta time* value?

    <details>
    <summary>Question 4 Answer</summary>

    > Through the `GameTime` parameter that is given to both the `Update` and the `Draw` methods.
    
    </details><br />
