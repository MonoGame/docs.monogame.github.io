[Conditional("DEBUG")]
public void DrawDebug()
{
	ImGui.Begin(Effect.Name);
	
	var currentSize = ImGui.GetWindowSize();
	ImGui.SetWindowSize(Effect.Name, new System.Numerics.Vector2(MathHelper.Max(100, currentSize.X), MathHelper.Max(100, currentSize.Y)));
	
	ImGui.AlignTextToFramePadding();
	ImGui.Text("Last Updated");
	ImGui.SameLine();
	ImGui.LabelText("##last-updated", Asset.UpdatedAt.ToString() + $" ({(DateTimeOffset.Now - Asset.UpdatedAt).ToString(@"h\:mm\:ss")} ago)");

	ImGui.NewLine();


	bool ScalarSlider(string key, ref float value)
	{
		float min = 0;
		float max = 1;
		
		return ImGui.SliderFloat($"##_prop{key}", ref value, min, max);
	}
	
	foreach (var prop in ParameterMap)
	{
		switch (prop.Value.ParameterType, prop.Value.ParameterClass)
		{
			case (EffectParameterType.Single, EffectParameterClass.Scalar):
				ImGui.AlignTextToFramePadding();
				ImGui.Text(prop.Key);
				ImGui.SameLine();
							
				var value = prop.Value.GetValueSingle();
				if (ScalarSlider(prop.Key, ref value))
				{
					prop.Value.SetValue(value);
				}
				break;
			
			case (EffectParameterType.Single, EffectParameterClass.Vector):
				ImGui.AlignTextToFramePadding();
				ImGui.Text(prop.Key);

				var vec2Value = prop.Value.GetValueVector2();
				ImGui.Indent();
				
				ImGui.Text("X");
				ImGui.SameLine();
				
				if (ScalarSlider(prop.Key + ".x", ref vec2Value.X))
				{
					prop.Value.SetValue(vec2Value);
				}
				
				ImGui.Text("Y");
				ImGui.SameLine();
				if (ScalarSlider(prop.Key + ".y", ref vec2Value.Y))
				{
					prop.Value.SetValue(vec2Value);
				}
				ImGui.Unindent();
				break;
			
			case (EffectParameterType.Texture2D, EffectParameterClass.Object):
				ImGui.AlignTextToFramePadding();
				ImGui.Text(prop.Key);
				ImGui.SameLine();

				var texture = prop.Value.GetValueTexture2D();
				if (texture != null)
				{
					var texturePtr = Core.ImGuiRenderer.BindTexture(texture);
					ImGui.Image(texturePtr, new System.Numerics.Vector2(texture.Width, texture.Height));
				}
				else
				{
					ImGui.Text("(null)");
				}
				break;
			
			default:
				ImGui.AlignTextToFramePadding();
				ImGui.Text(prop.Key);
				ImGui.SameLine();
				ImGui.Text($"(unsupported {prop.Value.ParameterType}, {prop.Value.ParameterClass})");
				break;
		}
	}
	ImGui.End();
}
