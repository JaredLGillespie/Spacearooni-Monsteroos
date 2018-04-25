using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityDamager : MonoBehaviour
{
    [SerializeField] private string DamageTag = "Enemy"; // Object to damage tag
    [SerializeField] private float Damage = 10.0f; // Amount of damage to do
    [SerializeField] private float DamageRadius = 0.15f; // Radius of damage
    [SerializeField] private bool ScaleDamage = true; // If true, damage is 0 at edge and maximum at center
    [SerializeField] private bool DestroyOnStart = true; // Destroy immediately after creation
    [SerializeField] private bool Continuous = false; // Keep damaging if true
    [SerializeField] private float DamageInterval = 0.1f;

    private void Start()
    {
        if (Continuous)
        {
            StartCoroutine("ContinueDamage");
        }
        else
        {
            var objects = GameObject.FindGameObjectsWithTag(DamageTag);

            foreach (var o in objects)
            {
                var dist = Vector2.Distance(this.transform.position, o.transform.position);

                if (dist <= DamageRadius)
                {
                    var damage = Damage;

                    if (ScaleDamage)
                        damage = Damage / (DamageRadius - dist);

                    var comp = o.GetComponent<DamageObject>();
                    if (comp != null)
                        comp.InflictDamage(Damage);
                }
            }

            if (DestroyOnStart)
                Destroy(this.gameObject);
        }
    }

    private IEnumerator ContinueDamage()
    {
        while (true)
        {
            var objects = GameObject.FindGameObjectsWithTag(DamageTag);

            foreach (var o in objects)
            {
                var dist = Vector2.Distance(this.transform.position, o.transform.position);

                if (dist <= DamageRadius)
                {
                    var damage = Damage;

                    if (ScaleDamage)
                        damage = Damage / (DamageRadius - dist);

                    var comp = o.GetComponent<DamageObject>();
                    if (comp != null)
                        comp.InflictDamage(Damage);
                }
            }

            yield return new WaitForSeconds(DamageInterval);
        }
    }
}
