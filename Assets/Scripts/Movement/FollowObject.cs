using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private Transform ObjectToFollow;
    [SerializeField] private bool Snap = true; // Whether to snap to the following object, otherwise moves towards with delay
    [SerializeField] private float Speed = 1.0f;
    [SerializeField] private Vector2 Offset;

    private void LateUpdate()
    {
        if (Snap)
            this.transform.position = (Vector2)ObjectToFollow.transform.position + Offset;
        else
        {
            this.transform.position = Vector2.MoveTowards((Vector2)this.transform.position + Offset, ObjectToFollow.position, Speed * Time.deltaTime);
        }
    }
}
