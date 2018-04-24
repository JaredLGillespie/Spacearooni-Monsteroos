using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BeamToPoint))]
public class BeamCastForward : MonoBehaviour
{
    [SerializeField] private LayerMask HitMask; // What to collide with
    [SerializeField] private string HitTag = "Player"; // Object to hit tag
    [SerializeField] private float Damage; // Amount of damage to do if hit object
    [SerializeField] private string Direction = "right"; // The direction of the beam
    [SerializeField] private float CastRadius = 0.1f; // The radius of the circle cast

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
        var direction = GetBeamDirection();

        var distance = Screen.width - this.transform.position.x;
        RaycastHit2D hit = Physics2D.CircleCast(this.transform.position, CastRadius, direction, distance, HitMask);

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

                if (go.tag.Equals(HitTag))
                {
                    var goco = go.GetComponent<DamageObject>();

                    if (goco != null)
                        goco.InflictDamage(Damage);
                }

                StartCoroutine(DoImpact(hit.point));
            }
        }
    }

    private Vector2 GetBeamDirection()
    {
        if (Direction.ToLower().Equals("right"))
            return Vector2.right;
        else if (Direction.ToLower().Equals("left"))
            return -Vector2.right;
        else if (Direction.ToLower().Equals("up"))
            return Vector2.up;
        else if (Direction.ToLower().Equals("down"))
            return -Vector2.up;
        else
            throw new UnityException("Invalid beam direction given: " + Direction);
    }

    private IEnumerator DoImpact(Vector3 position)
    {
        if (impactCreator != null)
            yield return impactCreator.CreateImpactAtPosition(position);

        collided = false;
    }
}
