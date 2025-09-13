_stencilTest = new DepthStencilState
{
	// instruct MonoGame to use the stencil buffer
	StencilEnable = true,
	
	// instruct only fragments that have a current value EQUAl to the
	//  ReferenceStencil value to interact
	StencilFunction = CompareFunction.Equal,
	
	// shadow hulls wrote `1`, so `0` means "not" shadow. 
	ReferenceStencil = 0,
	
	// do not change the value of the stencil buffer. KEEP the current value.
	StencilPass = StencilOperation.Keep,
	
	// ignore depth from the stencil buffer write/reads
	DepthBufferEnable = false
};
