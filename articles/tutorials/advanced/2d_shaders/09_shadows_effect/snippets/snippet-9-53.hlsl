float4 color = input.Color;  
color.a *= falloff * lightAmount;  
return color;
