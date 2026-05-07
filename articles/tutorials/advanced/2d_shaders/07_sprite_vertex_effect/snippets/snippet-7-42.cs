public override void LoadContent()
{
    // ...

    // Load the colorSwap map
    _colorMap = Content.Load<Texture2D>("images/color-map-dark-purple");
    _slimeColorMap = new RedColorMap();
    _slimeColorMap.SetColorsByExistingColorMap(_colorMap);
    _slimeColorMap.SetColorsByRedValue(new Dictionary<int, Color>
    {
        // main color
        [32] = Color.LightSteelBlue,
    }, false);    

    // Load the game material
    _gameMaterial = Content.WatchMaterial("effects/gameEffect");
    _gameMaterial.IsDebugVisible = true;
    _gameMaterial.SetParameter("ColorMap", _colorMap);
    _camera = new SpriteCamera3d();
    _gameMaterial.SetParameter("MatrixTransform", _camera.CalculateMatrixTransform());
    _gameMaterial.SetParameter("ScreenSize", new Vector2(Core.GraphicsDevice.Viewport.Width, Core.GraphicsDevice.Viewport.Height));
}