// ...

float4 MainPS(VertexShaderOutput input) : COLOR  
{  
    float2 uv = input.TextureCoordinates;  
    float transitioned = smoothstep(Progress, Progress + EdgeWidth, uv.y);
    return float4(0, 0, 0, transitioned);
}  

// ...