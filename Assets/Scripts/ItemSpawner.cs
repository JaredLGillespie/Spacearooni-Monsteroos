using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private Vector2 Offset; // Offset item above spawner
    [SerializeField] private AudioClip PickupSound;

    private Animator animator;
    private AudioSource audioSource;
    private bool active = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SpawnItem(GameObject o)
    {
        if (active) return;
        
        active = true;
        animator.SetBool("Active", true);

        var pos = this.transform.position;
        pos += this.transform.right * Offset.x;
        pos += this.transform.up * Offset.y;

        var item = Instantiate(o, pos, this.transform.rotation);

        // Attach delegate to item to mark spawner as inactive when destroyed
        var aod = item.GetComponent<ActivateOnDestroy>();

        if (aod != null)
            aod.OnActivation.AddListener(SetInactive);
    }

    public bool IsActive()
    {
        return active;
    }

    public void SetInactive()
    {
        active = false;
        animator.SetBool("Active", false);
        audioSource.PlayOneShot(PickupSound);
    }
}
