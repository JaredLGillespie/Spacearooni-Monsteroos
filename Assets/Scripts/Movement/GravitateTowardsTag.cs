using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class GravitateTowardsTag : MonoBehaviour
{

    private class Tuple<T1, T2>
    {
        // This is why can't we have nice things
        public T1 Item1 { get; private set; }
        public T2 Item2 { get; private set; }

        public Tuple(T1 item1, T2 item2)
        {
            this.Item1 = item1;
            this.Item2 = item2;
        }
    }

    [SerializeField] private string GravitateTag = "Ground"; // Objects with this tag are gravitated towards
    [SerializeField] private float Gravity = 0.1f; // The gravitational force
    [SerializeField] private float DelayBetweenSwitch = 1.0f; // Delay between switching targets

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
                // Rotate towards the object
                FaceObject(closest.Item2);

                // Gravitate towards the object
                rigidbody2D.AddRelativeForce(Vector3.down * Gravity, ForceMode2D.Force);

                previousTarget = closest.Item1;
            }
            else
            {
                if (canSwitch)
                {
                    // Rotate towards the object
                    FaceObject(closest.Item2);

                    // Gravitate towards the object
                    rigidbody2D.AddRelativeForce(Vector3.down * Gravity, ForceMode2D.Force);

                    StartCoroutine("EnableSwitch");

                    canSwitch = false;

                    previousTarget = closest.Item1;
                }
                else if (previousTarget != null)
                {
                    var rotationPoint = previousTarget.GetComponent<Collider2D>().bounds.ClosestPoint(this.transform.position);

                    // If circle, rotate towards center; otherwise use plane
                    if (previousTarget.GetComponent<CircleCollider2D>() != null)
                        rotationPoint = previousTarget.transform.position;

                    // Rotate towards the object
                    FaceObject(rotationPoint);

                    // Gravitate towards the object
                    rigidbody2D.AddRelativeForce(Vector3.down * Gravity, ForceMode2D.Force);
                }
            }
        }
    }

    private Tuple<GameObject, Vector2> FindClosestObject()
    {
        var objects = GameObject.FindGameObjectsWithTag(GravitateTag);
        var closestObject = (GameObject)null;
        var closestDistance = Mathf.Infinity;
        var rotationPoint = new Vector2();

        foreach (var o in objects)
        {
            var p = o.GetComponent<Collider2D>().bounds.ClosestPoint(this.transform.position);

            //var dist = Mathf.Abs(Vector2.Distance(p1, p2));
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
        
        return new Tuple<GameObject, Vector2>(closestObject, rotationPoint);
    }

    private void FaceObject(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - (Vector2)this.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        transform.rotation =  Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private IEnumerator EnableSwitch()
    {
        yield return new WaitForSeconds(DelayBetweenSwitch);

        canSwitch = true;
    }
}
