// cache the shader parameter for the MatrixTransform
_matrixParam = Parameters["MatrixTransform"];

// ... some code left out for readability

// create a projection matrix in the _projection variable
Matrix.CreateOrthographicOffCenter(0, vp.Width, vp.Height, 0, 0, -1, out _projection);

// ... some code left out for readability 

// assign the projection matrix to the MatrixTransform
_matrixParam.SetValue(_projection);
