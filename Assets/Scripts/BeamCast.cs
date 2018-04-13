using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BeamToPoint))]
public class BeamCast : MonoBehaviour
{
    [SerializeField] private LayerMask HitMask; // What to collide with
    [SerializeField] private string EnemyTag = "Enemy"; // Enemy tag
    [SerializeField] private float Damage; // Amount of damage to do if hit enemy

    private BeamToPoint beamToPoint;
    private ImpactCreator impactCreator;
    private bool collided = false;

    private void Awake()
    {
        this.beamToPoint = GetComponent<BeamToPoint>();
        impactCreator = GetComponent<ImpactCreator>();
    }

    private void Update()
    {
        var mouse = Camera.main.WorldToScreenPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, mouse, Mathf.Infinity, HitMask);

        if (hit == null || !hit)
            beamToPoint.SetTargetPosition(mouse);
        else
        {
            beamToPoint.SetTargetPosition(hit.transform.position);

            if (!collided)
            {
                collided = true;

                var go = hit.transform.gameObject;

                if (go.tag.Equals(EnemyTag))
                    go.GetComponent<DamageEnemy>().InflictDamage(Damage);

                StartCoroutine(DoImpact(hit.transform.position));
            }
        }
    }

    private IEnumerator DoImpact(Vector3 position)
    {
        if (impactCreator != null)
            yield return impactCreator.CreateImpactAtPosition(position);

        collided = false;
    }
}
