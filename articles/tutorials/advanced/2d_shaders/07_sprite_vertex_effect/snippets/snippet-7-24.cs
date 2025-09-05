// Load the 3d effect 
_3dMaterial = Core.SharedContent.WatchMaterial("effects/3dEffect");
_3dMaterial.IsDebugVisible = true;

var camera = new SpriteCamera3d();
_3dMaterial.SetParameter("MatrixTransform", camera.CalculateMatrixTransform());
