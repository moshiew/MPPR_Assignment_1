using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    private Slider slider;
    private Image fill;
    [SerializeField] private Gradient gradient;

    private Camera cam;
    private Transform enemy; // Reference to the enemy this health bar is attached to.
    [SerializeField] private Vector3 offset; // Offset to position the health bar above the enemy.

    private void Start()
    {
        slider = GetComponent<Slider>();
        fill = GetComponentInChildren<Image>();
        cam = Camera.main;
        enemy = transform.root;
    }

    private void Update()
    {
        // Ensure the health bar always faces the camera and stays above the enemy.
        transform.rotation = cam.transform.rotation; // Align health bar's rotation with the camera.
        transform.position = enemy.position + offset; // Position health bar above the enemy with the specified offset.
    }

    /// <summary>
    /// Initializes the enemy's maximum health and sets the health bar accordingly.
    /// Also updates the fill color to represent full health.
    /// </summary>
    /// <param name="health">The maximum health of the enemy.</param>
    public void setMaxHealth(int health)
    {
        slider.value = health; // Set the current health value.
        slider.maxValue = health; // Set the maximum health value.

        // Set the fill color to the gradient's full health color (1f).
        fill.color = gradient.Evaluate(1f);
    }

    /// <summary>
    /// Updates the enemy's current health and adjusts the health bar fill and color.
    /// </summary>
    /// <param name="health">The current health of the enemy.</param>
    public void setHealth(int health)
    {
        slider.value = health; // Update the slider's value to reflect the current health.

        // Update the fill color based on the normalized health value (0 to 1).
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
