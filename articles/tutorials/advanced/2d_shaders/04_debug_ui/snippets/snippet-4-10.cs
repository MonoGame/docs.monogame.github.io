[Conditional("DEBUG")]
public void DrawDebug()
{
    ImGuiNET.ImGui.Begin(Effect.Name);

    var currentSize = ImGuiNET.ImGui.GetWindowSize();
    ImGuiNET.ImGui.SetWindowSize(Effect.Name, new System.Numerics.Vector2(MathHelper.Max(100, currentSize.X), MathHelper.Max(100, currentSize.Y)));

    ImGuiNET.ImGui.AlignTextToFramePadding();
    ImGuiNET.ImGui.Text("Last Updated");
    ImGuiNET.ImGui.SameLine();
    ImGuiNET.ImGui.LabelText("##last-updated", Asset.UpdatedAt.ToString() + $" ({(DateTimeOffset.Now - Asset.UpdatedAt).ToString(@"h\:mm\:ss")} ago)");

    ImGuiNET.ImGui.AlignTextToFramePadding();
    ImGuiNET.ImGui.Text("Override Values");
    ImGuiNET.ImGui.SameLine();
    ImGuiNET.ImGui.Checkbox("##override-values", ref DebugOverride);
    
    // ...