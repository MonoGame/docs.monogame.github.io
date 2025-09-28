---
title: "03: Touch Gesture Handling"
description: "Learn the fundamentals of touch gesture recognition in MonoGame - registration, detection, and processing."
---

Before proceeding with the cross-platform conversion, let us review how to handle touch input in MonoGame. The concepts discussed here apply to any MonoGame project. For our Dungeon Slime demo, touch input is managed by the Gum library, but understanding the fundamentals is still important.

## Gesture Registration

Before your application can respond to touch input, you must explicitly register for the gesture types you want to detect.

This is done during initialization:

```csharp
protected override void Initialize()
{
    TouchPanel.DisplayWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
    
    TouchPanel.DisplayHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

    TouchPanel.EnabledGestures = GestureType.Tap |
                                 GestureType.DoubleTap |
                                 GestureType.Hold |
                                 GestureType.Flick |
                                 GestureType.FreeDrag |
                                 GestureType.HorizontalDrag |
                                 GestureType.VerticalDrag |
                                 GestureType.Pinch |
                                 GestureType.DragComplete |
                                 GestureType.PinchComplete;

    base.Initialize();
}
```

### Available Gesture Types

| Gesture Type | Description | Common Use Cases |
|--------------|-------------|------------------|
| `Tap` | Quick touch and release | Button presses, object selection |
| `DoubleTap` | Two quick taps in succession | Zoom to fit, special actions |
| `Hold` | Touch and hold for extended time | Context menus, charged actions |
| `Flick` | Quick swipe motion | Throwing objects, page navigation |
| `FreeDrag` | Continuous dragging motion | Moving objects, drawing |
| `HorizontalDrag` | Dragging constrained to horizontal | Sliders, horizontal scrolling |
| `VerticalDrag` | Dragging constrained to vertical | Vertical scrolling, pull-to-refresh |
| `Pinch` | Two-finger pinch/spread motion | Zoom in/out, scaling |
| `DragComplete` | End of any drag operation | Finalize object placement |
| `PinchComplete` | End of pinch operation | Finalize zoom level |

> [!NOTE]
> Only register for gestures you actually need. Each enabled gesture type has a small performance cost.

## Gesture Detection Loop

Touch gestures are processed using a polling approach in your `Update` method. Use `TouchPanel.IsGestureAvailable` to check if gestures are waiting to be processed:

```csharp
protected override void Update(GameTime gameTime)
{
    // Process all available gestures each frame
    while (TouchPanel.IsGestureAvailable)
    {
        GestureSample gesture = TouchPanel.ReadGesture();
        ProcessGesture(gesture);
    }

    base.Update(gameTime);
}
```

### Why Use a Loop?

Multiple gestures can occur between frames, especially during complex touch interactions. The `while` loop ensures you process all queued gestures rather than missing any:

- **Single `if` statement** - Might miss gestures if multiple occur
- **`while` loop** - Processes all queued gestures until none remain

## Reading and Processing Gestures

Each gesture is read using `TouchPanel.ReadGesture()`, which returns a `GestureSample` containing the gesture details:

```csharp
void ProcessGesture(GestureSample gesture)
{
    switch (gesture.GestureType)
    {
        case GestureType.Tap:
            HandleTap(gesture.Position);
            break;
            
        case GestureType.Flick:
            HandleFlick(gesture.Position, gesture.Delta);
            break;
            
        case GestureType.Pinch:
            HandlePinch(gesture.Position, gesture.Position2);
            break;
            
        // Handle other gesture types...
    }
}
```

### GestureSample Properties

Each `GestureSample` provides different data depending on the gesture type:

- **`GestureType`** - The type of gesture detected
- **`Position`** - Primary touch point location (screen coordinates)
- **`Position2`** - Secondary touch point (used for pinch gestures)
- **`Delta`** - Movement vector for drag and flick gestures
- **`Timestamp`** - When the gesture occurred

### Example: Processing Different Gesture Data

```csharp
switch (gesture.GestureType)
{
    case GestureType.Tap:
        // Use: Position
        Vector2 tapLocation = gesture.Position;
        break;
        
    case GestureType.Flick:
        // Use: Position (start), Delta (direction/speed)
        Vector2 flickStart = gesture.Position;
        Vector2 flickDirection = gesture.Delta;
        break;
        
    case GestureType.Pinch:
        // Use: Position (finger 1), Position2 (finger 2)
        Vector2 finger1 = gesture.Position;
        Vector2 finger2 = gesture.Position2;
        break;
}
```

## Complete Example

Here is a minimal working example demonstrating all three concepts:

```csharp
public class TouchGame : Game
{
    protected override void Initialize()
    {
        // 1. Register for gestures
        TouchPanel.EnabledGestures = GestureType.Tap | GestureType.Flick;
        base.Initialize();
    }

    protected override void Update(GameTime gameTime)
    {
        // 2. Check for available gestures
        while (TouchPanel.IsGestureAvailable)
        {
            // 3. Read and process each gesture
            GestureSample gesture = TouchPanel.ReadGesture();
            
            switch (gesture.GestureType)
            {
                case GestureType.Tap:
                    Console.WriteLine($"Tap at {gesture.Position}");
                    break;
                    
                case GestureType.Flick:
                    Console.WriteLine($"Flick from {gesture.Position} with delta {gesture.Delta}");
                    break;
            }
        }
        
        base.Update(gameTime);
    }
}
```

## Dungeon Slime Sample

In the _DungeonSlime_ game, touch input is encapsulated in a new **TouchInfo** class, which internally uses the **TouchPanel** API to detect and process gestures. 

This class abstracts gesture handling and exposes methods like `IsTouchSwipeUp()`, `IsTouchSwipeDown()`, etc., making it easy to check for swipe actions. 

The **TouchInfo** class is then integrated into the **GameController** class, allowing the game logic to respond to touch gestures in a platform-agnostic way. This modular approach keeps input handling clean and maintainable, and ensures that touch support works seamlessly alongside keyboard and gamepad input.

### Extending Move Functions with Touch Gestures

To support touch input alongside keyboard and gamepad, extend your movement functions to include checks for swipe gestures. 

For example:

```csharp
public static bool MoveUp()
{
    return s_keyboard.WasKeyJustPressed(Keys.Up) ||
           s_keyboard.WasKeyJustPressed(Keys.W) ||
           s_gamePad.WasButtonJustPressed(Buttons.DPadUp) ||
           s_gamePad.WasButtonJustPressed(Buttons.LeftThumbstickUp) ||
           IsTouchSwipeUp(); <=== extended to support touch
}
```

This approach ensures your game responds to swipe gestures for movement, providing a consistent experience across touch, keyboard, and gamepad input.

## Conclusion

This foundation enables you to respond to touch input across iOS and Android platforms using MonoGame's cross-platform gesture system.
