using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BeamToPoint : MonoBehaviour
{
    [SerializeField] private string Direction = "right"; // The direction to cast

    private LineRenderer lineRenderer;
    private float distance = 0.0f; // Distance to point

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        lineRenderer.SetPosition(1, GetBeamDirection() * distance);
    }

    private Vector2 GetBeamDirection()
    {
        if (Direction.ToLower().Equals("right"))
            return this.transform.right;
        else if (Direction.ToLower().Equals("left"))
            return -this.transform.right;
        else if (Direction.ToLower().Equals("up"))
            return this.transform.up;
        else if (Direction.ToLower().Equals("down"))
            return -this.transform.up;
        else
            throw new UnityException("Invalid beam direction given: " + Direction);
    }

    public void SetTargetPosition(float distance)
    {
        this.distance = distance;
    }
}
