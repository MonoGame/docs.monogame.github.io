float4 MainPS(VertexShaderOutput input) : COLOR {
    clip(input.Color.a);
    return float4(0,0,0,1); // return black
}