float4 SwapColors(float4 color)
{
    // produce the key location
    //  note the x-offset by half a texel solves rounding errors.
    float2 keyUv = float2(color.r , 0);
    
    // read the swap color value
    float4 swappedColor = tex2D(ColorMapSampler, keyUv) * color.a;
    
    // ignore the swap if the map does not have a value
    bool hasSwapColor = swappedColor.a > 0;
    if (!hasSwapColor)
    {
        return color;
    }
    
    // return the result color
    return lerp(swappedColor, color, OriginalAmount);
}
