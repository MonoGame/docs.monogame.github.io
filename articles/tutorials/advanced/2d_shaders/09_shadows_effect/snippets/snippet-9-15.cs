public PointLight()
{
	var viewPort = Core.GraphicsDevice.Viewport;
	ShadowBuffer = new RenderTarget2D(Core.GraphicsDevice, viewPort.Width, viewPort.Height, false, SurfaceFormat.Color,  DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
}
