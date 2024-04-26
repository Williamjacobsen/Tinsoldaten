using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private bool coolDown = false;

    [SerializeField] private GameObject BulletPrefab;

    private readonly float bulletSpeed = 2500;

    private void Fire()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        
        audioSource.Play();

        GameObject bullet = Instantiate(BulletPrefab, transform.position, transform.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed);
        bullet.transform.rotation = transform.parent.transform.rotation.normalized;
    }

    private void Update() 
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.M)) && !coolDown)
        {    
            Fire();
            StartCoroutine(CoolDown());
        }
    }

    private IEnumerator CoolDown()
    {
        coolDown = true;
        yield return new WaitForSeconds(0.5f);
        coolDown = false;
    }
}
