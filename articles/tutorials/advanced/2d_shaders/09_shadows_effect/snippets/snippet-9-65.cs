public void DrawLights(List<PointLight> lights, List<ShadowCaster> shadowCasters)
{
	Core.GraphicsDevice.SetRenderTarget(LightBuffer);
	Core.GraphicsDevice.Clear(Color.Black);

	foreach (var light in lights)
	{
		var diameter = light.Radius * 2;
		var rect = new Rectangle(
			(int)(light.Position.X - light.Radius),
			(int)(light.Position.Y - light.Radius),
			diameter, diameter);

		Core.GraphicsDevice.Clear(ClearOptions.Stencil, Color.Black, 0, 0);

		Core.ShadowHullMaterial.SetParameter("LightPosition", light.Position);

		// ...
	}
	// ...
}