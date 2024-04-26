using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        StartCoroutine(_FadeOut());
    }

    private void OnEnable()
    {
        StartCoroutine(_FadeOut());
    }

    private IEnumerator _FadeOut()
    {
        for (float i = 1; i > -0.1f; i -= 0.025f)
        {
            yield return new WaitForSeconds(0.025f);
            _renderer.color = new Color(0, 0, 0, i);
        }
    }
}
