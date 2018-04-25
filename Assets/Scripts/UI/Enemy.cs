using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Stat health;
    [SerializeField] private UnityEvent OnDeath;

    private Animator animator;
    private bool isDead = false;

    private void Awake() {
        animator = GetComponent<Animator>();
        health.Initialize();
    }

    public void Damage(float damage)
    {
        health.CurrentVal = Mathf.Max(0, health.CurrentVal - damage);

        if (health.CurrentVal <= 0.01 && !isDead)
        {
            health.CurrentVal = 0;
            isDead = true;
            animator.SetBool("Dead", true);
            OnDeath.Invoke();
        }
        
    }
}
