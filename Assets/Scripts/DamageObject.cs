using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageObject : MonoBehaviour
{
    [Serializable]
    private class DamageEvent : UnityEvent<float> { };

    [SerializeField] private DamageEvent OnDamage;

    public void InflictDamage(float damage)
    {
        OnDamage.Invoke(damage);
    }
}
