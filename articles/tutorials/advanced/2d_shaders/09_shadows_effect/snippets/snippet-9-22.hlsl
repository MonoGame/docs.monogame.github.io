float2 UnpackVector2FromColor_SNorm(float4 color)  
{  
    // Convert [0,1] to byte range [0,255]  
    float4 bytes = color * 255.0;  
  
    // Reconstruct 16-bit unsigned ints (x and y)  
    float xInt = bytes.r * 256.0 + bytes.g;  
    float yInt = bytes.b * 256.0 + bytes.a;  
  
    // Convert from unsigned to signed short range [-32768, 32767]  
    if (xInt >= 32768.0) xInt -= 65536.0;  
    if (yInt >= 32768.0) yInt -= 65536.0;  
  
    // Convert from signed 16-bit to float in [-1, 1]  
    float x = xInt / 32767.0;  
    float y = yInt / 32767.0;  
  
    return float2(x, y);  
}
