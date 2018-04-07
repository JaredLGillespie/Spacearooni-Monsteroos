using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform FollowTarget; // The target to follow
    [SerializeField] private float DampTime = 0.15f; // The damping factor (set to 0.0 to snap)

    private new Camera camera;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (FollowTarget)
        {
            var point = camera.WorldToViewportPoint(FollowTarget.position);
            var delta = FollowTarget.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));

            var destination = transform.position + delta;

            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, DampTime);
        }
    }
}
