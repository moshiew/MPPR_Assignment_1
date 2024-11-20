using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private List<Transform> waypoints;  // List of waypoints the enemy will follow

    private float enemySpd = 10f;       // Enemy speed
    int waypointsTracker = 0;           // Track which waypoints that the enemy have to follow next

    private float tParam = 0;           // Parameter 't' used to interpolate along the Bezier curve (0 to 1)

    private BezierCurve bezierCurve = new BezierCurve();  // Helper class to calculate Bezier curve points and control points

    private void Start()
    {
        // Start the coroutine to move the enemy along the waypoints
        if (waypoints != null && waypoints.Count >= 2)
        {
            StartCoroutine(EnemyMoveByRoute());
        }
    }

    /// <summary>
    /// Coroutine to move the enemy smoothly along the waypoints using Bezier curves
    /// </summary>
    /// <returns>Coroutine for continuous movement</returns>
    private IEnumerator EnemyMoveByRoute()
    {
        while (true)
        {
            Vector3 p0 = waypoints[waypointsTracker].position;
            Vector3 p2 = waypoints[waypointsTracker + 1].position;
            Vector3 p1 = bezierCurve.CalculateSmoothControlPoint(waypointsTracker == 0 ? p0 : waypoints[waypointsTracker - 1].position, p0, p2);

            tParam = 0f; // Reset t parameter for the segment

            float pathLength = CalculatePathLength(p0, p1, p2); // Calculate the length of the Bezier curve for this segment
            float totalTime = pathLength / enemySpd; // Calculate the total time required to traverse the segment based on speed

            float elapsedTime = 0f;

            // Interpolate along the curve until t reaches 1
            while (tParam < 1f)
            {
                tParam = elapsedTime/totalTime;

                // Get the next position on the curve
                transform.position = bezierCurve.CalculateBezierPoint(tParam, p0, p1, p2);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            waypointsTracker++;

            // Check if enemy reached the final waypoint
            if (Vector3.Distance(transform.position, waypoints[waypoints.Count - 1].transform.position) < 0.5)
            {
                Destroy(gameObject);
                break;
            }
        }
    }


    /// <summary>
    /// Estimate the length of the Bezier curve by summing distances between interpolated points
    /// </summary>
    /// <param name="p0">start point</param>
    /// <param name="p1">control point</param>
    /// <param name="p2">end point</param>
    /// <returns>The length of the bezier curve</returns>
    private float CalculatePathLength(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float pathLength = 0f;
        Vector3 previousPoint = p0;

        for (int i = 1; i <= bezierCurve.segmentResolution; i++)
        {
            // splitting the t value to a smaller points using segmentResolution
            float t = i / (float)bezierCurve.segmentResolution;
            Vector3 currentPoint = bezierCurve.CalculateBezierPoint(t, p0, p1, p2);

            pathLength += Vector3.Distance(previousPoint, currentPoint); // use Vector3.Distance to calculate the distance and sum up.
            previousPoint = currentPoint;
        }

        return pathLength;
    }
}
