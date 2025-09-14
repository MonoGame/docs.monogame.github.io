public void DebugDraw()
{
    // ...

    // the debug view for the normal buffer lives in the top-right.  
    var normalBorderRect = new Rectangle(
    x: viewportBounds.X,
    y: viewportBounds.Height / 2,
    width: viewportBounds.Width / 2,
    height: viewportBounds.Height / 2);

    // shrink the normal rect by 8 pixels  
    var normalRect = normalBorderRect;
    normalRect.Inflate(-8, -8);

    // ...

    // draw a debug border  
    Core.SpriteBatch.Draw(Core.Pixel, normalBorderRect, Color.MintCream);

    // draw the light buffer  
    Core.SpriteBatch.Draw(NormalBuffer, normalRect, Color.White);
}