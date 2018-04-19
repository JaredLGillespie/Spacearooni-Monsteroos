using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveDirection : MonoBehaviour
{
    [SerializeField] private float Speed = 1.0f; // The movement speed
    [SerializeField] private string GroundTag = "Ground"; // Using tag to determine ground objects
    [SerializeField] private float GroundProximity = 0.1f; // Proximity to ground to consider colliding
    [SerializeField] private string Direction = "left"; // Direction to move (either left or right or forward)
    [SerializeField] private bool OnlyWhenGrounded = true; // Only move when grounded

    private new Rigidbody2D rigidbody2D;
    private bool grounded = false;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        grounded = IsGrounded();

        if (grounded || !OnlyWhenGrounded)
        {
            if (Direction.ToLower().Equals("left"))
                rigidbody2D.velocity -= (Vector2)this.gameObject.transform.right * Speed * Time.deltaTime;
            else if (Direction.ToLower().Equals("right"))
                rigidbody2D.velocity += (Vector2)this.gameObject.transform.right * Speed * Time.deltaTime;
            else if (Direction.ToLower().Equals("forward"))
                rigidbody2D.velocity += (Vector2)this.rigidbody2D.velocity.normalized * Speed * Time.deltaTime;
            else
                throw new UnityException("Invalid direction given: " + Direction);
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
