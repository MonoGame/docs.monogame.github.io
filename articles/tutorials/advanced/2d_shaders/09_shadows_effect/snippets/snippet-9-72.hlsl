float ShadowFadeStartDistance;
float ShadowFadeEndDistance;
float ShadowIntensity;

float4 MainPS(VertexShaderOutput input) : COLOR
{
    // get an ordered dither value
    int2 pixel = int2(input.TextureCoordinates * ScreenSize);
    int idx = (pixel.x % 4) + (pixel.y % 4) * 4;
    float ditherValue = bayer4x4[idx];

    // produce the fade-out gradient
    float maxDistance = ScreenSize.x + ScreenSize.y;
    float endDistance = ShadowFadeEndDistance;
    float startDistance = ShadowFadeStartDistance;
    float fade = saturate((input.TextureCoordinates.x - endDistance) / (startDistance - endDistance));
    fade = min(fade, ShadowIntensity);
    
    if (ditherValue > fade){
        clip(-1);
    }

    clip(input.Color.a);
    return float4(0,0,0,1); // return black
}