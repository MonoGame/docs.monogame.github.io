

float4 MainPS(VertexShaderOutput input) : COLOR
{
    // Sample the texture
    float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;

    // Calculate the grayscale value based on human perception of colors
    float grayscale = dot(color.rgb, float3(0.3, 0.59, 0.11));

    // create a grayscale color vector (same value for R, G, and B)
    float3 grayscaleColor = float3(grayscale, grayscale, grayscale);

    // Linear interpolation between he grayscale color and the original color's
    // rgb values based on the saturation parameter.
    float3 finalColor = lerp(grayscale, color.rgb, Saturation);

    // modify the final color, just for debug visualization
    finalColor *= float3(1, 0, 0);

    // Return the final color with the original alpha value
    return float4(finalColor, color.a);
}