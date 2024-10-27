using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{ 
    [SerializeField] public float speed = 1;
    [SerializeField] private int downwardShift = 1;


    void Update()
    {
        // Have enemy move towards the blocker
        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
    }

    // Once they reach a blocker make them shift one unit down
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Multiply by downwardShift to continue and proceed to the opposite end of screen
        if(other.gameObject.tag == "Blocker")
        {
            transform.position += new Vector3(0, -downwardShift, 0);
            speed *= -1;
        }
    }
}
