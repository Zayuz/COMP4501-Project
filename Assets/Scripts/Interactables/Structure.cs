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

    private float timer;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        timer = 0f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (timer >= 2.5f) // regenerate hp every 2.5 seconds
        {
            currentHealth += 10;
            currentHealth = (currentHealth > maxHealth) ? maxHealth : currentHealth;
            timer = 0f;
        }
    }
}
