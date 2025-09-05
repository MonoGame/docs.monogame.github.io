// render the shadow buffers  
var casters = new List<ShadowCaster>();  
casters.AddRange(_shadowCasters);  
casters.AddRange(_slime.ShadowCasters);  
PointLight.DrawShadows(_lights, casters);
