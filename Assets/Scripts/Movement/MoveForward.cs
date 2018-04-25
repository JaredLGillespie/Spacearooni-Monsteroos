using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveForward : MonoBehaviour
{
    [SerializeField] public float Speed = 1.0f;
    [SerializeField] private string Direction = "right"; // right, left, down, up

    private new Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        SetDirection(Direction);
    }

    public void SetDirection(string direction)
    {
        Direction = direction;

        var t = Vector3.zero;

        if (Direction.ToLower().Equals("right"))
            t = transform.right;
        else if (Direction.ToLower().Equals("left"))
            t = -transform.right;
        else if (Direction.ToLower().Equals("up"))
            t = transform.up;
        else if (Direction.ToLower().Equals("down"))
            t = -transform.up;
        else
            throw new UnityException("Unknown direction given: " + Direction);

        rigidbody2D.velocity = t * Speed;
    }
}
