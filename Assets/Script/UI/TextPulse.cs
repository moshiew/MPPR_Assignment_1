using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextPulse : MonoBehaviour
{
    public int playerHealth = 100;
    public TextMeshProUGUI healthtext;
    public TextMeshProUGUI waveText;
    public float pulseSpeed = 2f;
    public float minScale = 0.85f;
    public float maxScale = 1.0f;

    private float t;

    void Update()
    {
        //Calculates current interpolation value using PingPong which oscillates between 0 and 1
        t = Mathf.PingPong(Time.time * pulseSpeed, 1);
        
        //Calculate the scale based on interpolation value
        float scale = minScale + (maxScale - minScale) * t;

        //Apply pulse effect to healthText
        if (healthtext != null)
        {
            //Update scale of RectTransform
            healthtext.rectTransform.localScale = new Vector3(scale, scale, scale);

            healthtext.text = playerHealth.ToString();
        }

        //Apply pulse effect to healthText
        if (waveText != null)
        {
            //Update scale of RectTransform
            waveText.rectTransform.localScale = new Vector3(scale, scale, scale);

            //waveText.text = waveNumber.ToString();
        }
    }
}
