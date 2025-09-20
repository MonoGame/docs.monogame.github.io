// render the shadow buffers  
var casters = new List<ShadowCaster>();  
casters.AddRange(_shadowCasters);  
casters.AddRange(_slime.ShadowCasters);  
casters.Add(_bat.ShadowCaster);  
  
// start rendering the lights  
_deferredRenderer.DrawLights(_lights, casters);  
  
// finish the deferred rendering  
_deferredRenderer.Finish();
