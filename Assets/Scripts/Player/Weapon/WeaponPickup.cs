using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class WeaponPickup : MonoBehaviour
{
    [Serializable]
    private class PickupEvent : UnityEvent<string> { }

    [SerializeField] private string WeaponTag = "Weapon"; // Tag for weapons
    [SerializeField] private float PickupProximity = 5.0f; // How close you have to be to pickup weapon
    [SerializeField] private PickupEvent Pickup; // Event to trigger object being picked up

    private void FixedUpdate()
    {
        var pickups = GameObject.FindGameObjectsWithTag(WeaponTag);

        foreach (var pickup in pickups)
        {
           if (Vector2.Distance(this.transform.position, pickup.transform.position) <= PickupProximity)
            {
                // Was gonna use a tag approach, but since I need something to specify the weapon attributes,
                // I'm relying on the object to have a WeaponAttributes component to signify it is a weapon
                var wa = pickup.GetComponent<WeaponAttributes>();

                if (wa != null)
                {
                    Pickup.Invoke(wa.WeaponName);

                    Destroy(pickup);
                }
                else
                    throw new UnityException("Tagged Weapon does not have an attached WeaponAttributes component!");
            }
        }
    }
}
