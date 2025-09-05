technique SpriteDrawing  
{  
   pass P0  
   {  
      PixelShader = compile PS_SHADERMODEL MainPS();  
      VertexShader = compile VS_SHADERMODEL ShadowHullVS();  
   }  
};
