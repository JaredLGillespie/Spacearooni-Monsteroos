using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(Animator))]
public class WeaponSwitch : MonoBehaviour
{
    [Serializable]
    private class WeaponResource
    {
        public string Name;
        public string Path;
    }

    [Serializable]
    private class SwitchWeaponEvent : UnityEvent<string> { };

    [SerializeField] private WeaponResource[] WeaponResources;
    [SerializeField] private string DefaultWeapon = "pistol";
    [SerializeField] private SwitchWeaponEvent SwitchWeapon;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        UseDefault();
    }

    public void SetWeapon(string name)
    {
        var wr = WeaponResources.Where(w => w.Name.ToLower().Equals(name.ToLower())).FirstOrDefault();

        if (wr == null) 
            throw new UnityException("Weapon resource not defined: " + name);

        // Any resource has to be defined in Assets/Resources to be loaded at runtime
        animator.runtimeAnimatorController = Resources.Load(wr.Path, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

        SwitchWeapon.Invoke(name);
    }

    public void UseDefault()
    {
        SetWeapon(DefaultWeapon);
    }
}
