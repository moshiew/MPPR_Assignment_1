using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletMovementScript : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float duration = 2.0f;
    public enum EasingType { Linear,EaseIn,EaseOut,EaseInOut }
    public EasingType easeType = EasingType.Linear; 

    private float elaspedTime = 0.0f;
    private Vector3 positionA;
    private Vector3 positionB;

    public GameObject currentenemy;
    public TowerShootScript towerShootScript;
    public int dmgPoints;

    // Start is called before the first frame update

    
    float EaseInCubic(float x)
    { return x * x *x; }

    float EaseOutCubic(float x)
    { return 1 - Mathf.Pow(1-x, 3); }

    float EaseInOutCubic(float x)
    { return x < 0.5f ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2; }

    void Start()
    {
        positionA = transform.position; // Starting position of the bullet

        if (currentenemy != null)
        {
            positionB = currentenemy.transform.position; // Target position
            
        }
        else
        {
            Destroy(gameObject); // No target enemy; destroy the bullet
        }

    }

    // Update is called once per frame
    void Update()
    {
        //If there is a position set for the bullet then the bullet will shoot based on the easetype of the bullet
        if (elaspedTime < duration && positionB != null)
        {
            elaspedTime += Time.deltaTime;
            float t = elaspedTime / duration;
            t = Mathf.Clamp01(t);

            switch (easeType)
            {
                case EasingType.Linear:
                    break;
                case EasingType.EaseIn:
                    t = EaseInCubic(t);
                    break;
                case EasingType.EaseOut:
                    t = EaseOutCubic(t);
                    break;
                case EasingType.EaseInOut:
                    t=EaseInOutCubic(t);
                    break;
            }
            //This is the interpolation from Position A to Position B
            Vector3 interpolatedPosition = (1 - t) * positionA + t * positionB;

            transform.position = interpolatedPosition;

            Vector3 direction = positionB - positionA;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(90,targetRotation.eulerAngles.y,0);
            

        }
        else
        {
            //When the the bullet reach the destination and the enemy exists then the enemy will take damage and destroy the bullet afterwards
            if (currentenemy != null)
            {
                TempEnemy tempEnemy = currentenemy.GetComponent<TempEnemy>();
                if (tempEnemy != null)
                {
                    tempEnemy.TakeDamage(dmgPoints); // Deal damage to the correct enemy
                   
                }
            }

            Destroy(gameObject); // Destroy the bullet
        }

    }

   
}
