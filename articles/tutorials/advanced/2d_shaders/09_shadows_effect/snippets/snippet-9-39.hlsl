// cull faces  
float2 normal = float2(-direction.y, direction.x);  
float alignment = dot(normal, (LightPosition - A));  
if (alignment < 0){  
    modified.Color.a = -1;  
}
