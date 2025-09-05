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
    
    ImGui.AlignTextToFramePadding();
    ImGui.Text("Override Values");
    ImGui.SameLine();
    ImGui.Checkbox("##override-values", ref DebugOverride);
    
    // ...