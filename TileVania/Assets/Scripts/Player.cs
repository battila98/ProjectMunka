using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] float runSpeed = 6.5f;
    [SerializeField] float jumpVelocity = 17f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float waitForRespawn = 0.8f;
    [SerializeField] Vector2 deathKick = new Vector2(0f, 15f);

    [SerializeField] float fallMultiplier = 5f;
    [SerializeField] float lowJumpMultiplier = 2f;


    [SerializeField] AudioClip jumpSFX;

    // State
    bool isAlive = true;

    // Cashed comp. refs.
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeet;
    float gravityScaleAtStart;
    float flashSpeed = 0.3f;
    SpriteRenderer mySprite;
    // Message then methods

    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;
    }


    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        JumpV2();
        ClimbLadder();
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            mySprite.color = Color.Lerp(Color.white, Color.gray, flashSpeed);
            StartCoroutine(Die());
        }      
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); //-1 to +1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y); // ami a mostani sebességed y-on, az lesz a sebességed, azaz 0
        myRigidBody.velocity = playerVelocity;

        //print(playerVelocity);
        bool playerRunning = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerRunning);
    }

    private void ClimbLadder()
    {
        myRigidBody.gravityScale = gravityScaleAtStart;  
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing"))) //csak a létrán "máasszon" (else gravity = 0 és elrepül)
        {
            myAnimator.SetBool("Climbing", false);
            return;
        }
        if (myRigidBody.velocity.y > climbSpeed) //Tud ugrani, nem akad meg
        {
            return;
        }
        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
        myRigidBody.velocity = climbVelocity;
        myRigidBody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon; // mozog-e?
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);
        JumpV2();
    }

    private void JumpV2()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")) && !myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            return;
        }
        if (Input.GetButtonDown("Jump"))
        {
            myRigidBody.velocity = Vector2.up * jumpVelocity;
        }
        if (myRigidBody.velocity.y < 0)
        {
            myRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime; //Ha nyomod, nagyot ugrik
        }
        else if (myRigidBody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            myRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime; //Ha elengeded hamar az ugrást, kicsit ugrik
        }
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
    IEnumerator Die()
    {
            myAnimator.SetTrigger("Dying");
            GetComponent<Rigidbody2D>().velocity = deathKick;
            isAlive = false;
            yield return new WaitForSecondsRealtime(waitForRespawn);
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }


    private void FlipSprite()
    {
        bool playerHasHorizotalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon; //ha a movespeed > 0 akkor true
        if (playerHasHorizotalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), transform.localScale.y); //Megfordítja az x értékét, y = változatlan
        }
    }

    void DamageTaken() //Does nothing
    {
       
    }
    public IEnumerator Halt() // does nothing
    {
        yield return new WaitForSecondsRealtime(3);
    }
}
