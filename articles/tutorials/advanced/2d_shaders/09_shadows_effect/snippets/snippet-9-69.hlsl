float2 ScreenSize;
float BoxBlurStride;

float4 Blur(float2 texCoord)
{
	float4 color = float4(0, 0, 0, 0);

	float2 texelSize = 1 / ScreenSize;
	int kernalSize = 1;
	float stride = BoxBlurStride * 30; // allow the stride to range up a size of 30
    for (int x = -kernalSize; x <= kernalSize; x++)
    {
        for (int y = -kernalSize; y <= kernalSize; y++)
        {
            float2 offset = float2(x, y) * texelSize * stride;
            color += tex2D(LightBufferSampler, texCoord + offset);
        }
    }

	int totalSamples = pow(kernalSize*2+1, 2);
    color /= totalSamples;
	color.a = 1;
    return color;
}
