---
title: How to move the Camera on a Curve
description: Demonstrates how to use the Curve and CurveKey classes to move a camera along the shape of a curve.
---

# Moving the Camera on a Curve

Demonstrates how to use the [Curve](xref:Microsoft.Xna.Framework.Curve) and [CurveKey](xref:Microsoft.Xna.Framework.CurveKey) classes to move a camera along the shape of a curve.

Using [Curve](xref:Microsoft.Xna.Framework.Curve)s allows a path to be defined by a small number of control points with the [Curve](xref:Microsoft.Xna.Framework.Curve)s calculating the points on the path between the control points.

## Scripting the Camera to Follow a Curve

### To script camera movement

1. Create an instance of the [Curve](xref:Microsoft.Xna.Framework.Curve) class for each component being scripted.

    In this case, you need two sets of three curves. One is for each of the x, y, and z components of the camera's position, and the other is for the position at which the camera is looking (the "look-at" position).

    ```csharp
    class Curve3D
    {
    
        public Curve curveX = new Curve();
        public Curve curveY = new Curve();
        public Curve curveZ = new Curve();
        ...
    }
    ```

2. Set the **PreLoop** and **PostLoop** type of each [Curve](xref:Microsoft.Xna.Framework.Curve).

    The **PreLoop** and **PostLoop** types determine how the curve will interpret positions before the first key or after the last key. In this case, the values will be set to [CurveLoopType.Oscillate](xref:Microsoft.Xna.Framework.CurveLoopType). Values past the ends of the curve will change direction and head toward the opposite side of the curve.

    ```csharp
    curveX.PostLoop = CurveLoopType.Oscillate;
    curveY.PostLoop = CurveLoopType.Oscillate;
    curveZ.PostLoop = CurveLoopType.Oscillate;
    
    curveX.PreLoop = CurveLoopType.Oscillate;
    curveY.PreLoop = CurveLoopType.Oscillate;
    curveZ.PreLoop = CurveLoopType.Oscillate;
    ```

3. Add [CurveKey](xref:Microsoft.Xna.Framework.CurveKey)s to the [Curve](xref:Microsoft.Xna.Framework.Curve)s.

4. Specify the time each [CurveKey](xref:Microsoft.Xna.Framework.CurveKey) should be reached and the camera position when the [CurveKey](xref:Microsoft.Xna.Framework.CurveKey) is reached.

    In this case, each point in time will have three [CurveKey](xref:Microsoft.Xna.Framework.CurveKey)s associated with it – one for each of the x, y, and z coordinates of the point on the [Curve](xref:Microsoft.Xna.Framework.Curve).

    ```csharp
    public void AddPoint(Vector3 point, float time)
    {
        curveX.Keys.Add(new CurveKey(time, point.X));
        curveY.Keys.Add(new CurveKey(time, point.Y));
        curveZ.Keys.Add(new CurveKey(time, point.Z));
    }
    ```

5. Loop through each [Curve](xref:Microsoft.Xna.Framework.Curve) setting the **TangentIn** and **TangentOut** of each [CurveKey](xref:Microsoft.Xna.Framework.CurveKey).

    The tangents of the [CurveKey](xref:Microsoft.Xna.Framework.CurveKey)s control the shape of the [Curve](xref:Microsoft.Xna.Framework.Curve). Setting the tangents of the [CurveKey](xref:Microsoft.Xna.Framework.CurveKey)s to the slope between the previous and next [CurveKey](xref:Microsoft.Xna.Framework.CurveKey) will give a curve that moves smoothly through each point on the curve.

    ```csharp
    public void SetTangents()
    {
        CurveKey prev;
        CurveKey current;
        CurveKey next;
        int prevIndex;
        int nextIndex;
        for (int i = 0; i < curveX.Keys.Count; i++)
        {
            prevIndex = i - 1;
            if (prevIndex < 0) prevIndex = i;
    
            nextIndex = i + 1;
            if (nextIndex == curveX.Keys.Count) nextIndex = i;
    
            prev = curveX.Keys[prevIndex];
            next = curveX.Keys[nextIndex];
            current = curveX.Keys[i];
            SetCurveKeyTangent(ref prev, ref current, ref next);
            curveX.Keys[i] = current;
            prev = curveY.Keys[prevIndex];
            next = curveY.Keys[nextIndex];
            current = curveY.Keys[i];
            SetCurveKeyTangent(ref prev, ref current, ref next);
            curveY.Keys[i] = current;
    
            prev = curveZ.Keys[prevIndex];
            next = curveZ.Keys[nextIndex];
            current = curveZ.Keys[i];
            SetCurveKeyTangent(ref prev, ref current, ref next);
            curveZ.Keys[i] = current;
        }
    }
    ```

6. Add code to evaluate the x, y, and z coordinates of the [Curve](xref:Microsoft.Xna.Framework.Curve)s at any given time by passing the elapsed time to the **Evaluate** method of each of the [Curve](xref:Microsoft.Xna.Framework.Curve)s.

    ```csharp
    public Vector3 GetPointOnCurve(float time)
    {
        Vector3 point = new Vector3();
        point.X = curveX.Evaluate(time);
        point.Y = curveY.Evaluate(time);
        point.Z = curveZ.Evaluate(time);
        return point;
    }
    ```

7. Create a variable to track the amount of time that has passed since the camera started moving.

    ```csharp
    double time;
    ```

8. In [Game](xref:Microsoft.Xna.Framework.Game) **Update**, set the camera's position and look-at position based on the elapsed time since the camera started moving, and then set the camera's view and projection matrices as in [Rotating and Moving the Camera](HowTo_RotateMoveCamera.md).

    ```csharp
    // Calculate the camera's current position.
    Vector3 cameraPosition =
        cameraCurvePosition.GetPointOnCurve((float)time);
    Vector3 cameraLookat =
        cameraCurveLookat.GetPointOnCurve((float)time);
    ```

9. In [Game.Update](xref:Microsoft.Xna.Framework.Game#Microsoft_Xna_Framework_Game_Update_Microsoft_Xna_Framework_GameTime_), use [gameTime.ElapsedGameTime.TotalMilliseconds](xref:Microsoft.Xna.Framework.GameTime.ElapsedGameTime) to increment the time since the camera started moving.

    ```csharp
    time += gameTime.ElapsedGameTime.TotalMilliseconds;
    ```

## See Also

[Rotating and Moving the Camera](HowTo_RotateMoveCamera.md)  

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
