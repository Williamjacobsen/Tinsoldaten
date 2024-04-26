using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSettings : MonoBehaviour
{
    [SerializeField] private GameObject BrightToggle;
    [SerializeField] private GameObject IntensitySlider;

    private void Start()
    {
        if (Settings.LightMode == 2)
        {
            BrightToggle.GetComponent<Toggle>().isOn = true;
        }

        IntensitySlider.GetComponent<Slider>().value = Settings.LightIntensity / 4;
    }

    private void OnEnable()
    {        
        if (Settings.LightMode == 2)
        {
            BrightToggle.GetComponent<Toggle>().isOn = true;
        }

        IntensitySlider.GetComponent<Slider>().value = Settings.LightIntensity / 4;
    }

    private void OnDisable()
    {
        if (BrightToggle.GetComponent<Toggle>().isOn)
        {
            Settings.LightMode = 2;
        }

        Settings.LightIntensity = IntensitySlider.GetComponent<Slider>().value * 4;
    }
}
