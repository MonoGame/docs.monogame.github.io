---
title: What Is a View Frustum?
description: The definition for the View Frustum for MonoGame!
---

# What Is a View Frustum?

A view frustum is a 3D volume that defines how models are projected from camera space to projection space. Objects must be positioned within the 3D volume to be visible.

The MonoGame Framework uses a projection matrix to calculate a vertex position in the view frustum. The matrix reverses the y-coordinate (so it is top down) to reflect a screen origin at the top-left corner. After the matrix is applied, the homogenous vertices (that is, the vertices contain (x,y,z,w) coordinates) are converted to non-homogeneous coordinates so they can be rasterized.

The most common type of projection is called a perspective projection, which makes objects near the camera appear bigger than objects in the distance. For a perspective projection, the view frustum can be visualized as a clipped pyramid whose top and bottom are defined by the near and far clipping planes as shown in the figure.

![A diagram visualisation of the View Frustrum](../images/frustum.jpg)

A view frustum is defined by a field of view (fov), and by the distances of the front and back clipping planes, which are specified in z-coordinates. Set up a perspective fov using the [CreatePerspectiveFieldOfView](/api/Microsoft.Xna.Framework.Matrix.html#Microsoft_Xna_Framework_Matrix_CreatePerspectiveFieldOfView_System_Single_System_Single_System_Single_System_Single_) method.

## See Also

[What Is a Viewport?](WhatIs_Viewport.md)  

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
