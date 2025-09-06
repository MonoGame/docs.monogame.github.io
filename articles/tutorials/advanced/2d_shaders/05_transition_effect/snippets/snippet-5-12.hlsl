// ...

float4 MainPS(VertexShaderOutput input) : COLOR  
{  
    float2 uv = input.TextureCoordinates;  
    return float4(0, uv.y, 0, 1);
}  
  
// ...