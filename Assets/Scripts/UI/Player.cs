using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour {
    [SerializeField] private Stat health;
    [SerializeField] private UnityEvent OnDeath; // Perform something on death

    private Animator animator;
    private bool isDead = false;

    private void Awake() {
        animator = GetComponent<Animator>();
        health.Initialize();
    }

    public void pickUpHealth() {
        health.CurrentVal += 15;
    }

    public void fillHealth() {
        health.CurrentVal = 100;
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
