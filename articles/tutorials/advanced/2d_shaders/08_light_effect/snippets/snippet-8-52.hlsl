PixelShaderOutput MainPS(VertexShaderOutput input)  
{  
    PixelShaderOutput output;  
    output.color = ColorSwapPS(input);  
      
    // read the normal data from the NormalMap  
    float4 normal = tex2D(NormalMapSampler,input.TextureCoordinates);  
    output.normal = normal;  
      
    return output;  
}
