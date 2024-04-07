---
title: What is the Game Loop
description: The MonoGame Framework Game class implements a game loop, which provides not only the window which displays your game, but also provides overloadable methods that your game implements to facilitate communication between your game and the operating system. This topic provides an overview of the basic functionality provided by the game loop.
---

# Initializing a Game

The MonoGame Framework [Game](xref:Microsoft.Xna.Framework.Game) class implements a game loop, which provides not only the window which displays your game, but also provides overloadable methods that your game implements to facilitate communication between your game and the operating system. This topic provides an overview of the basic functionality provided by the game loop.

* [Making a New Game](#making-a-new-game)
* [Game Loop Timing](#game-loop-timing)
* [Game Components](#fixed-step-game-loops)
* [Game Services](#game-services)
* [Game Components Consuming Game Services](#game-components-consuming-game-services)

## Making a New Game

The first step in creating a new game is to make a class that derives from [Game](xref:Microsoft.Xna.Framework.Game). The new class needs to override the following methods:

* Initialize - which is the method is responsible for game setup before the first frame of the game.
* Update - which is the method is responsible for handling game logic.
* Draw - which is the method responsible for drawing content to the screen.

## Game Loop Timing

A [Game](xref:Microsoft.Xna.Framework.Game) is either fixed step or variable step, defaulting to fixed step. The type of step determines how often [Update](xref:Microsoft.Xna.Framework.Game) will be called and affects how you need to represent time-based procedures such as movement and animation.

## Fixed-Step Game Loops

A fixed-step [Game](xref:Microsoft.Xna.Framework.Game) tries to call its **Update** method on the fixed interval specified in **TargetElapsedTime**. Setting **Game.IsFixedTimeStep** to **true** causes a [Game](xref:Microsoft.Xna.Framework.Game) to use a fixed-step game loop. A new MonoGame project uses a fixed-step game loop with a default **TargetElapsedTime** of 1/60th of a second.

In a fixed-step game loop, [Game](xref:Microsoft.Xna.Framework.Game) calls **Update** once the **TargetElapsedTime** has elapsed. After **Update** is called, if it is not time to call **Update** again, the [Game](xref:Microsoft.Xna.Framework.Game) calls **Draw**. After **Draw** is called, if it is not time to call **Update** again, [Game](xref:Microsoft.Xna.Framework.Game) idles until it is time to call **UpdateS**.

If **Update** takes too long to process, [Game](xref:Microsoft.Xna.Framework.Game) sets **IsRunningSlowly** to **true** and calls **Update** again, without calling **Draw** in between. When an update runs longer than the **TargetElapsedTime**, [Game](xref:Microsoft.Xna.Framework.Game) responds by calling **Update** extra times and dropping the frames associated with those updates to catch up. This ensures that **Update** will have been called the expected number of times when the game loop catches up from a slowdown. You can check the value of **IsRunningSlowly** in your **Update** if you want to detect dropped frames and shorten your **Update** processing to compensate. You can reset the elapsed times by calling **ResetElapsedTime**.

When your game pauses in the debugger, [Game](xref:Microsoft.Xna.Framework.Game) will not make extra calls to **Update** when the game resumes.

### Variable-Step Game Loops

A variable-step game calls its **Update** and **Draw** methods in a continuous loop without regard to the [TargetElapsedTime](xref:Microsoft.Xna.Framework.Game). Setting [Game.IsFixedTimeStep](xref:Microsoft.Xna.Framework.Game) to **false** causes a [Game](xref:Microsoft.Xna.Framework.Game) to use a variable-step game loop.

### Animation and Timing

For operations that require precise timing, such as animation, the type of game loop your game uses (fixed-step or variable-step) is important.

Using a fixed step allows game logic to use the **TargetElapsedTime** as its basic unit of time and assume that **Update** will be called at that interval. Using a variable step requires the game logic and animation code to be based on **ElapsedGameTime** to ensure smooth gameplay. Because the **Update** method is called immediately after the previous frame is drawn, the time between calls to **Update** can vary. Without taking the time between calls into account, the game would seem to speed up and slow down. The time elapsed between calls to the **Update** method is available in the **Update** method's _gameTime_ parameter. You can reset the elapsed times by calling **ResetElapsedTime**.

When using a variable-step game loop, you should express rates—such as the distance a sprite moves—in game units per millisecond (ms). The amount a sprite moves in any given update can then be calculated as the rate of the sprite times the elapsed time. Using this approach to calculate the distance the sprite moved ensures that the sprite will move consistently if the speed of the game or computer varies.

## Game Components

Game components provide a modular way of adding functionality to a game. You create a game component by deriving the new component either from the [GameComponent](xref:Microsoft.Xna.Framework.GameComponent) class, or, if the component loads and draws graphics content, from the [DrawableGameComponent](xref:Microsoft.Xna.Framework.DrawableGameComponent) class. You then add game logic and rendering code to the game component by overriding **GameComponent.Update**, **DrawableGameComponent.Draw** and **GameComponent.Initialize**. A game component is registered with a game by passing the component to **Game.Components.Add**. A registered component will have its initialize, update, and draw methods called from the **Game.Initialize**, **Game.Update**, and **Game.Draw** methods, respectively.

## Game Services

Game services are a mechanism for maintaining loose coupling between objects that need to interact with each other. Services work through a mediator—in this case, [Game.Services](xref:Microsoft.Xna.Framework.Game.Services). Service providers register with [Game.Services](xref:Microsoft.Xna.Framework.Game.Services), and service consumers request services from [Game.Services](xref:Microsoft.Xna.Framework.Game.Services). This arrangement allows an object that requires a service to request the service without knowing the name of the service provider.

Game services are defined by an interface. A class specifies the services it provides by implementing interfaces and registering the services with [Game.Services](xref:Microsoft.Xna.Framework.Game.Services). A service is registered by calling **Game.Services.AddService** specifying the type of service being implemented and a reference to the object providing the service. For example, to register an object that provides a service represented by the interface IMyService, you would use the following code.

```csharp
    Services.AddService( typeof( IMyService ), myobject );
```

Once a service is registered, the object providing the service can be retrieved by **Game.Services.GetService** and specifying the desired service. For example, to retrieve [IGraphicsDeviceService](xref:Microsoft.Xna.Framework.Graphics.IGraphicsDeviceService), you would use the following code.

```csharp
    IGraphicsDeviceService graphicsservice = (IGraphicsDeviceService)Services.GetService( typeof(IGraphicsDeviceService) );
```

## Game Components Consuming Game Services

The [GameComponent](xref:Microsoft.Xna.Framework.GameComponent) class provides the [Game](xref:Microsoft.Xna.Framework.GameComponent.Game) property so a [GameComponent](xref:Microsoft.Xna.Framework.GameComponent) can determine what [Game](xref:Microsoft.Xna.Framework.Game) it is attached to. With the [Game](xref:Microsoft.Xna.Framework.GameComponent.Game) property, a [GameComponent](xref:Microsoft.Xna.Framework.GameComponent) can call [Game.Services.GetService](xref:Microsoft.Xna.Framework.GameServiceContainer) to find a provider of a particular service. For example, a [GameComponent](xref:Microsoft.Xna.Framework.GameComponent) would find the [IGraphicsDeviceService](xref:Microsoft.Xna.Framework.Graphics.IGraphicsDeviceService) provider by using the following code.

```csharp
    IGraphicsDeviceService graphicsservice = (IGraphicsDeviceService)Game.Services.GetService( typeof( IGraphicsDeviceService ) );
```

## Related articles

[Creating a Full-Screen Game](../howto/graphics/HowTo_FullScreen.md)

Demonstrates how to start a game in full-screen mode.

[Resizing a Game](../howto/HowTo_PlayerResize.md)

Demonstrates how to resize an active game window.

[Restricting Aspect Ratio on a Graphics Device](../howto/graphics/HowTo_AspectRatio.md)

Demonstrates how to create a custom [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager) that only selects graphics devices with widescreen aspect ratios in full-screen mode.

[Automatic Rotation and Scaling](../howto/HowTo_AutomaticRotation.md)

Describes rotation and scaling in the MonoGame Framework on Mobile Platforms.

---

© 2012 Microsoft Corporation. All rights reserved.

© 2023 The MonoGame Foundation.
