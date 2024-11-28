using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Waypoints")]
    private int waypointsTracker = 0;                        // Index to track the current waypoint

    [Header("Enemy")]
    private float enemySpd = 10f;                            // Enemy movement speed

    [Header("Lerp")]
    private float tParam = 0;                                // Interpolation parameter (ranges from 0 to 1)

    [Header("Helper Class")]
    private BezierCurve bezierCurve = new BezierCurve();// Helper class to calculate Bezier curve points
    private EnemyPathwayRenderer enemyPathwayRenderer; // Renderer for visualizing pathways
    private EnemySpawner enemySpawner;

    private void Start()
    {
        enemyPathwayRenderer = GameObject.Find("QuadraticBezierPoints").GetComponent<EnemyPathwayRenderer>();
        enemySpawner = GameObject.Find("EnemySpawnPoint").GetComponent<EnemySpawner>();

        // Ensure there are enough waypoints and start the movement coroutine
        if (enemySpawner.waypointsList != null && enemySpawner.waypointsList.Count >= 2)
        {
            StartCoroutine(EnemyMoveByRoute());
        }
    }

    /// <summary>
    /// Coroutine to move the enemy smoothly along the waypoints using Bezier curves.
    /// </summary>
    /// <returns>An enumerator for coroutine execution.</returns>
    private IEnumerator EnemyMoveByRoute()
    {
        while (true)
        {
            // Define Bezier curve points
            Vector3 p0 = enemySpawner.waypointsList[waypointsTracker].position;     // Start point
            Vector3 p2 = enemySpawner.waypointsList[waypointsTracker + 1].position; // End point
            Vector3 p1 = bezierCurve.CalculateSmoothControlPoint(waypointsTracker == 0 ? p0 : enemySpawner.waypointsList[waypointsTracker - 1].position, p0, p2); // Control point

            tParam = 0f; // Reset interpolation parameter for the current segment

            // Calculate the curve length and determine the traversal time
            float pathLength = CalculatePathLength(p0, p1, p2);
            float totalTime = pathLength / enemySpd;

            float elapsedTime = 0f;

            // Move the enemy along the curve until the end of the segment
            while (tParam < 1f)
            {
                tParam = elapsedTime / totalTime; // Update interpolation parameter

                // Update the enemy's position based on the Bezier curve
                transform.position = bezierCurve.CalculateBezierPoint(tParam, p0, p1, p2);

                elapsedTime += Time.deltaTime; // Increment elapsed time
                yield return null;
            }

            waypointsTracker++; // Move to the next waypoint

            // Check if the enemy has reached the final waypoint
            if (Vector3.Distance(transform.position, enemySpawner.waypointsList[enemySpawner.waypointsList.Count - 1].position) < 0.5f)
            {
                Destroy(gameObject); // Destroy the enemy object
                break;
            }
        }
    }

    /// <summary>
    /// Estimates the length of a Bezier curve by summing distances between sampled points.
    /// </summary>
    /// <param name="p0">Start point of the curve.</param>
    /// <param name="p1">Control point of the curve.</param>
    /// <param name="p2">End point of the curve.</param>
    /// <returns>The estimated length of the Bezier curve.</returns>
    private float CalculatePathLength(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float pathLength = 0f;
        Vector3 previousPoint = p0; // Start from the initial point

        // Sample points along the curve and calculate segment lengths
        for (int i = 1; i <= enemyPathwayRenderer.segmentResolution; i++)
        {
            float t = i / (float)enemyPathwayRenderer.segmentResolution; // Interpolation parameter
            Vector3 currentPoint = bezierCurve.CalculateBezierPoint(t, p0, p1, p2); // Current point on the curve

            pathLength += Vector3.Distance(previousPoint, currentPoint); // Add the segment length
            previousPoint = currentPoint; // Update the previous point
        }

        return pathLength;
    }
}
