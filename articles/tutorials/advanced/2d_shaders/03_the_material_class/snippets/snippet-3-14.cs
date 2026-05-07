/// <summary>
/// Set a float parameter on the shader
/// </summary>
/// <param name="name">The parameter name</param>  
/// <param name="value">The float value to set</param>
public void SetParameter(string name, float value)
{
    if (TryGetParameter(name, out var parameter))
    {
        parameter.SetValue(value);
    }
    else
    {
        Console.WriteLine($"Warning: cannot set parameter=[{name}] as it does not exist in the shader=[{Asset.AssetName}]");
    }
}
