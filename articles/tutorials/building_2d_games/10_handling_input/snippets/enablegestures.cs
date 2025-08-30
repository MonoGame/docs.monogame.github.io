protected override void Initialize()
{
    base.Initialize();
    
    // Enable gestures we want to handle.
    TouchPanel.EnabledGestures =
        GestureType.Tap |
        GestureType.HorizontalDrag |
        GestureType.VerticalDrag;
}