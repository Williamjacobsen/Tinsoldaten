using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject Player;

    [SerializeField] private int health = 5;
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float knockbackFactor = 2.5f;

    private void Awake()
    {
        Player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, Player.transform.position) < 15f)
        {
            transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate() 
    {
        EnemyLookAtPlayer();    
    }

    private void EnemyLookAtPlayer() 
    {
        Vector2 playerPosition = Player.transform.position;
        Vector2 direction = (playerPosition - (Vector2)transform.position).normalized;
        transform.right = -direction;
    }

    private IEnumerator KnockBack(Vector2 hitFromPos)
    {
        Vector2 vector = new Vector2(transform.position.x, transform.position.y) - hitFromPos;
        float vectorLength = (float)Math.Sqrt(vector.x * vector.x + vector.y * vector.y);
        Vector2 direction = new(vector.x/vectorLength, vector.y/vectorLength);

        for (float i = knockbackFactor; i >= 0; i -= 0.01f)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(i * direction.x, i * direction.y);
            yield return new WaitForSeconds(0.001f);
        }
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }

    [SerializeField] private GameObject BloodSplatter;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Bullet(Clone)" || other.gameObject.name == "Player")
        {
            StartCoroutine(KnockBack(other.gameObject.transform.position));
        }

        if (other.gameObject.name == "Bullet(Clone)")
        {
            health -= 1;

            if (health <= 0)
            {
                GameObject BloodSplatterTemp = Instantiate(BloodSplatter, transform);
                BloodSplatterTemp.transform.parent = null;
                if (name == "RatKing(Clone)")
                {
                    BloodSplatterTemp.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);    
                }
                else 
                {
                    BloodSplatterTemp.transform.localScale = new Vector3(1, 1, 1);
                }
                Destroy(gameObject);
            }
        }
    }
}
