_stencilWrite = new DepthStencilState
{
    // instruct MonoGame to use the stencil buffer
    StencilEnable = true,

    // instruct every fragment to interact with the stencil buffer
    StencilFunction = CompareFunction.LessEqual,

    // every operation will increase the shadow value (up to the max of 255), but only when the original 
    //  stencil value was greater or equal to '1'. ('1' is the default clear value)
    StencilPass = StencilOperation.IncrementSaturation,

    // this is the value that will be written into the stencil buffer
    ReferenceStencil = 1,

    // ignore depth from the stencil buffer write/reads
    DepthBufferEnable = false
};

_stencilTest = new DepthStencilState
{
    // instruct MonoGame to use the stencil buffer
    StencilEnable = true,
    
    // instruct only fragments that have a current value greater or equal to the
    //  ReferenceStencil value to interact
    StencilFunction = CompareFunction.GreaterEqual,
    
    // '1' and `0` are the "non shadow" values
    ReferenceStencil = 1,
    
    // don't change the value of the stencil buffer. KEEP the current value.
    StencilPass = StencilOperation.Keep,
    
    // ignore depth from the stencil buffer write/reads
    DepthBufferEnable = false
};