public void DrawLights(List<PointLight> lights, List<ShadowCaster> shadowCasters)
{
    // ...
    
		Core.SpriteBatch.Draw(NormalBuffer, rect, light.Color);
		Core.SpriteBatch.End();
	}

	Core.GraphicsDevice.Clear(ClearOptions.Stencil, Color.Black, 0, 0);
}