using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class HealthPickup : MonoBehaviour
{
    [Serializable]
    private class PickupEvent : UnityEvent { }

    [SerializeField] private string HealthTag = "Health"; // Tag for weapons
    [SerializeField] private float PickupProximity = 5.0f; // How close you have to be to pickup weapon
    [SerializeField] private PickupEvent Pickup; // Event to trigger object being picked up

    private void FixedUpdate()
    {
        var pickups = GameObject.FindGameObjectsWithTag(HealthTag);

        foreach (var pickup in pickups)
        {
            if (Vector2.Distance(this.transform.position, pickup.transform.position) <= PickupProximity)
            {
                Pickup.Invoke();
                Destroy(pickup);
            }
        }
    }
}
