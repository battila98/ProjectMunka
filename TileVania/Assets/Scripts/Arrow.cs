using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy component)) //ha enemy amibe ütközök, akkor true
        {
            Destroy(gameObject);
        }
    }
}
