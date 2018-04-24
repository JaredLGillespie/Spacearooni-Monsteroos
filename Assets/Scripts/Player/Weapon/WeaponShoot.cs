using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System;

[RequireComponent(typeof(Animator))]
public class WeaponShoot : MonoBehaviour
{
    [Serializable]
    private class WeaponInfo
    {
        public string Name; // Name of the weapon
        public int NumberOfBullets; // Number of bullets / shots / whatever (set to -1 for infinite)
        public bool IsAutomatic = false; // Whether weapon is automatic
        public bool IsHold = false; // Whether object is held
        public float HoldTime = 0.0f; // The amount of time the item can be held (only used if IsHold = true)
        public float RateOfFire = 0.0f; // Rate of fire (if automatic)
        public Vector2 BulletPositionOffset = Vector2.zero; // Set to end of gun muzzle
        public GameObject BulletObject; // Bullet object
        public AudioClip shoot;
    }

    [SerializeField] private WeaponInfo[] WeaponInfos;
    [SerializeField] private UnityEvent UseDefaultWeapon;
    [SerializeField] public GameManager gameManager;
    [SerializeField] public MusicPlayer musicPlayer;
    private Animator animator;
    private WeaponInfo currentWeapon;
    private bool canShoot = true;
    private int numberOfBullets = -1;
    private float holdTime = 0.0f;
    private float lastHeldTime = 0.0f;
    private GameObject heldObject;
    private IEnumerator enableShootCoroutine;
    private IEnumerator disableHoldCoroutine;
    private AudioSource loopSound;
    private AudioSource shooting;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        loopSound = gameObject.AddComponent<AudioSource>();
        shooting = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (gameManager.optionsMenu.activeSelf || gameManager.inGameMenu.activeSelf) {
            return;
        }
        gameManager.pistol.SetActive(false);
        gameManager.laser.SetActive(false);
        gameManager.machine.SetActive(false);
        gameManager.rocket.SetActive(false);
        if (currentWeapon.Name == "pistol")
            gameManager.pistol.SetActive(true);
        if (currentWeapon.Name == "laser")
            gameManager.laser.SetActive(true);
        if (currentWeapon.Name == "machine")
            gameManager.machine.SetActive(true);
        if (currentWeapon.Name == "rocket")
            gameManager.rocket.SetActive(true);
        gameManager.bulletCount = numberOfBullets;
        gameManager.laserTime = holdTime;
        if (currentWeapon == null) return;

        if (currentWeapon.IsAutomatic)
        {
            // Hold mouse button to shoot
            if (Input.GetMouseButton(0))
            {
                if (canShoot)
                {
                    shooting.clip = currentWeapon.shoot;
                    ShootWeapon();

                    if (numberOfBullets > 0)
                    {
                        canShoot = false;
                        enableShootCoroutine = EnableShoot();
                        StartCoroutine(enableShootCoroutine);
                    }
                    else if (numberOfBullets == 0)
                        UseDefaultWeapon.Invoke();
                }
            }
        }
        else if (!currentWeapon.IsHold)
        {
            // Click mouse button to shoot
            if (Input.GetMouseButtonDown(0))
            {
                if (canShoot)
                {
                    shooting.clip = currentWeapon.shoot;
                    ShootWeapon();

                    if (numberOfBullets == 0)
                        UseDefaultWeapon.Invoke();
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                if (canShoot && currentWeapon.IsHold)
                {
                    if (disableHoldCoroutine == null)
                    {
                        loopSound.clip = currentWeapon.shoot;
                        loopSound.loop = true;
                        disableHoldCoroutine = DisableHold();
                        StartCoroutine(disableHoldCoroutine);
                        HoldWeapon();
                    }
                }
            }
            else
            {
                if (canShoot && currentWeapon.IsHold)
                {
                    if (disableHoldCoroutine != null)
                    {
                        loopSound.Stop();
                        StopCoroutine(disableHoldCoroutine);
                        disableHoldCoroutine = null;

                        holdTime -= (Time.fixedTime - lastHeldTime);
                        gameManager.laserTime = holdTime;
                        if (heldObject != null)
                            Destroy(heldObject);

                        if (holdTime < 0)
                        {
                            animator.SetBool("Shoot", false);
                            UseDefaultWeapon.Invoke();
                        }
                    }
                }
            }
        }
    }

    private void ShootWeapon()
    {
        //  GetComponent<AudioSource>().PlayOneShot(currentWeapon.shoot);
        shooting.Play();
        if (numberOfBullets > 0)
            numberOfBullets--;

        animator.SetTrigger("Shoot");

        var position = this.transform.position;

        if (this.transform.parent.localScale.x < 0)
            position -= this.transform.right * currentWeapon.BulletPositionOffset.x;
        else
            position += this.transform.right * currentWeapon.BulletPositionOffset.x;

        position += this.transform.up * currentWeapon.BulletPositionOffset.y;

        var bullet = Instantiate(currentWeapon.BulletObject, position, this.transform.rotation);

        // Fix bullet rotation and movement direction
        var mfc = bullet.GetComponent<MoveForward>();

        if (this.transform.parent != null)
        {
            if (this.transform.parent.localScale.x < 0)
            {
                bullet.transform.Rotate(0, 0, 180, Space.Self);

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
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), this.transform.parent.gameObject.GetComponent<Collider2D>());
    }

    private void HoldWeapon()
    {
        loopSound.Play();
        animator.SetBool("Shoot", true);
        lastHeldTime = Time.fixedTime;

        var position = this.transform.position;

        if (this.transform.parent.localScale.x < 0)
            position -= this.transform.right * currentWeapon.BulletPositionOffset.x;
        else
            position += this.transform.right * currentWeapon.BulletPositionOffset.x;

        position += this.transform.up * currentWeapon.BulletPositionOffset.y;

        heldObject = Instantiate(currentWeapon.BulletObject, this.transform, true);
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
        Physics2D.IgnoreCollision(heldObject.GetComponent<Collider2D>(), this.transform.parent.gameObject.GetComponent<Collider2D>());
    }

    public void SetWeapon(string name)
    {
        var wi = WeaponInfos.Where(w => w.Name.ToLower().Equals(name.ToLower())).FirstOrDefault();

        if (wi == null)
            throw new UnityException("Weapon info not defined: " + name);

        currentWeapon = wi;

        numberOfBullets = wi.NumberOfBullets;
        holdTime = wi.HoldTime;

        if (enableShootCoroutine != null)
            StopCoroutine(enableShootCoroutine);

        if (disableHoldCoroutine != null)
            StopCoroutine(disableHoldCoroutine);

        if (heldObject != null)
            Destroy(heldObject);

        canShoot = true;
    }

    private IEnumerator EnableShoot()
    {
        yield return new WaitForSeconds(currentWeapon.RateOfFire);

        canShoot = true;
    }

    private IEnumerator DisableHold()
    {
        yield return new WaitForSeconds(holdTime);
        Debug.Log("disableHold");
        animator.SetBool("Shoot", false);
        holdTime = 0.0f;
        UseDefaultWeapon.Invoke();
    }

    public void updateSFX() {
        loopSound.volume = musicPlayer.sfxSlider.value;
        shooting.volume = musicPlayer.sfxSlider.value;
    } 
}
