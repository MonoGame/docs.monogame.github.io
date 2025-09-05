float4 MainPS(LightVertexShaderOutput input) : COLOR  
{  
    float4 normal = tex2D(NormalBufferSampler,input.ScreenCoordinates);  
    return normal;
}
