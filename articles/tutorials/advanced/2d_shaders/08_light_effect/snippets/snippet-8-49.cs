public override void LoadContent()
{
    // ...

    // Load the normal maps  
    _normalAtlas = Content.Load<Texture2D>("images/atlas-normal");

    _gameMaterial = Content.WatchMaterial("effects/gameEffect");
    _gameMaterial.IsDebugVisible = false;
    _gameMaterial.SetParameter("ColorMap", _colorMap);
    _camera = new SpriteCamera3d();
    _gameMaterial.SetParameter("MatrixTransform", _camera.CalculateMatrixTransform());
    _gameMaterial.SetParameter("ScreenSize", new Vector2(Core.GraphicsDevice.Viewport.Width, Core.GraphicsDevice.Viewport.Height));
    _gameMaterial.SetParameter("NormalMap", _normalAtlas);
}