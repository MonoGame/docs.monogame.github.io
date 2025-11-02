float4 MainPS(LightVertexShaderOutput input) : COLOR
{
	// correct the perspective divide. 
	input.ScreenData /= input.ScreenData.z;

	// put the clip-space coordinates into screen space.
	float2 screenCoords = .5*(input.ScreenData.xy + 1);
	screenCoords.y = 1 - screenCoords.y;

	return float4(screenCoords, 0, 1);
}
