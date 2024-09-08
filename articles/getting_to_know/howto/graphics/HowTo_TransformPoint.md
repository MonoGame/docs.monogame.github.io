---
title: How to transform a Point
description: This example demonstrates how to use the Vector3 and Matrix classes to transform a point. A matrix transform can include scaling, rotating, and translating information.
requireMSLicense: true
---

## Overview

Transformation matrices are the cornerstone of 3D rendering and how we manipulate content to be drawn in a 3D environment.

> [!NOTE]
> For a great primer on Transformation matrices, check out:
>
> [Transformation Matrix Overview - Geeks for Geeks](https://www.geeksforgeeks.org/transformation-matrix/)

## Transforming a Point with a Matrix

In MonoGame, matrices are transformed using the [Vector3.Transform](xref:Microsoft.Xna.Framework.Vector3#Microsoft_Xna_Framework_Vector3_Transform_Microsoft_Xna_Framework_Vector3_Microsoft_Xna_Framework_Matrix_) function with an applied matrix.

1. Create a [Matrix](xref:Microsoft.Xna.Framework.Matrix) by using **CreateRotationY** or one of the other **Create** methods.

    Create Rotation Y will create a matrix that is rotated a number of radians around its center.

2. Pass the point and the [Matrix](xref:Microsoft.Xna.Framework.Matrix) to the [Vector3.Transform](xref:Microsoft.Xna.Framework.Vector3#Microsoft_Xna_Framework_Vector3_Transform_Microsoft_Xna_Framework_Vector3_Microsoft_Xna_Framework_Matrix_) method.

The following code example is a static function that accept a [Vector3](xref:Microsoft.Xna.Framework.Vector3) and translate its position (or rotate it) by the amount of radians (NOT degrees) provided.

```csharp
static Vector3 RotatePointOnYAxis(Vector3 point, float angle)
{
    // Create a rotation matrix that represents a rotation of angle radians.
    Matrix rotationMatrix = Matrix.CreateRotationY(angle);

    // Apply the rotation matrix to the point.
    Vector3 rotatedPoint = Vector3.Transform(point, rotationMatrix);

    return rotatedPoint;
}
```

## See Also

* [Drawing 3D Primitives using Lists or Strips](HowTo_Draw_3D_Primitives.md)

### Matrix Creation Methods

[Matrix](xref:Microsoft.Xna.Framework.Matrix)

* CreateRotationX
* CreateRotationY
* CreateRotationZ
* CreateScale
* CreateTranslation
