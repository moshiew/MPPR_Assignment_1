using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpAndBezierCurves : MonoBehaviour
{
    /// <summary>
    /// Performs linear interpolation between two points in 3D space.
    /// </summary>
    /// <param name="A">The starting point (Vector3).</param>
    /// <param name="B">The ending point (Vector3).</param>
    /// <param name="t">A value between 0 and 1 that represents the interpolation factor.</param>
    /// <returns>The interpolated point (Vector3) between A and B based on t.</returns>
    public static Vector3 myLerp(Vector3 A, Vector3 B, float t)
    {
        // Ensure t is clamped between 0 and 1
        t = Mathf.Clamp01(t);

        // Calculate the interpolated point
        Vector3 result = (1 - t) * A + t * B;

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pStart">The starting point (Vector3).</param>
    /// <param name="pEnd">The ending point (Vector3).</param>
    /// <param name="pCtrl">The control point (Vector3).</param>
    /// <param name="t">A value between 0 and 1 that represents the interpolation factor.</param>
    /// <returns>The </returns>
    public Vector3 bezierCurve(Vector3 pStart, Vector3 pEnd, Vector3 pCtrl, float t)
    {
        Vector3 pointA = myLerp(pStart, pCtrl, t);
        Vector3 pointB = myLerp(pCtrl, pEnd, t);

        Vector3 pointAB = myLerp(pointA, pointB, t);

        return pointAB;
    }
}
