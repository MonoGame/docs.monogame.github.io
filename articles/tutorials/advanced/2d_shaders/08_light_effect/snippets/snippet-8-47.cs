public void DebugDraw()
{
    // ...

    // the debug view for the normal buffer lives in the bottom-left.
    var normalBorderRect = new Rectangle(
    x: viewportBounds.X,
    y: viewportBounds.Height / 2,
    width: viewportBounds.Width / 2,
    height: viewportBounds.Height / 2);

    // shrink the normal rect by 8 pixels  
    var normalRect = normalBorderRect;
    normalRect.Inflate(-8, -8);

    // ...

	// draw a debug border for the normal buffer
    Core.SpriteBatch.Draw(Core.Pixel, normalBorderRect, Color.MintCream);

    // draw the normal buffer
    Core.SpriteBatch.Draw(NormalBuffer, normalRect, Color.White);

    Core.SpriteBatch.End();
}