---
title: How to work with Touch Input
description: This topic demonstrates how to detect and use multi-touch input in a MonoGame game.
---

# Working with Touch Input

MonoGame supports multi-touch input on Mobile. The primary class that provides this support is [TouchPanel](xref:Microsoft.Xna.Framework.Input.Touch.TouchPanel), which can:

* Determine the touch capabilities of the current device.
* Get the current state of the touch panel.
* Detect touch gestures such as flicks, pinches, and drags. (For more information, see [Detecting Gestures on a multi-touch Screen](HowTo_Detect_Gestures.md).)

## Determining the Capabilities of the Touch Input Device

By using [TouchPanel.GetCapabilities](xref:Microsoft.Xna.Framework.Input.Touch.TouchPanel.GetCapabilities) you can determine if the touch panel is available. You also can determine the maximum touch count (the number of touches that can be detected simultaneously).

## To determine the capabilities of the touch device

1. Call [TouchPanel.GetCapabilities](xref:Microsoft.Xna.Framework.Input.Touch.TouchPanel), which returns a [TouchPanelCapabilities](xref:Microsoft.Xna.Framework.Input.Touch.TouchPanelCapabilities) structure.

2. Ensure [TouchPanelCapabilities.IsConnected](xref:Microsoft.Xna.Framework.Input.Touch.TouchPanelCapabilities) is **true**, indicating that the touch panel is available for reading.

3. You then can use the [TouchPanelCapabilities.MaximumTouchCount](xref:Microsoft.Xna.Framework.Input.Touch.TouchPanelCapabilities) property to determine how many touch points are supported by the touch panel.

> All touch panels for mobile return a [MaximumTouchCount](xref:Microsoft.Xna.Framework.Input.Touch.TouchPanelCapabilities) value of 4 in MonoGame.

The following code demonstrates how to determine if the touch panel is connected, and then reads the maximum touch count.

    ```csharp
    TouchPanelCapabilities tc = TouchPanel.GetCapabilities();
    if(tc.IsConnected)
    {
        return tc.MaximumTouchCount;
    }
    ```

## Getting multi-touch Data from the Touch Input Device

You can use [TouchPanel.GetState](xref:Microsoft.Xna.Framework.Input.Touch.TouchPanel) to get the current state of the touch input device. It returns a [TouchCollection](xref:Microsoft.Xna.Framework.Input.Touch.TouchCollection) structure that contains a set of [TouchLocation](xref:Microsoft.Xna.Framework.Input.Touch.TouchLocation) structures, each containing information about position and state for a single touch point on the screen.

## To read multi-touch data from the touch input device

1. Call [TouchPanel.GetState](xref:Microsoft.Xna.Framework.Input.Touch.TouchPanel) to get a [TouchCollection](xref:Microsoft.Xna.Framework.Input.Touch.TouchCollection) representing the current state of the device.

2. For each [TouchLocation](xref:Microsoft.Xna.Framework.Input.Touch.TouchLocation) in the [TouchCollection](xref:Microsoft.Xna.Framework.Input.Touch.TouchCollection), read the location and state data provided for each touch point.

The following code demonstrates how to get the current state of the touch input device and read touch data from each [TouchLocation](xref:Microsoft.Xna.Framework.Input.Touch.TouchLocation). It checks to see if a touch location has been pressed or has moved since the last frame, and if so, draws a sprite at the touch location.

    ```csharp
    // Process touch events
    TouchCollection touchCollection = TouchPanel.GetState();
    foreach (TouchLocation tl in touchCollection)
    {
        if ((tl.State == TouchLocationState.Pressed)
                || (tl.State == TouchLocationState.Moved))
        {
    
            // add sparkles based on the touch location
            sparkles.Add(new Sparkle(tl.Position.X,
                     tl.Position.Y, ttms));
    
        }
    }
    ```

## See Also

### Reference

[Microsoft.Xna.Framework.Input.Touch](xref:Microsoft.Xna.Framework.Input.Touch)  
[TouchPanel](xref:Microsoft.Xna.Framework.Input.Touch.TouchPanel)  
[TouchPanelCapabilities](xref:Microsoft.Xna.Framework.Input.Touch.TouchPanelCapabilities)  
[TouchLocation](xref:Microsoft.Xna.Framework.Input.Touch.TouchLocation)  
[TouchLocationState](xref:Microsoft.Xna.Framework.Input.Touch.TouchLocationState)  

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
