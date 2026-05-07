// Load the 3d effect 
_3dMaterial = Core.SharedContent.WatchMaterial("effects/3dEffect");
_3dMaterial.IsDebugVisible = true;

// this matrix code is taken from the default vertex shader code.
Matrix.CreateOrthographicOffCenter(
	left: 0, 
	right: Core.GraphicsDevice.Viewport.Width, 
	bottom: Core.GraphicsDevice.Viewport.Height, 
	top: 0, 
	zNearPlane: 0, 
	zFarPlane: -1, 
	out var projection);
_3dMaterial.SetParameter("MatrixTransform", projection);
