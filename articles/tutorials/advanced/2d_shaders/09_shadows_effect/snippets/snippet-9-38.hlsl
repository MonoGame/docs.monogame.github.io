VertexShaderOutput ShadowHullVS(VertexShaderInput input) {
    // ...

    float2 aToB = UnpackVector2FromColor_SNorm(input.Color) * ScreenSize;  
    float2 B = A + aToB;

    float2 direction = normalize(aToB);  
    A -= direction; // move A back along the segment by one unit
    B += direction; // move B forward along the segment by one unit

    float2 lightRayA = normalize(A - LightPosition);  
    float2 a = A + distance * lightRayA;  
    float2 lightRayB = normalize(B - LightPosition);  
    float2 b = B + distance * lightRayB;   

    // ...
}