float2 uv = input.TextureCoordinates;  
float value = tex2D(SpriteTextureSampler, uv).r;  
float transitioned = smoothstep(Progress, Progress + EdgeWidth, value);  
return float4(0, 0, 0, transitioned);
