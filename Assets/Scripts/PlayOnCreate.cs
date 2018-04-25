using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayOnCreate : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
 
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();   
    }

    private void Start()
    {
        audioSource.PlayOneShot(clip);
    }
}
