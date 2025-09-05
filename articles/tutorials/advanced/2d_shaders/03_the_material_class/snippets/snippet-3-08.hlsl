// overwrite all existing operations and set the final color to white  
finalColor.rgb = 1;  
  
// Return the final color with the original alpha value  
return float4(finalColor, color.a);
