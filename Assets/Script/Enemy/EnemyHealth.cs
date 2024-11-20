using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public Slider slider;
    public int health;

    /// <summary>
    /// Initializing the enemy health
    /// </summary>
    /// <param name="health">Health of the enemy</param>
    public void setMaxHealth(int health)
    {
        slider.value = health;
        slider.maxValue = health;
    }
}