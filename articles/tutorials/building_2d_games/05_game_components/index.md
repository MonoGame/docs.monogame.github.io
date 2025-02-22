---
title: "Chapter 05: Game Components"
description: "Learn about MonoGame's component system and how to create reusable game components that automatically update and draw."
---

In [Chapter 03](../03_the_game1_file/index.md#the-game-loop) you learned about the [**Game**](xref:Microsoft.Xna.Framework.Game) class and how MonoGame manages the game loop through its [**Update**](xref:Microsoft.Xna.Framework.Game.Update(Microsoft.Xna.Framework.GameTime)) and [**Draw**](xref:Microsoft.Xna.Framework.Game.Draw(Microsoft.Xna.Framework.GameTime)) methods. While you could implement all your game's logic directly in these methods, MonoGame provides a component system that helps organize code into reusable pieces.

In this chapter, you will:

- Learn about MonoGame's component interfaces and base classes.
- Understand how components integrate with the game loop.
- Create reusable game components.
- Manage components through the game's component collection.

Let's start by understanding what game components are and how they work.

## Understanding Game Components

Game components are self-contained pieces of game functionality that can be easily added to or removed from a game. Components can:

- Automatically receive updates each frame.
- Handle their own drawing if needed.
- Be enabled or disabled as needed.
- Clean up their resources when disposed.

MonoGame provides several interfaces and base classes for creating components:

- [**IUpdateable**](xref:Microsoft.Xna.Framework.IUpdateable): Interface for components that need to update each frame.
- [**IDrawable**](xref:Microsoft.Xna.Framework.IDrawable): Interface for components that need to draw to the screen.
- [**GameComponent**](xref:Microsoft.Xna.Framework.GameComponent): Base class for updateable components.
- [**DrawableGameComponent**](xref:Microsoft.Xna.Framework.DrawableGameComponent): Base class for components that both update and draw.

### The IUpdatable Interface

The [**IUpdateable**](xref:Microsoft.Xna.Framework.IUpdateable) interface defines the contract for objects that can be updated each frame. It contains the following members:

```cs
public interface IUpdateable
{
    bool Enabled { get; }
    int UpdateOrder { get; }
    event EventHandler<EventArgs> EnabledChanged;
    event EventHandler<EventArgs> UpdateOrderChanged;
    void Update(GameTime gameTime);
}
```

Key aspects of [**IUpdateable**](xref:Microsoft.Xna.Framework.IUpdateable):

- [**Enabled**](xref:Microsoft.Xna.Framework.IUpdateable.Enabled): Controls whether the component should receive updates.
- [**UpdateOrder**](xref:Microsoft.Xna.Framework.IUpdateable.UpdateOrder): Determines the order components are updated relative to each other.
- [**Update**](xref:Microsoft.Xna.Framework.IUpdateable.Update(Microsoft.Xna.Framework.GameTime)): The method called each frame to update the component's state.

> [!NOTE]
> The [**EnabledChanged**](xref:Microsoft.Xna.Framework.IUpdateable.EnabledChanged) and [**UpdateOrderChanged**](xref:Microsoft.Xna.Framework.IUpdateable.UpdateOrderChanged) events notify when these properties change. This allows other components to react to these changes if needed.

### The IDrawable Interface

The [**IDrawable**](xref:Microsoft.Xna.Framework.IDrawable) interface defines the contract for objects that can draw to the screen. It contains the following members:

```cs
public interface IDrawable
{
    bool Visible { get; }
    int DrawOrder { get; }
    event EventHandler<EventArgs> VisibleChanged;
    event EventHandler<EventArgs> DrawOrderChanged;
    void Draw(GameTime gameTime);
}
```

Key aspects of [**IDrawable**](xref:Microsoft.Xna.Framework.IDrawable):

- [**Visible**](xref:Microsoft.Xna.Framework.IDrawable.Visible): Controls whether the component should be drawn.
- [**DrawOrder**](xref:Microsoft.Xna.Framework.IDrawable.DrawOrder): Determines the order components are drawn relative to each other.
- [**Draw**](xref:Microsoft.Xna.Framework.IDrawable.Draw(Microsoft.Xna.Framework.GameTime)): The method called each frame to draw the component.

> [!NOTE]
> Like [**IUpdateable**](xref:Microsoft.Xna.Framework.IUpdateable), the events notify when these properties change.

### The GameComponent Class

While you could implement [**IUpdateable**](xref:Microsoft.Xna.Framework.IUpdateable) directly, MonoGame provides the [**GameComponent**](xref:Microsoft.Xna.Framework.GameComponent) base class that handles most of the boilerplate code. Let's convert our existing `FramesPerSecondCounter` from [Chapter 04](../04_creating_a_class_library/index.md#creating-our-first-library-module) into a proper game component:

```cs
using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary;

/// <summary>
/// Tracks and calculates the number of frames rendered per second.
/// </summary>
public class FramesPerSecondCounter : GameComponent
{
    /// A static TimeSpan representing one second, used for FPS calculation intervals.
    private static readonly TimeSpan s_oneSecond = TimeSpan.FromSeconds(1);

    /// Tracks the number of frames rendered in the current second.
    private int _frameCounter;

    /// Tracks the elapsed time since the last FPS calculation.
    private TimeSpan _elapsedTime;

    /// <summary>
    /// Gets the current frames per second calculation.
    /// </summary>
    public float FramesPerSecond { get; private set; }

    /// <summary>
    /// Creates a new FramesPerSecondCounter.
    /// </summary>
    public FramesPerSecondCounter(Game game) : base(game)
    {
    }

    /// <summary>
    /// Updates the FPS calculation based on elapsed game time.
    /// </summary>
    /// <param name="gameTime">A snapshot of the game's timing values.</param>
    public override void Update(GameTime gameTime)
    {
        _elapsedTime += gameTime.ElapsedGameTime;

        if (_elapsedTime > s_oneSecond)
        {
            FramesPerSecond = _frameCounter;
            _frameCounter = 0;
            _elapsedTime -= s_oneSecond;
        }
    }

    /// <summary>
    /// Increments the frame counter. Should be called once per frame during the game's Draw method.
    /// </summary>
    public void UpdateCounter()
    {
        _frameCounter++;
    }
}
```

Key changes from our original implementation:

- The class now inherits from [**GameComponent**](xref:Microsoft.Xna.Framework.GameComponent).
- Added constructor requiring a [**Game**](xref:Microsoft.Xna.Framework.Game) reference.
- `Update` is now an override of the base component's method.

The [**GameComponent**](xref:Microsoft.Xna.Framework.GameComponent) class:

- Requires a reference to the [**Game**](xref:Microsoft.Xna.Framework.Game) instance it belongs to.
- Implements [**IUpdateable**](xref:Microsoft.Xna.Framework.IUpdateable) and handles the events.
- Virtual [**Initialize**](xref:Microsoft.Xna.Framework.GameComponent.Initialize) and [**Update**](xref:Microsoft.Xna.Framework.GameComponent.Update(Microsoft.Xna.Framework.GameTime)) methods you can override.

Using this component is simpler than our previous implementation.  Open *Game1.cs* and make the following changes:

1. In the constructor, replace the code creating the `FramesPerSecondCounter` with the following which will also add it to the game's component collection:

    ```cs
    // Create and add the component to the game's components collection
    _fpsCounter = new FramesPerSecondCounter(this);
    Components.Add(_fpsCounter);
    ```

2. In [**Update**](xref:Microsoft.Xna.Framework.Game.Update(Microsoft.Xna.Framework.GameTime)), remove the `_fpsCounter.Update(gameTime)` call.  This is no longer needed.

The [**Update**](xref:Microsoft.Xna.Framework.Game.Update(Microsoft.Xna.Framework.GameTime)) method should now look like it did originally:

```cs
protected override void Update(GameTime gameTime)
{
    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

    // TODO: Add your update logic here
    
    base.Update(gameTime);
}
```

> [!TIP]
> When `base.Update(gameTime)` is called, the game handles updating all [**IUpdateable**](xref:Microsoft.Xna.Framework.IUpdateable) components in the games's [**GameComponentCollection**](xref:Microsoft.Xna.Framework.GameComponentCollection).  You don't need to manually update them in your game's [**Update**](xref:Microsoft.Xna.Framework.Game.Update(Microsoft.Xna.Framework.GameTime)) method anymore.

Running the game now, the `FramesPerSecondCounter` is created, added to the game's component collection, and automatically updated.  You should still see the calculated FPS displayed as the game's window title.

### The DrawableGameComponent Class

For components that need both update and draw functionality, MonoGame provides the [**DrawableGameComponent**](xref:Microsoft.Xna.Framework.DrawableGameComponent) class. This class inherits from [**GameComponent**](xref:Microsoft.Xna.Framework.GameComponent) and implements [**IDrawable**](xref:Microsoft.Xna.Framework.IDrawable).

Let's modify our FPS counter so that it uses [**DrawableGameComponent**](xref:Microsoft.Xna.Framework.DrawableGameComponent) instead of [**GameComponent**](xref:Microsoft.Xna.Framework.GameComponent):

```cs
using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary;

/// <summary>
/// Tracks and calculates the number of frames rendered per second.
/// </summary>
public class FramesPerSecondCounter : DrawableGameComponent
{
    /// A static TimeSpan representing one second, used for FPS calculation intervals.
    private static readonly TimeSpan s_oneSecond = TimeSpan.FromSeconds(1);

    /// Tracks the number of frames rendered in the current second.
    private int _frameCounter;

    /// Tracks the elapsed time since the last FPS calculation.
    private TimeSpan _elapsedTime;

    /// <summary>
    /// Gets the current frames per second calculation.
    /// </summary>
    public float FramesPerSecond { get; private set; }

    /// <summary>
    /// Creates a new FramesPerSecondCounter.
    /// </summary>
    public FramesPerSecondCounter(Game game) : base(game)
    {
    }

    /// <summary>
    /// Updates the FPS calculation based on elapsed game time.
    /// </summary>
    /// <param name="gameTime">A snapshot of the game's timing values.</param>
    public override void Update(GameTime gameTime)
    {
        _elapsedTime += gameTime.ElapsedGameTime;

        if (_elapsedTime > s_oneSecond)
        {
            FramesPerSecond = _frameCounter;
            _frameCounter = 0;
            _elapsedTime -= s_oneSecond;
        }
    }

    /// <summary>
    /// Draws the FPS calculation to the window title.
    /// </summary>
    /// <param name="gameTime">A snapshot of the game's timing values.</param>
    public override void Draw(GameTime gameTime)
    {
        // Increment the frame counter only during draw.
        _frameCounter++;
        Game.Window.Title = $"FPS: {FramesPerSecond}";
    }
}
```

Key changes from the [**GameComponent**](xref:Microsoft.Xna.Framework.GameComponent) implementation:

- The class now inherits from [**DrawableGameComponent**](xref:Microsoft.Xna.Framework.DrawableGameComponent).
- Removed the `UpdateCounter` method.
- An override for the base component's `Draw` method was added.
- The update to the windows title was moved to the `Draw` method here.

[**DrawableGameComponent**](xref:Microsoft.Xna.Framework.DrawableGameComponent) provides:

- All the features of [**GameComponent**](xref:Microsoft.Xna.Framework.GameComponent).
- Implementation of [**IDrawable**](xref:Microsoft.Xna.Framework.IDrawable).
- Access to the [**GraphicsDevice**](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice).
- Virtual [**LoadContent**](xref:Microsoft.Xna.Framework.DrawableGameComponent.LoadContent),  [**UnloadContent**](xref:Microsoft.Xna.Framework.DrawableGameComponent.UnloadContent), and [**Draw**](xref:Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime)) methods you can override.

