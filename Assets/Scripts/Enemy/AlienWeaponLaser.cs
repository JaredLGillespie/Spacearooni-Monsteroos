using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AlienWeaponLaser : MonoBehaviour
{
    [SerializeField] GameObject LaserObject;
    [SerializeField] Vector2 PositionOffset;
    [SerializeField] private float InitialDelay = 3.0f;

    private Animator animator;
    private GameObject heldObject;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine("CreateLaser");
    }

    private void OnDestroy()
    {
        if (heldObject != null)
            Destroy(heldObject);
    }

    private IEnumerator CreateLaser()
    {
        yield return new WaitForSeconds(InitialDelay);

        animator.SetBool("Attack", true);

        var position = this.transform.position;

        if (this.transform.parent != null && this.transform.parent.localScale.x < 0)
            position -= this.transform.right * PositionOffset.x;
        else
            position += this.transform.right * PositionOffset.x;

        position += this.transform.up * PositionOffset.y;

        heldObject = Instantiate(LaserObject, this.transform, true);
        heldObject.transform.position = position;
        heldObject.transform.rotation = this.transform.rotation;

        // Fix bullet rotation and movement direction
        var mfc = heldObject.GetComponent<MoveForward>();

        if (this.transform.parent != null)
        {
            if (this.transform.parent.localScale.x < 0)
            {
                heldObject.transform.Rotate(0, 0, 180, Space.Self);

                if (mfc != null)
                    mfc.SetDirection("left");
            }
            else
            {
                if (mfc != null)
                    mfc.SetDirection("right");
            }
        }

        // Ignore player and weapon collisions
        Physics2D.IgnoreCollision(heldObject.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());

        if (this.transform.parent != null)
            Physics2D.IgnoreCollision(heldObject.GetComponent<Collider2D>(), this.transform.parent.gameObject.GetComponent<Collider2D>());
    }
}
