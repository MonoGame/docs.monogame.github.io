/// <summary>
/// Enable this variable to visualize the debugUI for the material
/// </summary>
public bool IsDebugVisible
{
	get
	{
		return s_debugMaterials.Contains(this);
	}
	set
	{
		if (IsDebugVisible)
		{
			s_debugMaterials.Remove(this);
		}
		else
		{
			s_debugMaterials.Add(this);
		}
	}
}
