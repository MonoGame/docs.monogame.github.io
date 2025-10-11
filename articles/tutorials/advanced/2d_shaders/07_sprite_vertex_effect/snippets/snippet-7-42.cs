public override void LoadContent()
{
    // ...

    // Load the game material
    _gameMaterial = Content.WatchMaterial("effects/gameEffect");
    _gameMaterial.IsDebugVisible = true;
    _gameMaterial.SetParameter("ColorMap", _colorMap);
    _camera = new SpriteCamera3d();
    _gameMaterial.SetParameter("MatrixTransform", _camera.CalculateMatrixTransform());
    _gameMaterial.SetParameter("ScreenSize", new Vector2(Core.GraphicsDevice.Viewport.Width, Core.GraphicsDevice.Viewport.Height));
}