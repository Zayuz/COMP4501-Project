using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : Unit
{
    public enum StructureType {
        Base,
        Shield
    }

    [Header("Structure Settings")]
    public StructureType structureType;

    private float regenTimer;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        regenTimer = 0f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        regenTimer -= Time.deltaTime;

        if (regenTimer <= 0f)
        {
            currentHealth += 10;
            currentHealth = (currentHealth > maxHealth) ? maxHealth : currentHealth;
            regenTimer = 2.5f; // regenerate hp every 2.5 seconds
        }
    }
}
