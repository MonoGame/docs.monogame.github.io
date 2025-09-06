// ...

float4 MainPS(VertexShaderOutput input) : COLOR  
{  
    float2 uv = input.TextureCoordinates;  
    float transitioned = Progress > uv.x;  
    return float4(0, 0, 0, transitioned);
}  
  
// ...