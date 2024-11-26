using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EnemyPathwayRenderer : MonoBehaviour
{
    public EnemyPathwayBezierCurve bezierCurve; // Reference to the EnemyPathwayBezierCurve for path points
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
        if (bezierCurve.waypoints == null) return;

        foreach (var waypoint in bezierCurve.waypoints)
        {
            if (waypoint != null)
            {
                waypoint.gameObject.SetActive(isActive);
            }
        }
    }

    void ClearPath()
    {
        // Clear line renderer path
        lineRenderer.positionCount = 0;
    }
    void DrawPathway()
    {
        // Checks if showPath is true and if there are enough waypoints
        if (!showPath || bezierCurve.waypoints == null || bezierCurve.waypoints.Count < 2) return;

        List<Vector3> pathPoints = bezierCurve.GetPathPoints();

        // Update the LineRenderer with all points
        lineRenderer.positionCount = pathPoints.Count;
        lineRenderer.SetPositions(pathPoints.ToArray());
    }
}
