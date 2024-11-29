using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextPulse : MonoBehaviour
{
    public TextMeshProUGUI[] text;
    public RawImage[] images;
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

        //Apply scale to each text in array
        foreach (var t in text)
        {
            if(t != null)
            {
                //Update scale of RectTransform
                t.rectTransform.localScale = new Vector3(scale, scale, scale);
            }
        }

        //Apply scale to each image in array
        foreach (var img in images)
        {
            if (img != null)
            {
                //Update scale of RectTransform
                img.rectTransform.localScale = new Vector3(scale, scale, scale);
            }
        }
    }
}
