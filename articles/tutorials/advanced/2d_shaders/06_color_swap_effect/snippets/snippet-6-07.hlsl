// ...

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 originalColor = tex2D(SpriteTextureSampler,input.TextureCoordinates) * input.Color;
    
    // the color values are stored between 0 and 1, 
    //  this converts the 0 to 1 range to 0 to 255, and casts to an int.
    int red = originalColor.r * 255;
    int green = originalColor.g * 255;
    int blue = originalColor.b * 255;

    // check for the hard-coded blue color
    if (red == 32 && green == 40 && blue == 78)
    {
        float4 hotPink = float4(.9, 0, .7, 1);
        return hotPink;
    }

    return originalColor;
}

// ...