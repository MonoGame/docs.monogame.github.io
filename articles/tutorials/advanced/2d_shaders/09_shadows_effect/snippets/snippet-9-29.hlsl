float4 MainPS(LightVertexShaderOutput input) : COLOR {
    // ...

	float2 screenCoords = .5*(input.ScreenData.xy + 1);
	screenCoords.y = 1 - screenCoords.y;

    float shadow = tex2D(ShadowBufferSampler,screenCoords).r;
	
    float4 normal = tex2D(NormalBufferSampler,screenCoords);

    // ...

    float4 color = input.Color;
    color.a *= falloff * lightAmount * shadow;
    return color;
}