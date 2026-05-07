private void InitializeLights()
{
	// torch 1
	_lights.Add(new PointLight
	{
		Position = new Vector2(260, 100),
		Color = Color.CornflowerBlue,
		Radius = 500
	});
	// torch 2
	_lights.Add(new PointLight
	{
		Position = new Vector2(520, 100),
		Color = Color.CornflowerBlue,
		Radius = 500
	});
	// torch 3
	_lights.Add(new PointLight
	{
		Position = new Vector2(740, 100),
		Color = Color.CornflowerBlue,
		Radius = 500
	});
	// torch 4
	_lights.Add(new PointLight
	{
		Position = new Vector2(1000, 100),
		Color = Color.CornflowerBlue,
		Radius = 500
	});
	
	// random lights
	_lights.Add(new PointLight
	{
		Position = new Vector2(Random.Shared.Next(50, 400),400),
		Color = Color.MonoGameOrange,
		Radius = 500
	});
	_lights.Add(new PointLight
	{
		Position = new Vector2(Random.Shared.Next(650, 1200),300),
		Color = Color.MonoGameOrange,
		Radius = 500
	});
}