Using this component now is even simpler than our previous implementation.  Open *Game1.cs* and, in [**Draw**](xref:Microsoft.Xna.Framework.Game.Draw(Microsoft.Xna.Framework.GameTime)), remove the `_fpsCounter.Update` call and the `Window.Title` change.  The Draw method should now look like it did originally:

```cs
protected override void Draw(GameTime gameTime)
{
    GraphicsDevice.Clear(Color.CornflowerBlue);

    // TODO: Add your drawing code here

    base.Draw(gameTime);
}
```

> [!TIP]
> Just like in [**Update**](xref:Microsoft.Xna.Framework.Game.Update(Microsoft.Xna.Framework.GameTime)), when `base.Draw(gameTime)` is called, the game handles drawing all [**IDrawable**](xref:Microsoft.Xna.Framework.IUpdateable) components in the games's [**GameComponentCollection**](xref:Microsoft.Xna.Framework.GameComponentCollection).  You don't need to manually draw them in your game's [**Draw**](xref:Microsoft.Xna.Framework.Game.Draw(Microsoft.Xna.Framework.GameTime)) method anymore.

Running the game now, the `FramesPerSecondCounter` is created, added to the game's component collection, and automatically updated and drawn.  You should still see the calculated FPS displayed as the game's window title.

## Component Order and Dependencies

