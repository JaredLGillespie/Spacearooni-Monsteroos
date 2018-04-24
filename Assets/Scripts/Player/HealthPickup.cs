using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class HealthPickup : MonoBehaviour
{
    [Serializable]
    private class PickupEvent : UnityEvent<string> { }

    [SerializeField] private string HealthTag = "Health"; // Tag for weapons
    [SerializeField] private float PickupProximity = 5.0f; // How close you have to be to pickup weapon
    [SerializeField] private PickupEvent PickUp; // Event to trigger weapon being picked up
    [SerializeField] public Player player;
    private void FixedUpdate()
    {
        var pickups = GameObject.FindGameObjectsWithTag(HealthTag);

        foreach (var pickup in pickups)
        {
           if (Vector2.Distance(this.transform.position, pickup.transform.position) <= PickupProximity)
            {
                // Was gonna use a tag approach, but since I need something to specify the weapon attributes,
                // I'm relying on the object to have a WeaponAttributes component to signify it is a weapon
                var pa = pickup.GetComponent<HealthAttributes>();

                if (pa != null)
                {
                    PickUp.Invoke(pa.PickupName);
                    player.pickUpHealth();
                    Destroy(pickup);
                }
                else
                    throw new UnityException("Tagged Weapon does not have an attached WeaponAttributes component!");
            }
        }
    }
}
