using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyPathwayBezierCurve
{
    public List<Transform> waypoints; // Nummber of waypoints
    public int segmentResolution = 50; // Number of points for each curve

    public List<Vector3> GetPathPoints()
    {
        List<Vector3> pathPoints = new List<Vector3>(); // Creates list to store pathpoints

        // Returns empty list pathPoints even if waypoints is null or less than 2
        if (waypoints == null || waypoints.Count < 2)
        {
            Debug.LogWarning("Not enough waypoints to create a path!");
            return pathPoints;
        }

        // Loop through each pair of waypoints to create curve segments
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            Vector3 p0 = waypoints[i].position;
            Vector3 p2 = waypoints[i + 1].position;

            // Calculate smooth control points based on neighboring waypoints
            Vector3 previousPoint;

            if (i == 0)
            {
                previousPoint = p0; // Uses the start point
            }
            else
            {
                previousPoint = waypoints[i - 1].position; // Uses the previous waypoint
            }

            // Calculate control point for Bezier curve
            Vector3 p1 = CalculateSmoothControlPoint(previousPoint, p0, p2);


            // Add points along the Bezier curve for this segment
            // Divides into segmentResolution steps to generate curve points
            // t is a normalized value (from 0 to 1)
            for (int j = 0; j < segmentResolution; j++)
            {
                float t = j / (float)segmentResolution;
                Vector3 curvePoint = CalculateBezierPoint(t, p0, p1, p2);
                pathPoints.Add(curvePoint);
            }
        }

        // Ensures the last waypoint is added to the path
        pathPoints.Add(waypoints[waypoints.Count - 1].position);

        return pathPoints;
    }

    /// <summary>
    /// Calculate the smooth control point for Bezier curve based on neighboring waypoints.
    /// </summary>
    Vector3 CalculateSmoothControlPoint(Vector3 previousPoint, Vector3 p0, Vector3 p2)
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
    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        // Calculate a point on a quadratic Bezier curve based on t
        float u = 1 - t;
        return (u * u * p0) + (2 * u * t * p1) + (t * t * p2);
    }
}
