float4 MainPS(LightVertexShaderOutput input) : COLOR {
    // ...

    float shadow = tex2D(ShadowBufferSampler,input.ScreenCoordinates).r;

    float4 color = input.Color;
    color.a *= falloff * lightAmount * shadow;
    return color;
}