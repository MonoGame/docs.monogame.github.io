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
	
	
	// the debug view for the light buffer lives in the top-right.
	var lightBorderRect = new Rectangle(
		x: viewportBounds.Width / 2, 
		y: viewportBounds.Y, 
		width: viewportBounds.Width / 2,
		height: viewportBounds.Height / 2);
	
	// shrink the light rect by 8 pixels
	var lightRect = lightBorderRect;
	lightRect.Inflate(-8, -8);

	
	Core.SpriteBatch.Begin();
	
	// draw a debug border
	Core.SpriteBatch.Draw(Core.Pixel, colorBorderRect, Color.MonoGameOrange);
	
	// draw the color buffer
	Core.SpriteBatch.Draw(ColorBuffer, colorRect, Color.White);
	
	//draw a debug border
	Core.SpriteBatch.Draw(Core.Pixel, lightBorderRect, Color.CornflowerBlue);
	
	// draw the light buffer
	Core.SpriteBatch.Draw(LightBuffer, lightRect, Color.White);
	
	Core.SpriteBatch.End();
}
