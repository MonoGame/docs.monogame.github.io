PixelShaderOutput MainPS(VertexShaderOutput input)  
{  
    PixelShaderOutput output;  
    output.color = ColorSwapPS(input);  
    output.normal = float4(1, 0, 0, 1); // for now, hard-code the normal to be red.
    return output;  
}
