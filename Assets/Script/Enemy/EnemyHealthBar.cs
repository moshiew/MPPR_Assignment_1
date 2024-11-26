using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image fill;
    [SerializeField] private Gradient gradient;

    [SerializeField] private Camera cam;
    [SerializeField] private Transform enemy;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        // Making sure that the healthbar will always point at camera and will not spin around according the enemy rotation
        transform.rotation = cam.transform.rotation;
        transform.position = enemy.position + offset;
    }

    /// <summary>
    /// Initializing the enemy health and update the gradient colour
    /// </summary>
    /// <param name="health">Health of the enemy</param>
    public void setMaxHealth(int health)
    {
        slider.value = health;
        slider.maxValue = health;

        fill.color = gradient.Evaluate(1f);
    }

    /// <summary>
    /// Updating the enemy health and also update the gradient colour
    /// </summary>
    /// <param name="health">Health of the enemy</param>
    public void setHealth (int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
