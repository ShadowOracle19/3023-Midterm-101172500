using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterController : MonoBehaviour
{
    List<int> wew;

    [SerializeField]
    float speed = 5;
    
    [SerializeField]
    Rigidbody2D rigidBody;
    

    private ItemSlot DraggedSlot;


    // Update is called once per frame
    void Update()
    {
        Vector2 movementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        movementVector *= speed;
        rigidBody.velocity = movementVector;
    }
}