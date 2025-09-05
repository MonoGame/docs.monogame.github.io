private void MoveLightsAround(GameTime gameTime)
{
	var t = (float)gameTime.TotalGameTime.TotalSeconds * .25f;
	var bounds = Core.GraphicsDevice.Viewport.Bounds;
	bounds.Inflate(-100, -100);

	var halfWidth = bounds.Width / 2;
	var halfHeight = bounds.Height / 2;
	var center = new Vector2(halfWidth, halfHeight);
	_lights[^1].Position = center + new Vector2(halfWidth * MathF.Cos(t), .7f * halfHeight * MathF.Sin(t * 1.1f));
	_lights[^2].Position = center + new Vector2(halfWidth * MathF.Cos(t + MathHelper.Pi), halfHeight * MathF.Sin(t - MathHelper.Pi));
}
