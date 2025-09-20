/// <summary>  
/// Check if the given parameter name is available in the compiled shader code.  
/// Remember that a parameter will be optimized out of a shader if it is not being used  
/// in the shader's return value.  
/// </summary>  
/// <param name="name"></param>  
/// <param name="parameter"></param>  
/// <returns></returns>  
public bool TryGetParameter(string name, out EffectParameter parameter)  
{  
    return ParameterMap.TryGetValue(name, out parameter);  
}
