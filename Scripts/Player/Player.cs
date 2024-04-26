using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction moveAction;

    [SerializeField] private GameObject WaterSplashSound;

    private AudioSource audioSource;

    // normal 5
    // speedy testing 25
    private float moveSpeed = 5;

    private float moveHorizontal;
    private float moveVertical;

    private void Awake() 
    {
        audioSource = GetComponent<AudioSource>();

        CameraFollowPlayer();
        PlayerLookAtMouse();

        StartCoroutine(Health());
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
    }

    private void Update() 
    {
        // A = -1
        // D = +1
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        
        // W = +1
        // S = -1
        moveVertical = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate() 
    {
        MovePlayer();
        PlayerLookAtMouse();
    }

    private void PlayerLookAtMouse() 
    {
        // get position of mouse
        Vector2 pointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // get direction of player to mouse
        Vector2 direction = (pointerPosition - (Vector2)transform.position).normalized;
    
        // set direction of player
        transform.up = direction;
    }

    int wasInWater = 0;
    private void MovePlayer()
    {
        //// if W, A, S or D is pressed
        //if (moveHorizontal != 0 || moveVertical != 0)
        //{
        //    // move player
        //    transform.position += moveSpeed * Time.deltaTime * new Vector3(moveHorizontal, moveVertical);
        //    CameraFollowPlayer();
        //}

        Vector2 moveDirection = moveAction.ReadValue<Vector2>();

        /*if (moveDirection != Vector2.zero)
        {
            if (isInWater != 0)
            {
                audioSource.Stop();
                audioSource.PlayOneShot(WaterSplashSound);
            }
            else
            {
                audioSource.PlayOneShot(WalkingSound);
            }
        }
        else
        {
            audioSource.Stop();
        }*/

        if (moveDirection != Vector2.zero && isInWater == 0)
        {   
            if (!audioSource.isPlaying)
            {
                audioSource.Play();   
            }
        }
        else
        {
            audioSource.Stop();
        }

        if (wasInWater == 0 && isInWater == 1)
        {
            Instantiate(WaterSplashSound, transform);
        }
        wasInWater = isInWater;

        transform.position += moveSpeed * Time.deltaTime * new Vector3(moveDirection.x, moveDirection.y);
        CameraFollowPlayer();
    }

    private void CameraFollowPlayer()
    {
        // camera follow player
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
    }

    private IEnumerator KnockBack(Vector2 hitFromPos)
    {
        Vector2 vector = new Vector2(transform.position.x, transform.position.y) - hitFromPos;
        float vectorLength = (float)Math.Sqrt(vector.x * vector.x + vector.y * vector.y);
        Vector2 direction = new(vector.x/vectorLength, vector.y/vectorLength);

        for (float i = 6; i >= 0; i -= 0.01f)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(i * direction.x, i * direction.y);
            CameraFollowPlayer();
            yield return new WaitForSeconds(0.001f);
        }
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }

    [SerializeField] GameObject HurtOverlay;
    private void TakeDamage(float damage)
    {
        PlayerStats.health -= damage;
    }

    private IEnumerator Health()
    {
        SpriteRenderer HurtOverlayRenderer = HurtOverlay.GetComponent<SpriteRenderer>();
        while (true)
        {
            yield return new WaitForSeconds(0.025f);
            HurtOverlayRenderer.color = new Color(221, 55, 55, 1 - PlayerStats.health);

            if (PlayerStats.health <= 0)
            {
                Debug.Log("Dead");
                SceneManager.LoadScene("Main Menu");
            }
            else if (PlayerStats.health < 1)
            {
                PlayerStats.health += 0.00075f;
            }
            else 
            {
                PlayerStats.health = 1;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Rat(Clone)")
        {
            StartCoroutine(KnockBack(other.gameObject.transform.position));
            TakeDamage(0.25f);
        }
        else if (other.gameObject.name == "RatKing(Clone)")
        {
            StartCoroutine(KnockBack(other.gameObject.transform.position));
            TakeDamage(0.25f);
        }
    }

    // sometimes player enters the next water right before exiting the previous water
    private int isInWater = 0;
    
    [SerializeField] private GameObject KeySewergateMenu;

    [SerializeField] private GameObject WaterRipple;

    private IEnumerator PlayWaterRipple()
    {
        GameObject WaterRippleTemp = Instantiate(WaterRipple, transform);
        yield return new WaitForSeconds(0.05f);
        WaterRippleTemp.transform.parent = null;
        yield return new WaitForSeconds(1f);
        Destroy(WaterRippleTemp);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.name == "Key(Clone)")
        {
            PlayerStats.keys++;
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("block"))
        {
            moveSpeed = 2;
            
            isInWater++;

            if (isInWater == 1)
            {
                StartCoroutine(PlayWaterRipple());   
            }
        }

        if (other.gameObject.name == "Sewergate(Clone)")
        {
            // show popup (how many keys & "press E")

            GameObject SewergateMenuTemp = Instantiate(KeySewergateMenu, other.transform);
            SewergateMenuTemp.transform.localScale = new Vector2(3.3f, 3.3f);
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("block"))
        {
            isInWater--;

            if (isInWater == 0)
            {
                moveSpeed = 5;
            }
        }

        if (other.gameObject.name == "Sewergate(Clone)")
        {
            Destroy(other.transform.GetChild(1).gameObject);
        }
    }
}
