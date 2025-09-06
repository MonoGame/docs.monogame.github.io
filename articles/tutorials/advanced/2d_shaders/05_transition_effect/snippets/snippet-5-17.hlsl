// ...

float4 MainPS(VertexShaderOutput input) : COLOR  
{  
    float2 uv = input.TextureCoordinates;  
    float edgeWidth = .05;
    float transitioned = smoothstep(Progress, Progress + edgeWidth, uv.x);
    return float4(0, 0, 0, transitioned);
}  
  
// ...