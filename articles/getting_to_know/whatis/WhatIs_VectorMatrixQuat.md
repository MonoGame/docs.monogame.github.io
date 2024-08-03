---
title: What are Vectors, Matrices, and Quaternions?
description: What are What are Vectors, Matrices, and Quaternions definitions for MonoGame!
---

# What are Vectors, Matrices, and Quaternions?

The MonoGame Framework Math Libraries are in the [Microsoft.Xna.Framework](xref:Microsoft.Xna.Framework) namespace alongside a number of additional types that deal with The MonoGame Framework Application model.

* [Coordinate system](#coordinate-system)
* [Mathematical Constants and Scalar Manipulation](#mathematical-constants-and-scalar-manipulation)
* [Basic Geometric Types](#basic-geometric-types)
* [Precision and Performance](#precision-and-performance)

## Coordinate system

The MonoGame Framework uses a right-handed coordinate system, with the positive z-axis pointing toward the observer when the positive x-axis is pointing to the right, and the positive y-axis is pointing up.

## Mathematical Constants and Scalar Manipulation

The MonoGame Framework provides the [MathHelper Members](xref:Microsoft.Xna.Framework.MathHelper) class for manipulating scalar values and retrieving some common mathematical constants. This includes methods such as the ```ToDegrees``` and ```ToRadians``` utility methods for converting between degrees and radians.

## Basic Geometric Types

The MonoGame Framework Math library has multiple basic geometric types for manipulating objects in 2D or 3D space. Each geometric type has a number of mathematical operations that are supported for the type.

### Vectors

The MonoGame Framework provides the [Vector2](xref:Microsoft.Xna.Framework.Vector2), [Vector3](xref:Microsoft.Xna.Framework.Vector3), and [Vector4](xref:Microsoft.Xna.Framework.Vector4) classes for representing and manipulating vectors. A vector typically is used to represent a direction and magnitude. In The MonoGame Framework, however, it also could be used to store a coordinate or other data type with the same storage requirements.

Each vector class has methods for performing standard vector operations such as:

* [Dot product](/api/Microsoft.Xna.Framework.Vector3.html#Microsoft_Xna_Framework_Vector3_Dot_Microsoft_Xna_Framework_Vector3_Microsoft_Xna_Framework_Vector3_)
* [Cross product](/api/Microsoft.Xna.Framework.Vector3.html#Microsoft_Xna_Framework_Vector3_Cross_Microsoft_Xna_Framework_Vector3_Microsoft_Xna_Framework_Vector3_)
* [Normalization](/api/Microsoft.Xna.Framework.Vector3.html#Microsoft_Xna_Framework_Vector3_Normalize)
* [Transformation](/api/Microsoft.Xna.Framework.Vector3.html#Microsoft_Xna_Framework_Vector3_Transform_Microsoft_Xna_Framework_Vector3_Microsoft_Xna_Framework_Matrix_)
* [Linear](/api/Microsoft.Xna.Framework.Vector3.html#Microsoft_Xna_Framework_Vector3_Lerp_Microsoft_Xna_Framework_Vector3_Microsoft_Xna_Framework_Vector3_System_Single_), [Cubic](/api/Microsoft.Xna.Framework.Vector3.html#Microsoft_Xna_Framework_Vector3_SmoothStep_Microsoft_Xna_Framework_Vector3_Microsoft_Xna_Framework_Vector3_System_Single_), [Catmull-Rom](/api/Microsoft.Xna.Framework.Vector3.html#Microsoft_Xna_Framework_Vector3_CatmullRom_Microsoft_Xna_Framework_Vector3_Microsoft_Xna_Framework_Vector3_Microsoft_Xna_Framework_Vector3_Microsoft_Xna_Framework_Vector3_System_Single_), or [Hermite spline](/api/Microsoft.Xna.Framework.Vector3.html#Microsoft_Xna_Framework_Vector3_Hermite_Microsoft_Xna_Framework_Vector3_Microsoft_Xna_Framework_Vector3_Microsoft_Xna_Framework_Vector3_Microsoft_Xna_Framework_Vector3_System_Single_) interpolation.

### Matrices

The MonoGame Framework provides the [Matrix](xref:Microsoft.Xna.Framework.Matrix) class for transformation of geometry. The [Matrix](xref:Microsoft.Xna.Framework.Matrix) class uses row major order to address matrices, which means that the row is specified before the column when describing an element of a two-dimensional matrix. The [Matrix](xref:Microsoft.Xna.Framework.Matrix) class provides methods for performing standard matrix operations such as calculating the ```determinate``` or ```inverse``` of a matrix. There also are helper methods for creating scale, rotation, and translation matrices.

### Quaternions

The MonoGame Framework provides the [Quaternion](xref:Microsoft.Xna.Framework.Quaternion) structure to calculate the efficient rotation of a vector by a specified angle.

### Curves

The [Curve](xref:Microsoft.Xna.Framework.Curve) class represents a hermite curve for interpolating varying positions at different times without having to explicitly define each position. The curve is defined by a collection of [CurveKey](xref:Microsoft.Xna.Framework.CurveKey) points representing each varying position at different times. This class can be used not only for spatial motion, but also to represent any response that changes over time.

### Bounding Volumes

The MonoGame Framework provides the [BoundingBox](xref:Microsoft.Xna.Framework.BoundingBox), [BoundingFrustum](xref:Microsoft.Xna.Framework.BoundingFrustum), [BoundingSphere](xref:Microsoft.Xna.Framework.BoundingSphere), [Plane](xref:Microsoft.Xna.Framework.Plane), and [Ray](xref:Microsoft.Xna.Framework.Ray) classes for representing simplified versions of geometry for the purpose of efficient collision and hit testing. These classes have methods for checking for intersection and containment with each other.

## Precision and Performance

The MonoGame Framework Math libraries are single-precision. This means that the primitives and operations contained in this library use 32-bit floating-point numbers to achieve a balance between precision and efficiency when performing large numbers of calculations.

A 32-bit floating-point number ranges from ```–3.402823e38``` to ```+3.402823e38```. The 32 bits store the sign, mantissa, and exponent of the number that yields seven digits of floating-point precision. 

> Some numbers—for example π, 1/3, or the square root of two—can be approximated only with seven digits of precision, so be aware of rounding errors when using a binary representation of a floating-point number. 

For more information about single-precision numbers, see the documentation for the [Single](http://msdn.microsoft.com/en-us/library/system.single.aspx) data type.

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
