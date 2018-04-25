﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class Player : MonoBehaviour {
    [SerializeField]
    private Stat health;

	// Use this for initialization
	void Start () {
	}

    private void Awake() {
        health.Initialize();
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q)) {
            health.CurrentVal -= 10;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            health.CurrentVal += 10;
        }
    }

    public void pickUpHealth() {
        health.CurrentVal += 15;
    }

    public void fillHealth() {
        health.CurrentVal = 100;
    }

    public void Damage(float damage)
    {
        health.CurrentVal = Mathf.Max(0, health.CurrentVal - damage);
    }
}
