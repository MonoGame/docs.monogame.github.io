public void StartColorPhase()
{
	// all future draw calls will be drawn to the color buffer and normal buffer
	Core.GraphicsDevice.SetRenderTargets(new RenderTargetBinding[]
	{
		// gets the results from shader semantic COLOR0
		new RenderTargetBinding(ColorBuffer),
		
		// gets the results from shader semantic COLOR1
		new RenderTargetBinding(NormalBuffer)
	});
	Core.GraphicsDevice.Clear(Color.Transparent);
}
