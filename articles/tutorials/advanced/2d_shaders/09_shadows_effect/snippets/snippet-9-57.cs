_stencilWrite = new DepthStencilState
{
	// instruct MonoGame to use the stencil buffer
	StencilEnable = true,
	
	// instruct every fragment to interact with the stencil buffer
	StencilFunction = CompareFunction.Always,
	
	// every operation will replace the current value in the stencil buffer
	//  with whatever value is in the ReferenceStencil variable
	StencilPass = StencilOperation.Replace,
	
	// this is the value that will be written into the stencil buffer
	ReferenceStencil = 1,
	
	// ignore depth from the stencil buffer write/reads  
	DepthBufferEnable = false
};
