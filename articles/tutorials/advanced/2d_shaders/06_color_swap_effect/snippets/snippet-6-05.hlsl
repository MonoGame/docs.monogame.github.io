// ...

float4 MainPS(VertexShaderOutput input) : COLOR  
{  
   float4 originalColor = tex2D(SpriteTextureSampler,input.TextureCoordinates) * input.Color;  
   originalColor.r = 1; // force the red-channel  
   return originalColor;  
}

// ...