public void SetParameter(string name, float value)
{
	if (DebugOverride) return;

	if (TryGetParameter(name, out var parameter))
	{
		parameter.SetValue(value);
	}
	else
	{
		Console.WriteLine($"Warning: cannot set shader parameter=[{name}] because it does not exist in the compiled shader=[{Asset.AssetName}]");
	}
}
