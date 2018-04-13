using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BeamToPoint : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 targetPosition;
    private float distanceMultiplier = 1.0f; // Set to 1.0f to hit point, or > 1 to go through point

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        lineRenderer.SetPosition(1, Vector2.right * Vector2.Distance(targetPosition, this.transform.position) * distanceMultiplier);
    }

    public void SetTargetPosition(Vector3 position, float distance = 1.0f)
    {
        this.targetPosition = position;
        this.distanceMultiplier = distance;
    }
}
