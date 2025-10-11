// _gameMaterial.SetParameter("MatrixTransform", _camera.CalculateMatrixTransform()); <- Replace this line with:

var matrixTransform = _camera.CalculateMatrixTransform();  
_gameMaterial.SetParameter("MatrixTransform", matrixTransform);  
Core.PointLightMaterial.SetParameter("MatrixTransform", matrixTransform);
Core.PointLightMaterial.SetParameter("ScreenSize", new Vector2(Core.GraphicsDevice.Viewport.Width, Core.GraphicsDevice.Viewport.Height));
