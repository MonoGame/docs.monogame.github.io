/// <summary>  
/// Rebuild the <see cref="ParameterMap"/> based on the current parameters available in the effect instance  
/// </summary>  
public void UpdateParameterCache()  
{  
    ParameterMap = Effect.Parameters.ToDictionary(p => p.Name);  
}
