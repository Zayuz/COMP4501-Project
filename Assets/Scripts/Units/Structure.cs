using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : Unit
{
    [Header("Structure Settings")]
    public float regenAmount; 

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

        if (regenTimer <= 0f && regenAmount > 0)
        {
            if (currentHealth != maxHealth)
            {
                Heal(regenAmount);
            }
            
            regenTimer = 2.5f; // regenerate hp every 2.5 seconds
        }
    }
}
