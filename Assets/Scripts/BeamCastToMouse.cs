using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BeamToPoint))]
public class BeamCastToMouse : MonoBehaviour
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

        // Reset localscale if changed due to parent
        var scale = this.transform.localScale;
        if (scale.x < 0)
            scale.x = -scale.x;

        this.transform.localScale = scale;
    }

    private void Update()
    {
        var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = (mouse - this.transform.position).normalized;
        var distance = (mouse - this.transform.position).magnitude;
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, distance, HitMask);

        if (hit == null || !hit)
        {
            beamToPoint.SetTargetPosition(Screen.width);
        }
        else
        {
            
            beamToPoint.SetTargetPosition(Vector2.Distance(hit.point, this.transform.position));
            
            if (!collided)
            {
                collided = true;

                var go = hit.transform.gameObject;

                if (go.tag.Equals(EnemyTag))
                    go.GetComponent<DamageObject>().InflictDamage(Damage);

                StartCoroutine(DoImpact(hit.point));
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
