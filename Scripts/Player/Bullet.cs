using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject HitSparksPrefab;
    [SerializeField] private GameObject HitBloodyPrefab;

    private void Start()
    {
        StartCoroutine(KYS(2));
    }

    private IEnumerator KYS(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Rat(Clone)" || other.gameObject.name == "RatKing(Clone)")
        {
            Instantiate(HitBloodyPrefab, transform.position, Quaternion.identity);
            StartCoroutine(KYS(0));
        }
        else
        {
            Instantiate(HitSparksPrefab, transform.position, Quaternion.identity);
            StartCoroutine(KYS(0));
        }
    }
}
