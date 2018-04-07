using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ClampSpeed : MonoBehaviour
{
    [SerializeField] private bool AbsoluteClamp = false; // If true, speed will never go over max; else, braking is used
    [SerializeField] private float MaxSpeed = 5.0f; // The maximum speed allowed
    [SerializeField] private bool UseSpeedDiff = false; // If true, the speed diff between current speed and MaxSpeed is used for breaking
    [SerializeField] private float BreakSpeed = 1.0f; // The breaking speed to apply (only used if AbsoluteClamp = false and UseSpeedDiff = false)

    private new Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (AbsoluteClamp)
        {
            rigidbody2D.velocity = Vector2.ClampMagnitude(rigidbody2D.velocity, MaxSpeed);
        }
        else 
        {
            float speed = Vector2.SqrMagnitude(rigidbody2D.velocity) * Vector2.SqrMagnitude(rigidbody2D.velocity);

            if (speed > MaxSpeed)
            {
                var brakeSpeed = BreakSpeed; // Use given break speed

                if (UseSpeedDiff) // Else use diff
                    brakeSpeed = speed - MaxSpeed;

                var brakeVelocity = rigidbody2D.velocity.normalized * brakeSpeed;  // Create the brake vector

                rigidbody2D.AddForce(-brakeVelocity);  // Apply opposing brake force
            }
        }
    }
}
