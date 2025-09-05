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
