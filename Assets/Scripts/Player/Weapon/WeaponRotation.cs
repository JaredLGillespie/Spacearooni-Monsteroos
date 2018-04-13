using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(RotateTowardsMouse))]
public class WeaponRotation : MonoBehaviour
{
    private Transform player;
    private RotateTowardsMouse rotateTowardsMouse;
    private bool facingRight = true;

    private void Awake()
    {
        player = this.transform.parent.transform;
        rotateTowardsMouse = GetComponent<RotateTowardsMouse>();
    }

    private void Update()
    {
        var difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();

        var aimAngle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        aimAngle = (aimAngle + 270) % 360; // Line up w/ objectAngle for quick mafs

        var objectAngle = player.eulerAngles.z;

        // Fix angles because floats are dumb and break the formularoonis
        if (objectAngle < 0) objectAngle = 0;
        if (aimAngle < 0) aimAngle = 0;

        // Formulas are different at quadrants 1 & 3 OR 2 & 4
        // This is all silly math because of the angles being % 360
        if (objectAngle >= 0 && objectAngle <= 180)
        {
            if (aimAngle < objectAngle || aimAngle > objectAngle + 180)
            {
                if (!facingRight)
                    FlipImage();
            }
            else
            {
                if (facingRight)
                    FlipImage();
            }
        }
        else
        {
            if (aimAngle < objectAngle && aimAngle > objectAngle - 180)
            {
                if (!facingRight)
                    FlipImage();
            }
            else
            {
                if (facingRight)
                    FlipImage();
            }
        }
    }

    private void FlipImage()
    {
        facingRight = !facingRight;

        if (facingRight)
            rotateTowardsMouse.SetRotationOffset(0);
        else
            rotateTowardsMouse.SetRotationOffset(180);

        var scale = player.localScale;
        scale.x *= -1;
        player.localScale = scale;
    }
}
