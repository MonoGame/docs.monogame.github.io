_stencilShadowExclude = new DepthStencilState
{
    // instruct MonoGame to use the stencil buffer
    StencilEnable = true,
    
    // in the setup, always set the pixel to '0'
    StencilFunction = CompareFunction.Always,
    
    // Write a '0' anywhere we don't want a shadow to appear
    ReferenceStencil = 0,
    
    // Overwrite the current value
    StencilPass = StencilOperation.Replace,
    
    // ignore depth from the stencil buffer write/reads
    DepthBufferEnable = false
};