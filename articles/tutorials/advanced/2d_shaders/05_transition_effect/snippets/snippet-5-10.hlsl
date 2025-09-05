#if OPENGL  
    #define SV_POSITION POSITION  
    #define VS_SHADERMODEL vs_3_0  
    #define PS_SHADERMODEL ps_3_0  
#else  
    #define VS_SHADERMODEL vs_4_0_level_9_1  
    #define PS_SHADERMODEL ps_4_0_level_9_1  
#endif  
  
Texture2D SpriteTexture;  
  
float Progress;  
  
sampler2D SpriteTextureSampler = sampler_state  
{  
    Texture = <SpriteTexture>;  
};  
  
struct VertexShaderOutput  
{  
    float4 Position : SV_POSITION;  
    float4 Color : COLOR0;  
    float2 TextureCoordinates : TEXCOORD0;  
};  
  
float4 MainPS(VertexShaderOutput input) : COLOR  
{  
    return float4(Progress, 0, 0, 1);  
}  
  
  
technique SpriteDrawing  
{  
    pass P0  
    {  
        PixelShader = compile PS_SHADERMODEL MainPS();  
    }  
};
