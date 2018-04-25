using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class AlienWeaponLaser : MonoBehaviour
{
    [SerializeField] GameObject LaserObject;
    [SerializeField] Vector2 PositionOffset;
    [SerializeField] private float InitialDelay = 3.0f;
    [SerializeField] private AudioClip ShootSound;

    private Animator animator;
    private AudioSource audioSource;
    private GameObject heldObject;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine("CreateLaser");
    }

    private void OnDestroy()
    {
        audioSource.Stop();

        if (heldObject != null)
            Destroy(heldObject);
    }

    private IEnumerator CreateLaser()
    {
        yield return new WaitForSeconds(InitialDelay);

        animator.SetBool("Attack", true);
        audioSource.clip = ShootSound;
        audioSource.volume = GetShootVolume();
        audioSource.loop = true;
        audioSource.Play();

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

    private float GetShootVolume()
    {
        if (this.transform.parent != null)
        {
            var comp = this.transform.parent.gameObject.GetComponent<Enemy>();

            if (comp != null)
                return comp.GetVolume();
        }

        return 1.0f;
    }
}
