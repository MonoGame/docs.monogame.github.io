float2 direction = normalize(aToB);  
A -= direction; // move A back along the segment by one unit
B += direction; // move B forward along the segment by one unit
