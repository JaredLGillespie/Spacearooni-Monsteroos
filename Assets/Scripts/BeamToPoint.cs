using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BeamToPoint : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private float distance = 0.0f; // Distance to point

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        lineRenderer.SetPosition(1, Vector2.right * distance);
    }

    public void SetTargetPosition(float distance)
    {
        this.distance = distance;
    }
}
