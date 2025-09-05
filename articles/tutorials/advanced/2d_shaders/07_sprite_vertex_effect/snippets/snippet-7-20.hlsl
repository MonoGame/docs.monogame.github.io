float4 pos = input.Position;  
pos.xy += DebugOffset;  
output.Position = mul(position, MatrixTransform);
