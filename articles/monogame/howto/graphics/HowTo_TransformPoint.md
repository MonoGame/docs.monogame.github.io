---
title: How to transform a Point
description: This example demonstrates how to use the Vector3 and Matrix classes to transform a point. A matrix transform can include scaling, rotating, and translating information.
---

# Transforming a Point

This example demonstrates how to use the [Vector3](xref:Microsoft.Xna.Framework.Vector3) and [Matrix](xref:Microsoft.Xna.Framework.Matrix) classes to transform a point. A matrix transform can include scaling, rotating, and translating information.

## Transforming a Point with a Matrix

### To transform a point

1. Create a [Matrix](xref:Microsoft.Xna.Framework.Matrix) by using **CreateRotationY** or one of the other **Create** methods.
2. Pass the point and the [Matrix](xref:Microsoft.Xna.Framework.Matrix) to the [Vector3.Transform](xref:Microsoft.Xna.Framework.Vector3) method.

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

### Matrix Creation Methods

[Matrix](xref:Microsoft.Xna.Framework.Matrix)

* CreateRotationX
* CreateRotationY
* CreateRotationZ
* CreateScale
* CreateTranslation

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
