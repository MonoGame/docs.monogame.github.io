---
title: How to position the Camera
description: Demonstrates how to position the camera so that all objects in a scene are within the view frustum while maintaining the camera's original orientation.
requireMSLicense: true
---

## Overview

Managing cameras in a 3D environment can be one of the most challenging tasks for any MonoGame developer, if your camera is looking the wrong way then you will not see anything on screen, or at least not what you were expecting.

A simple technique to handle this is to create a [BoundingSphere](xref:Microsoft.Xna.Framework.BoundingSphere) around your visible game content and then have the camera look at the center of it.

## To position the camera to view all objects in a scene

1. Create a [BoundingSphere](xref:Microsoft.Xna.Framework.BoundingSphere) class that contains all of the objects in the scene. To create the sphere, loop through all of the objects in the scene, merging the [BoundingSphere](xref:Microsoft.Xna.Framework.BoundingSphere) classes that contain them with **CreateMerged**.

2. If you are not already tracking the [BoundingSphere](xref:Microsoft.Xna.Framework.BoundingSphere) classes for collision detection, use **CreateFromBoundingBox** or **CreateFromPoints** to create them from [BoundingBox](xref:Microsoft.Xna.Framework.BoundingBox) classes or points.

    In this example, the [BoundingSphere](xref:Microsoft.Xna.Framework.BoundingSphere) classes are created from [BoundingBox](xref:Microsoft.Xna.Framework.BoundingBox) classes.

    ```csharp
    BoundingSphere GetSceneSphere()
    {
        BoundingSphere sceneSphere =
            new BoundingSphere(new Vector3(.5f, 1, .5f), 1.5f);
        for (int z = 0; z < 5; z++)
        {
            for (int x = 0; x < 5; x++)
            {
                BoundingSphere boundingSphere =
                    sphere.Meshes[0].BoundingSphere;
                boundingSphere.Center = new Vector3(x * 3, 0, z * 3);
    
                sceneSphere = BoundingSphere.CreateMerged(
                    sceneSphere, boundingSphere);
            }
        }
    
        return sceneSphere;
    }
    ```

3. Set the position of the camera to the center of the [BoundingSphere](xref:Microsoft.Xna.Framework.BoundingSphere) that contains the scene.

    ```csharp
    cameraPosition = sceneSphere.Center;
    ```

4. Determine the distance from the center of the [BoundingSphere](xref:Microsoft.Xna.Framework.BoundingSphere) that the camera needs to be to view the entire scene.

    This distance is equal to the hypotenuse of the triangle formed by the center of the sphere, the desired camera position, and the point where the sphere touches the view frustum. One angle of the triangle is known to be the field of view of the camera divided by two. One leg of the triangle is known to be the radius of the sphere. Given these two measurements, you can calculate the hypotenuse as the radius of the sphere divided by the sine of half the field of view.

    ```csharp
    float distanceToCenter = sceneSphere.Radius / (float)Math.Sin(FOV / 2);
    ```

5. Get the [Backward](xref:Microsoft.Xna.Framework.Matrix.Backward) vector of the view [Matrix](xref:Microsoft.Xna.Framework.Matrix) and flip its X component.

    ```csharp
    Vector3 back = view.Backward;
    back.X = -back.X; //flip x's sign
    ```

6. To move the camera backward with respect to its orientation, multiply the desired distance by the adjusted back vector from the previous step.

    The camera is now facing the center of the sphere containing the scene and is far enough back that the sphere fits in the camera's view frustum.

    ```csharp
    cameraPosition += (back * distanceToCenter);
    ```

## See Also

- [Rotating and Moving the Camera](HowTo_RotateMoveCamera.md)

### Concepts

- [What Is a View Frustum?](../../whatis/graphics/WhatIs_ViewFrustum.md)
- [What Is a Viewport?](../../whatis/graphics/WhatIs_Viewport.md)

### Reference

- [BoundingSphere](xref:Microsoft.Xna.Framework.BoundingSphere)
- [Matrix](xref:Microsoft.Xna.Framework.Matrix)
