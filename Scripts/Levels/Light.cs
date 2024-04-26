using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Light : MonoBehaviour
{
    //private void Start()
    //{
    //    GetComponent<Light2D>().intensity = GetComponent<Light2D>().intensity * Settings.LightIntensity;
    //}

    private float initialLightIntensity;

    private void Awake()
    {
        initialLightIntensity = GetComponent<Light2D>().intensity;
    }

    float prevIntensity = 0;
    private void FixedUpdate()
    {
        if (Settings.LightIntensity != prevIntensity)
        {
            prevIntensity = Settings.LightIntensity;
            GetComponent<Light2D>().intensity = initialLightIntensity * Settings.LightIntensity;
        }
    }
}
