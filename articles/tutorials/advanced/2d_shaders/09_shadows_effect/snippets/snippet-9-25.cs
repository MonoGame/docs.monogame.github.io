public override void Update(GameTime gameTime)
{
    // ...


        var matrixTransform = _camera.CalculateMatrixTransform();
        _gameMaterial.SetParameter("MatrixTransform", matrixTransform);
        Core.PointLightMaterial.SetParameter("MatrixTransform", matrixTransform);
        Core.PointLightMaterial.SetParameter("ScreenSize", new Vector2(Core.GraphicsDevice.Viewport.Width, Core.GraphicsDevice.Viewport.Height));
        
        Core.ShadowHullMaterial.SetParameter("MatrixTransform", matrixTransform);
        Core.ShadowHullMaterial.SetParameter("ScreenSize", new Vector2(Core.GraphicsDevice.Viewport.Width, Core.GraphicsDevice.Viewport.Height));

    // ...
}