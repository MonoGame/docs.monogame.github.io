public void DebugDraw()
{
    // ...

    // the debug view for the light buffer lives in the top-right.
    // var lightBorderRect = new Rectangle(
    //     x: viewportBounds.Width / 2, 
    //     y: viewportBounds.Y, 
    //     width: viewportBounds.Width / 2,
    //     height: viewportBounds.Height / 2);

    var lightBorderRect = viewportBounds; // TODO: remove this; it makes the light rect take up the whole screen.

    // ...
}