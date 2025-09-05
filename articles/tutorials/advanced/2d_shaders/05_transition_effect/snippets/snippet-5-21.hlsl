float value = 1 - abs(.5 - uv.x) * 2;  
float transitioned = smoothstep(Progress, Progress + EdgeWidth, value);
