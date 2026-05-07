// start by putting the world space position into a variable...
float2 pos = input.Position.xy;

// `P` is the center of the shape, but the `pos` is half a pixel off.
float2 P = pos - (.5 * input.TexCoord) / ScreenSize;

// now we have identified `A` as `P`
float2 A = P;
