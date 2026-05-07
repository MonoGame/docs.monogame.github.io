public void DrawShadowBuffer(List<ShadowCaster> shadowCasters)
{
    /// ...
    
	foreach (var caster in shadowCasters)
	{
		for (var i = 0; i < caster.Points.Count; i++)
		{
			var a = caster.Position + caster.Points[i];
			var b = caster.Position + caster.Points[(i + 1) % caster.Points.Count];

			var aToB = (b - a) / screenSize;
			var packed = PackVector2_SNorm(aToB);
			Core.SpriteBatch.Draw(Core.Pixel, a, packed);
		}
	}
    
	// ...
}
