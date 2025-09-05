float4 normal = tex2D(NormalBufferSampler,input.ScreenCoordinates);  
  
// convert from [0,1] to [-1,1]  
float3 normalDir = (normal.xyz-.5)*2;  
  
// find the direction the light is travelling at the current pixel  
float3 lightDir = float3( normalize(input.TextureCoordinates - .5), 1);  
  
// how much is the normal direction pointing towards the light direction?  
float lightAmount = saturate(dot(normalDir, lightDir));
