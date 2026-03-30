VertexShaderOutput MainVS(VertexShaderInput input) {
    VertexShaderOutput output;

    float4 pos = input.Position;

    // create the center of rotation
    float2 centerXZ = float2(ScreenSize.x * .5, 0);

    // convert the debug variable into an angle from 0 to 2 pi. 
    //  shaders use radians for angles, so 2 pi = 360 degrees
    float angle = SpinAmount * 6.28;

    // ...
}