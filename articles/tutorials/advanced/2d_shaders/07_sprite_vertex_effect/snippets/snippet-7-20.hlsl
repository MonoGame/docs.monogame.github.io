// ...

VertexShaderOutput MainVS(VertexShaderInput input) 
{
    VertexShaderOutput output;
    float4 pos = input.Position;  
    pos.xy += DebugOffset;  
    output.Position = mul(pos, MatrixTransform);
    output.Color = input.Color;
    output.TextureCoordinates = input.TexCoord;
    return output;
}

// ...