When you have multiple components, you might need to control their update and draw order. Components provide two ways to handle this:

1. Using the [**UpdateOrder**](xref:Microsoft.Xna.Framework.IUpdateable.UpdateOrder) and [**DrawOrder**](xref:Microsoft.Xna.Framework.IDrawable.DrawOrder) properties:

    ```cs
    public class ExampleComponent : DrawableGameComponent
    {
        public ExampleComponent(Game game) : base(game)
        {
            // Update after components with order 0.
            UpdateOrder = 1;
    
            // Draw after components with order 0.
            DrawOrder = 1; 
        }
    }
    ```

2. Using the [**GameComponentCollection**](xref:Microsoft.Xna.Framework.GameComponentCollection) to order components:

    ```cs
    // Components are updated/drawn in the order they're added
    // Updated/draw first.
    Components.Add(backgroundComponent);
    
    // Updated/draw second.
    Components.Add(playerComponent);
    
    // Updated/drawn last.
    Components.Add(uiComponent);
    ```

    > [!TIP]
    > Use [**UpdateOrder**](xref:Microsoft.Xna.Framework.IUpdateable.UpdateOrder) and [**DrawOrder**](xref:Microsoft.Xna.Framework.IDrawable.DrawOrder) when components have specific dependencies. Use the Components collection order for general layering.

