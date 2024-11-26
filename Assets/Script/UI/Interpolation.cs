using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpolation : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float duration = 2.0f;

    private float elapsedTime = 0.0f;
    private Vector3 positionA;
    private Vector3 positionB;

    // Start is called before the first frame update
    void Start()
    {
        positionA = pointA.position;
        positionB = pointB.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            t = Mathf.Clamp01(t);

            // Ease In-Out Cubic
            if (t < 0.5f)
            {
                t = 4 * t * t * t; // Ease In
            }
            else
            {
                t = 1 - Mathf.Pow(-2 * t + 2, 3) / 2; // Ease Out
            }

            Vector3 interpolatedPosition = (1 - t) * positionA + t * positionB;

            transform.position = interpolatedPosition;
        }
        else
        {
            transform.position = positionB;
        }
    }
}
