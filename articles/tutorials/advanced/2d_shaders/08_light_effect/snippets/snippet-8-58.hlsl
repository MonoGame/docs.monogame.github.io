LightVertexShaderOutput LightVS(VertexShaderInput input)
{
    LightVertexShaderOutput output;

    VertexShaderOutput mainVsOutput = MainVS(input);

    // forward along the existing values from the MainVS's output
    output.Position = mainVsOutput.Position;// / mainVsOutput.Position.w;
    output.Color = mainVsOutput.Color;
    output.TextureCoordinates = mainVsOutput.TextureCoordinates;

	// pack the required position variables, x, y, and w, into the ScreenData
	output.ScreenData.xy = output.Position.xy;
	output.ScreenData.z = output.Position.w;
    
    return output;
}
