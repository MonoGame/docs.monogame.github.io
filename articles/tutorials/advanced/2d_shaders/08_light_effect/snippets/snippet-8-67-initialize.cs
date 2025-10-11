public override void Initialize()
{
	// ...

	// Replace this.....
	// _lights.Add(new PointLight
	// {
	// 	Position = new Vector2(300, 300),
	// 	Color = Color.CornflowerBlue
	// });

	// With this new call to initialize lights for the scene
	InitializeLights();
}
