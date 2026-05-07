float LightBrightness;  
  
float4 MainPS(VertexShaderOutput input) : COLOR  
{  
    float dist = length(input.TextureCoordinates - .5);     
    
    float falloff = saturate(.5 - dist) * (LightBrightness + 1);  
     
    return float4(falloff, 0, 0, 1);  
}
