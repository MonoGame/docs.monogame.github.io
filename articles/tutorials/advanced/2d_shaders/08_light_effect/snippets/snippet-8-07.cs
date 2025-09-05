public void DebugDraw()
{
	var viewportBounds = Core.GraphicsDevice.Viewport.Bounds;
	
	// the debug view for the color buffer lives in the top-left.
	var colorBorderRect = new Rectangle(
		x: viewportBounds.X, 
		y: viewportBounds.Y, 
		width: viewportBounds.Width / 2,
		height: viewportBounds.Height / 2);
	
	// shrink the color rect by 8 pixels
	var colorRect = colorBorderRect;
	colorRect.Inflate(-8, -8);
	
	Core.SpriteBatch.Begin();
	
	// draw a debug border
	Core.SpriteBatch.Draw(Core.Pixel, colorBorderRect, Color.MonoGameOrange);
	
	// draw the color buffer
	Core.SpriteBatch.Draw(ColorBuffer, colorRect, Color.White);
	
	Core.SpriteBatch.End();
}
