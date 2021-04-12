using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Config  
    [SerializeField] float waitForRespawn = 0.8f;
    [SerializeField] Vector2 deathKick = new Vector2(0f, 15f);

    [SerializeField] AudioClip jumpSFX;

    // State
    bool isAlive = true;

    // Cashed comp. refs.
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    float flashSpeed = 0.3f;
    SpriteRenderer mySprite;
    Movement movement;
    // Message then methods

    void Start()
    {
        movement = GetComponent<Movement>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myRigidBody = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isAlive) { return; }
        movement.Run();
        movement.FlipSprite();
        movement.horizontalInput = Input.GetAxis("Horizontal");
        movement.JumpV2();
        movement.ClimbLadder();
        
        /*if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            mySprite.color = Color.Lerp(Color.white, Color.gray, flashSpeed);
            //StartCoroutine(Die());
        }*/
        //movement.FireBow();
    }

    /*
    private void Jump()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(1f, jumpVelocity);
            myRigidBody.velocity += jumpVelocityToAdd; // BUG! néha double jump
            print(myRigidBody.velocity); //debug
            AudioSource.PlayClipAtPoint(jumpSFX, transform.position);
        }    
    }
    */
    public IEnumerator Die()
    {
        myAnimator.SetTrigger("Dying");
        GetComponent<Rigidbody2D>().velocity = deathKick;
        isAlive = false;
        yield return new WaitForSecondsRealtime(waitForRespawn);
        //FindObjectOfType<GameSession>().ProcessPlayerDamage();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        /*if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            FindObjectOfType<GameSession>().ProcessPlayerDamage(10);
            GetComponent<Rigidbody2D>().velocity = deathKick;
        }*/
        if (collision.gameObject.layer == 13 || collision.gameObject.layer == 12) 
        {
            FindObjectOfType<GameSession>().ProcessPlayerDamage(10);
            GetComponent<Rigidbody2D>().velocity = deathKick;
        }
    }

    /*private void FlipSprite()
    {
        bool playerHasHorizotalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon; //ha a movespeed > 0 akkor true
        if (playerHasHorizotalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), transform.localScale.y); //Megfordítja az x értékét, y = változatlan
        }
    }*/
}
