float4 MainPS(LightVertexShaderOutput input) : COLOR {
    // ...
    // how much is the normal direction pointing towards the light direction?
    float lightAmount = (dot(normalDir, lightDir));

    float4 color = input.Color;  
    color.a *= falloff * lightAmount;  
    return color;
}
