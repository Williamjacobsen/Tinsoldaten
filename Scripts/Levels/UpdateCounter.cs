using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateCounter : MonoBehaviour
{
    private TextMeshPro _text;
    
    private void Start()
    {
        _text = GetComponent<TextMeshPro>();
    }

    private void FixedUpdate()
    {
        _text.text = $"{PlayerStats.keys}/3";
    }
}
