using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    
    Rigidbody2D myRigidBody;
    BoxCollider2D myFeet;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myFeet = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (IsFacingRight())
        {
            myRigidBody.velocity = new Vector2(moveSpeed, 0f); //jobbra megy
        }
        else
        {
            myRigidBody.velocity = new Vector2(-moveSpeed, 0f); //balra megy
        }
    }

    bool IsFacingRight()
    {
        return transform.localScale.x > 0; //ha + akkor jobbra néz (pozitív a localsacle transform, ami default setting btw)
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f); // minusz hogy megfoduljon mindig a sprite
    }


}
