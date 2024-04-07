---
title: How to update a game with Variable or Fixed Timing
description: Demonstrates how to set up the runtime to call your Update method using variable or fixed timing.
---

# Updating with Variable or Fixed Timing

Demonstrates how to set up the runtime to call your Update method using variable or fixed timing.

There are two techniques for setting how often your **Update** method is called. Variable timing means to call **Update** as soon as other work finishes; this implies that it is up to a game developer to ensure that your render loop happens quickly enough so that **Update** will be called often enough to exceed your minimum frame rate. Fixed timing means that **Update** is called each time a fixed interval of time has passed. Fixed timing guarantees that **Update** will be called, however, so you may drop frames if the previous work needs to be interrupted to call **Update**.

## To use a variable time step

1. Create a class that derives from [Game](xref:Microsoft.Xna.Framework.Game).

2. Set [IsFixedTimeStep](xref:Microsoft.Xna.Framework.Game.IsFixedTimeStep) to **false**.

    This causes the [Update](xref:Microsoft.Xna.Framework.Game) method to be called as often as possible, instead of being called on a fixed interval.

    ```csharp
    this.IsFixedTimeStep = false;
    ```

## To use a fixed time step

1. Create a class that derives from [Game](xref:Microsoft.Xna.Framework.Game).

2. Set [IsFixedTimeStep](xref:Microsoft.Xna.Framework.Game.IsFixedTimeStep) to **true**.

    ```csharp
    this.IsFixedTimeStep = true;
    ```

    This causes the [Update](xref:Microsoft.Xna.Framework.Game) method to be called each time the fixed time interval has passed.

3. Set **TargetElapsedTime** to a fixed interval of time.

    This example sets the time between calls to 16 milliseconds.

    ```csharp
    this.TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 16);    // Update() is called every 16 milliseconds
    ```

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
