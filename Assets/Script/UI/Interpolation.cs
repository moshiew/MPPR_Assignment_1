using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpolation : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float duration = 2.0f;
    public BezierCurve bezierCurve;

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
        //Check if interpolation is still within the duration
        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            //Clamp t to stay between 0 and 1
            t = Mathf.Clamp01(t);

            // Ease In-Out Cubic interpolation
            if (t < 0.5f)
            {
               // Ease In
               bezierCurve.EaseIn(t);
            }
            else
            {
                // Ease Out
                bezierCurve.EaseOut(t);
            }

            //Calculate interpolated position using linear interpolation
            Vector3 interpolatedPosition = (1 - t) * positionA + t * positionB;

            //Update position of GameObject
            transform.position = interpolatedPosition;
        }
        else
        {
            transform.position = positionB;
        }
    }
}
