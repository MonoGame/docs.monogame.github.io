---
title: What are Tips for Developing Custom Importers and Processors?
description: The information provided here should help when you develop Content Pipeline extensions.
---

# Tips for Developing Custom Importers and Processors

The information provided should help when you develop Content Pipeline extensions.

## Importing Basic Graphics Objects

The following information should help you import basic graphics objects.

- Make your coordinate system right-handed.

    From the standpoint of the observer, the positive x-axis points to the right, the positive y-axis points up, and the positive z-axis points toward you (out from the screen).

- Create triangles that have a clockwise winding order.

    The default culling mode removes triangles that have a counterclockwise winding order.

- Call [MeshHelper.SwapWindingOrder](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.MeshHelper#Microsoft_Xna_Framework_Content_Pipeline_Graphics_MeshHelper_SwapWindingOrder_Microsoft_Xna_Framework_Content_Pipeline_Graphics_MeshContent_) to change the winding order of a triangle.

- Set the scale for graphical objects to 1 unit = 1 meter.

- Call [MeshHelper.TransformScene](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.MeshHelper#Microsoft_Xna_Framework_Content_Pipeline_Graphics_MeshHelper_TransformScene_Microsoft_Xna_Framework_Content_Pipeline_Graphics_NodeContent_Microsoft_Xna_Framework_Matrix_) to change the scale of an object.

## Taking Advantage of Content Pipeline Mesh Classes

There are several properties and classes that are particularly useful when using [NodeContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.NodeContent) objects to represent a 3D scene or mesh.

- The [NodeContent.Children](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.NodeContent#Microsoft_Xna_Framework_Content_Pipeline_Graphics_NodeContent_Children) property represents hierarchical information.

- The [NodeContent.Transform](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.NodeContent#Microsoft_Xna_Framework_Content_Pipeline_Graphics_NodeContent_Transform) property contains the local transform of the 3D object.

- The [Pipeline.Graphics.MeshContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.MeshContent) class (a subclass of [Pipeline.Graphics.NodeContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.NodeContent)) is used to represent meshes.

The Content Pipeline provides two classes that make it easier to create and work with [Pipeline.Graphics.MeshContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.MeshContent) objects.

- The [Pipeline.Graphics.MeshBuilder](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.MeshBuilder) class creates new [Pipeline.Graphics.MeshContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.MeshContent) objects, when necessary.

- The [Pipeline.Graphics.MeshHelper](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.MeshHelper) class implements useful operations on existing [Pipeline.Graphics.MeshContent](xref:Microsoft.Xna.Framework.Content.Pipeline.Graphics.MeshContent) objects.

## Debugging Custom Importers and Processors

In a manner similar to projects that create a DLL, Content Pipeline extension projects cannot be directly run or debugged. After completing a few simple steps, however, you can debug any custom importer and processor used by your game. The following procedure details these steps.

> **Tip**
>
> The Start External program control (located on the Debug page of a project's property pages) is unavailable in the Microsoft Visual C# Express Edition development environment.

### To debug a custom importer or processor

1. Load an existing MonoGame Content Pipeline extension project (later referred to as ProjCP) containing the custom importers or processors to be debugged.

2. Create a separate test game project (later referred to as "ProjG").

3. In the **References** node of ProjG's nested content project, add a project-to-project reference to ProjCP.

4. Add one or two appropriate items of test content to ProjG, and ensure they are set to use the importer or processor (in ProjCP) you wish to debug.

5. Open the property pages for ProjCP.

6. Click the **Debug** tab, and then select **Start external program**.

7. Enter the path to the local version of MSBuild.exe.

    For example, C:\\WINDOWS\\Microsoft.NET\\Framework\\v3.5\\msbuild.exe.

8. For the **Command line arguments** control, enter the path to ProjG's nested content project.

    If this path contains spaces, quote the entire path.

9. Set any required breakpoints in the importer or processor code in ProjCP.

10. Build and debug ProjCP.

Debugging ProjCP causes MSBuild to compile your test content while running under the debugger. This enables you to hit your breakpoints in ProjCP and to step through your code.

## See Also

- [What Is Content?](CP_Overview.md)  
- [What is the Content Pipeline?](CP_Architecture.md)  
- [Extending a Standard Content Processor](../../howto/Content_Pipeline/HowTo_Extend_Processor.md)  
- [Adding New Content Types](CP_Content_Advanced.md)  

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
