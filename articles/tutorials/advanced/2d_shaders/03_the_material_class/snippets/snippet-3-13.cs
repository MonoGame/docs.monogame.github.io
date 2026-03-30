/// <summary>  
/// Check if the given parameter name is available in the compiled shader code.  
/// Remember that a parameter will be optimized out of a shader if it is not being used  
/// in the shader's return value.  
/// </summary>  
/// <param name="name">The parameter name</param>  
/// <param name="parameter">The effect parameter if found</param>  
/// <returns>True if the parameter was found, otherwise false</returns>  
public bool TryGetParameter(string name, out EffectParameter parameter)  
{  
    return ParameterMap.TryGetValue(name, out parameter);  
}
