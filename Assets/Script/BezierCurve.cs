using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BezierCurve
{
    /// <summary>
    /// Calculate the smooth control point for Bezier curve based on neighboring waypoints.
    /// </summary>
    public Vector3 CalculateSmoothControlPoint(Vector3 previousPoint, Vector3 p0, Vector3 p2)
    {
        // Calculates the normalized direction vector from pMinus1 to p2
        Vector3 direction = (p2 - previousPoint).normalized;
        // Calculates the halfway distance between p0 and p2
        float distance = Vector3.Distance(p0, p2) * 0.5f;

        // Return control point adjusted based on direction
        return p0 + direction * distance;
    }

    /// <summary>
    /// Calculate a point on a quadratic Bézier curve based on t (progress from 0 to 1).
    /// </summary>
    public Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        // Calculate a point on a quadratic Bezier curve based on t
        float u = 1 - t;
        return (u * u * p0) + (2 * u * t * p1) + (t * t * p2);
    }

    /// <summary>
    /// Applies an Ease-In easing function, where progress starts slow and accelerates.
    /// </summary>
    /// <param name="t">The input progress value (0 to 1).</param>
    /// <returns>The eased progress value.</returns>
    public float EaseIn(float t)
    {
        return t * t;  // Quadratic easing in
    }

    /// <summary>
    /// Applies an Ease-Out easing function, where progress starts fast and decelerates.
    /// </summary>
    /// <param name="t">The input progress value (0 to 1).</param>
    /// <returns>The eased progress value.</returns>
    public float EaseOut(float t)
    {
        return 1 - (1 - t) * (1 - t);  // Quadratic easing out
    }

    /// <summary>
    /// Applies an Ease-In-Out easing function, where progress starts slow, 
    /// accelerates in the middle, and decelerates at the end.
    /// </summary>
    /// <param name="t">The input progress value (0 to 1).</param>
    /// <returns>The eased progress value.</returns>
    public float EaseInOut(float t)
    {
        // Use a cubic function for smoother transitions
        return t < 0.5 ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2;
    }
}
