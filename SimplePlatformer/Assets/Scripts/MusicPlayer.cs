using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    void Awake()
    {
        SetUpSingelton();
    }
    private void SetUpSingelton()
    {
        if (FindObjectsOfType(GetType()).Length > 1) //Get MusicPlayer, azaz a jelenlegi Class
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
