using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class BulletCollision : MonoBehaviour
{
    [System.Serializable]
    private class EnemyHitEvent : UnityEvent<float> { }

    [SerializeField] private string EnemyTag = "Enemy"; // Enemy tag
    [SerializeField] private float Damage; // Amount of damage to do if hit enemy

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
            collided = true;

            var go = collision.otherCollider.gameObject;

            if (go.tag.Equals(EnemyTag))
                go.GetComponent<DamageEnemy>().InflictDamage(Damage);

            StartCoroutine("DoImpact");
        }
    }

    private IEnumerator DoImpact()
    {
        if (impactCreator != null)
            yield return impactCreator.CreateImpact();

        Destroy(this.gameObject);
    }
}
