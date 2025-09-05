float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(SpriteTextureSampler,input.TextureCoordinates) * input.Color;
	float4 light = tex2D(LightBufferSampler,input.TextureCoordinates) * input.Color;

    return color * light;
}
