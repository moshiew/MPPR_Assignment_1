using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] // Allows script to run in edit mode
[RequireComponent(typeof(LineRenderer))]
public class EnemyPathway : MonoBehaviour
{
    public List<Transform> waypoints; // Waypoints for the path
    public int segmentResolution = 50; // Number of points for each curve segment
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        DrawPathway();
    }

    void Update()
    {
        if (Application.isPlaying)
            DrawPathway(); // Only update the path in play mode
    }

    void OnDrawGizmos()
    {
        DrawPathway(); // Draw path in Scene view even when not playing
    }

    void DrawPathway()
    {
        if (waypoints == null || waypoints.Count < 2) return;

        List<Vector3> pathPoints = new List<Vector3>();

        // Loop through each pair of waypoints to create curve segments
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            Vector3 p0 = waypoints[i].position;
            Vector3 p2 = waypoints[i + 1].position;

            // Calculate smooth control points based on neighboring waypoints
            Vector3 p1 = CalculateSmoothControlPoint(i == 0 ? p0 : waypoints[i - 1].position, p0, p2);

            // Add points along the Bézier curve for this segment
            for (int j = 0; j < segmentResolution; j++)
            {
                float t = j / (float)segmentResolution;
                Vector3 curvePoint = CalculateBezierPoint(t, p0, p1, p2);
                pathPoints.Add(curvePoint);
            }
        }

        // Add the final waypoint position
        pathPoints.Add(waypoints[waypoints.Count - 1].position);

        // Update the LineRenderer with all points
        lineRenderer.positionCount = pathPoints.Count;
        lineRenderer.SetPositions(pathPoints.ToArray());
    }

    Vector3 CalculateSmoothControlPoint(Vector3 pMinus1, Vector3 p0, Vector3 p2)
    {
        // Determine direction and distance for control point
        Vector3 direction = (p2 - pMinus1).normalized;
        float distance = Vector3.Distance(p0, p2) * 0.5f; // Halfway control point distance

        // Return control point adjusted based on direction
        return p0 + direction * distance;
    }

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        // Calculate a point on a quadratic Bezier curve based on t (progress from 0 to 1)
        float u = 1 - t;
        return (u * u * p0) + (2 * u * t * p1) + (t * t * p2);
    }
}
