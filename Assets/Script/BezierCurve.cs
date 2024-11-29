using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BezierCurve
{
    /// <summary>
    /// Calculates a smooth control point for a quadratic Bezier curve.
    /// </summary>
    /// <param name="previousPoint">The previous point in the curve.</param>
    /// <param name="p0">The current point in the curve.</param>
    /// <param name="p2">The next point in the curve.</param>
    /// <returns>A Vector3 representing the calculated control point.</returns>
    public Vector3 CalculateSmoothControlPoint(Vector3 previousPoint, Vector3 p0, Vector3 p2)
    {
        // Calculate the normalized direction vector from the previous point to the next point
        Vector3 direction = (p2 - previousPoint).normalized;
        // Calculate half the distance between p0 and p2
        float distance = Vector3.Distance(p0, p2) * 0.5f;

        // Return the control point positioned along the direction vector from p0
        return p0 + direction * distance;
    }

    /// <summary>
    /// Calculates a point on a quadratic Bezier curve given a progress value t.
    /// </summary>
    /// <param name="t">The progress along the curve (0 to 1).</param>
    /// <param name="p0">The start point of the curve.</param>
    /// <param name="p1">The control point of the curve.</param>
    /// <param name="p2">The end point of the curve.</param>
    /// <returns>A Vector3 representing the position on the curve at the given t.</returns>
    public Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        // Calculate the complementary value of t
        float u = 1 - t;
        // Compute the quadratic Bezier formula
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
