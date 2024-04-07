---
title: Selecting an Object with a Mouse
description: Demonstrates how to check whether the mouse is positioned over a 3D object by creating a ray starting at the camera's near clipping plane and ending at its far clipping plane.
---

# Selecting an Object with a Mouse

Demonstrates how to check whether the mouse is positioned over a 3D object by creating a ray starting at the camera's near clipping plane and ending at its far clipping plane.

> [!NOTE]
> This example applies only to Windows or mobile development. The [Mouse](xref:Microsoft.Xna.Framework.Input.Mouse) and [MouseState](xref:Microsoft.Xna.Framework.Input.MouseState) objects are not supported on consoles.

## Detecting Whether a User Clicked a 3D Object

### To check whether the mouse is positioned over a 3D object

1. Get the current state of the mouse by using [GetState](xref:Microsoft.Xna.Framework.Input.Mouse).

    ``` csharp
    MouseState mouseState = Mouse.GetState();
    ```

2. Get the current screen coordinates of the mouse from [X](xref:Microsoft.Xna.Framework.Input.Mouse) and [Y](xref:Microsoft.Xna.Framework.Input.Mouse).

    ``` csharp
    int mouseX = mouseState.X;
    int mouseY = mouseState.Y;
    ```

3. Using [Viewport.Unproject](xref:Microsoft.Xna.Framework.Graphics.Viewport#Microsoft_Xna_Framework_Graphics_Viewport_Unproject_Microsoft_Xna_Framework_Vector3_Microsoft_Xna_Framework_Matrix_Microsoft_Xna_Framework_Matrix_Microsoft_Xna_Framework_Matrix_), determine points in world space on the near and far clipping planes. For the point on the near plane, pass a source vector with x and y set to the mouse position, and z set to 0.

4. For the point on the far plane, pass a source vector with x and y set to the mouse position, and z set to 1.

5. Create a translation matrix for a point that is the origin, (0,0,0).

6. For both points, pass [Viewport.Unproject](xref:Microsoft.Xna.Framework.Graphics.Viewport#Microsoft_Xna_Framework_Graphics_Viewport_Unproject_Microsoft_Xna_Framework_Vector3_Microsoft_Xna_Framework_Matrix_Microsoft_Xna_Framework_Matrix_Microsoft_Xna_Framework_Matrix_) the current projection matrix, the view matrix.

    ``` csharp
    Vector3 nearsource = new Vector3((float)mouseX, (float)mouseY, 0f);
    Vector3 farsource = new Vector3((float)mouseX, (float)mouseY, 1f);
    
    Matrix world = Matrix.CreateTranslation(0, 0, 0);
    
    Vector3 nearPoint = GraphicsDevice.Viewport.Unproject(nearsource,
        proj, view, world);
    
    Vector3 farPoint = GraphicsDevice.Viewport.Unproject(farsource,
        proj, view, world);
    ```

7. Create a [Ray](xref:Microsoft.Xna.Framework.Ray) whose origin is at the near point and whose direction points to the far point.

    ``` csharp
    // Create a ray from the near clip plane to the far clip plane.
    Vector3 direction = farPoint - nearPoint;
    direction.Normalize();
    Ray pickRay = new Ray(nearPoint, direction);
    ```

8. Loop through each object in the scene using the [Intersects](xref:Microsoft.Xna.Framework.Ray) method to check whether the [Ray](xref:Microsoft.Xna.Framework.Ray) intersects each object.

9. If the [Ray](xref:Microsoft.Xna.Framework.Ray) intersects an object, check whether it is the closest object intersected so far. If it is, store the object and the distance at which it was intersected, replacing any previously stored object.

10. When you completely loop through the objects, the last object stored will be the closest object underneath the area the user clicked.

## See Also

[Rotating and Moving the Camera](HowTo_RotateMoveCamera.md)

© 2012 Microsoft Corporation. All rights reserved.

© 2023 The MonoGame Foundation.
