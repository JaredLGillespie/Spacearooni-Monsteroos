using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class WeaponPickup : MonoBehaviour
{
    [Serializable]
    private class SwitchWeaponEvent : UnityEvent<string> { }

    [SerializeField] private string WeaponTag = "Weapon"; // Tag for weapons
    [SerializeField] private float PickupProximity = 5.0f; // How close you have to be to pickup weapon
    [SerializeField] private SwitchWeaponEvent SwitchWeapon; // Event to trigger weapon being picked up

    private void FixedUpdate()
    {
        var weapons = GameObject.FindGameObjectsWithTag(WeaponTag);

        foreach (var weapon in weapons)
        {
           if (Vector2.Distance(this.transform.position, weapon.transform.position) <= PickupProximity)
            {
                // Was gonna use a tag approach, but since I need something to specify the weapon attributes,
                // I'm relying on the object to have a WeaponAttributes component to signify it is a weapon
                var wa = weapon.GetComponent<WeaponAttributes>();

                if (wa != null)
                {
                    SwitchWeapon.Invoke(wa.WeaponName);

                    Destroy(weapon);
                }
                else
                    throw new UnityException("Tagged Weapon does not have an attached WeaponAttributes component!");
            }
        }
    }
}
