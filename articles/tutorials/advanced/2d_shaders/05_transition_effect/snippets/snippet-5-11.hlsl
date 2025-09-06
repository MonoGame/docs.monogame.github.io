// ...

float4 MainPS(VertexShaderOutput input) : COLOR  
{  
    float2 uv = input.TextureCoordinates;  
    return float4(uv.x, 0, 0, 1);
}  
  
// ...