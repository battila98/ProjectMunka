﻿using UnityEngine;

public class Enemy : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.StartsWith("Arrow"))
        {
            Destroy(gameObject);
        }
    }


}
