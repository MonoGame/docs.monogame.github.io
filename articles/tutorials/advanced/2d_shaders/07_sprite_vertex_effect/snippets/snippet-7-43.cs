public override void Update(GameTime gameTime)
{
    // ...

    // Set the camera view to look at the player slime
    var viewport = Core.GraphicsDevice.Viewport;
    var center = .5f * new Vector2(viewport.Width, viewport.Height);
    var slimePosition = new Vector2(_slime?.GetBounds().X ?? center.X, _slime?.GetBounds().Y ?? center.Y);
    var offset = .01f * (slimePosition - center);
    _camera.LookOffset = offset;
    _gameMaterial.SetParameter("MatrixTransform", _camera.CalculateMatrixTransform());

    // ...
}