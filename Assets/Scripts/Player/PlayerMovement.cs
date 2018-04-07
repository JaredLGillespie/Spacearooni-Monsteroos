using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float Speed = 1.0f; // The player's movement speed
    [SerializeField] private float JumpForce = 400.0f; // Amount of force added when the player jumps
    [SerializeField] private bool AirControl = true; // Whether the player can steer while jumping
    [SerializeField] private string GroundTag = "Ground"; // Using tag to determine ground objects
    [SerializeField] private float GroundProximity = 0.1f; // Proximity to ground to consider colliding

    private Animator animator;
    private new Rigidbody2D rigidbody2D;
    private bool grounded = false; // Whether or not the player is grounded
    private bool canJump = true; // Whether player is able to jump by holding button

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (animator.GetBool("Dead")) return;
        animator.SetBool("Walking", false);

        // Check if we're colliding with ground
        grounded = IsGrounded();

        animator.SetBool("Grounded", grounded);

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) // Move Left
        {
            if (AirControl || grounded)
            {
                rigidbody2D.velocity -= (Vector2)this.gameObject.transform.right * Speed * Time.deltaTime;
                animator.SetBool("Walking", true);
            }
            else
            {
                animator.SetBool("Walking", false);
            }
        }
        else if (!Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) // Move Right
        {
            if (AirControl || grounded)
            {
                rigidbody2D.velocity += (Vector2)this.gameObject.transform.right * Speed * Time.deltaTime;
                animator.SetBool("Walking", true);
            }
            else
            {
                animator.SetBool("Walking", false);
            }
        }
        
        if (Input.GetKey(KeyCode.Space)) // Jump
        {
            if (grounded && canJump)
            {
                rigidbody2D.velocity += (Vector2)this.gameObject.transform.up * JumpForce * Time.deltaTime;
                canJump = false;
            }
        }
        else if (grounded)
        {
            canJump = true;
        }
    }

    private bool IsGrounded()
    {
        // Would normally use this, however the rotations around objects make "touching"
        // difficult to trigger. Instead we use a proximity based approach.
        // return rigidbody2D.IsTouchingLayers(WhatIsGround);

        return GameObject.FindGameObjectsWithTag(GroundTag)
            .Where(w => rigidbody2D.Distance(w.GetComponent<Collider2D>()).distance <= GroundProximity)
            .Any();
    }
}
