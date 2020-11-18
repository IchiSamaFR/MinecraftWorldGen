using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : EntityMovement
{
    KeyCollection key;


    void Start()
    {
        key = KeyCollection.instance;
        _init_();
    }

    void Update()
    {
        Inputs();
        Movement();
    }

    void Inputs()
    {
        if (Input.GetKey(key.forward))
        {
            forward = true;
        }
        else
        {
            forward = false;
        }
        if (Input.GetKey(key.backward))
        {
            backward = true;
        }
        else
        {
            backward = false;
        }
        if (Input.GetKey(key.left))
        {
            left = true;
        }
        else
        {
            left = false;
        }
        if (Input.GetKey(key.right))
        {
            right = true;
        }
        else
        {
            right = false;
        }
        if (Input.GetKeyDown(key.jump))
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
    }
}
