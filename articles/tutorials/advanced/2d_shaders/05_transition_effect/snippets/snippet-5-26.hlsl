// ...

float4 MainPS(VertexShaderOutput input) : COLOR  
{  
    float2 uv = input.TextureCoordinates;  
    float value = tex2D(SpriteTextureSampler, uv).r;  
    return float4(value, value, value, 1);
}  

// ...