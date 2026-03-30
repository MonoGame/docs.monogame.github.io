float4 MainPS(LightVertexShaderOutput input) : COLOR {
	// ...

    float shadow = sample2D(ShadowBuffer,screenCoords).r;
    float4 normal = sample2D(NormalBuffer,screenCoords);

	// ...
}