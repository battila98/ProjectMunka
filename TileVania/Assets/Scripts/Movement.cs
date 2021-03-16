using UnityEngine;

public class Movement : MonoBehaviour
{    
    [SerializeField] float runSpeed = 5.5f;
    [SerializeField] float jumpVelocity = 17.5f;
    [SerializeField] float climbSpeed = 5f;

    [SerializeField] float fallMultiplier = 1.2f;
    [SerializeField] float lowJumpMultiplier = 1f;
    
   
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    BoxCollider2D myFeet;
    float gravityScaleAtStart;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myFeet = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;
    }

    public void Shoot() //BUG -> Nem jön ki az animációból
    {
        bool playerShooting = true;
        if (Input.GetButton("Fire1"))
        {
            myAnimator.SetBool("Shooting", playerShooting);
            playerShooting = false;
            return;
        }           
    }

    public void Run()
    {
        float controlThrow = Input.GetAxis("Horizontal"); //-1 to +1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y); // a mostani sebességed y-on, az lesz a sebességed, azaz 0
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
        if (myRigidBody.velocity.y > climbSpeed) //Tud ugrani, nem akad meg
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
            print("Nyomottan ugrik");
        }
        else if (myRigidBody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            myRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime; //Ha elengeded hamar az ugrást, kicsit ugrik
            print("Simán  ugrik");
        }
    }

    public void JumpV2() //Futás és jump bug (fall változók)
    {
        IsFalling();
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")) && !myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            return;
        }
        if (Input.GetButtonDown("Jump"))
        {
            myRigidBody.velocity = Vector2.up * jumpVelocity;
        }     
    }

}