## Conclusion

Let's review what you accomplished in this chapter:

- Learned about MonoGame's component interfaces and their purposes.
- Explored the [**GameComponent**](xref:Microsoft.Xna.Framework.GameComponent) and [**DrawableGameComponent**](xref:Microsoft.Xna.Framework.DrawableGameComponent) base classes.
- Converted our FPS counter into a proper game component.
- Understood how to manage component order and dependencies.

In the next chapter, we'll learn about the Content Pipeline and how to load game assets.

## Test Your Knowledge

1. What is the main difference between [**GameComponent**](xref:Microsoft.Xna.Framework.GameComponent) and [**DrawableGameComponent**](xref:Microsoft.Xna.Framework.DrawableGameComponent)?

   <details>
   <summary>Question 1 Answer</summary>

   > [**DrawableGameComponent**](xref:Microsoft.Xna.Framework.DrawableGameComponent) inherits from [**GameComponent**](xref:Microsoft.Xna.Framework.GameComponent) and adds drawing capabilities by implementing [**IDrawable**](xref:Microsoft.Xna.Framework.IDrawable). This makes it suitable for components that need both update and draw functionality.
   </details><br />

2. How does MonoGame handle calling [**Update**](xref:Microsoft.Xna.Framework.GameComponent.Update(Microsoft.Xna.Framework.GameTime)) and [**Draw**](xref:Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime)) on components?

   <details>
   <summary>Question 2 Answer</summary>

   > The [**Game**](xref:Microsoft.Xna.Framework.Game) class automatically calls [**Update**](xref:Microsoft.Xna.Framework.GameComponent.Update(Microsoft.Xna.Framework.GameTime)) during `base.Update()` and [**Draw**](xref:Microsoft.Xna.Framework.DrawableGameComponent.Draw(Microsoft.Xna.Framework.GameTime)) during `base.Draw()` on all components in its [**Components**](xref:Microsoft.Xna.Framework.Game.Components) collection. Components are processed in order based on their [**UpdateOrder**](xref:Microsoft.Xna.Framework.IUpdateable.UpdateOrder)/[**DrawOrder**](xref:Microsoft.Xna.Framework.IDrawable.DrawOrder) properties and the order they were added to the collection.
   </details><br />

3. What advantages did we gain by converting our FPS counter to a proper game component?

    <details>
    <summary>Question 3 Answer</summary>

    > Converting to a game component:
    >
    > - Eliminated manual `Update` and `Draw` calls in the game class.
    > - Provided automatic lifecycle management.
    > - Made the component more reusable.
    > - Added enable/disable functionality.
    >
    </details><br />
