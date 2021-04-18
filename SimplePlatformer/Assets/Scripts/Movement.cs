using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{    
    [SerializeField] float runSpeed = 7f;
    [SerializeField] float jumpVelocity = 17.5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float fallMultiplier = 2.26f;
    [SerializeField] float lowJumpMultiplier = 1.8f;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] float arrowSpeedY = 3.2f;
    [SerializeField] float arrowSpeedX = 43f;
    
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    BoxCollider2D myFeet;
    UnityEvent OnArrowShot;
    UnityEvent OnJump;

    float gravityScaleAtStart;
    //bool isFacingRight;
    public float horizontalInput; //-1 to +1
    float faceDirection = 1f;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myFeet = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;
        //OnArrowShot = FindObjectOfType<StatsHandler>().OnArrowShot;
        //OnJump = FindObjectOfType<StatsHandler>().OnJump;
        OnArrowShot = StatsHandler.Instance.OnArrowShot;
        OnJump = StatsHandler.Instance.OnJump;

    }
    void Update()
    {
        if (horizontalInput > 0)
        {
            faceDirection = 1f;
        }
        else if (horizontalInput < 0)
        {
            faceDirection = -1f;
        }
    }

    public void FlipSprite()
    {
        /*bool playerHasHorizotalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon; //ha a movespeed > 0 akkor true
        if (playerHasHorizotalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), transform.localScale.y); //Megfordítja az x értékét, y = változatlan
        }*/
        transform.localScale = new Vector2(faceDirection, transform.localScale.y); //Megfordítja az x értékét, y = változatlan
    }

    public void FireBow()
    {
        if (Input.GetButtonDown("Fire1"))
        {  
            StartCoroutine(ShootArrow());           
            OnArrowShot.Invoke();
        }
    }

    IEnumerator ShootArrow()
    {
        myAnimator.SetBool("Shooting", true);
        yield return new WaitForSecondsRealtime(0.25f); //magic number animációhoz igazítás
        Vector2 startingArrowPosition = new Vector2(transform.position.x + faceDirection * 0.75f, transform.position.y); 
        GameObject arrow = Instantiate(arrowPrefab, startingArrowPosition, Quaternion.identity)
            as GameObject;
        arrow.name = "Arrow";
        arrow.transform.localScale = new Vector2(faceDirection, transform.localScale.y);
        arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(arrowSpeedX * faceDirection, arrowSpeedY); //melyik irányba lőjön
        yield return new WaitForSecondsRealtime(0.25f);
        myAnimator.SetBool("Shooting", false);
    }

    public void Run()
    {            
        /*if (horizontalInput > 0)
        {
            isFacingRight = true;
        }
        else if (horizontalInput < 0)
        {
            isFacingRight = false;
        }*/

        Vector2 playerVelocity = new Vector2(horizontalInput * runSpeed, myRigidBody.velocity.y); // a mostani sebességed y-on lesz a sebességed, azaz 0
        myRigidBody.velocity = playerVelocity;

        //print(playerVelocity);
        bool playerRunning = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerRunning);
    }

    public void ClimbLadder()
    {
        myRigidBody.gravityScale = gravityScaleAtStart;
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing"))) //csak a létrán "másszon" (else gravity = 0 és elrepül)
        {
            myAnimator.SetBool("Climbing", false);
            return;
        }
        if (myRigidBody.velocity.y > climbSpeed) //Tud ugrani, nem akad meg a létrában
        {
            return;
        }
        float controlThrow = Input.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
        myRigidBody.velocity = climbVelocity;
        myRigidBody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon; // mozog-e?
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);
        JumpV2();
    }

    void IsFalling()
    {
        if (myRigidBody.velocity.y < 0)
        {
            myRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime; //Ha nyomod, nagyot ugrik
            //print("Nyomottan ugrik");
        }
        else if (myRigidBody.velocity.y > 0.08 && !Input.GetButton("Jump"))
        {
            myRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime; //Ha elengeded hamar az ugrást, kicsit ugrik
            //print("Simán ugrik");
        }
    }

    public void JumpV2()
    {
        IsFalling();
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")) && 
            !myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing")) &&
            !myFeet.IsTouchingLayers(LayerMask.GetMask("Projectile")))
        {
            return;
        }
        if (Input.GetButtonDown("Jump"))
        {
            myRigidBody.velocity = Vector2.up * jumpVelocity;
            OnJump.Invoke();
        }       
    }
    
    public void Catapulted()
    {
        if (myFeet.IsTouchingLayers(LayerMask.GetMask("Bouncy")))
        {
            StartCoroutine(FindObjectOfType<Bouncy>().PushUp());
        }
    }
}
