---
title: How to test collisions with Bounding Volumes
description: A walkthrough what is involved in figuring out if two objects collide for MonoGame!
---

# Bounding Volumes and Collisions

Collision detection determines whether objects in a game world overlap each other.

The MonoGame Framework provides several classes and methods to speed implementation of collision detection systems in games.

* [Bounding Volume Classes](#bounding-volume-classes)
* [Non-Bounding Volume Classes](#non-bounding-volume-classes)
* [Contains and Intersects Methods](#contains-and-intersects-methods)
* [Adding New Collision Data Structures](#adding-new-collision-data-structures)

## Bounding Volume Classes

The MonoGame Framework has three classes that represent three-dimensional volumes. Use a bounding volume class to approximate the volume occupied by a complex object using a volume that is less complex and faster for performing collision checking. All of the bounding volume classes support intersection and containment tests with each other and the plane and ray classes.

### Bounding Sphere

The [BoundingSphere Structure](xref:Microsoft.Xna.Framework.BoundingSphere) represents the space occupied by a sphere.

There are several benefits of using a bounding sphere for collision detection.

* Sphere to sphere checks are very fast. To check for collision between two spheres, the distance between the centers of the spheres is compared to the sum of the radii of both spheres. If the distance is less than the combined radii of both spheres, the spheres intersect.
* The [BoundingSphere Structure](xref:Microsoft.Xna.Framework.BoundingSphere) class is compact. It stores only a vector representing its center and its radius.
* Unlike a bounding box, a bounding sphere doesn’t need to be recreated if the model rotates. If the model being bounded rotates, the bounding sphere will still be large enough to contain it.
* Moving a bounding sphere is inexpensive. Just add a value to the center.

There is one major drawback to using the bounding sphere class for collision detection.

* Unless the object being approximated is sphere shaped, the bounding sphere will have some empty space, which could result in false positive results. Long narrow objects will have the most empty space in their bounding spheres.

### Bounding Box

The [BoundingBox Structure](xref:Microsoft.Xna.Framework.BoundingBox) represents the space occupied by a box. The bounding box class is axis aligned. Each face of the bounding box is perpendicular to the x-axis, the y-axis, or the z-axis.

There are several benefits of using the bounding box for collision detection.

* The bounding box class fits rectangular shapes aligned with the axis very well. Compared to the bounding sphere class, the bounding box class provides a much tighter fit for non-rotated rectangular objects.
* Because the bounding box class is axis aligned, you can make certain assumptions that result in collision checks between bounding boxes being quicker than a bounding box that can be rotated.

There are a few drawbacks of using the bounding box for collision detection.

* Rotating a bounding box causes it to no longer be axis aligned. Because of this, if you rotate a model being bounded, you will need to recreate the bounding box. Doing so can be slow, since all the points in an object are iterated through to get the bounding box. If the model has not changed orientation, you can translate the bounding box instead of recreating it.
* If the model being bounded is not aligned to the axis, the bounding box will have some empty space. The amount of empty space will be greatest when the object is rotated 45 degrees from an axis.
* Empty space in the bounding box can result in false positives when checking for collision.

### Bounding Frustum

Use a [BoundingFrustum Class](xref:Microsoft.Xna.Framework.BoundingFrustum) to create a bounding volume that corresponds to the space visible to the camera. You create a bounding frustum from the combined view and projection matrix that the camera is using currently. If the camera moves or rotates, you need to recreate the bounding frustum. The bounding frustum isn’t used to determine when two objects collide, but rather when an object intersects with the volume of space viewable by the camera. Objects that do not intersect and are not contained by the bounding frustum are not visible to the camera and don’t need to be drawn. For complex models, this can reduce the number of pixels that need to be rendered.

## Non-Bounding Volume Classes

### Plane

The [Plane Structure](xref:Microsoft.Xna.Framework.Plane) describes a 2D plane. The plane is defined by a normal vector (perpendicular to the plane) and a point on the plane. The plane class supports intersection tests with the bounding volume classes. The plane class’s intersection test returns the tested object's position relative to the plane. The return value indicates whether the object intersects the plane. If the object does not intersect the plane, the return value indicates whether the object is on the plane’s front side or back side.

### Ray

The [Ray Structure](xref:Microsoft.Xna.Framework.Ray) describes a ray starting at a point in space. The ray structure supports intersection tests with the bounding volume classes. The return value of the ray intersection tests is the distance the intersection occurred at, or null if no intersection occurred.

### Model

In addition to the information needed to draw a model, the [Model Class](xref:Microsoft.Xna.Framework.Graphics.Model) contains bounding volumes for its parts. When a model is imported, the content pipeline calculates the bounding sphere for each of the model's parts. To check for collision between two models, you can compare the bounding spheres for one model to all of the bounding spheres of the other model.

## Contains and Intersects Methods

Bounding volume classes have methods to support two types of collision tests: intersection tests and containment tests. The intersects methods check whether the two objects being tested overlap in any way. As soon as the test finds that the objects do intersect, it returns without trying to determine the degree of the intersection. The contains methods determine whether the objects simply intersect or whether one of the objects is completely contained by the other. Since the intersects methods need only determine whether an intersection occurred, they tend to be faster than the contains methods.

## Adding New Collision Data Structures

When implementing other bounding volume classes and intersection tests, you will probably need to add a custom content pipeline processor. For example, your game might need to use convex hulls for collision detection. You could use a custom processor to determine the convex hull and then place it in the model's tag field. Then, when the model is loaded at run time, the convex hull information would be available in the model. For more information, see [Extending a Standard Content Processor](Content_Pipeline/HowTo_Extend_Processor.md).

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
