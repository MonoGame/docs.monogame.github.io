float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(SpriteTextureSampler,input.TextureCoordinates) * input.Color;
	float4 light = Blur(input.TextureCoordinates) * input.Color;

    float3 toneMapped = light.xyz / (.5 + dot(light.xyz, float3(0.299, 0.587, 0.114)));
    light.xyz = toneMapped;
    
    light = saturate(light + AmbientLight);
	return color * light;
}