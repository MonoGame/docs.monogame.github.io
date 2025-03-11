// Get the current state of touch input.
TouchCollection touchCollection = TouchPanel.GetState();

foreach(TouchLocation touchLocation in touchCollection)
{
    if(touchLocation.State == TouchLocationState.Pressed || touchLocation.State == TouchLocationState.Moved)
    {
        // The the location at touchLocation.Position is currently being pressed,
        // so we can act on that information.
    }
}