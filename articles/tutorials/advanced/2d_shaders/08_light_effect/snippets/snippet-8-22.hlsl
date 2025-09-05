float4 MainPS(VertexShaderOutput input) : COLOR  
{  
    float dist = length(input.TextureCoordinates - .5);     
    return float4(dist, 0, 0, 1);  
}
