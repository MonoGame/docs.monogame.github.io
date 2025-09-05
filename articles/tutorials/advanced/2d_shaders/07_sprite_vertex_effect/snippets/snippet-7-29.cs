var spinAmount = Core.Input.Mouse.X / (float)Core.GraphicsDevice.Viewport.Width;  
spinAmount = MathHelper.SmoothStep(-.1f, .1f, spinAmount);  
_3dMaterial.SetParameter("SpinAmount", spinAmount);
