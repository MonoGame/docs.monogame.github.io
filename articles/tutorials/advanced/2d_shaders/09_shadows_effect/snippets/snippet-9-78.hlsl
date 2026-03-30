PixelShaderOutput MainPS(VertexShaderOutput input)  
{  
    PixelShaderOutput output;  
    output.color = ColorSwapPS(input);  
    
    // do not even render the pixel if the alpha is blank.
    clip(output.color.a - 1);
    
    // read the normal data from the NormalMap  
    float4 normal = tex2D(NormalMapSampler,input.TextureCoordinates);  
    output.normal = normal;  
      
    return output;  
}