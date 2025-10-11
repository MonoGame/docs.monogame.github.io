float4 MainPS(LightVertexShaderOutput input) : COLOR {
    float dist = length(input.TextureCoordinates - .5);   

    float range = 5; // arbitrary maximum. 

    float falloff = saturate(.5 - dist) * (LightBrightness * range + 1);
    falloff = pow(abs(falloff), LightSharpness * range + 1);

    float4 normal = tex2D(NormalBufferSampler,input.ScreenCoordinates);
    // flip the y of the normals, because the art assets have them backwards.
    normal.y = 1 - normal.y;

    // convert from [0,1] to [-1,1]
    float3 normalDir = (normal.xyz-.5)*2;

    // find the direction the light is travelling at the current pixel
    float3 lightDir = float3(normalize(.5 - input.TextureCoordinates), 1);

    // how much is the normal direction pointing towards the light direction?
    float lightAmount = (dot(normalDir, lightDir));

    float4 color = input.Color;
    color.a *= falloff * lightAmount;
    return color;
}