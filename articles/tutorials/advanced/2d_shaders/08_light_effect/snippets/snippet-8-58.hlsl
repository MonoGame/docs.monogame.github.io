LightVertexShaderOutput LightVS(VertexShaderInput input)
{
    LightVertexShaderOutput output;

    VertexShaderOutput mainVsOutput = MainVS(input);

    // forward along the existing values from the MainVS's output
    output.Position = mainVsOutput.Position;
    output.Color = mainVsOutput.Color;
    output.TextureCoordinates = mainVsOutput.TextureCoordinates;
    
    // normalize the clip-space position
    float4 normalized = output.Position / output.Position.w;
    
    // normalize from -1,1 to 0,1
    output.ScreenCoordinates = .5 * (float2(normalized.xy) + 1);
    
    // invert the y coordinate, because MonoGame flips it. 
    output.ScreenCoordinates.y = 1 - output.ScreenCoordinates.y;
    
    return output;
}