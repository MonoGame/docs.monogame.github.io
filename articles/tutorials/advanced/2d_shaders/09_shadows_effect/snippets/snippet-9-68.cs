private void InitializeLights()
{
    // torch 1
    _lights.Add(new PointLight
    {
        Position = new Vector2(260, 100),
        Color = Color.CornflowerBlue,
        Radius = 600
    });
    
    // torch 2
    _lights.Add(new PointLight
    {
        Position = new Vector2(1000, 100),
        Color = Color.CornflowerBlue,
        Radius = 600
    });
    
    // underlight
    _lights.Add(new PointLight
    {
        Position = new Vector2(600, 660),
        Color = Color.MonoGameOrange,
        Radius = 1200
    });
}