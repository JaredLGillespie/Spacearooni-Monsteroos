using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DestroyAfterAnimationEnd : MonoBehaviour
{
    [SerializeField] private string AnimationName; // Name of animation to destroy on

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(AnimationName))
            Destroy(this.gameObject);
    }
}
