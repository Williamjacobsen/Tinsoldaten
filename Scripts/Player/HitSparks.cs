using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSparks : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(KYS());
    }

    private IEnumerator KYS()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
