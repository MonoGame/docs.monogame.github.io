public void DrawShadowBuffer(List<ShadowCaster> shadowCasters)
{
	Core.GraphicsDevice.SetRenderTarget(ShadowBuffer);
	Core.GraphicsDevice.Clear(Color.Black);
 
	Core.ShadowHullMaterial.SetParameter("LightPos", Position);
	var screenSize = new Vector2(ShadowBuffer.Width, ShadowBuffer.Height);
	Core.ShadowHullMaterial.SetParameter("ScreenSize", screenSize);
	Core.SpriteBatch.Begin(
			effect: Core.ShadowHullMaterial.Effect, 
			rasterizerState: RasterizerState.CullNone
			);
	foreach (var caster in shadowCasters)
	{
		var posA = caster.A;
		// TODO: pack the (B-A) vector into the color channel.
		Core.SpriteBatch.Draw(Core.Pixel, posA, Color.White);
	}
	Core.SpriteBatch.End();
}
