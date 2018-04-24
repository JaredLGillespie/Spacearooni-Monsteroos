using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Linq;

[RequireComponent(typeof(Animator))]
public class PlayerSwitch : MonoBehaviour
{
    [Serializable]
    private class SwitchPlayerEvent : UnityEvent<string> { };

    [SerializeField] private string DefaultResourcePath;
    [SerializeField] private string AltResourcePath;
    [SerializeField] private SwitchPlayerEvent SwitchPlayer;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        UseDefault();
    }

    public void UseDefault()
    {
        // Any resource has to be defined in Assets/Resources to be loaded at runtime
        animator.runtimeAnimatorController = Resources.Load(DefaultResourcePath, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

        SwitchPlayer.Invoke(name);
    }

    public void UseAlt()
    {
        // Any resource has to be defined in Assets/Resources to be loaded at runtime
        animator.runtimeAnimatorController = Resources.Load(AltResourcePath, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

        SwitchPlayer.Invoke(name);
    }
}
