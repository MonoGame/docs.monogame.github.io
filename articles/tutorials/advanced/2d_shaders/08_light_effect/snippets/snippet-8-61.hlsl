float4 MainPS(LightVertexShaderOutput input) : COLOR
{
    return float4(input.ScreenCoordinates.xy, 0, 1);
}
