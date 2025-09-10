// ...

float4 MainPS(VertexShaderOutput input) : COLOR
{
    // read the original color value
    float4 originalColor = tex2D(SpriteTextureSampler,input.TextureCoordinates);
    
    // produce the key location
    float2 keyUv = float2(originalColor.r, 0);
    
    // read the swap color value
    float4 swappedColor = tex2D(ColorMapSampler, keyUv) * originalColor.a;
    
    // return the result color
    return lerp(swappedColor, originalColor, OriginalAmount);
}

// ...