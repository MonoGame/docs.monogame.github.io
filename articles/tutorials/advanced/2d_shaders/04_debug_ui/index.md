---
title: "Chapter 04: Debug UI"
description: "Add ImGui.NET to the project for debug visualization"
---

So far, any time we need to adjust a shader's parameter values, we need to edit C# code and recompile. It would be much faster to have a debug UI in the game itself that expose all of the shader parameters as editable text fields and slider widgets. We can also use the sliders to change a shader's input parameter, and visualize the difference in realtime, which is a fantastic way to build intuition about our shader code. 

In this chapter, we will add a popular library called ImGui.NET to create a developer-facing debug UI for our materials. Let's get it set up. 

If you're following along with code, here is the code from the end of the [previous chapter](https://github.com/MonoGame/MonoGame.Samples/tree/3.8.4/Tutorials/2dShaders/src/03-The-Material-Class).

## Adding a Debug UI Library

A common approach to building debug UIs in games is to use an _Immediate Mode_ system. An immediate mode UI redraws the entire UI from scratch every frame. Immediate mode UIs make developing developer-facing debug tools easy. A popular library is called `DearImGui`, which has a dotnet C# port called `ImGui.NET`. 

To add `ImGUI.NET`, add the following Nuget package reference to the _MonoGameLibrary_ project,
```xml
<PackageReference Include="ImGui.NET" Version="1.91.6.1" />
```

In order to render the `ImGui.NET` UI in MonoGame, we need a few supporting classes that convert the `ImGui.NET` data into MonoGame's graphical representation. There is a [sample project](https://github.com/ImGuiNET/ImGui.NET/tree/master/src/ImGui.NET.SampleProgram.XNA) on `ImGui.NET`'s public repository that we can copy for our use cases. 

Create a new folder in the _MonoGameLibrary_ project called _ImGui_ and copy and paste the following files into the folder, 
- The [`ImGuiRenderer.cs`](https://github.com/ImGuiNET/ImGui.NET/blob/v1.91.6.1/src/ImGui.NET.SampleProgram.XNA/ImGuiRenderer.cs)
- The [`DrawVertDeclaration.cs`](https://github.com/ImGuiNET/ImGui.NET/blob/v1.91.6.1/src/ImGui.NET.SampleProgram.XNA/DrawVertDeclaration.cs)

There is `unsafe` code in the `ImGui` code, like this snippet, so you will need to enable `unsafe` code in the `MonoGameLibrary.csproj` file. Add this property.
```xml
<AllowUnsafeBlocks>true</AllowUnsafeBlocks>
```

> [!note]
> Why `unsafe`?
> The unsafe keyword in C# allows code to work directly with memory addresses (pointers). This is generally discouraged for safety reasons, but it's necessary for high-performance libraries. The `ImGuiRenderer` uses pointers to efficiently send vertex data to the GPU.

In order to play around with the new UI tool, we will set up a simple _Hello World_ UI in the main `GameScene`. As we experiment with `ImGui`, we will build towards a re-usable debug UI for future shaders. To get started, we need to have an instance of `ImGuiRenderer`. Similar to how there is a single `static SpriteBatch` , we will create a single `static ImGuiRenderer` to be re-used throughout the game. 

In the `Core.cs` file, add the following property to the `Core` class.

```csharp
/// <summary>  
/// Gets the ImGui renderer used for debug UIs.  
/// </summary>  
public static ImGuiRenderer ImGuiRenderer { get; private set; }
```

And then to initialize the instance, in the `Initialize()` method, add the following snippet,
```csharp
// Create the ImGui renderer.  
ImGuiRenderer = new ImGuiRenderer(this);  
ImGuiRenderer.RebuildFontAtlas();
```

Similar to `SpriteBatch`'s `.Begin()` and `.End()` calls, the `ImGuiRenderer` has a start and end function call. In the `GameScene` class, add these lines to end of the `.Draw()` method.
```csharp
// Draw debug UI
Core.ImGuiRenderer.BeforeLayout(gameTime);  
// draw the debug UI here  
Core.ImGuiRenderer.AfterLayout();
```

`ImGui` draws by adding draggable windows to the screen. To create a simple window that just prints out `"Hello World"`, use the following snippet,
```csharp
// Draw debug UI  
Core.ImGuiRenderer.BeforeLayout(gameTime);  
ImGui.Begin("Demo Window");  
ImGui.Text("Hello world!");  
ImGui.End();  
Core.ImGuiRenderer.AfterLayout();
```

>[!tip]
>Don't forget to add a using statement at the top of the file for `using ImGuiNET;` 

![Figure 4.1: a simple ImGui window](./gifs/imgui-hello-world.gif)

## Building a Material Debug UI

Each instance of `Material` is going to draw a custom debug window. The window will show the latest time the shader was reloaded into the game, which will help demonstrate when a new shader is being used. The window can also show the parameter values for the shader. 

Add the following function to the `Material` class, 
```csharp

[Conditional("DEBUG")]
public void DrawDebug()
{
    ImGui.Begin(Effect.Name);
    
    var currentSize = ImGui.GetWindowSize();
    ImGui.SetWindowSize(Effect.Name, new System.Numerics.Vector2(MathHelper.Max(100, currentSize.X), MathHelper.Max(100, currentSize.Y)));
    
    ImGui.AlignTextToFramePadding();
    ImGui.Text("Last Updated");
    ImGui.SameLine();
    ImGui.LabelText("##last-updated", Asset.UpdatedAt.ToString() + $