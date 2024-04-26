using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BrightMode : MonoBehaviour
{
    private void Start()
    {
        if (Settings.LightMode != 2)
        {
            gameObject.SetActive(false);
        }
    }
}
