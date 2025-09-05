private void InitializeLights()
{
	// torch 1
	_lights.Add(new PointLight
	{
		Position = new Vector2(500, 360),
		Color = Color.CornflowerBlue,
		Radius = 700
	});
	
	// simple shadow caster
	_shadowCasters.Add(new ShadowCaster
	{
		A = new Vector2(700, 320),
		B = new Vector2(700, 400)
	});
}
