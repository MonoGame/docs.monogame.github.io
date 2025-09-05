// render the shadow buffers  
var casters = new List<ShadowCaster>();  
casters.AddRange(_shadowCasters);  
casters.AddRange(_slime.ShadowCasters);  
casters.Add(_bat.ShadowCaster);  
PointLight.DrawShadows(_lights, casters);
