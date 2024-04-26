using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningKey : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(SpinningKeyAnim());
    }

    private IEnumerator SpinningKeyAnim()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            transform.Rotate(0, 0, 1.5f); 
        }
    }
}
