// ...

VertexShaderOutput MainVS(VertexShaderInput input) 
{
    VertexShaderOutput output;
    output.Position = mul(input.Position, MatrixTransform);
    output.Position.xy += DebugOffset;
    output.Color = input.Color;
    output.TextureCoordinates = input.TexCoord;
    return output;
}

// ...