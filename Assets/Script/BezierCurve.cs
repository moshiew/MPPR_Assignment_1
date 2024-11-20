using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BezierCurve
{
    public List<Transform> waypoints; // Nummber of waypoints
    public int segmentResolution = 50; // Number of points for each curve

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
}
