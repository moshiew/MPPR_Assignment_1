using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EnemyPathwayRenderer : MonoBehaviour
{
    public List<Transform> waypoints; // Nummber of waypoints
    public int segmentResolution = 50; // Number of points for each curve

    public BezierCurve bezierCurve; // Reference to the EnemyPathwayBezierCurve for path points
    public bool showPath = true;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (showPath)
        {
            DrawPathway();
        }
    }

    void Update()
    {
        // Draw the path in runtime, and clear the path if showPath = false
        if (Application.isPlaying && showPath)
        {
            DrawPathway();
            SetWaypointsActive(true);

        }
        else if (Application.isPlaying && !showPath)
        {
            ClearPath();
            SetWaypointsActive(false);
        }
    }

    // Enable and disable waypoints along with line renderer
    void SetWaypointsActive(bool isActive)
    {
        if (waypoints == null) return;

        foreach (var waypoint in waypoints)
        {
            if (waypoint != null)
            {
                waypoint.gameObject.SetActive(isActive);
            }
        }
    }

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
            Vector3 p1 = bezierCurve.CalculateSmoothControlPoint(previousPoint, p0, p2);


            // Add points along the Bezier curve for this segment
            // Divides into segmentResolution steps to generate curve points
            // t is a normalized value (from 0 to 1)
            for (int j = 0; j < segmentResolution; j++)
            {
                float t = j / (float) segmentResolution;
                Vector3 curvePoint = bezierCurve.CalculateBezierPoint(t, p0, p1, p2);
                pathPoints.Add(curvePoint);
            }
        }

        // Ensures the last waypoint is added to the path
        pathPoints.Add(waypoints[waypoints.Count - 1].position);

        return pathPoints;
    }

    void ClearPath()
    {
        // Clear line renderer path
        lineRenderer.positionCount = 0;
    }
    void DrawPathway()
    {
        // Checks if showPath is true and if there are enough waypoints
        if (!showPath || waypoints == null || waypoints.Count < 2) return;

        List<Vector3> pathPoints = GetPathPoints();

        // Update the LineRenderer with all points
        lineRenderer.positionCount = pathPoints.Count;
        lineRenderer.SetPositions(pathPoints.ToArray());
    }
}
