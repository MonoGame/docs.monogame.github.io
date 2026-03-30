// the `input.Color` has the (B-A) vector
float2 aToB = unpack(input.Color);

// to find `B`, start at `A`, and move by the delta
float2 B = A + aToB;
