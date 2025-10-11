var camera = new SpriteCamera3d();
_3dMaterial.SetParameter("MatrixTransform", camera.CalculateMatrixTransform());
_3dMaterial.SetParameter("ScreenSize", new Vector2(Core.GraphicsDevice.Viewport.Width, Core.GraphicsDevice.Viewport.Height));