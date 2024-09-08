---
title: How to update a game with Variable or Fixed Timing
description: Demonstrates how to set up the runtime to call your Update method using variable or fixed timing.
requireMSLicense: true
---

## Overview

Demonstrates how to set up the runtime to call your Update method using variable or fixed timing.

There are two techniques for setting how often your `Update` method is called. 

- **Fixed timing** (Default) means that **Update** is called each time a fixed interval of time has passed. Fixed timing guarantees that **Update** will be called, however, you may drop frames if the previous work needs to be interrupted to call **Update**.
- **Variable timing** means to call **Update** as soon as other work finishes; this implies that it is up to a game developer to ensure that your render loop happens quickly enough so that **Update** will be called often enough to exceed your minimum frame rate.

## To use a fixed time step

> [!NOTE]
> Fixed timing is the default set to `60 FPS` (or `0.0166667` seconds per frame).

1. Create a class that derives from [Game](xref:Microsoft.Xna.Framework.Game).

2. Set [IsFixedTimeStep](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_IsFixedTimeStep) to **true**.

    ```csharp
    this.IsFixedTimeStep = true;
    ```

    This causes the [Update](xref:Microsoft.Xna.Framework.Game) method to be called each time the fixed time interval has passed.

3. Set **TargetElapsedTime** to a fixed interval of time.

    This example sets the time between calls to 16 milliseconds.

    ```csharp
    this.TargetElapsedTime = TimeSpan.FromSeconds(1 / 30.0);    // Update() is called every 30 times each second / 30 FPS
    ```

    > [!NOTE]
    > In older samples, you might see `TargetElapsedTime` registered as `TimeSpan.FromTicks(333333)`, which is the same as 30 FPS.

## To use a variable time step

A variable timestep has as much benefits as it has drawbacks and care must be taken as it will directly affect your rendering if you are not careful.

1. Create a class that derives from [Game](xref:Microsoft.Xna.Framework.Game).

2. Set [IsFixedTimeStep](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_IsFixedTimeStep) to **false**.

    This causes the [Update](xref:Microsoft.Xna.Framework.Game) method to be called as often as possible, instead of being called on a fixed interval.

    ```csharp
    this.IsFixedTimeStep = false;
    ```

    > [!NOTE]
    > The [Ship Game](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.2/ShipGame) sample implements a variable time step depending on the `vsync` setting of the device.  It is a good place to start when evaluating variable timings for your game.
    >
    > The [Racing Game](https://github.com/SimonDarksideJ/XNAGameStudio/tree/803e678597b103c499c2f9c3a7128753292105bf/Samples/XNA-4-Racing-Game-Kit-master) sample on the [XNA Game Studio Archive](https://github.com/SimonDarksideJ/XNAGameStudio) (still based on XNA 4) is written to ONLY work in a `Fixed Time` loop due to its physics implementation.  Beware it is an awesome project.

## See Also

- [Ship Game](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.2/ShipGame)
- [Racing Game](https://github.com/SimonDarksideJ/XNAGameStudio/tree/803e678597b103c499c2f9c3a7128753292105bf/Samples/XNA-4-Racing-Game-Kit-master)

### Concepts

- [What Is the GameLoop?](../whatis/game_loop/index.md)

### Reference

- [Game.IsFixedTimeStep](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_IsFixedTimeStep)
- [Game.TargetElapsedTime](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_TargetElapsedTime)
- [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager)
