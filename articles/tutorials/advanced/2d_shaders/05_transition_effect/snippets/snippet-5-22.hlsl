// ...

float4 MainPS(VertexShaderOutput input) : COLOR {
    float2 uv = input.TextureCoordinates;  
    float value = 1 - abs(.5 - uv.x) * 2;  
    return float4(value, value, value, 1);
}   

// ...