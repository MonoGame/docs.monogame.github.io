public DeferredRenderer()
{
	// ...

	LightBuffer = new RenderTarget2D(
		graphicsDevice: Core.GraphicsDevice,
		width: viewport.Width,
		height: viewport.Height,
		mipMap: false,
		preferredFormat: SurfaceFormat.Color,
		preferredDepthFormat: DepthFormat.None);
}