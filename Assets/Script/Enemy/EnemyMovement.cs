using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Waypoints")]
    private int waypointsTracker = 0; // Tracks the current waypoint index for the enemy.

    [Header("Enemy")]
    private float enemySpd = 2f; // Speed of the enemy's movement.

    [Header("Lerp")]
    private float tParam = 0; // Interpolation parameter (ranges from 0 to 1) for Bezier curve traversal.

    [Header("Helper Class")]
    private BezierCurve bezierCurve = new BezierCurve();
    private EnemyPathwayRenderer enemyPathwayRenderer;
    private EnemySpawner enemySpawner;
    private TextPulse textPulse;

    private void Start()
    {
        enemyPathwayRenderer = GameObject.Find("QuadraticBezierPoints").GetComponent<EnemyPathwayRenderer>();
        enemySpawner = GameObject.Find("EnemySpawnPoint").GetComponent<EnemySpawner>();
        textPulse = GameObject.Find("Canvas").GetComponentInChildren<TextPulse>();

        // Perform a check to see if there is more than 1 waypoints in waypointsList
        if (enemySpawner.waypointsList != null && enemySpawner.waypointsList.Count >= 2)
        {
            StartCoroutine(EnemyMoveByRoute());
        }
    }

    /// <summary>
    /// Coroutine to move the enemy smoothly along waypoints using Bezier curves.
    /// </summary>
    /// <returns>An enumerator for coroutine execution.</returns>
    private IEnumerator EnemyMoveByRoute()
    {
        while (true)
        {
            // Define the start (p0), end (p2), and control (p1) points for the Bezier curve.
            Vector3 p0 = enemySpawner.waypointsList[waypointsTracker].position; // Current waypoint.
            Vector3 p2 = enemySpawner.waypointsList[waypointsTracker + 1].position; // Next waypoint.
            Vector3 p1 = bezierCurve.CalculateSmoothControlPoint(waypointsTracker == 0 ? p0 : enemySpawner.waypointsList[waypointsTracker - 1].position, p0, p2); // Smooth control point.

            tParam = 0f; // Reset interpolation parameter for the current segment.

            // Calculate the length of the curve and determine traversal time.
            float pathLength = CalculatePathLength(p0, p1, p2);
            float totalTime = pathLength / enemySpd;

            float elapsedTime = 0f;

            // Interpolate along the curve until the enemy reaches the next waypoint.
            while (tParam < 1f)
            {
                tParam = elapsedTime / totalTime; // Update interpolation parameter based on elapsed time.
                tParam = Mathf.Clamp(tParam, 0f, 1f); // Clamp tParam to ensure it stays between 0 and 1.

                // Apply easing based on the enemy's name.
                if (transform.name.Contains("EaseInMinion"))
                {
                    tParam = bezierCurve.EaseIn(tParam);
                }
                else if (transform.name.Contains("EaseOutMinion"))
                {
                    tParam = bezierCurve.EaseOut(tParam);
                }
                else if (transform.name.Contains("EaseInOutMinion"))
                {
                    tParam = bezierCurve.EaseInOut(tParam);
                }

                // Update the enemy's position based on the Bezier curve.
                transform.position = bezierCurve.CalculateBezierPoint(tParam, p0, p1, p2);

                elapsedTime += Time.deltaTime; // Increment elapsed time.
                yield return null;
            }

            waypointsTracker++; // Advance to the next waypoint.

            // Check if the enemy has reached the final waypoint.
            if (Vector3.Distance(transform.position, enemySpawner.waypointsList[enemySpawner.waypointsList.Count - 1].position) < 0.5f)
            {
                EnemyReachedBase();
                break;
            }
        }
    }

    /// <summary>
    /// Estimates the length of a Bezier curve by summing distances between sampled points.
    /// </summary>
    /// <param name="p0">The start point of the curve.</param>
    /// <param name="p1">The control point of the curve.</param>
    /// <param name="p2">The end point of the curve.</param>
    /// <returns>The estimated length of the Bezier curve.</returns>
    private float CalculatePathLength(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float pathLength = 0f;
        Vector3 previousPoint = p0; // Start with the initial point.

        // Sample points along the curve to estimate its length.
        for (int i = 1; i <= enemyPathwayRenderer.segmentResolution; i++)
        {
            float t = i / (float)enemyPathwayRenderer.segmentResolution; // Interpolation parameter for the segment.
            Vector3 currentPoint = bezierCurve.CalculateBezierPoint(t, p0, p1, p2); // Point on the curve at t.

            pathLength += Vector3.Distance(previousPoint, currentPoint); // Add the segment's length to the total.
            previousPoint = currentPoint; // Update the previous point.
        }

        return pathLength;
    }

    /// <summary>
    /// Function to be performed after the enemy reached the player's base
    /// </summary>
    public void EnemyReachedBase()
    {
        // Notify the spawner that the enemy is destroyed and remove it from the scene.
        EnemySpawner.onEnemyDestroyed.Invoke();
        Destroy(gameObject);
        textPulse.playerHealth -= 10;
    }
}
