using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    private SpriteRenderer _renderer;

    private void OnEnable()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.color = new Color(0, 0, 0, 0);
        StartCoroutine(_FadeIn());
    }

    private IEnumerator _FadeIn()
    {
        for (float i = 0; i < 1.1f; i += 0.025f)
        {
            yield return new WaitForSeconds(0.025f);
            _renderer.color = new Color(0, 0, 0, i);
        }
    }
}
