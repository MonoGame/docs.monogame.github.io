float LightBrightness;  
float LightSharpness;  
  
float4 MainPS(VertexShaderOutput input) : COLOR  
{  
    float dist = length(input.TextureCoordinates - .5);     
    
    float range = 5; // arbitrary maximum.   
    
    float falloff = saturate(.5 - dist) * (LightBrightness * range + 1);  
    falloff = pow(falloff, LightSharpness * range + 1);  
     
    float4 color = input.Color;  
    color.a = falloff;  
    return color;  
}
