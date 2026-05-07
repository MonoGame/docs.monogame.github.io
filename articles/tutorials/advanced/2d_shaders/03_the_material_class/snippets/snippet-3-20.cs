public void SetParameter(string name, Matrix value)
{
    if (TryGetParameter(name, out var parameter))
    {
        parameter.SetValue(value);
    }
    else
    {
        Console.WriteLine($"Warning: cannot set shader parameter=[{name}] because it does not exist in the compiled shader=[{Asset.AssetName}]");
    }
}

public void SetParameter(string name, Vector2 value)
{
    if (TryGetParameter(name, out var parameter))
    {
        parameter.SetValue(value);
    }
    else
    {
        Console.WriteLine($"Warning: cannot set shader parameter=[{name}] because it does not exist in the compiled shader=[{Asset.AssetName}]");
    }
}

public void SetParameter(string name, Texture2D value)
{
    if (TryGetParameter(name, out var parameter))
    {
        parameter.SetValue(value);
    }
    else
    {
        Console.WriteLine($"Warning: cannot set shader parameter=[{name}] because it does not exist in the compiled shader=[{Asset.AssetName}]");
    }
}
