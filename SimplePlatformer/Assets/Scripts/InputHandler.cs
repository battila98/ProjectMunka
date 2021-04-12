using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class InputHandler : MonoBehaviour
{


    [SerializeField] float shootTimer; // utolsó lövés óta eltelt idő
    [SerializeField] float timeBetweenShots = 1f;

    Movement movement;

    void Start()
    {
        movement = GetComponent<Movement>();
    }

    void Update()
    {
        shootTimer += Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && shootTimer > timeBetweenShots) //ToDo futás közben lövés legyen vagy ne???
        {
            movement.FireBow();
            shootTimer = 0f;
        }
    }

}

