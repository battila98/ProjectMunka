using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncy : MonoBehaviour
{
    Player player;
    Animator myAnimator;
    
    Vector2 pushUp = new Vector2(0f, 25f);

    private void Start()
    {
        player = FindObjectOfType<Player>();
        myAnimator = GetComponent<Animator>();
    }
    public IEnumerator PushUp()
    {
        myAnimator.SetBool("Touch", true);
        player.GetComponent<Rigidbody2D>().velocity = pushUp;
        yield return new WaitForSeconds(0.5f);
        myAnimator.SetBool("Touch", false);
    }

}
