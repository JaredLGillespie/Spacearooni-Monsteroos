using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class PlayerSwitch : MonoBehaviour
{
    [Serializable]
    private class SwitchPlayerEvent : UnityEvent<string> { };

    [SerializeField] private string DefaultResourcePath;
    [SerializeField] private string AltResourcePath;
    [SerializeField] private SwitchPlayerEvent SwitchPlayer;
    private IEnumerator healthSuit;
    private float infiniteHealthTime = 10f;
    public Text healthTimer;
    [SerializeField] Player player; 
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
        player.fillHealth();
        StartCoroutine(PowerUp());
    }

    public IEnumerator PowerUp()
    {
        healthSuit = HealthSuit();
        StartCoroutine(healthSuit);
        healthTimer.enabled = true;
        while (infiniteHealthTime > 0f)
        {
            healthTimer.text = string.Format("remaining: {0:00.00}", infiniteHealthTime);
            yield return new WaitForSeconds(1f);
            infiniteHealthTime -= 1f;
        }
        healthTimer.enabled = false;
        infiniteHealthTime = 10f;
        StopCoroutine(healthSuit);
        healthSuit = null;
        UseDefault();
    }

    public IEnumerator HealthSuit()
    {
        while (true)
        {
            player.fillHealth();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
