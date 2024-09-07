---
title: Selecting an Object with a Mouse
description: Demonstrates how to check whether the mouse is positioned over a 3D object by creating a ray starting at the camera's near clipping plane and ending at its far clipping plane.
requireMSLicense: true
---

## Overview

A common requirement in game development is to select 3D objects in a scene. This example walks through the most common method for achieving this.

> [!NOTE]
> This example applies only to Desktop or mobile development. The [Mouse](xref:Microsoft.Xna.Framework.Input.Mouse) and [MouseState](xref:Microsoft.Xna.Framework.Input.MouseState) objects are not supported on consoles.
>
> On consoles however the same pattern applies, you simply need to use either a fixed point (where the camera is looking, or screen center) or a cursor controlled by a stick to get the position.

## Detecting Whether a User Clicked a 3D Object

1. Get the current state of the mouse by using [GetState](xref:Microsoft.Xna.Framework.Input.Mouse).

    ``` csharp
    MouseState mouseState = Mouse.GetState();
    ```

2. Get the current screen coordinates of the mouse from [X](xref:Microsoft.Xna.Framework.Input.Mouse) and [Y](xref:Microsoft.Xna.Framework.Input.Mouse).

    ``` csharp
    int mouseX = mouseState.X;
    int mouseY = mouseState.Y;
    ```

    > [!NOTE]
    > Replace mouse position with a virtual cursor or the screen centre for consoles.

3. Using [Viewport.Unproject](xref:Microsoft.Xna.Framework.Graphics.Viewport#Microsoft_Xna_Framework_Graphics_Viewport_Unproject_Microsoft_Xna_Framework_Vector3_Microsoft_Xna_Framework_Matrix_Microsoft_Xna_Framework_Matrix_Microsoft_Xna_Framework_Matrix_), determine points in world space on the near and far clipping planes. For the point on the near plane, pass a source vector with x and y set to the mouse position, and z set to 0.

4. For the point on the far plane, pass a source vector with x and y set to the position, and z set to 1.

5. Create a translation matrix for a point that is the origin, (0,0,0).

6. For both points, pass [Viewport.Unproject](xref:Microsoft.Xna.Framework.Graphics.Viewport#Microsoft_Xna_Framework_Graphics_Viewport_Unproject_Microsoft_Xna_Framework_Vector3_Microsoft_Xna_Framework_Matrix_Microsoft_Xna_Framework_Matrix_Microsoft_Xna_Framework_Matrix_) the current projection matrix, the view matrix.

    ``` csharp
    // The closest point at which an object can interact.
    Vector3 nearsource = new Vector3((float)mouseX, (float)mouseY, 0f);
    // The furthest point at which an object can interact.
    Vector3 farsource = new Vector3((float)mouseX, (float)mouseY, 1f);
    
    Matrix world = Matrix.CreateTranslation(0, 0, 0);
    
    // Assuming you have the current Projection and View Matrices available.
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

8. Loop through each object in the scene using the [Intersects](xref:Microsoft.Xna.Framework.Ray) method to check whether the [Ray](xref:Microsoft.Xna.Framework.Ray) intersects each object. If it is hit, store the object and the distance at which it was intersected.

    ```csharp
    // Assuming you have an array containing a list of all "hittable" objects in your game
    foreach (var obj in objects)
    {
        float? distance = ray.Intersects(obj.BoundingSphere);
        if (distance.HasValue)
        {
            // A cachable list of all object "HIT" this frame.
            intersectedObjects.Add(new Tuple<MyObject, float>(obj, distance.Value));
        }
    }
    ```

9. Sort the items that have been hit in order of which is closest.

    ```csharp
    // Sort the intersected objects by distance
    intersectedObjects.Sort((a, b) => a.Item2.CompareTo(b.Item2));
    ```

10. When you completely loop through the objects, the last object stored will be the closest object underneath the area the user clicked.

## See Also

- [Rotating and Moving the Camera](HowTo_RotateMoveCamera.md)

### Concepts

- [What Is 3D Rendering?](../../whatis/graphics/WhatIs_3DRendering.md)

### Reference

- [Ray](xref:Microsoft.Xna.Framework.Ray)
- [Ray.Intersects<BoungingBox>](xref:Microsoft.Xna.Framework.Ray#Microsoft_Xna_Framework_Ray_Intersects_Microsoft_Xna_Framework_BoundingBox_)
- [Ray.Intersects<BoungingSphere>](xref:Microsoft.Xna.Framework.Ray#Microsoft_Xna_Framework_Ray_Intersects_Microsoft_Xna_Framework_BoundingSphere_)
- - [Ray.Intersects<Plane>](xref:Microsoft.Xna.Framework.Ray#Microsoft_Xna_Framework_Ray_Intersects_Microsoft_Xna_Framework_Plane_)
- [BoundingBox](xref:Microsoft.Xna.Framework.BoundingBox)
- [BoundingSphere](xref:Microsoft.Xna.Framework.BoundingSphere)
- [Plane](xref:Microsoft.Xna.Framework.Plane)
