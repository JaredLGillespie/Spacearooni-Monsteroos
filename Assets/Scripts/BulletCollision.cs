using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class BulletCollision : MonoBehaviour
{
    [System.Serializable]
    private class EnemyHitEvent : UnityEvent<float> { }

    [SerializeField] private string DamageTag = "Enemy"; // Object to damage tag
    [SerializeField] private float Damage; // Amount of damage to do if hit enemy
    [SerializeField] private GameObject CollisionCreator; // Collision creator object

    private new Collider2D collider2D;
    private ImpactCreator impactCreator;
    private bool collided = false;

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        impactCreator = GetComponent<ImpactCreator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collided)
        {
            var go = collision.collider.gameObject;

            if (go.tag.Equals(DamageTag))
            {
                collided = true;
                // Why is this not C#7 ?????????
                var comp = go.GetComponent<DamageObject>();
                if (comp != null)
                    comp.InflictDamage(Damage);
            }

            if (CollisionCreator != null)
            {
                Instantiate(CollisionCreator, this.transform.position, this.transform.rotation);
            }

            Destroy(this.gameObject);
        }
    }
}
