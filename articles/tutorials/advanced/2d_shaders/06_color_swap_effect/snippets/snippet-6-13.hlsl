// ...

float4 MainPS(VertexShaderOutput input) : COLOR
{
    // read the original color value
    float4 originalColor = tex2D(SpriteTextureSampler,input.TextureCoordinates);
    
    // produce the key location
    float2 keyUv = float2(originalColor.r, 0);
    
    // read the swap color value
    float4 swappedColor = tex2D(ColorMapSampler, keyUv) * originalColor.a;

    // ignore the swap if the map does not have a value  
    bool hasSwapColor = swappedColor.a > 0;  
    if (!hasSwapColor)  
    {  
        return originalColor;  
    }

    // return the result color
    return lerp(swappedColor, originalColor, OriginalAmount);
}

// ...