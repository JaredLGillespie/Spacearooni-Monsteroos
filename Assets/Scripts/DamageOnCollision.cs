using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageOnCollision : MonoBehaviour
{
    [SerializeField] private string DamageTag = "Enemy"; // Object to damage tag
    [SerializeField] private float Damage; // Amount of damage to do if hit enemy

    private new Collider2D collider2D;

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
