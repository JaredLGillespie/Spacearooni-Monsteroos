using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsMouse : MonoBehaviour
{
    [SerializeField] private float RotationOffset = 0.0f;

    private void Update()
    {
        var difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();

        var angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + RotationOffset);
    }

    public void SetRotationOffset(float offset)
    {
        this.RotationOffset = offset;
    }

}
