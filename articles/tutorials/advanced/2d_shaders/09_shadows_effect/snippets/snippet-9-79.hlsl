#if OPENGL
	#define SV_POSITION POSITION
	#define sample2D(textureName, uv) tex2D(textureName##Sampler, uv)
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define sample2D(textureName, uv) textureName.Sample(textureName##Sampler, uv) 
	#define sampler2D SamplerState
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif