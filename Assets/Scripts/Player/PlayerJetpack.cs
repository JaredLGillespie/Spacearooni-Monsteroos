using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerJetpack : MonoBehaviour
{
    [SerializeField] private float ThrustForce = 200.0f; // The thrusting force of the jetpack
    [SerializeField] private string GroundTag = "Ground"; // Using tag to determine ground objects
    [SerializeField] private float GroundProximity = 0.1f; // Proximity to ground to consider colliding

    private Animator animator;
    private new Rigidbody2D rigidbody2D;
    private bool grounded = false;
    private bool canFly = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Check if we're colliding with ground
        grounded = IsGrounded();

        if (!grounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                canFly = true;
                animator.SetBool("Jetpacking", true);
            }
            else if (Input.GetKey(KeyCode.Space) && canFly) {
                rigidbody2D.velocity += (Vector2)this.gameObject.transform.up * ThrustForce * Time.deltaTime;

                animator.SetBool("Jetpacking", true);
            }
            else
            {
                animator.SetBool("Jetpacking", false);
            }
        }
        else
        {
            canFly = false;
            animator.SetBool("Jetpacking", false);
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
