Core.ShadowHullMaterial.SetParameter("LightPosition", light.Position);         
Core.SpriteBatch.Begin(
	effect: Core.ShadowHullMaterial.Effect,
	blendState: BlendState.Opaque,
	rasterizerState: RasterizerState.CullNone
);
foreach (var caster in shadowCasters)
{
	for (var i = 0; i < caster.Points.Count; i++)
	{
		var a = caster.Position + caster.Points[i];
		var b = caster.Position + caster.Points[(i + 1) % caster.Points.Count];

		var screenSize = new Vector2(LightBuffer.Width, LightBuffer.Height);
		var aToB = (b - a) / screenSize;
		var packed = PointLight.PackVector2_SNorm(aToB);
		Core.SpriteBatch.Draw(Core.Pixel, a, packed);
	}
}

Core.SpriteBatch.End();
