using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    [System.NonSerialized]
    public bool forward;
    [System.NonSerialized]
    public bool backward;
    [System.NonSerialized]
    public bool left;
    [System.NonSerialized]
    public bool right;
    [System.NonSerialized]
    public bool jump;
    [System.NonSerialized]
    public Rigidbody rb;

    public float maxSpeed;
    public float speed;

    public float jumpForce;
    public bool canJump;

    Animator anim;
    

    public void _init_()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        speed = maxSpeed;
    }

    public void CheckAnim()
    {
        if (!anim)
        {
            return;
        }

        if ((forward && backward || (!forward && !backward)))
        {
            anim.SetBool("forward", false);
            anim.SetBool("backward", false);
        }
        else if (forward)
        {
            anim.SetBool("forward", true);
            anim.SetBool("backward", false);
        }
        else if (backward)
        {
            anim.SetBool("forward", false);
            anim.SetBool("backward", true);
        }

        if ((left && right) || (!left && !right))
        {
            anim.SetBool("left", false);
            anim.SetBool("right", false);
        }
        else if (left)
        {
            anim.SetBool("left", true);
            anim.SetBool("right", false);
        }
        else if (right)
        {
            anim.SetBool("left", false);
            anim.SetBool("right", true);
        }
        if (!forward && !backward && !left && !right)
        {
            anim.SetBool("idle", true);
        }
        else
        {
            anim.SetBool("idle", false);
        }
    }

    public virtual void Movement(float _timeDelt)
    {
        float _speed = Speed() * _timeDelt;
        if (forward)
        {
            transform.position += transform.forward * _speed;
        }
        if (backward)
        {
            transform.position -= transform.forward * _speed;
        }
        if (left)
        {
            transform.position -= transform.right * _speed;
        }
        if (right)
        {
            transform.position += transform.right * _speed;
        }
        if (jump && canJump)
        {
            rb.AddForce(Vector3.up * jumpForce);
            canJump = false;
        }
    }

    public float Speed()
    {
        float _speed = maxSpeed;
        if (forward)
        {
            if (left)
            {
                _speed *= 0.8f;
            }
            else if (right)
            {
                _speed *= 0.8f;
            }
        }
        else if (backward)
        {

            if (left)
            {
                _speed *= 0.8f;
            }
            else if (right)
            {
                _speed *= 0.8f;
            }
        }

        speed = _speed;
        return speed;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Block"))
        {
            canJump = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Block"))
        {
            canJump = false;
        }
    }
}
