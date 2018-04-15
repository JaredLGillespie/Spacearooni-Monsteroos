using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class GravitateTowardsTag : MonoBehaviour
{

    private class Tuple<T1, T2, T3>
    {
        // This is why can't we have nice things
        public T1 Item1 { get; private set; }
        public T2 Item2 { get; private set; }
        public T3 Item3 { get; private set; }

        public Tuple(T1 item1, T2 item2, T3 item3)
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
        }
    }

    [SerializeField] private string GravitateTag = "Ground"; // Objects with this tag are gravitated towards
    [SerializeField] private float Gravity = 0.1f; // The gravitational force
    [SerializeField] private float DelayBetweenSwitch = 1.0f; // Delay between switching targets
    [SerializeField] private float MinimumSwitchProximity = 10.0f; // Must be this close to object to switch gravitate towards from different object
    [SerializeField] private float MinimumGravitateDistance = Mathf.Infinity; // Must be this cloest to object to gravitate towards
    [SerializeField] private bool RotateTowards = true; // Whether to rotate towards the object
    [SerializeField] private bool AlwaysSnapRotation = false; // Whether to always snap rotate towards the object
    [SerializeField] private float RotateSpeed = 3.0f; // The rotation speed (only used if SnapRotation = false)
    [SerializeField] private float DistanceToSnapRotate = 0.0f; // The minimum distance from the object to enable snap rotation (useful when very close)

    private new Collider2D collider2D;
    private new Rigidbody2D rigidbody2D;
    private bool canSwitch = true;
    private GameObject previousTarget;

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Find the object
        var closest = FindClosestObject();

        // Object with tag must exist to rotate towards
        if (closest.Item1 != null)
        {
            if (previousTarget == null || closest.Item1 == previousTarget)
            {
                previousTarget = closest.Item1;

                if (closest.Item3 <= MinimumGravitateDistance)
                {
                    // Rotate towards the object
                    if (RotateTowards)
                        FaceObject(closest.Item2, closest.Item3);

                    // Gravitate towards the object
                    rigidbody2D.AddRelativeForce(Vector3.down * Gravity, ForceMode2D.Force);

                }
            }
            else
            {
                if (canSwitch && closest.Item3 <= MinimumSwitchProximity)
                {
                    StartCoroutine("EnableSwitch");
                    canSwitch = false;
                    previousTarget = closest.Item1;

                    if (closest.Item3 <= MinimumGravitateDistance)
                    {
                        // Rotate towards the object
                        if (RotateTowards)
                            FaceObject(closest.Item2, closest.Item3);

                        // Gravitate towards the object
                        rigidbody2D.AddRelativeForce(Vector3.down * Gravity, ForceMode2D.Force);
                    }
                }
                else if (previousTarget != null)
                {
                    var dist = collider2D.Distance(previousTarget.GetComponent<Collider2D>()).distance;

                    if (dist <= MinimumGravitateDistance)
                    {
                        var rotationPoint = previousTarget.GetComponent<Collider2D>().bounds.ClosestPoint(this.transform.position);

                        // If circle, rotate towards center; otherwise use plane
                        if (previousTarget.GetComponent<CircleCollider2D>() != null)
                            rotationPoint = previousTarget.transform.position;

                        // Rotate towards the object
                        if (RotateTowards)
                            FaceObject(rotationPoint, closest.Item3);

                        // Gravitate towards the object
                        rigidbody2D.AddRelativeForce(Vector3.down * Gravity, ForceMode2D.Force);
                    }
                }
            }
        }
    }

    private Tuple<GameObject, Vector2, float> FindClosestObject()
    {
        var objects = GameObject.FindGameObjectsWithTag(GravitateTag);
        var closestObject = (GameObject)null;
        var closestDistance = Mathf.Infinity;
        var rotationPoint = new Vector2();

        foreach (var o in objects)
        {
            var p = o.GetComponent<Collider2D>().bounds.ClosestPoint(this.transform.position);
            var dist = collider2D.Distance(o.GetComponent<Collider2D>()).distance;

            if (dist < closestDistance)
            {
                closestObject = o;
                closestDistance = dist;
                rotationPoint = p;

                // If circle, rotate towards center; otherwise use plane
                if (o.GetComponent<CircleCollider2D>() != null)
                    rotationPoint = o.transform.position;
            }
        }
        
        return new Tuple<GameObject, Vector2, float>(closestObject, rotationPoint, closestDistance);
    }

    private void FaceObject(Vector2 targetPosition, float distance)
    {
        Vector2 direction = targetPosition - (Vector2)this.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;

        var target = Quaternion.AngleAxis(angle, Vector3.forward);

        if (AlwaysSnapRotation || distance <= DistanceToSnapRotate)
            this.transform.rotation = target;
        else
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, target, RotateSpeed * Time.deltaTime);
    }

    private IEnumerator EnableSwitch()
    {
        yield return new WaitForSeconds(DelayBetweenSwitch);

        canSwitch = true;
    }
}
