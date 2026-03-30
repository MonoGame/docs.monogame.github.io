float2 LightPosition;  
VertexShaderOutput ShadowHullVS(VertexShaderInput input)   
{     
    VertexShaderInput modified = input;  
    float distance = ScreenSize.x + ScreenSize.y;  
    float2 pos = input.Position.xy;  
      
    float2 P = pos - (.5 * input.TexCoord) / ScreenSize;  
    float2 A = P;  
      
    float2 aToB = UnpackVector2FromColor_SNorm(input.Color) * ScreenSize;  
    float2 B = A + aToB;  
      
    float2 lightRayA = normalize(A - LightPosition);  
    float2 a = A + distance * lightRayA;  
    float2 lightRayB = normalize(B - LightPosition);  
    float2 b = B + distance * lightRayB;      
      
    int id = input.TexCoord.x + input.TexCoord.y * 2;  
    if (id == 0) {        // S --> A  
       pos = A;  
    } else if (id == 1) { // D --> a  
       pos = a;  
    } else if (id == 3) { // F --> b  
       pos = b;  
    } else if (id == 2) { // G --> B  
       pos = B;  
    }  
      
    modified.Position.xy = pos;  
    VertexShaderOutput output = MainVS(modified);  
    return output;  
}
