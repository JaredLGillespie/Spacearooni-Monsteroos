using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class AlienWeaponGun : MonoBehaviour
{
    [SerializeField] public GameObject BulletObject;
    [SerializeField] private Vector2 BulletPositionOffset = Vector2.zero;
<<<<<<< HEAD
    [SerializeField] private float RateOfFire = 0.8f; // Interval between firing bullets
    [SerializeField] private float InitialDelay = 3.0f; // Delay between using weapon
    [SerializeField] private AudioClip ShootSound; 
=======
    [SerializeField] public float RateOfFire = 0.8f; // Interval between firing bullets
    [SerializeField] public float InitialDelay = 3.0f; // Delay between using weapon
>>>>>>> b18fb64c859ad0d3f3cd07e12f9a2440c3c7a8e8

    private Animator animator;
    private AudioSource audioSource;
    private bool canShoot = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine("EnableShoot", InitialDelay);
    }

    private void FixedUpdate()
    {
        if (canShoot)
        {
            Shoot();
            
            canShoot = false;
            StartCoroutine("EnableShoot", RateOfFire);
        }
    }

    private void Shoot()
    {
        animator.SetTrigger("Attack");

        audioSource.PlayOneShot(ShootSound, GetShootVolume());

        // Offset bullet position
        var position = this.transform.position;

        if (this.transform.parent != null) // Why can't this be C#7.0? Why does Unity be like this?
            if (this.transform.parent.localScale.x < 0)
                position -= this.transform.right * BulletPositionOffset.x;
            else
                position += this.transform.right * BulletPositionOffset.x;
        else
            position += this.transform.right * BulletPositionOffset.x;

        position += this.transform.up * BulletPositionOffset.y;

        var bullet = Instantiate(BulletObject, position, this.transform.rotation);

        // Fix bullet rotation and movement direction
        var mfc = bullet.GetComponent<MoveForward>();

        if (this.transform.parent != null)
        {
            if (this.transform.parent.localScale.x < 0)
            {
                if (mfc != null)
                    mfc.SetDirection("left");
                bullet.transform.Rotate(0, 0, 180, Space.Self);
            }
            else
            {
                if (mfc != null)
                    mfc.SetDirection("right");
            }
        }

        // Ignore alien and weapon collisions
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
        if (this.transform.parent != null)
            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), this.transform.parent.gameObject.GetComponent<Collider2D>());
    }

    private IEnumerator EnableShoot(float delay)
    {
        yield return new WaitForSeconds(delay);
        canShoot = true;
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
