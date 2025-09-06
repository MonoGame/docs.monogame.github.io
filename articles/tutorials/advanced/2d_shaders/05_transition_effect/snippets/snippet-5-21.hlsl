// ...

float4 MainPS(VertexShaderOutput input) : COLOR  
{  
    float2 uv = input.TextureCoordinates;  
    float value = 1 - abs(.5 - uv.x) * 2;  
    float transitioned = smoothstep(Progress, Progress + EdgeWidth, value);

    return float4(0, 0, 0, transitioned);
}  

// ...